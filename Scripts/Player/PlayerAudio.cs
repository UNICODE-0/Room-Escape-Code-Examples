using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    private AudioSource _AudioSource;
    private void Start() 
    {
        _AudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(PlayerManager.movement.isMoving && !_AudioSource.isPlaying)
        {
            _AudioSource.pitch = Random.Range(0.9f,1.1f);
            _AudioSource.Play();
        }
    }
}
