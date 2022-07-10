using System.Collections;
using UnityEngine;

public abstract class UseableItem : DragableObject
{
    [SerializeField] protected Transform _CollisionCheckTransform;
    [SerializeField] protected LayerMask _ObjectsCollisionLayer;
    [SerializeField] protected LayerMask _PlayerCollisionLayer;
    [SerializeField] private Vector3 _OverlapBoxSize;
    [SerializeField] private bool _DrawOverlapBox;
    [SerializeField] protected bool _SafePhysicEnabled = true;
    protected bool _ReachPos = false, _ReachRot = false, _ReachTarget = false;
    public bool reachTarget
    {
        get { return _ReachTarget; }
    }
    private void OnDrawGizmos()
    {
        if(_DrawOverlapBox) Gizmos.DrawCube(_CollisionCheckTransform.position,_OverlapBoxSize.MultiplyBy(2f));
    }
    private void OnEnable()
    {
        UniLib.objectMoved += ObjReachPos;
        UniLib.objectRotated += ObjReachRot;
    }
    private void OnDisable() 
    {
        UniLib.objectMoved -= ObjReachPos;
        UniLib.objectRotated -= ObjReachRot;
    }
    private void ObjReachRot(int ObjectId)
    {
        if(ObjectId == gameObject.GetInstanceID())
        {
            _ReachRot = true;
            CheckTargetPosition();
        }
    }
    private void ObjReachPos(int ObjectId)
    {
        if(ObjectId == gameObject.GetInstanceID())
        {
            _ReachPos = true;
            CheckTargetPosition();
        }
    }
    private void CheckTargetPosition()
    {
        if(_ReachPos && _ReachRot) OnTargetPosition();
    }
    protected virtual void OnTargetPosition()
    {
        _ReachTarget = true;
        _ReachPos = false;
        _ReachRot = false;
    }
    public override void EnablePhysic()
    {
        if(_SafePhysicEnabled) StartCoroutine(TryEnablePhysic());
        else base.EnablePhysic();
    }
    private IEnumerator TryEnablePhysic()
    {
        bool PhysicIsDisable = true;
        while(PhysicIsDisable)
        {
            yield return new WaitForFixedUpdate();
            if(CheckOccurrencePossibility(_ObjectsCollisionLayer) 
            && CheckOccurrencePossibility(_PlayerCollisionLayer))
            {
                base.EnablePhysic();
                _ReachTarget = false;
                yield break;
            }
        }
    }
    public override void EnableColliderWithoutPhysic()
    {
        base.EnableColliderWithoutPhysic();
        _ReachTarget = false;
    }
    private bool CheckOccurrencePossibility(LayerMask Layer)
    {
        if(Physics.OverlapBox(_CollisionCheckTransform.position, _OverlapBoxSize, transform.rotation, Layer).Length == 0)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

}
