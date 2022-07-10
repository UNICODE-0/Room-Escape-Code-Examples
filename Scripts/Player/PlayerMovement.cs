using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [HeaderAttribute("Definition in space")]
    [Space]
    [SerializeField] private Transform _GroundCheck;
    [SerializeField] private float _GroundCheckRadius;
    [SerializeField] private Transform _СeilingCheck;
    [SerializeField] private float _СeilingCheckRadius;
    [SerializeField] private Transform _BumpCheck;
    [SerializeField] private float _BumpCheckRadius;
    [SerializeField] private LayerMask _Layer;

    [HeaderAttribute("Movement")]
    [Space]
    [SerializeField] private float _MovementSpeed = 5f;
    //[SerializeField] private float _SprintBonus = 2f;
    [SerializeField] private float _JumpStrenght = 1f;
    [SerializeField] private float _Gravity = -10f;

    [HeaderAttribute("Crouching")]
    [Space]
    [SerializeField] private Transform _Head;
    [SerializeField] private float _CrouchHeadYPos = 0.21f;
    [SerializeField] private float _CrouchColliderHeight = 2.025f;
    [SerializeField] private float _CrouchColliderCenter = -0.4f;

    [Range(0.01f,1f)]
    [SerializeField] private float _SquatSpeed = 0.2f;
    [HeaderAttribute("Physics")]
    [Space]
    [SerializeField] private Transform _CapsuleCenter;
    [SerializeField] private float _PhysicCapsuleRadius = 0.6f;
    [Range(0.01f,1f)]
    [SerializeField] private float _SphereRadiusCorrection = 0.32f; //Displacement of the capsule spheres in y by their radius
    [SerializeField] private ForceMode TypeOfImpact;

    private Vector3 _Velocity, _MovementDir = Vector3.zero;
    private Vector3 _HeadLocalPos, _ColliderCenter;
    private float _ColliderHeight;
    private CharacterController _CharCon;
    private bool _JumpButtonHeldUp = true;
    private bool _CeilingBump = false , _Ceiling = false , _Crouching = false;

    public bool movementPossibility { get; set; } = true;
    public bool onItem { get; private set; }
    public bool isMoving { get; private set; }
    public bool isRunning { get; private set; }
    public GameObject standingObject { get; private set; }

    private void Awake() 
    {
        _CharCon = GetComponent<CharacterController>();

        _HeadLocalPos = _Head.localPosition;
        _ColliderHeight = _CharCon.height;
        _ColliderCenter = _CharCon.center;
    }
    private void OnEnable()
    {
        PlayerManager.movement = this;
        PlayerManager.characterController = _CharCon;
    } 
    private void OnDisable() 
    {
        PlayerManager.movement = null;
        PlayerManager.characterController = null;
    }
    private void OnDrawGizmos() 
    {
        Gizmos.DrawSphere(_GroundCheck.position,_GroundCheckRadius);
        Gizmos.DrawSphere(_СeilingCheck.position,_СeilingCheckRadius);
    }
    void FixedUpdate()
    {
        if(!PlayerManager.playerExist || movementPossibility == false) return;

        //CheckPhysicsCollision();
        GroundCheck();
        СeilingCheck();
        BumpCheck();
        
        float MoveVertical = Input.GetAxis("Vertical");
        float MoveHorizontal = Input.GetAxis("Horizontal");

        //float SprintSpeedBonus = 0f;
        // if(Input.GetAxis("Sprint") > 0 && !_Crouching )
        // {
        //     isRunning = true;
        //     SprintSpeedBonus = _SprintBonus;
        // }
        // else
        // {
        //     isRunning = false;
        // }
        
        _MovementDir = (transform.right * MoveHorizontal + transform.forward * MoveVertical).normalized;
        //Move(_MovementDir,_MovementSpeed + SprintSpeedBonus,_Gravity); // with sprint
        Move(_MovementDir,_MovementSpeed ,_Gravity);

        if(Input.GetAxis("Jump") == 0) _JumpButtonHeldUp = true;
        if(Input.GetAxis("Jump") > 0 && _JumpButtonHeldUp && !_Crouching) Jump(_Gravity);

        if(Input.GetAxis("Crouch") > 0) SitDown(_SquatSpeed);
        else if(!_Ceiling) StandUp(_SquatSpeed); 
    }
    private void SitDown(float Speed)
    {
        Vector3 HeadLocalPos = _Head.localPosition;

        Vector3 TargetHeadPos = new Vector3(HeadLocalPos.x,_CrouchHeadYPos,HeadLocalPos.z);
        _Head.localPosition = Vector3.Lerp(HeadLocalPos,TargetHeadPos,Speed);

        Vector3 TargetColliderCenter = new Vector3(_CharCon.center.x,_CrouchColliderCenter,_CharCon.center.z);
        _CharCon.center = Vector3.Lerp(_CharCon.center,TargetColliderCenter,Speed);

        _CharCon.height = Mathf.Lerp(_CharCon.height,_CrouchColliderHeight,Speed);

        _Crouching = true;
    }
    private void StandUp(float Speed)
    {
        _Head.localPosition = Vector3.Lerp( _Head.localPosition, _HeadLocalPos, Speed);
        _CharCon.center = Vector3.Lerp( _CharCon.center, _ColliderCenter, Speed);
        _CharCon.height = Mathf.Lerp( _CharCon.height, _ColliderHeight, Speed);

        _Crouching = false;
    }
    private void Move(Vector3 Direction, float Speed, float Gravity)
    {
        if(Direction == Vector3.zero) isMoving = false;
        else isMoving = true;

        if(_CharCon.isGrounded && _Velocity.y < 0) _Velocity.y = 0f;
        if(_CeilingBump && !_CharCon.isGrounded && _Velocity.y > 0) _Velocity.y = -_Velocity.y / 3;
        _Velocity.y += Gravity * Time.deltaTime;

        Vector3 FullDirection = _Velocity + Direction * Speed;

        _CharCon.Move(FullDirection * Time.deltaTime);
    }
    private void Jump(float Gravity)
    {
        if(_CharCon.isGrounded)
        { 
            _Velocity.y = Mathf.Sqrt(_JumpStrenght * -2f * Gravity);
            _JumpButtonHeldUp = false;
        }
    }

    private void CheckPhysicsCollision()
    {
        Vector3 CapsuleCenterPos = _CapsuleCenter.position;
        float VerticalRadius = _CharCon.height * _SphereRadiusCorrection;

        float BottomPosY = CapsuleCenterPos.y - VerticalRadius;
        Vector3 CapsuleBottomPos = new Vector3(CapsuleCenterPos.x,BottomPosY,CapsuleCenterPos.z);

        float UpPosY = 0f;
        if(_Crouching) UpPosY = CapsuleCenterPos.y;
        else UpPosY = CapsuleCenterPos.y + VerticalRadius;
        Vector3 CapsuleUpPos = new Vector3(CapsuleCenterPos.x,UpPosY,CapsuleCenterPos.z);

        var Collision = Physics.OverlapCapsule(CapsuleBottomPos, CapsuleUpPos, _PhysicCapsuleRadius);
        foreach (var Col in Collision)
        {
            Rigidbody _Rgbd;
            if(Col.gameObject.TryGetComponent(out _Rgbd))
            _Rgbd.AddForce(_MovementDir,TypeOfImpact);
        }
    }
    private void GroundCheck()
    {
        var Colliders = Physics.OverlapSphere(_GroundCheck.position, _GroundCheckRadius, _Layer);
        if(Colliders.Length != 0)
        {
            foreach (var Col in Colliders)
            {
                if(Col.gameObject.GetComponent<DragableObject>() != null 
                || Col.gameObject.GetComponent<InteractionObject>() != null 
                || Col.gameObject.GetComponent<SlidingObject>() != null) // if you add new type also add it to RuntimeInfo(Any object exist condition)
                {
                    onItem = true;
                    standingObject = Col.gameObject;
                    break;
                } else
                {
                     onItem = false;
                     standingObject = null;
                }
            }
        }
    }
    private void СeilingCheck()
    {   
        _Ceiling = Physics.OverlapSphere(_СeilingCheck.position, _СeilingCheckRadius, _Layer).Length != 0;
    }

    private void BumpCheck()
    {   
        _CeilingBump = Physics.OverlapSphere(_BumpCheck.position, _BumpCheckRadius, _Layer).Length != 0;
    }
}
