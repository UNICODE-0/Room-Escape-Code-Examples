using UnityEditor;

[CustomEditor(typeof(CipherLock))]
[CanEditMultipleObjects]
public class CipherLockEditor : Editor {
    SerializedProperty m_LockCipher;
    private const int BUTTONSCOUNT = 6;
    private void OnEnable() 
    {
        m_LockCipher = serializedObject.FindProperty("_LockCipher");
    }
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        if(m_LockCipher.arraySize > BUTTONSCOUNT) m_LockCipher.DeleteArrayElementAtIndex(BUTTONSCOUNT);
        if(m_LockCipher.arraySize == 0) m_LockCipher.InsertArrayElementAtIndex(0);
        serializedObject.ApplyModifiedProperties();
    }
}
