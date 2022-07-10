using UnityEngine;
public interface ISliding 
{
    void SlideStart();
    bool OnSlide(); 
    void SlideEnd(); 
}
[RequireComponent(typeof(Rigidbody))]
public class SlidingObject : MonoBehaviour, ISliding
{
    [SerializeField] private float _OpenSpeed;
    [SerializeField] private bool _IsAvalible = true;
    [SerializeField] private SlidingSide _SlidingSide;
    [SerializeField] private Vector3 _MinPosition, _MaxPosition;

    public Vector3 minPosition
    {
        get { return _MinPosition; }
        set { _MinPosition = value; }
    }
    public Vector3 maxPosition
    {
        get { return _MaxPosition; }
        set { _MaxPosition = value; }
    }
    public bool isAvalible
    {
        get { return _IsAvalible; }
    }

    private Vector3 _SlidingDirection;
    private Vector3 _Velocity;
    private Rigidbody _Rgbd; 
    private bool _Collision = false;
    private bool _OnSlide = false;
    private Vector3 _HitPoint;

    enum SlidingSide
    {
        Forward,
        Right,
        Up
    }
    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.layer == 8) _Collision = true;
    }
    private void OnCollisionStay(Collision other) 
    {  
        float MaxImpulse = 5;
        Vector3 Impulse = other.impulse;
        if((Impulse.x < -MaxImpulse || Impulse.y < -MaxImpulse || Impulse.z < -MaxImpulse) && _OnSlide)
        {
            _Collision = true;
            transform.Translate( _SlidingDirection.Abs().MultiplyBy(-_Velocity) * Time.deltaTime);
        }
        if((Impulse.x > MaxImpulse || Impulse.y > MaxImpulse || Impulse.z > MaxImpulse) && _OnSlide)
        {
            _Collision = true;
            transform.Translate( _SlidingDirection.Abs().MultiplyBy(-_Velocity) * Time.deltaTime);
        }
    }
    private void Start() 
    {
        _Rgbd = GetComponent<Rigidbody>();
        _Rgbd.isKinematic = true;

        switch (_SlidingSide)
        {
            case SlidingSide.Forward:
            _SlidingDirection = transform.forward;
            break;
            case SlidingSide.Right:
            _SlidingDirection = transform.right;
            break;
            case SlidingSide.Up:
            _SlidingDirection = transform.up;
            break;
        }
    }
    public void SlideStart()
    {
        _OnSlide = true;
        _Collision = false;
        PlayerManager.playerCamera.RotationPossibility = false;
        _HitPoint = PlayerManager.viewRaycast.hit.point;
    }
    public bool OnSlide()
    {
        if(_Collision)
        {
            SlideEnd();
            return false;
        }

        float DistanceBetweenPlayerAndObject = (PlayerManager.playerCamera.transform.position - _HitPoint).magnitude;
        if( DistanceBetweenPlayerAndObject > PlayerManager.viewRaycast.maxRayDistance)
        {
            SlideEnd();
            return false;
        }

        if(RuntimeInfo.mouseOffsetY != 0)
        {
            Vector3 CurrentPos = transform.position;
            bool MoveForwardAndWithinLim = UniLib.CompareVectors( CurrentPos , _MinPosition, UniLib.СomparisonMethod.Bigger) && RuntimeInfo.mouseOffsetY > 0,
            MoveBackwardAndWithinLim = UniLib.CompareVectors( CurrentPos , _MaxPosition, UniLib.СomparisonMethod.Smaller) && RuntimeInfo.mouseOffsetY < 0;
            if(MoveForwardAndWithinLim || MoveBackwardAndWithinLim)
            {
                int MouseDir = RuntimeInfo.mouseOffsetY > 0 ? 1 : -1;
                _Velocity = _SlidingDirection * MouseDir;
                transform.Translate(_Velocity * Time.deltaTime);
            }
        }

        return true;
    }

    public void SlideEnd()
    {
        _OnSlide = false;
        PlayerManager.playerCamera.RotationPossibility = true;
    }
}
