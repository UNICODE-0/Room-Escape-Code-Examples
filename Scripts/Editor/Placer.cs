using UnityEngine;
using UnityEditor;

public class Placer : EditorWindow {

    [MenuItem("Tools/Placer")]
    private static void ShowWindow() 
    {
        var window = GetWindow<Placer>();
        window.titleContent = new GUIContent("Placer");
        window.Show();
    }

    [SerializeField] private GameObject _SpawnObject;
    [SerializeField] private bool _ShowNormal;

    private SerializedObject _SerObj;
    private SerializedProperty _ObjProp;
    private SerializedProperty _ShowNormalProp;
    void OnEnable()
    {
        _SerObj = new SerializedObject(this);
        _ObjProp = _SerObj.FindProperty("_SpawnObject");
        _ShowNormalProp = _SerObj.FindProperty("_ShowNormal");

        SceneView.duringSceneGui += DuringSceneGUI;
    } 
    void OnDisable() => SceneView.duringSceneGui -= DuringSceneGUI;
    void DuringSceneGUI(SceneView sceneView)
    {
        Transform CamTf = sceneView.camera.transform;

        if(Event.current.type == EventType.MouseMove) sceneView.Repaint();

        Ray Ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        if(Physics.Raycast(Ray, out RaycastHit Hit) && _ShowNormal)
        {
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(5,Hit.point,Hit.point + Hit.normal);
            Handles.color = Color.white;
        }

        if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
        TrySpawnObject(Hit);
    }

    private void TrySpawnObject(RaycastHit Hit)
    {
        if(_SpawnObject == null)
        {
            Debug.Log("Spawn object doesn't exist");
            return;
        }

        GameObject SpawnedObj = (GameObject)PrefabUtility.InstantiatePrefab(_SpawnObject);
        Undo.RegisterCreatedObjectUndo(SpawnedObj, "Spawn Objects");
        SpawnedObj.transform.position = Hit.point;

        Quaternion ObjRotation = Quaternion.LookRotation(Hit.normal) * Quaternion.Euler(90f,0f,0f);
        SpawnedObj.transform.rotation = ObjRotation;
        
        if(SpawnedObj.tag == "Chest")
        {
            GameObject ChestLid = SpawnedObj.transform.GetChild(0).gameObject;
            ChestLid.GetComponent<HingeJoint>().connectedAnchor = ChestLid.transform.position;
        } 
    }

    private void OnGUI() 
    {
        _SerObj.Update();
        EditorGUILayout.PropertyField(_ObjProp);
        EditorGUILayout.PropertyField(_ShowNormalProp);
        _SerObj.ApplyModifiedProperties();    
    }
}
