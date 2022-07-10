using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Chest : InteractionObject
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
        PlayerManager.playerCamera.RotationPossibility = false;
        _Rigbd.useGravity = false;
        _Collision = false;
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
        bool MoveUpAndWithinLim = XAngle < _MaxAngle && RuntimeInfo.mouseOffsetY > 0,
        MoveDownAndWithinLim = XAngle > _MinAngle && RuntimeInfo.mouseOffsetY < 0;
        if(MoveUpAndWithinLim || MoveDownAndWithinLim)
        {
            int MouseDir = RuntimeInfo.mouseOffsetY > 0 ? -1 : 1;
            Quaternion RotAxis = Quaternion.Euler(MouseDir * _OpenSpeed  * Time.deltaTime,0,0);
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
