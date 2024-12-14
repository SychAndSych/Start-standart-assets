//Активирует анимацию по требованию
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / General / Animation activation")]
    [DisallowMultipleComponent]
    public class Animation_activation : MonoBehaviour
    {
        [Tooltip("Аниматор")]
        [SerializeField]
        Animator Anim = null;

        string Name_value = null;

        /// <summary>
        /// Запустить насильно нужную анимацию
        /// </summary>
        /// <param name="_name_animation">Имя анимации</param>
        public void Play_animation(string _name_animation)
        {
            Anim.Play(_name_animation);
        }

        /// <summary>
        /// Установить bool переменную в true
        /// </summary>
        /// <param name="_name_animation">название bool переменной</param>
        public void Set_bool_true(string _name_bool)
        {
            Anim.SetBool(_name_bool, true);
        }

        /// <summary>
        /// Установить bool переменную в false
        /// </summary>
        /// <param name="_name_animation">название bool переменной</param>
        public void Set_bool_false(string _name_bool)
        {
            Anim.SetBool(_name_bool, false);
        }

        /// <summary>
        /// Установить trigger переменную
        /// </summary>
        /// <param name="_name_animation">название trigger переменной</param>
        public void Set_trigger(string _name_trigger)
        {
            Anim.SetTrigger(_name_trigger);
        }

        /// <summary>
        /// Сохранить имя переменной
        /// </summary>
        /// <param name="_name">имя переменной</param>
        public void Save_name(string _name)
        {
            Name_value = _name;
        }

        /// <summary>
        /// Установить int переменную
        /// </summary>
        /// <param name="_name_animation">название int переменной</param>
        public void Set_int(int _value_int)
        {
            Anim.SetInteger(Name_value, _value_int);
        }

        /// <summary>
        /// Установить float переменную
        /// </summary>
        /// <param name="_name_animation">название float переменной</param>
        public void Set_float(float _value_float)
        {
            Anim.SetFloat(Name_value, _value_float);
        }

        /// <summary>
        /// Ресетнуть триггер по имени
        /// </summary>
        /// <param name="_name">имя триггера</param>
        public void Reset_trigger_name(string _name)
        {
            Anim.ResetTrigger(_name);
        }

        /// <summary>
        /// Ресетнуть триггер по id
        /// </summary>
        /// <param name="_name">id триггера</param>
        public void Reset_trigger_id(int _id)
        {
            Anim.ResetTrigger(_id);
        }

    }
}