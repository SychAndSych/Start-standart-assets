//Всякие "полезные" методы
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    public static class Game_Method
    {

        /// <summary>
        /// Телепортировать цель
        /// </summary>
        /// <param name="_target">Цель</param>
        /// <param name="_final_point">Место куда телепортируем</param>
        /// <param name="_reset_physics">Сбросить скорость движения?</param>
        public static void Teleport(Transform _target, Vector3 _final_point, bool _reset_physics)
        {
            {
                if (_reset_physics)
                {
                    if (_target.GetComponent<Rigidbody>())
                        _target.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }

                if (_target.GetComponent<Player_character>())
                {
                    _target.GetComponent<Player_character>().Player_Motor_script.Forced_Teleport(_final_point);
                }
                else
                {
                    _target.position = _final_point;
                }

            }
        }
    }
}