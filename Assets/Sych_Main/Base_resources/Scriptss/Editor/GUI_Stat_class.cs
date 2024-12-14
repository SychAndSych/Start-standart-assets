#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Sych_scripts;

/*
[CustomPropertyDrawer(typeof(Stat_class))]
public class GUI_Stat_class : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var amountRect = new Rect(position.x, position.y, 100, position.height);
        //var unitRect = new Rect(position.x + 105, position.y, 100, position.height);
        //var nameRect = new Rect(position.x + 210, position.y, position.width - 90, position.height);

        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("Test"), GUIContent.none);
        //EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("Name"), GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}

*/
#endif
