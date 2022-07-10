using System.Collections;
using UnityEngine;

public class KnockingController : MonoBehaviour
{
    [SerializeField] private AudioSource[] _Knocking;
    [SerializeField] private float _IntervalBetweenKnocks = 215f;
    [SerializeField] private float _IntervalDeviation = 35f;
    private Coroutine _KnockingCor;
    private void OnEnable() => KeyLock.unclocked += LockUnclocked;
    private void OnDisable() => KeyLock.unclocked -= LockUnclocked;
    private void LockUnclocked(int LockID)
    {
        StopCoroutine(_KnockingCor);
        _KnockingCor = null;
    }
    private void Start()
    {
        _KnockingCor = StartCoroutine(KnockingDelay());
    }
    IEnumerator KnockingDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(
            _IntervalBetweenKnocks - _IntervalDeviation,
            _IntervalBetweenKnocks + _IntervalDeviation));

            _Knocking[Random.Range(0,_Knocking.Length)].Play();
        }
    }
}
