using UnityEngine;

public class Casket : DragableObject
{
    [SerializeField] private Transform _CubeTf;
    [SerializeField] private Collider _CubeCol;
    [SerializeField] private Transform _CylinderTf;
    [SerializeField] private Collider _CylinderCol;
    [SerializeField] private Transform _SphereTf;
    [SerializeField] private Collider _SphereCol;
    [SerializeField] private DragableObject _Cover, _Body;
    [SerializeField] private DragableObject[] _ObjectsInside; 
    
    private CasketFigure _CasketFigure;
    private bool _IsOpen = false;
    private bool _SphereFind = false, _CubeFind = false, _CylinderFind = false;
    private bool _Blocked = false;
    private void OnTriggerEnter(Collider other) 
    {
        if(_IsOpen == false)
        {
            if(_Rgbd.velocity != Vector3.zero || _Blocked || _IsAvalible == false) return;

            if(other.gameObject.TryGetComponent(out _CasketFigure))
            {
                _Rgbd.isKinematic = true;
                _IsAvalible = false;

                switch (_CasketFigure.casketFigureType)
                {
                    case CasketFigureType.Cube:
                    if(_CubeFind) return;
                    _CubeFind = true;
                    _CubeCol.enabled = true;

                    SetFigure(_CasketFigure,_CubeTf);
                    break;

                    case CasketFigureType.Sphere:
                    if(_SphereFind) return;
                    _SphereFind = true;
                    _SphereCol.enabled = true;

                    SetFigure(_CasketFigure,_SphereTf);
                    break;

                    case CasketFigureType.Cylinder:
                    if(_CylinderFind) return;
                    _CylinderFind = true;
                    _CylinderCol.enabled = true;

                    SetFigure(_CasketFigure,_CylinderTf);
                    break;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other) 
    {
        if(other.GetComponent<CasketFigure>() == null) _Blocked = true;
    }
    private void OnTriggerExit(Collider other) 
    {
        _Blocked = false;
    }
    private void SetFigure(CasketFigure Figure, Transform TargetTf)
    {
        Figure.DisablePhysic();
        Figure.transform.SetParent(_Cover.transform);

        UniLib.MoveObjToTargetPos(this,Figure.transform,TargetTf.position, UseEvent: true);
        UniLib.RotateObjToTargetRot(this,Figure.transform,TargetTf.rotation, UseEvent: true);
    }
    private void Start() 
    {
        _Body.DisablePhysic();
        _Cover.DisablePhysic();
        foreach (var ObjectInside in _ObjectsInside)
        {
            ObjectInside.DisablePhysic();
        }
    }
    private void Update() 
    {
        if(_CasketFigure != null)
        {
            if(_CasketFigure.reachTarget)
            {   
                _IsAvalible = true;
                _Rgbd.isKinematic = false;
                
                if(_CubeFind && _CylinderFind && _SphereFind && _IsOpen == false)
                {
                    Open();
                }
            } 
        }
    }
    private void Open()
    {
        _AudioSource.Play();
        DisablePhysic();
        _Cover.EnablePhysic();
        _Body.EnablePhysic();
        foreach (var ObjectInside in _ObjectsInside)
        {
            ObjectInside.EnablePhysic();
        }

        _IsOpen = true;
        _CasketFigure = null;
    }
}
