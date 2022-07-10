using UnityEngine;

public class BottleWithObject : Bottle
{
    [SerializeField] private DragableObject _ObjectInside;
    private void Start() 
    {
        _ObjectInside.DisablePhysic();
    }
    protected override void Open()
    {
        base.Open();
        _ObjectInside.EnablePhysic();
    }
}
