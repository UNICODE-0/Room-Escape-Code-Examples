using UnityEngine;

public class BottlePart : DragableObject
{
    [SerializeField] private float _ImpulsePower = 3f;
    void Start()
    {
        DisablePhysic();
    }
    public override void EnablePhysic()
    {
        base.EnablePhysic();
        Vector3 ImpulseVector = new Vector3(Random.Range(0.35f,1f), 0f, Random.Range(0.35f,1f));
        _Rgbd.AddForce(ImpulseVector * _ImpulsePower,ForceMode.Impulse);
    }
}
