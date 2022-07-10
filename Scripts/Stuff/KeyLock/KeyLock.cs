using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class KeyLock : PresenceСheckerObject
{
    [SerializeField] private LockPart[] _LockParts;
    [SerializeField] private Transform _KeyTargetPosition;

    private AudioSource _AudioSource;

    private bool _IsOpen = false, _KeyFind = false;
    private Key _key;

    public bool isOpen
    {
        get { return _IsOpen; }
    }

    public delegate void Unclocked(int LockID);
    public static event Unclocked unclocked;
    private void Start() 
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    private void Update() 
    {
        if(_IsOpen == false)
        {
            if(_KeyFind == false)
            {
                Collider[] AllCollisions = CheckPresence();
                if(AllCollisions != null)
                {
                    foreach (var collision in AllCollisions)
                    {
                        if(collision.TryGetComponent(out _key)) break;
                    }
                }

                if(_key != null)
                {
                    _KeyFind = true;
                    _key.DisablePhysic();
                    UniLib.MoveObjToTargetPos(this, _key.transform, _KeyTargetPosition.position, UseEvent: true);
                    UniLib.RotateObjToTargetRot(this, _key.transform, _KeyTargetPosition.rotation, UseEvent: true);
                }
            } else if(_key.reachTarget)
            {
                _IsOpen = true;
                _key.UnlockKeyLock(this);
            }
        }
    }
    public void Open()
    {
        _AudioSource.Play();
        foreach (var lockpart in _LockParts)
        {
            lockpart.EnablePhysic();
        }
        unclocked(gameObject.GetInstanceID());
    }
}
