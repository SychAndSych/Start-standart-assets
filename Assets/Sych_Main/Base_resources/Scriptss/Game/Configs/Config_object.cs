using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [System.Serializable]
    public class Config_object
    {
        [field: Tooltip("�������� ���������")]
        [field: SerializeField]
        public string Name_category { get; private set; } = "�������� ���������";

        [field: Expandable]
        [field: Tooltip("������ ��������")]
        [field: SerializeField]
        public Config_object_SO[] Object_array { get; private set; } = new Config_object_SO[0];
        //public Object_parameter[] Object_array { get; private set; } = new Object_parameter[0];
    }

    /*
    [System.Serializable]
    public class Object_parameter
    {
        [field: Tooltip("��� �������")]
        [field: SerializeField]
        public string Name { get; private set; } = null;


        //[Expandable]
        [field: Tooltip("������ ���������")]
        public Config_object_SO Personal_config = null;

    }
    */
}