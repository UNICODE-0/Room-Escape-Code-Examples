using UnityEngine;

public class Crowbar : UseableItem
{
    [SerializeField] private float _AddAngle = 5f;
    [SerializeField] private float _RotationSpeed = 1f;
    private bool _IsUsed = false;
    public bool isUsed
    {
        get { return _IsUsed; }
        set { _IsUsed = value; }
    }
    
    protected override void OnTargetPosition()
    {
        base.OnTargetPosition();
        if(_IsUsed) EnablePhysic();
        _IsUsed = true;
    }
    public void PlayAnimation()
    {
        UniLib.RotateObjToTargetRot(this,transform,UniLib.AddRotation(transform,Vector3.right, _AddAngle),RotationSpeed: _RotationSpeed);
    }
}
