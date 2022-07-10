using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( InteractionObject ), true )]
public class InteractionObjectEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();


        InteractionObject interactionObject = (InteractionObject)target;
        Axis InteractionObjectAngleAxis = interactionObject.angleAxis;
        if(GUILayout.Button("Set min rotation to current rotation"))
        {
            Undo.RecordObject(interactionObject, "Set current rotation");
            interactionObject.minAngle = GetRotationByAxis(interactionObject,InteractionObjectAngleAxis);
        }
        if(GUILayout.Button("Set max rotation to current rotation"))
        {
            Undo.RecordObject(interactionObject, "Set current rotation");
            interactionObject.maxAngle = GetRotationByAxis(interactionObject,InteractionObjectAngleAxis);
        }
    }

    private float GetRotationByAxis(InteractionObject interactionObject, Axis axis)
    {
        float RotationByAxis = 0f;
        switch (axis)
        {
            case Axis.X:
            RotationByAxis = interactionObject.transform.rotation.x;
            break;
            case Axis.Y:
            RotationByAxis = interactionObject.minAngle = interactionObject.transform.rotation.y;
            break;
            case Axis.Z:
            RotationByAxis = interactionObject.minAngle = interactionObject.transform.rotation.z;
            break;
        }
        return RotationByAxis;
    }
}
