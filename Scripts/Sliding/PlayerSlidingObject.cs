using UnityEngine;

public class PlayerSlidingObject : MonoBehaviour
{
    private int _SlideButton;
    private RaycastHit _Hit;
    private SlidingObject _SlidingObject;
    public bool onSlide { get; private set; }
    private void OnEnable() => PlayerManager.slidingObjects = this;
    private void OnDisable() => PlayerManager.slidingObjects = null;
    void Start()
    {
        _SlideButton = Controls.PlayerControls["Use"];
    }

    void Update()
    {
        if(!PlayerManager.playerExist) return;

        _Hit = PlayerManager.viewRaycast.hit;

        if(RuntimeInfo.playerCanInteract && !onSlide)
        {
            InteractionObjects interactionObjects = RuntimeInfo.interactionObjects;
            _SlidingObject = interactionObjects.slidingObject;
            if(_SlidingObject == null) return;

            if(Input.GetMouseButtonDown(_SlideButton))
            {
                onSlide = true;
                _SlidingObject.SlideStart();
            }
        } 
        else
        {
            if(Input.GetMouseButton(_SlideButton) && onSlide)
            {
                onSlide = _SlidingObject.OnSlide();
            }
            if(Input.GetMouseButtonUp(_SlideButton) && onSlide)
            {
                onSlide = false;
                _SlidingObject.SlideEnd();
            }
        }
    }
}
