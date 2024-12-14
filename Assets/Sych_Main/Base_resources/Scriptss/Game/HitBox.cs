//Триггер нанесения урона
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Battle / HitBox")]
    [DisallowMultipleComponent]
    public class HitBox : MonoBehaviour
    {
        [Tooltip("Нанесения урона")]
        [SerializeField]
        [Min(1)]
        int Damage = 1;




        [Tooltip("Можно ли атаковать своих же")]
        [SerializeField]
        bool Friendly_fire = false;

        [ShowIf(nameof(Friendly_fire))]
        [Tooltip("Владелец (тот кто наносит урон)")]
        [SerializeField]
        Game_character_abstract My_Character = null;




        [Tooltip("Может ли владелец этой атаки нанести себе урон?")]
        [SerializeField]
        bool Self_damage = true;

        [HideIf(nameof(Self_damage))]
        [Tooltip("Здоровье владельца (что бы не нанести урон самому себе)")]
        [SerializeField]
        Health My_Health = null;




        [Tooltip("Будет ли толкать объект при попадание в триггер")]
        [SerializeField]
        bool Pushing_bool = false;

        [ShowIf(nameof(Pushing_bool))]
        [Tooltip("Сила толкания")]
        [SerializeField]
        float Force_push = 5000f;

        [ShowIf(nameof(Pushing_bool))]
        [Tooltip("Если не Null, то от этого объекта будет расчет толкания")]
        [SerializeField]
        Transform Transform_pusher = null;

        [ShowIf(nameof(Pushing_bool))]
        [Tooltip("Направление толкания относительно триггер (или объекта")]
        [SerializeField]
        Vector3 Direction_push = Vector3.forward;

        private void OnTriggerEnter(Collider other)
        {

            if (other.GetComponent<I_damage>() != null)
            {
                Target_damage(other.GetComponent<I_damage>(), other.gameObject);
            }


            if (other.GetComponent<Rigidbody>() && Pushing_bool)
            {
                Pushing_target(other.GetComponent<Rigidbody>());
            }

        }

        void Pushing_target(Rigidbody _body_push)
        {
            Transform main_transform = transform;

            if (Transform_pusher)
                main_transform = Transform_pusher;

            /*
            Vector3 direction = transform.position - main_transform.position;
            direction.Normalize();
            direction.x += Direction_push.x;
            direction.y += Direction_push.y;
            direction.z += Direction_push.z;
            */
            Vector3 direction = main_transform.forward * Direction_push.z + main_transform.up * Direction_push.y + main_transform.right * Direction_push.x;

            _body_push.AddForce(direction * Force_push);
        }


        /// <summary>
        /// Нанести урон цели
        /// </summary>
        /// <param name="_target_interface">Интерфейс нанесения урона</param>
        /// /// <param name="_target">Цель</param>
        public void Target_damage(I_damage _target_interface, GameObject _target)
        {
            if (Check_condition_Friendly_fire(_target) && Check_condition_Self_damage(_target_interface))
            {
                _target_interface.Add_Damage(Damage, My_Character);
            }
        }


        /// <summary>
        /// Узнать подходит ли цель под условие "Огонь по своим"
        /// </summary>
        /// <param name="_target">Цель</param>
        /// <returns></returns>
        bool Check_condition_Friendly_fire(GameObject _target)
        {
            bool result = false;

            if (Friendly_fire)
            {
                result = true;
            }
            else
            {
                if (_target.GetComponent<Game_character_abstract>())
                {
                    if (_target.GetComponent<Game_character_abstract>().Fraction_name != My_Character.Fraction_name)
                    {
                        result = true;
                    }
                }
                else
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Узнать подходит ли цель под условие "Самоповреждение" (можно нанести себе урон)
        /// </summary>
        /// <param name="_target">Цель</param>
        /// <returns></returns>
        bool Check_condition_Self_damage(I_damage _target_interface)
        {
            bool result = false;

            if (Self_damage)
            {
                result = true;
            }
            else
            {
                    if (_target_interface.Main_health != My_Health || _target_interface.Main_health == null)
                    {
                        result = true;
                    }
            }

            return result;
        }

    }
}