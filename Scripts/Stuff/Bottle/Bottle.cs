using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Bottle : DragableObject
{
    [SerializeField] private BottlePart[] _BottleParts;
    [SerializeField] private float _OpenDuration = 10f;

    private Coroutine _TimerToOpen;
    private bool _IsOpening;
    public bool isOpening
    {
        get { return _IsOpening; }
        set { _IsOpening = value; }
    }
    
    public void StartOpenTimer()
    {
        if(_TimerToOpen == null) _TimerToOpen = StartCoroutine(TimerToOpen());
    }
    public void StopOpenTimer()
    {
        if(_TimerToOpen != null)
        {
            StopCoroutine(_TimerToOpen);
            _TimerToOpen = null;
            _IsOpening = false;
        } 
    }
    private IEnumerator TimerToOpen()
    {
        _IsOpening = true;
        yield return new WaitForSecondsRealtime(_OpenDuration);
        Open();
    }
    protected virtual void Open()
    {
        _AudioSource.Play();
        DisablePhysic();
        foreach (var BottlePart in _BottleParts)
        {
            BottlePart.EnablePhysic();
        }
    }
}
