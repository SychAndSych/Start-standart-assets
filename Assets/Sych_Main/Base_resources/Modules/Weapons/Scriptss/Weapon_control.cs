//Стрелять из активного оружия
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sych_scripts;

enum Type_change_weapon
{
    Two_weapon,
    List_weapon
}

[AddComponentMenu("Sych scripts / Game / Weapon / Weapon control")]
public class Weapon_control : MonoBehaviour
{
    [Tooltip("Первичное оружие")]
    [SerializeField]
    Weapon_abstract First_wepoan = null;

    [Tooltip("Дополнительное оружие")]
    [SerializeField]
    Weapon_abstract Second_weapon = null;

    [Tooltip("Массив оружия")]
    [SerializeField]
    Weapon_abstract[] Weapon_array = new Weapon_abstract[0];

    Weapon_abstract Active_weapon = null;

    [Tooltip("Управление")]
    [SerializeField]
    Input_player Input = null;

    bool First_weapon_bool = true;

    int Id_weapon_active = 0;

    bool Press_bool = false;

    #region Системные методы
    private void Start()
    {
        Preparation();
    }

    private void Update()
    {
        if (Input.Attack_bool && Active_weapon != null)
        {
            Active_weapon.Attack(Input.Attack_bool);
            Press_bool = Input.Attack_bool;
        }
        else if (!Input.Attack_bool && Press_bool)
        {
            Active_weapon.Attack(Input.Attack_bool);
            Press_bool = Input.Attack_bool;
        }
    }
    #endregion


    #region Методы

    /// <summary>
    /// Подготовка
    /// </summary>
    void Preparation()
    {
        /*
        if (First_wepoan != null)
            First_wepoan.gameObject.SetActive(true);
        else if (Second_weapon != null)
            Second_weapon.gameObject.SetActive(true);

                if (Second_weapon != null)
            Second_weapon.gameObject.SetActive(false);
        */

        foreach(Weapon_abstract weapon in Weapon_array)
        {
            weapon.gameObject.SetActive(false);
        }

        Change_two_weapon();
        Change_two_weapon();

        Input.Reload_event.AddListener(Reload_weapon);
        Input.Change_two_weapon_event.AddListener(Change_two_weapon);
        Input.Change_scroll_weapon_event.AddListener(Change_scroll_weapon);
    }


    /// <summary>
    /// Перезарядить оружие
    /// </summary>
    void Reload_weapon()
    {
        if(Active_weapon != null && Active_weapon.GetComponent<I_reload>() != null)
        {
            Active_weapon.GetComponent<I_reload>().Reload();
        }
    }

    /// <summary>
    /// Меняет активное оружие между первичным и вторичным и обратно
    /// </summary>
    void Change_two_weapon()
    {
        if (Active_weapon != null)
            Active_weapon.gameObject.SetActive(false);

        if (!First_weapon_bool && First_wepoan != null)
        {
            Active_weapon = First_wepoan;
        }
        else if (First_weapon_bool && Second_weapon != null)
        {
            Active_weapon = Second_weapon;
        }

        if (Active_weapon != null)
            Active_weapon.gameObject.SetActive(true);

        First_weapon_bool = !First_weapon_bool;
    }

    /// <summary>
    /// Поменять оружие по колёсику мыши (можно и на другие клавиши)
    /// </summary>
    void Change_scroll_weapon(bool _next_weapon)
    {
        if (Active_weapon != null)
            Active_weapon.gameObject.SetActive(false);

        if (_next_weapon)
        {
            Id_weapon_active++;

            if (Weapon_array.Length <= Id_weapon_active)
                Id_weapon_active = 0;
        }

        else
        {
            Id_weapon_active--;

            if (0 > Id_weapon_active)
                Id_weapon_active = Weapon_array.Length - 1;
        }

        Active_weapon = Weapon_array[Id_weapon_active];


        if (Active_weapon != null)
            Active_weapon.gameObject.SetActive(true);
    }
    #endregion


    #region Публичные методы


    #endregion
}
