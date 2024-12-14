//�������� ������� � ����������� ����� (���� ����� ����� ������ �����, �� � �� ���� ���), ����� ���� ���������� �����
//�������� ����� ��� ����������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Handlers / Detect handler")]
    [DisallowMultipleComponent]
    public class Detect_handler : MonoBehaviour
    {

        [Tooltip("����� ������� ������������ ��� ���������� �������")]
        public UnityEvent Event_detect = new UnityEvent();

        [Tooltip("����� ������� ������������ ��� ���������� �������� � ������� ��������� ������")]
        public UnityEvent<Transform> Event_detect_Object_array = new UnityEvent<Transform>();

        [Tooltip("������ ��������")]
        [SerializeField]
        float Radius_detect = 1f;

        [Tooltip("���� ������������� ��������")]
        [SerializeField]
        LayerMask Layer = 1 << 0;

        [Tooltip("��� ������� ��������")]
        [SerializeField]
        string Name_find_tag = null;

        bool Debug_mode = false;

        Color Color_debug = new Color(1, 0, 0, 0.2f);

        private void LateUpdate()
        {
            Detect_player();
        }

        /// <summary>
        /// ��������
        /// </summary>
        void Detect_player()
        {
            Collider[] check_array = Physics.OverlapSphere(transform.position, Radius_detect, Layer);

            if (check_array.Length > 0)
            {
                if (Name_find_tag != null)
                {

                    for (int x = 0; x < check_array.Length; x++)
                    {
                        if (check_array[x].tag == Name_find_tag)
                        {
                            Event_detect_Object_array.Invoke(check_array[x].transform);

                            break;
                        }

                    }
                }
                else
                {
                    Event_detect.Invoke();
                }
            }

        }


        private void OnDrawGizmos()
        {
            if (Debug_mode)
            {
                Gizmos.color = Color_debug;
                Gizmos.DrawSphere(transform.position, Radius_detect);
            }
        }

    }
}