using UnityEditor;
using UnityEngine;
 
//[CustomEditor(typeof(PlayerInfo))]
public class NPC_SentencesCustomEditor : Editor
{
 
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Custom editor:");
        var serializedObject = new SerializedObject(target);
        var max_hp = serializedObject.FindProperty("max_HP");
        var max_mp = serializedObject.FindProperty("max_MP");
        var max_sp = serializedObject.FindProperty("max_SP");
        Vector3 v = new Vector3(max_hp.intValue, max_mp.intValue, max_sp.intValue);
        serializedObject.Update();
        EditorGUILayout.Vector3Field("HP/MP/SP",v);
        serializedObject.ApplyModifiedProperties();
    }
}
