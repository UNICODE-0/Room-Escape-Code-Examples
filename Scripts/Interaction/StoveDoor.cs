using UnityEngine;

public class StoveDoor : InteractionObject
{
    private float _YAngVelocity;
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
            transform.RotateAround(transform.position,transform.up, -_YAngVelocity  * Time.deltaTime);
        } 
        if((Impulse.x > MaxImpulse || Impulse.y > MaxImpulse || Impulse.z > MaxImpulse ) && _OnInteract)
        {
            _Collision = true;
            transform.RotateAround(transform.position,transform.up, -_YAngVelocity  * Time.deltaTime);
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
        
        float YAngle = transform.rotation.y;
        bool MoveLeftAndWithinLim = YAngle < _MaxAngle && RuntimeInfo.mouseOffsetX > 0,
        MoveRightAndWithinLim = YAngle > _MinAngle && RuntimeInfo.mouseOffsetX < 0;
        if(MoveLeftAndWithinLim || MoveRightAndWithinLim)
        {
            int MouseDir = RuntimeInfo.mouseOffsetX > 0 ? 1 : -1;
            _YAngVelocity = MouseDir * _OpenSpeed;
            transform.RotateAround(transform.position,transform.up,_YAngVelocity  * Time.deltaTime);
        }
        return base.OnInteract();
    }
    public override void InteractEnd()
    {
        _OnInteract = false;
        PlayerManager.playerCamera.RotationPossibility = true;
    }
}
