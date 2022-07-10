using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Switch : MonoBehaviour, IInteractive
{
    [SerializeField] private Lamp[] Lamps;

    private AudioSource _AudioSource;
    private Animator _Animator;
    private bool _State = false;
    private void Start() 
    {
        _Animator = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();
    }
    public void Use()
    {
        _AudioSource.Play();
        if(_State)
        {
            _Animator.SetTrigger("Down");
            _State = false;
            foreach (var Lamp in Lamps)
            {
                Lamp.DisabLight();
            }
        }
        else
        {
            _State = true;
            _Animator.SetTrigger("Up");
            foreach (var Lamp in Lamps)
            {
                Lamp.EnableLight();
            }
        }

    }

}