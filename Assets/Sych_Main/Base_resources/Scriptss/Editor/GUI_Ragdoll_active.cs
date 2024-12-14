//Добавляет кнопку ленивого автоматического поиска коллайдеров и физики Ragdoll частей
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Sych_scripts
{
    [CustomEditor(typeof(Ragdoll_activity))]
    public class GUI_Ragdoll_active : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Ragdoll_activity Ra = (Ragdoll_activity)target;

            if (GUILayout.Button("Найти компоненты Ragdoll"))
            {
                Ra.Auto_Find_components();
            }

            if (GUILayout.Button("Добавить скрипт HurtBox для частей куклы"))
            {
                Ra.Auto_add_HurtBox_component();
                EditorUtility.SetDirty(Ra);
            }

            if (GUILayout.Button("Переключает Projection и Preprocessing"))
            {
                Ra.Enable_Projection_and_Preprocessing();
                EditorUtility.SetDirty(Ra);
            }
        }
    }
}
#endif


