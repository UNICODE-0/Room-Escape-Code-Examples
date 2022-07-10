using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LighterFluid : UseableItem
{
    [SerializeField] private GameObject _FuelFlow, _FuelParticles;
    [SerializeField] private float _FlowSpeed = 0.75f;
    [SerializeField] private float _DelayBeforeFlowAnimation = 0.8f;
    [SerializeField] private float _FlowDuration = 0.75f;

    const string TRESHOLD_DOWN = "_TransparencyTresholdDown",
    TRESHOLD_UP = "_TransparencyTresholdUp";

    public bool isUsed { get; private set; }
    private Animator _Animator;
    private void Start() 
    {
        _Animator = GetComponent<Animator>();
    }
    override protected void OnTargetPosition()
    {
        base.OnTargetPosition();
        if(isUsed) EnablePhysic();
    }
    
    public void OpenSpout()
    {
        _Animator.SetTrigger("Open");
        _ReachTarget = false;
        isUsed = true;
        if(_FuelFlow.TryGetComponent(out MeshRenderer mesh))
        {
            StartCoroutine(AnimateFlow(mesh));
        } 
        else Debug.LogError("Fuel Flow doesn't contain a Mesh Renderer");
    }
    private IEnumerator AnimateFlow(MeshRenderer mesh)
    {
        Material FlowMat = mesh.material;
        float _TransparencyTresholdDown = FlowMat.GetFloat(TRESHOLD_DOWN),
        _TransparencyTresholdUp = FlowMat.GetFloat(TRESHOLD_UP);

        yield return new WaitForSeconds(_DelayBeforeFlowAnimation);

        _FuelFlow.SetActive(true);
        _FuelParticles.SetActive(true);
        while (_TransparencyTresholdDown > 0f)
        {
            yield return new WaitForFixedUpdate();
            mesh.material.SetFloat("_TransparencyTresholdDown", _TransparencyTresholdDown); 
            _TransparencyTresholdDown -= _FlowSpeed * Time.deltaTime;
        }
        yield return new WaitForSeconds(_FlowDuration);
        _FuelParticles.SetActive(false);
        while (_TransparencyTresholdUp > 0.01f)
        {
            yield return new WaitForFixedUpdate();
            _TransparencyTresholdUp -= _FlowSpeed * Time.deltaTime;
            mesh.material.SetFloat("_TransparencyTresholdUp", _TransparencyTresholdUp); 
        }

        Resources.UnloadUnusedAssets();
        _FuelFlow.SetActive(false);
    }

    public void CloseSpout()
    {
        _Animator.SetTrigger("Close");
    }
}
