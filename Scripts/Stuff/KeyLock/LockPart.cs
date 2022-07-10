using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class LockPart : MonoBehaviour
{
    [SerializeField] private bool _EnableSpreading;
    [SerializeField] private Vector3 _SpreadingDirection;
    [SerializeField] private float _SpreadingForce = 0.45f;

    private Rigidbody _Rgbd;
    private MeshRenderer _MeshRnd;
    private void Start()
    {
        _Rgbd = GetComponent<Rigidbody>();
        _MeshRnd = GetComponent<MeshRenderer>();
    }
    public void EnablePhysic()
    {
        gameObject.transform.parent = null;
        _Rgbd.isKinematic = false;
        if(_EnableSpreading) _Rgbd.AddForce(_SpreadingDirection * _SpreadingForce,ForceMode.Impulse);
        
        if(TryGetComponent(out DragableObject dragableObject))
        {
            dragableObject.enabled = true;
            dragableObject.isAvalible = true;
        } else
        {
            UniLib.DisappearObject(this,_MeshRnd, DisappearanceRate: 0.004f);
        }
        if(TryGetComponent(out ThrowableObject throwableObject))
        {
            throwableObject.enabled = true;
        }
    }
}
