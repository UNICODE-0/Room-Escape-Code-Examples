using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SlidingObject))]
public class SlidingObjectEditor : Editor {
    public override void OnInspectorGUI() 
    {
        DrawDefaultInspector();

        SlidingObject slidingObject = (SlidingObject)target;
        if(GUILayout.Button("Set min postion to current position"))
        {
            Undo.RecordObject(slidingObject, "Set current position");
            slidingObject.minPosition = slidingObject.transform.position;
        }
        if(GUILayout.Button("Set max postion to current position"))
        {
            Undo.RecordObject(slidingObject, "Set current position");
            slidingObject.maxPosition = slidingObject.transform.position;
        }
    }
}
