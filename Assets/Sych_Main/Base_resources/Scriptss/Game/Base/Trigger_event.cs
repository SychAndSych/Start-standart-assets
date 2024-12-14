using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / Trigger event")]
    public class Trigger_event : MonoBehaviour
    {

        enum Enum_type_detect
        {
            Find_script,
            Find_obj_name,
            Find_tag
        }

        [Tooltip("������������ ����� ��� ��������� ���� � ���� ��������")]
        [SerializeField]
        UnityEvent OnTriggerEnter_event = new UnityEvent();

        [Tooltip("���� ������� ������ ����� � ������� (���������, ��� ������� ��� �� ����. ������� �� ���������� �������� ����.)")]
        [SerializeField]
        string Target_name = null;

        [Tooltip("��� �������� (��� ������ ������ �����)")]
        [SerializeField]
        Enum_type_detect Type_detect = Enum_type_detect.Find_script;

        private void OnTriggerEnter(Collider other)
        {
            switch (Type_detect)
            {
                case Enum_type_detect.Find_obj_name:
                    if (other.name == Target_name)
                    {
                        Activation();
                    }
                    break;

                case Enum_type_detect.Find_script:
                    if (other.GetComponent(Target_name) as MonoBehaviour)
                    {
                        Activation();
                    }
                    break;

                case Enum_type_detect.Find_tag:
                    if (other.tag == Target_name)
                    {
                        Activation();
                    }
                    break;
            }

        }

        /// <summary>
        /// ������������
        /// </summary>
        void Activation()
        {
            OnTriggerEnter_event.Invoke();
        }

    }
}