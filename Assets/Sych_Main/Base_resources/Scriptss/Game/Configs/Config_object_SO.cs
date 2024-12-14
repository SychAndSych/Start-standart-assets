using System;
using UnityEngine;
using System.Collections;
using NaughtyAttributes;

namespace Sych_scripts
{

    [CreateAssetMenu(fileName = "Object", menuName = "Sych_SO / Config / New_Object_Config", order = 1)]

    public class Config_object_SO : ScriptableObject
    {
        
        [field: Tooltip("Имя объекта")]
        [field: SerializeField]
        public string Name { get; private set; } = null;

        [field: ShowAssetPreview]
        [field: Tooltip("Картинка для отображения в UI")]
        [field: SerializeField]
        public Sprite Sprite { get; private set; } = null;

        [field: ShowAssetPreview]
        [field: Tooltip("Префаб объекта который используется в деле")]
        [field: SerializeField]
        public Entity_abstract Prefab_game { get; private set; } = null;

        [Space(20)]
        [Tooltip("Параметры объекта")]
        [SerializeField]
        public Stat_class[] Stat_array = new Stat_class[0];

        [Space(30)]

        internal string Main_name_category = "";//Костыль напоминалка, нужно потом подумать над, тем что бы Alert по другому узнавал Id этого сегмента сам
        internal int Main_number_config = -1;
        int Number_find_stat_class = -1;

        /// <summary>
        /// Ищем нужный стат по имени
        /// </summary>
        /// <param name="_name">Имя стата</param>
        /// <returns></returns>
        Stat_class Find_stat_class(string _name)
        {
            Stat_class result = null;

            bool positive_result_bool = false;

            for (int x = 0; x < Stat_array.Length; x++)
            {
                if (Stat_array[x].Name == _name)
                {
                    Number_find_stat_class = x;

                    result = Stat_array[x];

                    positive_result_bool = true;
                    break;
                }

            }

            if (!positive_result_bool)
                Debug.LogError("У конфига по Категории  " + Main_name_category + "  и номеру конфига  " + Main_number_config + "  Параметр по имени  " + _name + "  не был найден!");

            return result;
        }

        /// <summary>
        /// Узнать значение переменной
        /// </summary>
        /// <typeparam name="T">Тип переменной</typeparam>
        /// <param name="_name">Имя переменной (для поиска по массиву SO)</param>
        /// <returns></returns>
        public T Get_parameter<T>(string _name)
        {
            /*
T newT1;
string newT2 = "Привет рыба";

if (typeof(T) == typeof(string))
{
    newT1 = (T)(object)newT2;
    newT2 = (string)(object)newT1;
}
*/

            dynamic r = 1;
            
            string type_string = typeof(T).ToString();

            Stat_class find_stat = Find_stat_class(_name);

            switch (type_string)
            {
                case "System.Single":
                    r = find_stat.Value_float;

                    if (find_stat.Type != Config_enum.Float)
                        Alert(_name, find_stat.Type.ToString(), "Float");

                    break;

                case "System.Int32":
                    r = find_stat.Value_int;

                    if (find_stat.Type != Config_enum.Int)
                        Alert(_name, find_stat.Type.ToString(), "Int");

                    break;

                case "UnityEngine.Vector2":
                    r = find_stat.Value_vector2;

                    if (find_stat.Type != Config_enum.Vector2)
                        Alert(_name, find_stat.Type.ToString(), "Vector2");

                    break;

                case "UnityEngine.Vector3":
                    r = find_stat.Value_vector3;

                    if (find_stat.Type != Config_enum.Vector3)
                        Alert(_name, find_stat.Type.ToString(), "Vector3");

                    break;

                case "UnityEngine.Color":
                    r = find_stat.Value_color;

                    if (find_stat.Type != Config_enum.Color)
                        Alert(_name, find_stat.Type.ToString(), "Color");

                    break;

                case "System.Boolean":
                    r = find_stat.Value_bool;

                    if (find_stat.Type != Config_enum.Bool)
                        Alert(_name, find_stat.Type.ToString(), "Bool");

                    break;

                case "System.String":
                    r = find_stat.Value_string;

                    if (find_stat.Type != Config_enum.String)
                        Alert(_name, find_stat.Type.ToString(), "String");

                    break;

                case "UnityEngine.GameObject":
                    r = find_stat.Value_gameObject;

                    if (find_stat.Type != Config_enum.GameObject)
                        Alert(_name, find_stat.Type.ToString(), "GameObject");

                    break;

                default:
                    Debug.LogError("Ищется неправильный тип переменной! " + "(Ищется тип " + type_string + ")");
                    break;
            }

            return (T)(object)r;
        }

