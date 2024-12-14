using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sych_scripts
{
    public class UI_weapon_administrator : Singleton<UI_weapon_administrator>
    {

        [Tooltip("������ ������")]
        [SerializeField]
        public Image Aim_image = null;

        [Tooltip("���������� ��������")]
        [SerializeField]
        TMP_Text[] Ammo_value_array = new TMP_Text[0];

        [Tooltip("���������� ��������� (��� �������� � ������)")]
        [SerializeField]
        TMP_Text[] Magazine_value_array = new TMP_Text[0];

        [Tooltip("���������� ������")]
        [SerializeField]
        Image Charged_value = null;

        [Tooltip("���� ������������ ������ �������")]
        [SerializeField]
        GameObject Only_ammo_menu = null;

        [Tooltip("���� ������������ ������� � �������� �������� (����� � �������� �������)")]
        [SerializeField]
        GameObject Ammo_and_magazine_menu = null;

        [Tooltip("���� ������������ ������� ������")]
        [SerializeField]
        GameObject Charged_menu = null;

        /// <summary>
        /// ����� �������� ����� ������������
        /// </summary>
        /// <param name="_only_ammo_bool">���� ������������ ������ �������</param>
        /// <param name="_ammo_and_magazine_bool">���� ������������ ������� � �������� (�������� �������)</param>
        /// <param name="_charged_bool">���� ������������ ������� ������ (����� ����� ������� ��� ���������� ��������)</param>
        public void Activity (bool _only_ammo_bool, bool _ammo_and_magazine_bool, bool _charged_bool)
        {
            Only_ammo_menu.SetActive(false);
            Ammo_and_magazine_menu.SetActive(false);
            Charged_menu.SetActive(false);

            if (_only_ammo_bool)
                Only_ammo_menu.SetActive(true);

            if(_ammo_and_magazine_bool)
                Ammo_and_magazine_menu.SetActive(true);

            if (_charged_bool)
                Charged_menu.SetActive(true);
        }

        /// <summary>
        /// ������ ��������� ��������� ��������
        /// </summary>
        /// <param name="_value"></param>
        public void Update_Ammo_value(int _value)
        {
            foreach (TMP_Text text in Ammo_value_array)
            {
                text.text = _value.ToString();
            }
        }

        /// <summary>
        /// ������ ��������� ��������� ��������� (�������� �������� ��������)
        /// </summary>
        /// <param name="_value"></param>
        public void Update_Magazine_value(int _value)
        {
            foreach (TMP_Text text in Magazine_value_array)
            {
                text.text = _value.ToString();
            }
        }

        /// <summary>
        /// ������ ��������� ������� ������
        /// </summary>
        /// <param name="_value">�������� �� 0 �� 1</param>
        public void Update_Charged_value(float _value)
        {
            Charged_value.fillAmount = _value;
        }

        /// <summary>
        /// �������� ������ ������� ��-�� ��������
        /// </summary>
        public void Change_spread_Aim()
        {

        }

        /// <summary>
        /// �������� ���������� �������
        /// </summary>
        public void Activity_Aim(bool _activity)
        {
            Aim_image.enabled = _activity;
        }
    }
}