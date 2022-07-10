using UnityEngine;

public class IronBox : InteractionObject
{
    private float _XAngVelocity;
    private bool _OnInteract = false;
    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.layer == 8) _Collision = true;
    }
    private void OnCollisionStay(Collision other) 
    {  
        float MaxImpulse = 5;
        Vector3 Impulse = other.impulse;
        if((Impulse.x < -MaxImpulse || Impulse.y < -MaxImpulse || Impulse.z < -MaxImpulse) && _OnInteract)
        {
            _Collision = true;
            transform.RotateAround(transform.position,transform.right, -_XAngVelocity  * Time.deltaTime);
        } 
        if((Impulse.x > MaxImpulse || Impulse.y > MaxImpulse || Impulse.z > MaxImpulse ) && _OnInteract)
        {
            _Collision = true;
            transform.RotateAround(transform.position,transform.right, -_XAngVelocity  * Time.deltaTime);
        } 
    }
    public override void InteractStart()
    {
        PlayerManager.playerCamera.RotationPossibility = false;
        _Collision = false;
        _OnInteract = true;
        base.InteractStart();
    }
    public override bool OnInteract()
    {
        if(_Collision)
        {
            InteractEnd();
            return false;
        }
        
        float XAngle = transform.rotation.x;
        bool MoveLeftAndWithinLim = XAngle < _MaxAngle && RuntimeInfo.mouseOffsetY > 0,
        MoveRightAndWithinLim = XAngle > _MinAngle && RuntimeInfo.mouseOffsetY < 0;
        if(MoveLeftAndWithinLim || MoveRightAndWithinLim)
        {
            int MouseDir = RuntimeInfo.mouseOffsetY > 0 ? 1 : -1;
            _XAngVelocity = MouseDir * _OpenSpeed;
            transform.RotateAround(transform.position,transform.right,_XAngVelocity  * Time.deltaTime);
        }
        return base.OnInteract();
    }
    public override void InteractEnd()
    {
        _OnInteract = false;
        PlayerManager.playerCamera.RotationPossibility = true;
    }
}
