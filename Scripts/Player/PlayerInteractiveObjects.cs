using UnityEngine;

public class PlayerInteractiveObjects : MonoBehaviour
{
    private int _PressButton;
    private IInteractive _Interactive;
    void Start()
    {
        _PressButton = Controls.PlayerControls["Use"];
    }
    void Update()
    {
        if(RuntimeInfo.playerCanInteract)
        {
            InteractionObjects interactionObjects = RuntimeInfo.interactionObjects;
            _Interactive = interactionObjects.interactive;
            if(_Interactive == null) return;

            if(Input.GetMouseButtonDown(_PressButton))
            {
                _Interactive.Use();
            }
        } 
    }
}
