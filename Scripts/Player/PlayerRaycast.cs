using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] private float _MaxRayDistance;
    public float maxRayDistance
    {
        get { return _MaxRayDistance; }
        set { _MaxRayDistance = value; }
    }
    
    private RaycastHit _Hit;
    public RaycastHit hit
    {
        get { return _Hit; }
        private set { _Hit = value; }
    }
    private void OnEnable() => PlayerManager.viewRaycast = this;
    private void OnDisable() => PlayerManager.viewRaycast = null;
    void Update()
    {
        Ray CheckObjRay = new Ray(transform.position,transform.forward);
        Physics.Raycast(CheckObjRay,out _Hit, _MaxRayDistance,Physics.DefaultRaycastLayers,QueryTriggerInteraction.Ignore);
    }
}