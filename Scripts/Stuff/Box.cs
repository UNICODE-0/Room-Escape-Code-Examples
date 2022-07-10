using System.Collections;
using UnityEngine;

public class Box : DragableObject
{
    [SerializeField] private Transform _CrowbarTargetTF;
    [SerializeField] private Transform _OutTF;
    [SerializeField] private Vector3 _ImpulseVector;
    [SerializeField] private float _ImpulsePower;

    private Crowbar _Crowbar;
    private bool _IsOpen;
    private void OnCollisionEnter(Collision other) 
    {
        if(_IsOpen == false && _Crowbar == null)
        {
            if(other.gameObject.TryGetComponent(out _Crowbar))
            {
                _Crowbar.DisablePhysic();
                UniLib.MoveObjToTargetPos(this,_Crowbar.transform,_CrowbarTargetTF.position, UseEvent: true);
                UniLib.RotateObjToTargetRot(this,_Crowbar.transform,_CrowbarTargetTF.rotation, UseEvent: true);
            }
        }
    }
    private void Update() 
    {
        if(_IsOpen == false && _Crowbar != null)
        {
            if(_Crowbar.reachTarget)
            {
                StartCoroutine(Open());
                _IsOpen = true;
            }
        }
    }
    private IEnumerator Open()
    {
        _AudioSource.Play();
        _Crowbar.PlayAnimation();
        EnablePhysic();
        _Rgbd.AddForce(_ImpulseVector * _ImpulsePower,ForceMode.Impulse);
        yield return new WaitForSecondsRealtime(1f);
        UniLib.MoveObjToTargetPos(this,_Crowbar.transform,_OutTF.position, UseEvent: true);
        UniLib.RotateObjToTargetRot(this,_Crowbar.transform,_OutTF.rotation, UseEvent: true);
        _Crowbar = null;
    } 
}
