using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Sych_scripts
{
    [DisallowMultipleComponent]
    public class Firearm_shutter_logic : MonoBehaviour
    {
        #region ����������
        [Space(10)]
        [Tooltip("�������� ����� �������� � �������������� ������� (�������� ��� ����������� �������� � ��������)")]
        [SerializeField]
        bool Use_shutter_bool = false;

        internal bool Shutter_bool { get; private set; } = true;//��������� �� "������"

        [ShowIf(nameof(Use_shutter_bool))]
        [Tooltip("�������������� ����� ��, �� ��� �� ������ �� ����� �������� ����� ������������� ������� (�����, ��� ����, ��� �� �� ������� ��� � ��������������� ���������)")]
        [SerializeField]
        float Additional_time_stop_shutter_reload = 0.2f;

        [ShowIf(nameof(Use_shutter_bool))]
        [Tooltip("������� ����������� � ������ ������������� ������� (� �������� ��� ������� ��������, ������� � ����� ����������� End_shutter_reload)")]
        [SerializeField]
        UnityEvent Start_shutter_reload_event = new UnityEvent();

        #endregion


        #region ��������� ������

        private void OnEnable()
        {
            Preparation_shutter();
        }


        #endregion


        #region ������

        /// <summary>
        /// ��������� ���������������� (����� ��������� �� ���� ������)
        /// </summary>
        void Preparation_shutter()
        {
            if (Use_shutter_bool)
            {
                if (!Shutter_bool)
                {
                    Shutter_reload();
                }
            }
        }


        /// <summary>
        /// �������������� ����� ��, �� ��� �� ������ �� ����� �������� ����� ������������� ������� (�����, ��� ����, ��� �� �� ������� ��� � ��������������� ���������)
        /// </summary>
        void Additional_time_shutter_reload_method()
        {
            Shutter_bool = true;
        }

        #endregion


        #region ��������� ������

        /// <summary>
        /// ������ ������������� �������
        /// </summary>
        public void Shutter_reload()
        {
            if (Use_shutter_bool)
            {
                Shutter_bool = false;
                Start_shutter_reload_event.Invoke();
            }

        }

        /// <summary>
        /// ����� ������������� "�������"
        /// </summary>
        public void End_reload_shutter()
        {
            Invoke(nameof(Additional_time_shutter_reload_method), Additional_time_stop_shutter_reload);
        }

        /// <summary>
        /// ��������� �����
        /// </summary>
        public void Early_reset()
        {
            Shutter_bool = true;
        }
        #endregion

    }
}