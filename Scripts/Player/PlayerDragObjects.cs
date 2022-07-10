using UnityEngine;

public class PlayerDragObjects : MonoBehaviour
{
    [SerializeField] private Transform _HandTf;

    private int _DragButton, _ThrowButton;
    private Ray _Ray;
    private IDragable _DragableObject;
    private IThrowable _ThrowableObject;
    private RaycastHit _Hit;

    public bool onDrag { get; private set; } = false;

    private void OnEnable() => PlayerManager.dragObjects = this;
    private void OnDisable() => PlayerManager.dragObjects = null;
    private void Start() 
    {
        _DragButton = Controls.PlayerControls["Use"];
        _ThrowButton = Controls.PlayerControls["Throw"];
    }
    void Update()
    {
        if(!PlayerManager.playerExist) return;

        _Hit = PlayerManager.viewRaycast.hit;

        if(RuntimeInfo.playerCanInteract && !onDrag)
        {
            InteractionObjects interactionObjects = RuntimeInfo.interactionObjects;
            _DragableObject = interactionObjects.dragableObject;
            if(_DragableObject == null) return;

            _ThrowableObject = interactionObjects.throwableObject;

            if(Input.GetMouseButtonDown(_DragButton))
            {
                Collider HitCol = _Hit.collider;
                if(HitCol == null) return; // if all are broken, maybe del this

                onDrag = true;
                _HandTf.position = HitCol.transform.position;
                _DragableObject.DragStart(_HandTf.GetComponent<Rigidbody>());
            }
        } 
        else
        {
            if(Input.GetMouseButton(_DragButton) && onDrag)
            {
                // if(RuntimeInfo.mouseOffsetX > 0.05f || RuntimeInfo.mouseOffsetY > 0.05f)
                // {
                //     _DragableObject.DragEnd();
                //     onDrag = false;
                //     return;
                // }
                
                if(Input.GetMouseButton(_ThrowButton)) 
                {
                    if(_ThrowableObject == null) return;
                    _ThrowableObject.Throw(_HandTf.forward);
                    _DragableObject.DragEnd();
                    onDrag = false;
                    return;
                }
                onDrag = _DragableObject.OnDrag(_HandTf.position);
            }
            if(Input.GetMouseButtonUp(_DragButton) && onDrag)
            {
                onDrag = false;
                _DragableObject.DragEnd();
            }
        }

    }
}
