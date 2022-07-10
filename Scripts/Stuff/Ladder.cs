using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private float _StepOffset = 0.5f;
    private float _OriginalStepOffset = 0.1f;
    private CharacterController _CharCon;
    private void Start() 
    {
        _OriginalStepOffset = PlayerManager.characterController.stepOffset;
    }
    private void OnTriggerStay(Collider other) 
    {
        if(other.TryGetComponent(out _CharCon))
        {
            _CharCon.stepOffset = _StepOffset;
        } 
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.TryGetComponent(out _CharCon))
        _CharCon.stepOffset = _OriginalStepOffset;
    }
}
