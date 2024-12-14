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

        [Tooltip("Активировать ивент при вхождение цели в поле действия")]
        [SerializeField]
        UnityEvent OnTriggerEnter_event = new UnityEvent();

        [Tooltip("Цель которая должна войти в триггер (компонент, имя объекта или по тегу. Зависит от выбранного варианта ниже.)")]
        [SerializeField]
        string Target_name = null;

        [Tooltip("Тип детекции (что именно искать будем)")]
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
        /// Активировать
        /// </summary>
        void Activation()
        {
            OnTriggerEnter_event.Invoke();
        }

    }
}