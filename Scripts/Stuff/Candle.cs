using UnityEngine;

public class Candle : UseableItem
{
    [SerializeField] private Transform _CenterOfMass;
    public bool isUsed { get; private set; }

    protected override void OnTargetPosition()
    {
        base.OnTargetPosition();
        if(isUsed) EnablePhysic();
    }
    public void Use()
    {
        isUsed = true;
        _ReachTarget = false;
    }
    void Start()
    {
        _Rgbd.centerOfMass = _CenterOfMass.localPosition;
    }
}
