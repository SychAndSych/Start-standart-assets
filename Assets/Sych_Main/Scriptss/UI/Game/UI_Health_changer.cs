using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / UI / Health changer")]
    public class UI_Health_changer : MonoBehaviour
    {
        [Tooltip("����������� �������")]
        [SerializeField]
        Image Standart_image = null;

        [Tooltip("����������� �������")]
        [SerializeField]
        Image Harm_image = null;

        [Tooltip("������� �������")]
        [SerializeField]
        Image Heal_image = null;

        [Tooltip("�������� ��������� ��������")]
        [SerializeField]
        float Speed = 0.001f;

        float Active_Speed = 0;

        [Tooltip("�������������� �������� ��������� � ����������� �� ������� ��������� ������")]
        [SerializeField]
        float Acceleration = 0.01f;

        float Save_value = 1;
        float Active_value = 1;

        bool Active_bool = false;

        private void Update()
        {
            if (Active_bool)
                Update_change_health();
        }

        /// <summary>
        /// ��������� ���������� ��������
        /// </summary>
        void Update_change_health()
        {
            if (Save_value < Active_value)
            {
                Active_value = Mathf.MoveTowards(Active_value, Save_value, Active_Speed);
                Harm_image.fillAmount = Active_value;
            }
            else
            {
                Active_value = Mathf.MoveTowards(Active_value, Save_value, Active_Speed);
                Standart_image.fillAmount = Active_value;
            }
        }


        /// <summary>
        /// �������� ��������, ��� ���������� ��������
        /// </summary>
        /// <param name="_value"></param>
        public void Change(float _value)
        {
            Save_value = _value;
            Active_bool = true;

            float add_speed = Save_value < Active_value ? Active_value - Save_value : Save_value - Active_value;
            Active_Speed = Speed + Acceleration * add_speed;

            if (Save_value < Active_value)
            {
                Standart_image.fillAmount = _value;
                Heal_image.fillAmount = _value;
            }
            else
            {
                Heal_image.fillAmount = _value;
            }
        }
    }
}