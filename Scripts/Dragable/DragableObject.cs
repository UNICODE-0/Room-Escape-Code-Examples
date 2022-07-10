using UnityEngine;

public interface IDragable
{
    bool OnDrag(Vector3 TargetPos);
    void DragStart(Rigidbody ConnectedBody);
    void DragEnd();
}
public interface IThrowable
{
    void Throw(Vector3 Direction);
}
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(AudioSource))]
public class DragableObject : MonoBehaviour, IDragable
{
    [SerializeField] private DragableObjectSciptableObject _Data;
    [SerializeField] protected bool _IsAvalible = true;
    public bool isAvalible
    {
        get { return _IsAvalible; }
        set { _IsAvalible = value; }
    }
    private float _Spring = 3000f;
    private float _Dumper = 500f;
    private float _MaxForce = 100f;
    private float _LinerLimit = 0.1f;
    private float _MaxDeviationX, _MaxDeviationY, _MaxDeviationZ;
    private float _MaxImpulse = 200f;
    private float _MaxImpulseWithDragable = 35f;
    private float _MaxImpulseWithInteraction = 5f;

    private JointDrive _JointDrive = new JointDrive();
    private SoftJointLimit _SoftLimit = new SoftJointLimit();
    protected Rigidbody _Rgbd;
    private ConfigurableJoint _ConfigurableJoint;
    protected Collider[] _Colliders;
    protected AudioSource _AudioSource;

    private bool _OnDrag;

    private void OnCollisionStay(Collision other) 
    {  
        if(_OnDrag)
        {
            float MaxImpulse = _MaxImpulse;
            Vector3 Impulse = other.impulse;
            GameObject ColObj = other.gameObject;
            if(ColObj.GetComponent<DragableObject>() != null) MaxImpulse = _MaxImpulseWithDragable;
            if(ColObj.GetComponent<InteractionObject>() != null) MaxImpulse = _MaxImpulseWithInteraction;

            if(Impulse.x < -MaxImpulse || Impulse.y < -MaxImpulse || Impulse.z < -MaxImpulse) DragEnd();
            if(Impulse.x > MaxImpulse || Impulse.y > MaxImpulse || Impulse.z > MaxImpulse) DragEnd();
        }
    }
    private void Awake() 
    {
        _OnDrag = false;
        _Rgbd = GetComponent<Rigidbody>();
        _ConfigurableJoint = GetComponent<ConfigurableJoint>();
        _JointDrive = new JointDrive();
        _Colliders = GetComponents<Collider>();
        _AudioSource = GetComponent<AudioSource>();
        _AudioSource.playOnAwake = false;
        _AudioSource.loop = false;
        _AudioSource.spatialBlend = 1f;

        _Spring = _Data.spring;
        _Dumper = _Data.dumper;
        _MaxForce = _Data.maxForce;
        _LinerLimit = _Data.linerLimit;
        _MaxDeviationX = _Data.maxDeviationX;
        _MaxDeviationY = _Data.maxDeviationY;
        _MaxDeviationZ = _Data.maxDeviationZ;
        _MaxImpulse = _Data.maxImpulse;
        _MaxImpulseWithDragable = _Data.maxImpulseWithDragable;
        _MaxImpulseWithInteraction = _Data.maxImpulseWithInteraction;

        _JointDrive.positionSpring = _Spring;
        _JointDrive.positionDamper = _Dumper;
        _JointDrive.maximumForce = _MaxForce;
        _SoftLimit.limit = _LinerLimit;
        _ConfigurableJoint.linearLimit = _SoftLimit;
    }

    public void DragStart(Rigidbody ConnectedBody)
    {
        _OnDrag = true;
        _Rgbd.useGravity = false;
        SetJointDrive(_JointDrive);
        SetMotionMode(ConfigurableJointMotion.Limited);
        SetAngularMotionMode(ConfigurableJointMotion.Locked);
        _ConfigurableJoint.linearLimit = _SoftLimit;
        _ConfigurableJoint.connectedBody = ConnectedBody;
    }

    public bool OnDrag(Vector3 TargetPos)
    {
        Vector3 CurrentPos = transform.position;
        float DeviationX = Mathf.Abs(TargetPos.x - CurrentPos.x),DeviationY = Mathf.Abs(TargetPos.y - CurrentPos.y),DeviationZ = Mathf.Abs(TargetPos.z - CurrentPos.z);

        if(DeviationX > _MaxDeviationX)
        {
            DragEnd();
            return false;
        } 
        if(DeviationY > _MaxDeviationY)
        {
            DragEnd();
            return false;
        } 
        if(DeviationZ > _MaxDeviationZ)        
        {
            DragEnd();
            return false;
        }
        return true;
    }
    public void DragEnd()
    {
        _OnDrag = false;
        _Rgbd.useGravity = true;
        _ConfigurableJoint.connectedBody = null;
        SetJointDrive(new JointDrive());
        SetMotionMode(ConfigurableJointMotion.Free);
        SetAngularMotionMode(ConfigurableJointMotion.Free);
    }
    private void SetMotionMode(ConfigurableJointMotion Mode)
    {
        _ConfigurableJoint.xMotion = Mode;
        _ConfigurableJoint.yMotion = Mode;
        _ConfigurableJoint.zMotion = Mode;
    }
    private void SetAngularMotionMode(ConfigurableJointMotion Mode)
    {
        _ConfigurableJoint.angularXMotion = Mode;
        _ConfigurableJoint.angularYMotion = Mode;
        _ConfigurableJoint.angularZMotion = Mode;
    }
    private void SetJointDrive(JointDrive Joint)
    {
        _ConfigurableJoint.xDrive = Joint;
        _ConfigurableJoint.yDrive = Joint;
        _ConfigurableJoint.zDrive = Joint;
    }
    public virtual void DisablePhysic()
    {
        foreach (var Col in _Colliders) Col.enabled = false;
        _Rgbd.isKinematic = true;
        isAvalible = false;
    }
    public virtual void EnablePhysic()
    {
        foreach (var Col in _Colliders) Col.enabled = true;
        _Rgbd.isKinematic = false;
        isAvalible = true;
    }
    public virtual void EnableColliderWithoutPhysic()
    {
        foreach (var Col in _Colliders) Col.enabled = true;
        gameObject.layer = 0;
    }
}
