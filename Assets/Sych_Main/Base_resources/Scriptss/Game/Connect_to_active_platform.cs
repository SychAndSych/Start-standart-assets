//��������� � ����������� ���������� ��������� �������
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
        [Tooltip("�������� � �������� ������������ �������")]
        [SerializeField]
        Transform Owner = null;

        [Tooltip("������ ���������")]
        [SerializeField]
        Rigidbody Body_owner = null;

        [Tooltip("��� ��������� (��� Children ������ ��������� ��������, ��� ToPosition ������������ ������ ������������")]
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
        /// ���������� ������� �� �����
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
        /// ������������ ������ ��� ��������
        /// </summary>
        /// <param name="_other">���� ��������</param>
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
        /// ����������� ������ (������� ��� ���������������, � �� ��������)
        /// </summary>
        /// <param name="_other">���� ������� �����������</param>
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
        /// ��������� ���� �� � ������
        /// </summary>
        /// <param name="_target">���� ��������</param>
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