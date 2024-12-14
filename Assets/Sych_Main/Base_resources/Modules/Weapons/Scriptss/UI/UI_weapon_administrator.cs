using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sych_scripts
{
    public class UI_weapon_administrator : Singleton<UI_weapon_administrator>
    {

        [Tooltip("Прицел игрока")]
        [SerializeField]
        public Image Aim_image = null;

        [Tooltip("Показатель патронов")]
        [SerializeField]
        TMP_Text[] Ammo_value_array = new TMP_Text[0];

        [Tooltip("Показатель магазинов (или патронов в запасе)")]
        [SerializeField]
        TMP_Text[] Magazine_value_array = new TMP_Text[0];

        [Tooltip("Показатель заряда")]
        [SerializeField]
        Image Charged_value = null;

        [Tooltip("Меню показывающее только патроны")]
        [SerializeField]
        GameObject Only_ammo_menu = null;

        [Tooltip("Меню показывающее патроны и запасные магазины (может и запасные патроны)")]
        [SerializeField]
        GameObject Ammo_and_magazine_menu = null;

        [Tooltip("Меню показывающее зарядку оружия")]
        [SerializeField]
        GameObject Charged_menu = null;

        /// <summary>
        /// Какие элементы нужно активировать
        /// </summary>
        /// <param name="_only_ammo_bool">Меню показывающие только патроны</param>
        /// <param name="_ammo_and_magazine_bool">Меню показывающие патроны и магазины (запасные патроны)</param>
        /// <param name="_charged_bool">Меню показывающее зарядку оружия (когда нужно держать для усиленного выстрела)</param>
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
        /// Меняет показания имеющихся патронов
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
        /// Меняет показания имеющихся магазинов (возможно запасных патронов)
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
        /// Менять показания зарядки оружия
        /// </summary>
        /// <param name="_value">Значение от 0 до 1</param>
        public void Update_Charged_value(float _value)
        {
            Charged_value.fillAmount = _value;
        }

        /// <summary>
        /// Изменить размер прицела из-за разброса
        /// </summary>
        public void Change_spread_Aim()
        {

        }

        /// <summary>
        /// Изменить активность прицела
        /// </summary>
        public void Activity_Aim(bool _activity)
        {
            Aim_image.enabled = _activity;
        }
    }
}