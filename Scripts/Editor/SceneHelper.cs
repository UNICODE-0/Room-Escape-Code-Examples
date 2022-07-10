using UnityEngine;
using UnityEditor;

public class SceneHelper : EditorWindow {

    [MenuItem("Tools/SceneHelper")]
    private static void ShowWindow() 
    {
        var window = GetWindow<SceneHelper>();
        window.titleContent = new GUIContent("SceneHelper");
        window.Show();
    }

    private void OnGUI() 
    {
        if( GUILayout.Button("Set all hinge anchors"))
        {
            SetAnchors();
        }
    }
    private void SetAnchors()
    {
        InteractionObject[] Objs = GameObject.FindObjectsOfType<InteractionObject>();
        foreach (var Obj in Objs)
        {
            Undo.RecordObject(Obj.GetComponent<HingeJoint>(), "Set anchor");
            Obj.GetComponent<HingeJoint>().connectedAnchor = Obj.transform.position; 
        }
    }
}
