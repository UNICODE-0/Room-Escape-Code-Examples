using UnityEngine;

public class Picture : MonoBehaviour
{
    [Range(0,0.95f)]
    [SerializeField] private float _Threshold = 0.3f;
    private Material _material;
    private Transform  _PlayerTransform;
    void Start()
    {
        _material = GetComponent<Renderer>().material;
        _PlayerTransform = PlayerManager.playerCamera.viewCamera.transform;
    }

    private void Update() 
    {
        Vector3 PictureUpVector = transform.up;
        Vector3 PictureToPlayerVec = (_PlayerTransform.position - transform.position).normalized;
        float AngleToPlayer = Mathf.Abs(Vector3.Dot(PictureUpVector,PictureToPlayerVec)); 
        _material.SetFloat("_FrontPictureVisibility", AngleToPlayer > _Threshold ? AngleToPlayer - _Threshold : 0);
    }
    
}
