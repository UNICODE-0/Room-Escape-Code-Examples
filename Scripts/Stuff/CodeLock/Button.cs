using UnityEngine;
public interface IInteractive
{
    void Use();    
}

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public abstract class Button : MonoBehaviour, IInteractive
{
    private AudioSource _AudioSource;
    protected Animator _Animator;
    protected Collider _Collider;
    
    private void Start() 
    {
        _Animator = GetComponent<Animator>();
        _Collider = GetComponent<Collider>();
        _AudioSource = GetComponent<AudioSource>();
    }
    public virtual void Use()
    {
        _AudioSource.Play();

    }
}
