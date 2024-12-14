using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Sych_SO / New_Quest")]
    public class Quest_SO : ScriptableObject
    {
        #region Переменные
        [Tooltip("ID Имя квеста (для поиска, а не для интерфейса)")]
        [SerializeField]
        public string Name_id_quest { get; private set; } = "Quest_name";

        [field: Tooltip("Название квеста (для интерфейса)")]
        [field: SerializeField]
        internal string Name_quest { get; private set; } = "Название квеста";

        [field: Tooltip("Описание квеста (для интерфейса")]
        [field: SerializeField]
        [field: TextArea]
        internal string Description_quest { get; private set; } = "Описание квеста";

        [field: Tooltip("Подзадачи")]
        [field: SerializeField]
        internal string[] Description_Tasks_array { get; private set; } = new string[0];

        [Tooltip("Требования по завершённым квестам, что бы этот квест заработал")]
        [SerializeField]
        Requirements_quest_class[] Requirements_array = new Requirements_quest_class[0];
        #endregion

        #region Публичные методы
        /// <summary>
        /// Проверить выполнены ли прошлые квесты, что бы можно было взять этот квест
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
                            Debug.LogError("Не указан ID искомого квеста!");
                    }
                        
                }

                return result;
            }
        }
        #endregion

        [System.Serializable]
         class Requirements_quest_class
        {
            [Tooltip("Имя искомого квеста (если пусто, то используется для поиска ID)")]
            [field: SerializeField]
            public string Requirement_name_quest { get; private set; } = "";

            [Tooltip("ID искомого квеста")]
            [field: SerializeField]
            public int Requirement_id_quest { get; private set; } = -1;
        }
    }
}