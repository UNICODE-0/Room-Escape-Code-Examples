using System.Collections;
using UnityEngine;

public class Stove : PresenceСheckerObject
{
    [SerializeField] private GameObject[] _FireObjects;
    [SerializeField] private Transform[] _LogTf;
    [SerializeField] private Transform _LighterFluidTf;
    [SerializeField] private Transform _CandleTf;
    [SerializeField] private Transform _OutPosition;
    [SerializeField] private MeshRenderer _HeatingPlane;
    [SerializeField] private ParticleSystem _Smoke;
    [SerializeField] private AudioSource _AudioSource;
    private ParticleSystem.EmissionModule _SmokeEmission;
    private int _LogCount = 0, _MaxLogCount;
    private bool _IsOnFire = false;
    private bool _LogsFind = false , _LightingFluidFind = false , _CandleFind = false;
    private LighterFluid _LighterFluid;
    private Candle _Candle;
    private Bottle _Bottle;
    private void OnTriggerStay(Collider other) 
    {
        if(other.TryGetComponent(out _Bottle) && _IsOnFire)
        {
            if(_Bottle.isOpening)
            {
                _SmokeEmission.enabled = true;
            } 
            else _Bottle.StartOpenTimer();

        } else
        {
            _SmokeEmission.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.TryGetComponent(out _Bottle) && _IsOnFire)
        {
            _Bottle.StopOpenTimer();
            _SmokeEmission.enabled = false;
        } 
    }
    private void Start() 
    {
        _MaxLogCount = _LogTf.Length;
        _SmokeEmission = _Smoke.emission;
        _SmokeEmission.enabled = false;
        _AudioSource.enabled = false;
    }
    private void Update() 
    {
        if(_IsOnFire == false)
        {
            Collider[] AllCollisions = CheckPresence();
            if(AllCollisions == null) return;

            if(_LogsFind == false)
            {
                foreach (var collision in AllCollisions)
                {
                    if(collision.TryGetComponent(out Log log))
                    {
                        log.DisablePhysic();
                        UniLib.MoveObjToTargetPos(this, log.transform, _LogTf[_LogCount].position, UseEvent: true);
                        UniLib.RotateObjToTargetRot(this, log.transform, _LogTf[_LogCount].rotation, UseEvent: true);
                        _LogCount++;
                        if(_LogCount == _MaxLogCount) _LogsFind = true;
                        log = null;
                        break;
                    }
                }
            } else if (_LightingFluidFind == false)
            {
                foreach (var collision in AllCollisions)
                {
                    if(collision.TryGetComponent(out _LighterFluid))
                    {
                        if(_LighterFluid.isUsed) continue;
                        _LighterFluid.DisablePhysic();
                        UniLib.MoveObjToTargetPos(this, _LighterFluid.transform, _LighterFluidTf.position, MoveSpeed: 0.08f , UseEvent: true);
                        UniLib.RotateObjToTargetRot(this, _LighterFluid.transform, _LighterFluidTf.rotation, RotationSpeed: 0.08f , UseEvent: true);
                        _LightingFluidFind = true;
                        break;
                    } 
                }
            } else if ( _LighterFluid.reachTarget && _LighterFluid.isUsed == false )
            {
                StartCoroutine(UseLighterFluid(_LighterFluid));
            } 
            else if(_CandleFind == false)
            {
                foreach (var collision in AllCollisions)
                {
                    if(collision.TryGetComponent(out _Candle))
                    {
                        _Candle.DisablePhysic();
                        UniLib.MoveObjToTargetPos(this, _Candle.transform, _CandleTf.position, MoveSpeed: 0.08f , UseEvent: true);
                        UniLib.RotateObjToTargetRot(this, _Candle.transform, _CandleTf.rotation, RotationSpeed: 0.08f , UseEvent: true);
                        _CandleFind = true;
                        break;
                    } 
                }
            } else if (_Candle.reachTarget)
            {
                StartCoroutine(UseCandle(_Candle));
                if(_Candle.isUsed)
                {
                    StartCoroutine(Heating());
                    _IsOnFire = true;
                    _AudioSource.enabled = true;
                } 
            }
        }
    }
    private IEnumerator Heating()
    {
        yield return new WaitForSeconds(3f);
        Material Mat = _HeatingPlane.material;
        float AlphaChangeSpeed = 0.03f;
        float Alpha = Mat.color.a;
        while (Alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            Alpha += AlphaChangeSpeed * Time.deltaTime;
            UniLib.ChangeMaterialColor(_HeatingPlane,new Color(Mat.color.r,Mat.color.g,Mat.color.b,Alpha));
        }
    }

    private IEnumerator UseLighterFluid(LighterFluid lighterFluid)
    {
        lighterFluid.OpenSpout();
        yield return new WaitForSeconds(4f);
        lighterFluid.CloseSpout();
        yield return new WaitForSeconds(1f);
        UniLib.MoveObjToTargetPos(this, _LighterFluid.transform, _OutPosition.position, MoveSpeed: 0.08f, UseEvent: true);
        UniLib.RotateObjToTargetRot(this, _LighterFluid.transform, _OutPosition.rotation, RotationSpeed: 0.08f, UseEvent: true);
    }
    private IEnumerator UseCandle(Candle candle)
    {
        LightStove();
        candle.Use();
        yield return new WaitForSeconds(2f);
        UniLib.MoveObjToTargetPos(this, candle.transform, _OutPosition.position, MoveSpeed:0.08f, UseEvent: true);
        UniLib.RotateObjToTargetRot(this, candle.transform, _OutPosition.rotation, RotationSpeed: 0.08f, UseEvent: true);
    }
    private void LightStove()
    {
        foreach (var fireObject in _FireObjects)
        {
            fireObject.SetActive(true);
        }
    }
}
