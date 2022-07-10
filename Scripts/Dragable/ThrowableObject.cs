using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ThrowableObject : MonoBehaviour,IThrowable
{
    [SerializeField] private float _ThrowPower = 50f;
    private Rigidbody _Rigbd;
    private void Start() 
    {
        _Rigbd = GetComponent<Rigidbody>();  
    }
    public void Throw(Vector3 Direction)
    {
        _Rigbd.AddForce(Direction * _ThrowPower, ForceMode.Impulse);
    }
}
