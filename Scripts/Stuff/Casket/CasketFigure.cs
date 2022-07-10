using UnityEngine;

public enum CasketFigureType
{
    Cube,
    Cylinder,
    Sphere
}
public class CasketFigure : UseableItem
{
    [SerializeField] private CasketFigureType _CasketFigureType;
    public CasketFigureType casketFigureType
    {
        get { return _CasketFigureType; }
        set { _CasketFigureType = value; }
    }
    protected override void OnTargetPosition()
    {
        base.OnTargetPosition();
    }
}
