using UnityEngine;

public class Door : InteractionObject
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<PlayerMovement>() != null && isAvalible)
        {
            _Rigbd.isKinematic = true;
            _Collision = true;
        }  
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.GetComponent<PlayerMovement>() != null && isAvalible)
        {
            _Rigbd.isKinematic = false;
            _Collision = false;
        } 
    }
    public override void InteractStart()
    {
        _Rigbd.velocity = Vector3.zero;
        PlayerManager.playerCamera.RotationPossibility = false;
        _Collision = false;
        _Rigbd.useGravity = false;
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
            Quaternion RotAxis = Quaternion.Euler(0,MouseDir * _OpenSpeed  * Time.deltaTime,0);
            _Rigbd.MoveRotation(RotAxis * _Rigbd.rotation);
        }
        return base.OnInteract();
    }
    public override void InteractEnd()
    {
        PlayerManager.playerCamera.RotationPossibility = true;
        _Rigbd.useGravity = true;
    }
}
