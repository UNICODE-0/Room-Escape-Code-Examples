using UnityEngine;

public abstract class PresenceСheckerObject : MonoBehaviour
{
    [SerializeField] private Transform _PresencePosition;
    [SerializeField] private float _PresenceRadius;
    [SerializeField] private LayerMask _Layer;
    private void OnDrawGizmos() 
    {
        Gizmos.DrawSphere(_PresencePosition.position,_PresenceRadius);
    }
    protected Collider[] CheckPresence()
    {
        Collider[] AllCollisions = Physics.OverlapSphere(_PresencePosition.position, _PresenceRadius, _Layer);
        if(AllCollisions.Length != 0) return AllCollisions;
        else return null;
    }
}
