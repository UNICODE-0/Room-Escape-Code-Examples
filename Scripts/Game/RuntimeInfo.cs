using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeInfo : MonoBehaviour
{
    private RaycastHit _Hit;

    static private InteractionObjects _InteractionObjects = new InteractionObjects();
    static public InteractionObjects interactionObjects
    {
        get { return _InteractionObjects; }
    }
    static private bool _PlayerCanInteract;
    static public bool playerCanInteract
    {
        get { return _PlayerCanInteract; }
    }
    static public float mouseOffsetX { get; private set; }
    static public float mouseOffsetY { get; private set; }
    
    void Update()
    {
        mouseOffsetX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime;
        mouseOffsetY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime;

        _Hit = PlayerManager.viewRaycast.hit;
        Collider collider = _Hit.collider;
        if(collider == null)
        {
            _PlayerCanInteract = false;
            return;
        } 

        DragableObject dragableObject = collider.GetComponent<DragableObject>();
        InteractionObject interactionObject = collider.GetComponent<InteractionObject>();
        SlidingObject slidingObject = collider.GetComponent<SlidingObject>();
        IInteractive interactive = collider.GetComponent<IInteractive>();

        bool AnyObjectExist = (dragableObject != null && dragableObject.isAvalible) || 
        (interactionObject != null && interactionObject.enabled && interactionObject.isAvalible) || 
        interactive != null || (slidingObject != null && slidingObject.isAvalible) // if you add new type, also add it to PlayerMovement(GroundCheck function)
        , DontStandingOnThis = true;

        if(PlayerManager.movement.onItem && AnyObjectExist) 
        DontStandingOnThis = _Hit.collider.gameObject.GetInstanceID() != PlayerManager.movement.standingObject.GetInstanceID();

        if(AnyObjectExist && DontStandingOnThis)
        {
            _PlayerCanInteract = true;
            _InteractionObjects.throwableObject = _Hit.collider.GetComponent<ThrowableObject>();
            _InteractionObjects.dragableObject = dragableObject;            
            _InteractionObjects.interactionObject = interactionObject;      
            _InteractionObjects.interactive = interactive;
            _InteractionObjects.slidingObject = slidingObject;
        } else _PlayerCanInteract = false;
    }   
}

public class InteractionObjects 
{
    public DragableObject dragableObject { get; set; }
    public ThrowableObject throwableObject { get; set; }

    public InteractionObject interactionObject { get; set; }

    public IInteractive interactive { get; set; }

    public SlidingObject slidingObject { get; set; }
}