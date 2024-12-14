using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Sych_SO / New_Quest")]
    public class Quest_SO : ScriptableObject
    {
        #region ����������
        [Tooltip("ID ��� ������ (��� ������, � �� ��� ����������)")]
        [SerializeField]
        public string Name_id_quest { get; private set; } = "Quest_name";

        [field: Tooltip("�������� ������ (��� ����������)")]
        [field: SerializeField]
        internal string Name_quest { get; private set; } = "�������� ������";

        [field: Tooltip("�������� ������ (��� ����������")]
        [field: SerializeField]
        [field: TextArea]
        internal string Description_quest { get; private set; } = "�������� ������";

        [field: Tooltip("���������")]
        [field: SerializeField]
        internal string[] Description_Tasks_array { get; private set; } = new string[0];

        [Tooltip("���������� �� ����������� �������, ��� �� ���� ����� ���������")]
        [SerializeField]
        Requirements_quest_class[] Requirements_array = new Requirements_quest_class[0];
        #endregion

        #region ��������� ������
        /// <summary>
        /// ��������� ��������� �� ������� ������, ��� �� ����� ���� ����� ���� �����
        /// </summary>
        public bool Check_requirements_true
        {
            get
            {
                bool result = true;

                for(int x = 0; x < Requirements_array.Length; x++)
                {
                    if (Requirements_array[x].Requirement_name_quest != "")
                        result = Save_PlayerPrefs.Know_parameter(Type_parameter_bool.Quest_completed_bool, Requirements_array[x].Requirement_name_quest);
                    else
                    {
                        if (Requirements_array[x].Requirement_id_quest > -1)
                            result = Save_PlayerPrefs.Know_parameter(Type_parameter_bool.Quest_completed_bool, Requirements_array[x].Requirement_id_quest);
                        else
                            Debug.LogError("�� ������ ID �������� ������!");
                    }
                        
                }

                return result;
            }
        }
        #endregion

        [System.Serializable]
         class Requirements_quest_class
        {
            [Tooltip("��� �������� ������ (���� �����, �� ������������ ��� ������ ID)")]
            [field: SerializeField]
            public string Requirement_name_quest { get; private set; } = "";

            [Tooltip("ID �������� ������")]
            [field: SerializeField]
            public int Requirement_id_quest { get; private set; } = -1;
        }
    }
}