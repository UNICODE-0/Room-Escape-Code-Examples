using UnityEngine;

public class CasketPart : DragableObject
{
    [SerializeField] private bool _EnableImpulse = false;
    [SerializeField] private float _ImpulsePower = 1f;
    public override void EnablePhysic()
    {
        base.EnablePhysic();
        if(_EnableImpulse)
        {
            Vector3 ImpulseVector = transform.up + transform.right;
            _Rgbd.AddForce( ImpulseVector * _ImpulsePower,ForceMode.Impulse);
        }
    }
}
