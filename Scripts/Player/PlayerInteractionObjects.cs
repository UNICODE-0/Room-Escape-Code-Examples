using UnityEngine;

public class PlayerInteractionObjects : MonoBehaviour
{
    private int _DragButton;
    private RaycastHit _Hit;
    private IIinteraction _Iinteraction;
    public bool onIinteract { get; private set; }
    private void OnEnable() => PlayerManager.interactionObjects = this;
    private void OnDisable() => PlayerManager.interactionObjects = null;
    void Start()
    {
        _DragButton = Controls.PlayerControls["Use"];
    }

    void Update()
    {
        if(!PlayerManager.playerExist) return;

        _Hit = PlayerManager.viewRaycast.hit;

        if(RuntimeInfo.playerCanInteract && !onIinteract)
        {
            InteractionObjects interactionObjects = RuntimeInfo.interactionObjects;
            _Iinteraction = interactionObjects.interactionObject;
            if(_Iinteraction == null) return;

            if(Input.GetMouseButtonDown(_DragButton))
            {
                onIinteract = true;
                _Iinteraction.InteractStart();
            }
        } 
        else
        {
            if(Input.GetMouseButton(_DragButton) && onIinteract)
            {
                onIinteract = _Iinteraction.OnInteract();
            }
            if(Input.GetMouseButtonUp(_DragButton) && onIinteract)
            {
                onIinteract = false;
                _Iinteraction.InteractEnd();
            }
        }

    }
}