        /// <summary>
        /// На случай ошибки
        /// </summary>
        void Alert(string _name , string _current_type, string _find_type)
        {
            Debug.LogError("У конфига по Категории  " + Main_name_category + "  и номеру конфига  " + Main_number_config + "  Поставленный тип переменной № " + Number_find_stat_class + " (имя " + _name + ")" + " не соответсвует типу искомой переменной!" + " (Ищут " + _find_type + ", а в переменной стоит тип " + _current_type + ")");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(Stat_array.Length > 0)
            for (int x = 0; x < Stat_array.Length; x++)
                {
                    Stat_array[x].Preparation();
                }
        }
#endif

    }


    public enum Config_enum
    {
        Int,
        Float,
        Vector2,
        Vector3,
        Color,
        Bool,
        String,
        GameObject

    }

    [Serializable]
    public class Stat_class
    {

        [field: Tooltip("Имя параметра (нужно для того, что бы он нашёл его по имени)")]
        [field: SerializeField]
        public string Name { get; private set; } = "Имя параметра";

        [Tooltip("Тип переменной")]
        [SerializeField]
        public Config_enum Type = Config_enum.Float;

        #region Переменные (нужно подумать над улучшением...)

        [HideInInspector]
        public bool Float_bool = true;

        [field: ConditionalHide(nameof(Float_bool), true)]
        [field: Tooltip("Значение параметра")]
        [field: SerializeField]
        public float Value_float { get; private set; } = 0;

        [HideInInspector]
        public bool Integer_bool = true;

        [field: ConditionalHide(nameof(Integer_bool), true)]
        [field: Tooltip("Значение параметра")]
        [field: SerializeField]
        public int Value_int { get; private set; } = 0;

        [HideInInspector]
        public bool Vector2_bool = true;

        [field: ConditionalHide(nameof(Vector2_bool), true)]
        [field: Tooltip("Значение параметра")]
        [field: SerializeField]
        public Vector2 Value_vector2 { get; private set; } = Vector2.zero;

        [HideInInspector]
        public bool Vector3_bool = true;

        [field: ConditionalHide(nameof(Vector3_bool), true)]
        [field: Tooltip("Значение параметра")]
        [field: SerializeField]
        public Vector3 Value_vector3 { get; private set; } = Vector3.zero;

        [HideInInspector]
        public bool Color_bool = true;

        [field: ConditionalHide(nameof(Color_bool), true)]
        [field: Tooltip("Значение параметра")]
        [field: SerializeField]
        public Color Value_color { get; private set; } = Color.white;

        [HideInInspector]
        public bool Boolean_bool = true;

        [field: ConditionalHide(nameof(Boolean_bool), true)]
        [field: Tooltip("Значение параметра")]
        [field: SerializeField]
        public bool Value_bool { get; private set; } = false;

        [HideInInspector]
        public bool String_bool = true;

        [field: ConditionalHide(nameof(String_bool), true)]
        [field: Tooltip("Значение параметра")]
        [field: SerializeField]
        public string Value_string { get; private set; } = "Чего нада?";

        [HideInInspector]
        public bool GameObject_bool = true;

        [field: ConditionalHide(nameof(GameObject_bool), true)]
        [field: Tooltip("Значение параметра")]
        [field: SerializeField]
        public GameObject Value_gameObject { get; private set; } = null;
        #endregion

        public void Preparation()
        {
            Float_bool = false;
            Integer_bool = false;
            Vector2_bool = false;
            Vector3_bool = false;
            Color_bool = false;
            Boolean_bool = false;
            String_bool = false;
            GameObject_bool = false;

            switch (Type)
            {
                case Config_enum.Float:
                    Float_bool = true;
                    break;
                case Config_enum.Int:
                    Integer_bool = true;
                    break;
                case Config_enum.Vector2:
                    Vector2_bool = true;
                    break;
                case Config_enum.Vector3:
                    Vector3_bool = true;
                    break;
                case Config_enum.Color:
                    Color_bool = true;
                    break;
                case Config_enum.Bool:
                    Boolean_bool = true;
                    break;
                case Config_enum.String:
                    String_bool = true;
                    break;
                case Config_enum.GameObject:
                    GameObject_bool = true;
                    break;
            }
        }
    }

}
