//Коннектит к поверхности движущейся платформы объекты
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    enum Connect_enum
    {
        Children,
        ToPosition
    }

    [AddComponentMenu("Sych scripts / Game / Movement / Connect to active platform")]
    [DisallowMultipleComponent]
    public class Connect_to_active_platform : MonoBehaviour
    {
        [Tooltip("Владелец к которому коннектиться объекты")]
        [SerializeField]
        Transform Owner = null;

        [Tooltip("Физика владельца")]
        [SerializeField]
        Rigidbody Body_owner = null;

        [Tooltip("Тип коннектра (при Children просто назначает родителя, при ToPosition сопровождает своими координатами")]
        [SerializeField]
        Connect_enum Connect_type = Connect_enum.Children;

        List<Transform> Connect_objects_list = new List<Transform>();

        Vector3 Old_position = Vector3.zero;

        private Quaternion activeLocalPlatformRotation;
        private Quaternion activeGlobalPlatformRotation;

        private void Start()
        {
            Old_position = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (!other.isTrigger)
            Connect(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.isTrigger)
                Disconnect(other.transform);
        }


        private void FixedUpdate()
        {
            Move_connect_objects();
        }

        /// <summary>
        /// Перемещает объекты из листа
        /// </summary>
        void Move_connect_objects()
        {
            Vector3 active_position = transform.position;

            Vector3 direction_move = active_position - Old_position;

            if (Connect_objects_list.Count > 0)
            {
                foreach(Transform obj in Connect_objects_list)
                {
                    if (obj.GetComponent<CharacterController>())
                        obj.GetComponent<CharacterController>().Move(direction_move);
                    else if (obj.GetComponent<Rigidbody>())
                    {
                        print(obj.name);
                        obj.GetComponent<Rigidbody>().MovePosition(obj.GetComponent<Rigidbody>().velocity + Body_owner.velocity);
                    }
                    else
                    {
                        obj.Translate(direction_move);
                    }
                    
                }
            }

            if (Old_position != transform.position)
                Old_position = transform.position;
        }

        /// <summary>
        /// Присойденить объект как дочерний
        /// </summary>
        /// <param name="_other">Цель коннекта</param>
        void Connect(Transform _other)
        {
            if ((_other.gameObject.GetComponent<Rigidbody>() && !_other.gameObject.GetComponent<Rigidbody>().isKinematic) || _other.gameObject.GetComponent<CharacterController>()) 
            {
                if (Connect_type == Connect_enum.Children)
                {
                    if (Owner)
                        _other.transform.SetParent(Owner);
                    else
                        _other.transform.SetParent(transform);
                }
                else if (Connect_type == Connect_enum.ToPosition)
                {
                    if (!Check_in_list(_other))
                    {
                        Connect_objects_list.Add(_other);
                    }
                }
            }
        }

        /// <summary>
        /// Отсойденить объект (сделать его самостоятельным, а не дочерним)
        /// </summary>
        /// <param name="_other">Цель которую отсойденяем</param>
        void Disconnect(Transform _other)
        {
            if ((_other.gameObject.GetComponent<Rigidbody>() && !_other.gameObject.GetComponent<Rigidbody>().isKinematic) || _other.gameObject.GetComponent<CharacterController>())
            {
                if (Connect_type == Connect_enum.Children)
                {
                    _other.transform.SetParent(null);
                }
                else if (Connect_type == Connect_enum.ToPosition)
                {
                    if (Check_in_list(_other))
                    {
                        Connect_objects_list.Remove(_other);
                    }
                }
            }
        }

        /// <summary>
        /// Проверить есть ли в списке
        /// </summary>
        /// <param name="_target">Цель проверки</param>
        /// <returns></returns>
        bool Check_in_list(Transform _target)
        {
            bool result = false;

            foreach(Transform obj in Connect_objects_list)
            {
                if (_target == obj)
                    result = true;
            }

            return result;
        }

    }
}