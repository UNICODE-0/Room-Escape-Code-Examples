using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private int _FrameRate = 144;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = _FrameRate;
    }
}
