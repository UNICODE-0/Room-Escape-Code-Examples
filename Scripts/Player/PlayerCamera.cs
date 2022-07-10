using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera _Playercamera;
    public Camera viewCamera
    {
        get { return _Playercamera; }
    }
    [SerializeField] private GameObject _Body;
    private float _Sensitivity = 100f,
    _MinimumVert = -80.0f,
    _MaximumVert = 80.0f,
    _xRotation = 0f;

    public bool RotationPossibility { get; set; } = true;
    private void OnEnable() => PlayerManager.playerCamera = this;
    private void OnDisable() => PlayerManager.playerCamera = null;
    void FixedUpdate()
    { 
        if(RotationPossibility)
        {
            float MouseX = RuntimeInfo.mouseOffsetX * _Sensitivity;
            float MouseY = RuntimeInfo.mouseOffsetY * _Sensitivity;

            _xRotation -= MouseY;
            _xRotation = Mathf.Clamp(_xRotation,_MinimumVert,_MaximumVert);

            transform.localRotation = Quaternion.Euler(_xRotation,0f,0f);
            _Body.transform.Rotate(_Body.transform.up * MouseX);
        }
    }


}
