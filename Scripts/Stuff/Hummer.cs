using UnityEngine;

public class Hummer : DragableObject
{
    [SerializeField] private Transform _CenterOfMass;

    void Start()
    {
        _Rgbd.centerOfMass = _CenterOfMass.localPosition;
    }
}
