using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _Animator;
    public Animator animator
    {
        get { return _Animator; }
        set { _Animator = value; }
    }
    private void OnEnable() 
    {
        PlayerManager.animator = this;
    }
    private void OnDisable() 
    {
        PlayerManager.animator = null;
    }
    private void Awake() 
    {
        _Animator = GetComponent<Animator>();
    }
    void Start()
    {
        PlayerManager.playerCamera.RotationPossibility = false;
        PlayerManager.movement.movementPossibility = false;
    }
    private void CallAnimationEnd(string AnimationName)
    {
        switch (AnimationName)
        {
            case "WakeUp":
                PlayerManager.playerCamera.RotationPossibility = true;
                PlayerManager.movement.movementPossibility = true;
            break;
            default:
                Debug.LogWarning("Unknown animation");
            break;
        }
    }
}
