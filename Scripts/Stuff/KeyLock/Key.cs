using System.Collections;
using UnityEngine;

public class Key : UseableItem
{
    [SerializeField] private float _RotationSpeed = 0.05f;
    [SerializeField] private float _RotationDuration = 1f;
    [SerializeField] private float _PauseBetweenRotations = 1.1f;
    [SerializeField] private float _PauseAfterRotations = 1f;
    [SerializeField] private float _OffsetAfterRotations = 0.3f;

    public void UnlockKeyLock(KeyLock keyLock)
    {
        StartCoroutine(_UnlockKeyLock(keyLock));
    }
    private IEnumerator _UnlockKeyLock(KeyLock keyLock)
    {
        UniLib.RotateObjToTargetRot(this, transform,UniLib.AddRotation(transform, transform.forward,180f), _RotationDuration, _RotationSpeed);

        yield return new WaitForSeconds(_PauseBetweenRotations);

        UniLib.RotateObjToTargetRot(this, transform,UniLib.AddRotation(transform, transform.forward,180f), _RotationDuration, _RotationSpeed);

        yield return new WaitForSeconds(_PauseBetweenRotations);

        UniLib.MoveObjToTargetPos(this, transform, transform.position + keyLock.transform.forward * _OffsetAfterRotations);

        yield return new WaitForSeconds(_PauseAfterRotations);

        keyLock.Open();
        
        _SafePhysicEnabled = true;
        EnablePhysic();
    }
}
