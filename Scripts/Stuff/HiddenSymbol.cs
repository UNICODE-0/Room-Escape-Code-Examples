using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HiddenSymbol : MonoBehaviour
{
    [SerializeField] private GameObject _LightSource;
    [SerializeField] private TextMeshPro _Text;
    [SerializeField] private float _Threshold = 2f;
    private float _StartTextAlpha;
    private MeshRenderer _MeshRnd;
    private void Start() 
    {
        _MeshRnd = GetComponent<MeshRenderer>();
        _StartTextAlpha = _Text.color.a;
    }
    private void Update() 
    {
        float DistanceToLight = (_LightSource.transform.position - transform.position).magnitude;
        Color MatCol = _MeshRnd.material.color;
        float ColAlpha = DistanceToLight < _Threshold ? (_Threshold - DistanceToLight) / _Threshold : 0f; 
        MatCol = new Color(MatCol.r,MatCol.g,MatCol.b,ColAlpha);
        _Text.color = new Color(_Text.color.r,_Text.color.g,_Text.color.b,ColAlpha * _StartTextAlpha);
        UniLib.ChangeMaterialColor(_MeshRnd,MatCol);
    }
}
