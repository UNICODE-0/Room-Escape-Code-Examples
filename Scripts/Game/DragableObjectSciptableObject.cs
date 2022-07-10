using UnityEngine;

[CreateAssetMenu(fileName = "DragableObject", menuName = "ScriptableObjects/DragableObject", order = 1)]
public class DragableObjectSciptableObject : ScriptableObject
{
    [HeaderAttribute("Drag settings")]
    [Space]
    [SerializeField] private float _Spring = 3000f;
    public float spring
    {
        get { return _Spring; }
        set { _Spring = value; }
    }
    
    [SerializeField] private float _Dumper = 500f;
    public float dumper
    {
        get { return _Dumper; }
        set { _Dumper = value; }
    }
    [SerializeField] private float _MaxForce = 100f;
    public float maxForce
    {
        get { return _MaxForce; }
        set { _MaxForce = value; }
    }
    [SerializeField] private float _LinerLimit = 0.1f;
    public float linerLimit
    {
        get { return _LinerLimit; }
        set { _LinerLimit = value; }
    }
    [SerializeField] private float _MaxDeviationX = 0.14f, _MaxDeviationY = 0.14f, _MaxDeviationZ = 0.14f;
    public float maxDeviationX
    {
        get { return _MaxDeviationX; }
        set { _MaxDeviationX = value; }
    }
    public float maxDeviationY
    {
        get { return _MaxDeviationY; }
        set { _MaxDeviationY = value; }
    }
    public float maxDeviationZ
    {
        get { return _MaxDeviationZ; }
        set { _MaxDeviationZ = value; }
    }
    
    [HeaderAttribute("Collision settings")]
    [Space]
    [SerializeField] private float _MaxImpulse = 200f;
    public float maxImpulse
    {
        get { return _MaxImpulse; }
        set { _MaxImpulse = value; }
    }
    [SerializeField] private float _MaxImpulseWithDragable = 35f;
    public float maxImpulseWithDragable
    {
        get { return _MaxImpulseWithDragable; }
        set { _MaxImpulseWithDragable = value; }
    }
    [SerializeField] private float _MaxImpulseWithInteraction = 5f;
    public float maxImpulseWithInteraction
    {
        get { return _MaxImpulseWithInteraction; }
        set { _MaxImpulseWithInteraction = value; }
    }
}
