using UnityEngine;

public class CipherLockedIronBox : IronBox
{
    [SerializeField] private CipherLock _CipherLock;
    private void OnEnable() => CipherLock.unclocked += Unlock;
    private void OnDisable() => CipherLock.unclocked -= Unlock;

    private void Unlock(int LockId)
    {
        if(_CipherLock.gameObject.GetInstanceID() == LockId)
        {
            _IsAvalible = true;
        }
    }
}
