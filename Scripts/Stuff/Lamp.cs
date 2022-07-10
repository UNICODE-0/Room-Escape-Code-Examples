using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Lamp : MonoBehaviour
{
    [SerializeField] private GameObject _LightSource;
    [SerializeField] private float _GlassTransparency = 0.2f;

    private MeshRenderer _MeshRnd;
    void Start()
    {
        _MeshRnd = GetComponent<MeshRenderer>();
    }

    public void DisabLight()
    {
        _MeshRnd.material.DisableKeyword("_EMISSION");
        UniLib.ChangeMaterialColor(_MeshRnd,new Color(1f,1f,1f, _GlassTransparency));
        _LightSource.SetActive(false);
    }
    public void EnableLight()
    {
        _MeshRnd.material.EnableKeyword("_EMISSION");
        UniLib.ChangeMaterialColor(_MeshRnd,new Color(1f,1f,1f,1f));
        _LightSource.SetActive(true);
    }
}
