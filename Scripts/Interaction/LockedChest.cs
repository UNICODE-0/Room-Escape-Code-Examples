using UnityEngine;

public class LockedChest : Chest
{
    [SerializeField] private GameObject _LocObj;

    private void OnEnable()
    {
        CodeLock.unclocked += EnableAcces;
    } 
    private void OnDisable() => CodeLock.unclocked -= EnableAcces;

    private void EnableAcces(int LockID)
    {
        if(_LocObj.GetInstanceID() == LockID)
        {
            _IsAvalible = true;
            _Rigbd.isKinematic = false;
        } 
    }

    new private void Start() 
    {
        _Hgjnt = GetComponent<HingeJoint>();
        _Rigbd = GetComponent<Rigidbody>();
        _JntLimits = _Hgjnt.limits;
        _IsAvalible = false;
        _Rigbd.isKinematic = true;
    }
}
