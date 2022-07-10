using UnityEngine;

public interface IIinteraction 
{
    void InteractStart();
    bool OnInteract(); 
    void InteractEnd(); 
}

public enum Axis
{
    X,
    Y,
    Z
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public abstract class InteractionObject : MonoBehaviour, IIinteraction
{
    [SerializeField] private float _MaxVelocity = 0.4f;
    [SerializeField] protected bool _IsAvalible = true;
    [SerializeField] protected float _MaxAngle = 1, _MinAngle = 0;
    [SerializeField] private Axis _AngleAxis;
    [SerializeField] protected float _OpenSpeed;
    public Axis angleAxis
    {
        get { return _AngleAxis; }
    }
    public bool isAvalible
    {
        get { return _IsAvalible; }
    }
    public float maxAngle
    {
        get { return _MaxAngle; }
        set { _MaxAngle = value; }
    }
    public float minAngle
    {
        get { return _MinAngle; }
        set { _MinAngle = value; }
    }
    protected Rigidbody _Rigbd;
    protected HingeJoint _Hgjnt;
    protected JointLimits _JntLimits;
    protected bool _Collision = false;
    private Vector3 _HitPoint;

    private void OnCollisionStay(Collision other)
    {
        if(_Rigbd.isKinematic == false)
        {
            Vector3 RgbdVel = _Rigbd.velocity;
            if(RgbdVel.x < -_MaxVelocity || RgbdVel.y < -_MaxVelocity || RgbdVel.z < -_MaxVelocity) _Collision = true;
            if(RgbdVel.x > _MaxVelocity || RgbdVel.y > _MaxVelocity || RgbdVel.z > _MaxVelocity) _Collision = true;
            if(other.gameObject.GetComponent<DragableObject>() != null) _Collision = true;
        }
    }

    protected void Start() 
    {
        _Hgjnt = GetComponent<HingeJoint>();
        _Rigbd = GetComponent<Rigidbody>();
        _JntLimits = _Hgjnt.limits;
    }
    
    public virtual void InteractStart()
    {
        _HitPoint = PlayerManager.viewRaycast.hit.point;
    }
    public virtual bool OnInteract()
    {
        float DistanceBetweenPlayerAndObject = (PlayerManager.playerCamera.transform.position - _HitPoint).magnitude;
        if( DistanceBetweenPlayerAndObject > PlayerManager.viewRaycast.maxRayDistance)
        {
            InteractEnd();  
            return false;
        }
        return true;
    }
    public virtual void InteractEnd()
    {
        throw new System.NotImplementedException();
    }

}
