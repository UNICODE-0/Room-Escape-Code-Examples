using UnityEngine;

public class KeyLockedDoor : Door
{
    [SerializeField] private GameObject _LocObj;

    private void OnEnable() => KeyLock.unclocked += EnableAcces;
    private void OnDisable() => KeyLock.unclocked -= EnableAcces;

    private void EnableAcces(int LockID)
    {
        if(_LocObj.GetInstanceID() == LockID) _IsAvalible = true;
    }

    private void Awake() 
    {
        _IsAvalible = false;
    }
}
