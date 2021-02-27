using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(NPC_Sentences))]
public class NPC_SentencesDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        label.text = "NPC";
        //position.height = 72;
        EditorGUI.BeginProperty(position, label, property);
        //Debug.Log(position);
        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
       //EditorGUI.DrawRect(position, Color.red);
        // Calculate rects
        var imageRect = new Rect(position.x-50, position.y, 60,      18);
        var nameRect  = new Rect(position.x+15, position.y, 110,       18);
        var sentencesRect  = new Rect(position.x-50, position.y+20, 50, 18);
        var nextRect  = new Rect(position.x+60, position.y, 50,      18);
        var testRect  = new Rect(position.x, position.y, 50,         18);

        //EditorGUI.ObjectField(nextRect, property.FindPropertyRelative("image"));
        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(imageRect, property.FindPropertyRelative("image"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.PropertyField(sentencesRect, property.FindPropertyRelative("sentences"));
        var list = property.FindPropertyRelative("sentences");
        label.text = "Size";
        EditorGUI.PrefixLabel(new Rect(position.x+50, position.y+20, 50, 18),GUIUtility.GetControlID(FocusType.Passive), label);
        string arraySize = EditorGUI.TextArea(new Rect(position.x+80, position.y+20, 50, 18), list.arraySize.ToString());
        //list.arraySize = 
        int n;
        if (System.Int32.TryParse(arraySize, out n))
            list.arraySize = n;

        if (list.isExpanded){
            int size = list.arraySize;
            for (int i = 0; i < size; i++) {
                string v = (string) list.GetArrayElementAtIndex(i).stringValue;
                string s = EditorGUI.TextArea(new Rect(sentencesRect.x, sentencesRect.y+(18 * (i+1)), 180 , 18), v);
                list.GetArrayElementAtIndex(i).stringValue = s;
            }
        }
        //EditorGUI.PropertyField(nextRect, property, GUIContent.none);
        //EditorGUI.PropertyField(testRect, property.FindPropertyRelative("test"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
        int height = 38;
        var list = property.FindPropertyRelative("sentences");
        if (list.isExpanded){
            int size = list.arraySize;
            for (int i = 0; i < size; i++)
            {
                height += 18;
            }
        }
        return height;
    }
}