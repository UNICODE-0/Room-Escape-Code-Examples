using System.Collections;
using UnityEngine;
public class UniLib : MonoBehaviour
{
    static private readonly int _ShPropCol = Shader.PropertyToID("_Color");
    static private MaterialPropertyBlock _Mpb;
    static private MaterialPropertyBlock mpb
    {
        get 
        { 
            if(_Mpb == null) _Mpb = new MaterialPropertyBlock(); 
            return _Mpb;
        }
    }

    public delegate void ObjectRotated(int ObjectId);
    public static event ObjectRotated objectRotated;
    public delegate void ObjectMoved(int ObjectId);
    public static event ObjectMoved objectMoved;
    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    public enum СomparisonMethod
    {
        Bigger,
        Smaller,
        Equal
    }

    static public void ChangeMaterialColor(MeshRenderer MeshRnd, Color NewColor)
    {
        mpb.Clear();
        
        mpb.SetColor(_ShPropCol,NewColor );
        MeshRnd.SetPropertyBlock(mpb);
    }
    static public void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.renderQueue = 2000;
                material.SetOverrideTag("RenderType", "Opaque");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.SetFloat("_SrcBlend", 1);
                material.SetFloat("_ZWrite", 1f);
                material.SetFloat("_DstBlend", 0);
                break;
            case RenderingMode.Cutout:
                material.renderQueue = 2450;
                material.SetOverrideTag("RenderType", "TransparentCutout");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.SetFloat("_SrcBlend", 1);
                material.SetFloat("_ZWrite", 1f);
                material.SetFloat("_DstBlend", 0);
                break;
            case RenderingMode.Fade:
                material.renderQueue = 3000;
                material.SetOverrideTag("RenderType", "Transparent");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.SetFloat("_SrcBlend", 5);
                material.SetFloat("_ZWrite", 0f);
                material.SetFloat("_DstBlend", 10);
                break;
            case RenderingMode.Transparent:
                material.renderQueue = 3000;
                material.SetOverrideTag("RenderType", "Transparent");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.SetFloat("_SrcBlend", 1);
                material.SetFloat("_ZWrite", 0f);
                material.SetFloat("_DstBlend", 10);
                break;
        }
    }
    static public void DisappearObject(MonoBehaviour Behaviour, MeshRenderer MeshToDisappear, float DisappearanceRate = 0.004f)
    {
        Behaviour.StartCoroutine(_DisappearObject(MeshToDisappear,DisappearanceRate));
    }
    static private IEnumerator _DisappearObject(MeshRenderer MeshToDisappear, float DisappearanceRate = 0.004f)
    {
        Material MeshMat = MeshToDisappear.material;
        SetMaterialRenderingMode(MeshMat, RenderingMode.Fade);
        Color Color = MeshMat.color;
        while(Color.a > 0f)
        {
            yield return new WaitForFixedUpdate();
            Color = new Color(Color.r,Color.g,Color.b,Color.a - DisappearanceRate);
            ChangeMaterialColor(MeshToDisappear,Color);
        }
        Destroy(MeshToDisappear.gameObject);
    }

    static public void MoveObjToTargetPos(MonoBehaviour Behaviour, Transform ObjTf, Vector3 TargetPos, float MoveSpeed = 0.1f, float MinDistance = 0.01f, bool UseEvent = false)
    {
        Behaviour.StartCoroutine(_MoveObjToTargetPos(ObjTf,TargetPos,MoveSpeed,MinDistance,UseEvent));
    }

    static private IEnumerator _MoveObjToTargetPos(Transform ObjTf, Vector3 TargetPos, float MoveSpeed, float MinDistance, bool UseEvent)
    {
        while((ObjTf.position - TargetPos).magnitude > MinDistance)
        {
            yield return new WaitForFixedUpdate();
            ObjTf.position = Vector3.Lerp(ObjTf.position,TargetPos,MoveSpeed);
        }
        ObjTf.position = TargetPos;

        if(UseEvent) objectMoved(ObjTf.gameObject.GetInstanceID());
    }
    static public void RotateObjToTargetRot(MonoBehaviour Behaviour, Transform ObjTf, Quaternion TargetRot, float Duration = 1f, float RotationSpeed = 0.1f, bool UseEvent = false)
    {
        Behaviour.StartCoroutine(_RotateObjToTargetRot(ObjTf,TargetRot,Duration,RotationSpeed,UseEvent));
    }
    static private IEnumerator _RotateObjToTargetRot(Transform ObjTf, Quaternion TargetRot, float Duration, float RotationSpeed, bool UseEvent)
    {
        float time = 0f;
        while(time < Duration)
        {
            ObjTf.rotation = Quaternion.Slerp(ObjTf.rotation,TargetRot, (time / Duration) * RotationSpeed);
            yield return null;
            time += Time.deltaTime;
        }
        ObjTf.rotation = TargetRot;
        if(UseEvent) objectRotated(ObjTf.gameObject.GetInstanceID());
    }
    static public bool CompareVectors(Vector3 Vec1,Vector3 Vec2, СomparisonMethod comparisonMethod)
    {
        switch (comparisonMethod)
        {
            case СomparisonMethod.Bigger:
            if(Vec1.x >= Vec2.x && Vec1.y >= Vec2.y && Vec1.z >= Vec2.z) return true;
            break;
            case СomparisonMethod.Smaller:
            if(Vec1.x <= Vec2.x && Vec1.y <= Vec2.y && Vec1.z <= Vec2.z) return true;
            break;
            case СomparisonMethod.Equal:
            if(Vec1.x == Vec2.x && Vec1.y == Vec2.y && Vec1.z == Vec2.z) return true;
            break;
        }
        return false;
    }
    static public Quaternion AddRotation(Transform Tf, Vector3 Axis, float Angle)
    {
        Quaternion RoataionAdd = Quaternion.AngleAxis(Angle, Axis),
        Rotation = Quaternion.LookRotation(Tf.forward,Tf.up);
        return RoataionAdd * Rotation;
    }
}
