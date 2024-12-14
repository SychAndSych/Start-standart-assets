using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    enum Direction_enum
    {
        X,
        Y,
        Z
    }


    [AddComponentMenu("Sych scripts / Game / Arcade / Trampoline")]
    [DisallowMultipleComponent]
    public class Trampoline : MonoBehaviour
    {

        [Tooltip("Сила толчка")]
        [SerializeField]
        float Force_physical_push = 20f;

        [Tooltip("Сила толчка (для не физики")]
        [SerializeField]
        float Force_motor_push = 10f;

        [Tooltip("Каждый раз старается толкать одинаково (для этого гасит изначальную силу физики)")]
        [SerializeField]
        bool Fixed_physics_bool = true;

        [Tooltip("Направление мира (то есть, если наклонить платформу, то она без изменений будет толкать)")]
        [SerializeField]
        bool World_direction_bool = true;

        //[ShowIf(nameof(World_direction_bool))]
        [Tooltip("Направление толкания цели")]
        [SerializeField]
        Vector3 Direction = Vector3.up;

        [HideIf(nameof(World_direction_bool))]
        [Tooltip("Тип направления")]
        [SerializeField]
        Direction_enum Direction_type = Direction_enum.Y;

        [Tooltip("Что бы увидеть больше")]
        [SerializeField]
        bool Gizmos_bool = false;

        private void OnTriggerEnter(Collider other)
        {
            Push_to_target(other.transform);
        }

        /// <summary>
        /// Толкнуть объект (цель) 
        /// </summary>
        /// <param name="_target">Цель-объект</param>
        void Push_to_target(Transform _target)
        {
            if (_target.GetComponent<Rigidbody>())
            {
                if (Fixed_physics_bool)
                {
                    StartCoroutine(Coroutine_push_gravity(_target));
                }
                else
                {
                    _target.GetComponent<Rigidbody>().AddForce(Find_out_direction * Force_physical_push, ForceMode.Impulse);
                }
                
            }

            else if (_target.GetComponent<Player_Motor>())
            {
                _target.GetComponent<Player_Motor>().Forced_Jump(Force_motor_push);
            }
        }

        IEnumerator Coroutine_push_gravity(Transform _target)
        {
            Rigidbody body = _target.GetComponent<Rigidbody>();

            body.velocity = Vector3.zero;

            yield return new WaitForSeconds(0.01f);

            body.AddForce(Find_out_direction * Force_physical_push, ForceMode.Impulse);
        }


        /// <summary>
        /// Получить направление
        /// </summary>
        Vector3 Find_out_direction
        {
            get
            {
                Vector3 local_direction = transform.rotation.eulerAngles;

                switch (Direction_type)
                {
                    case Direction_enum.X:
                        local_direction = transform.right;
                        break;

                    case Direction_enum.Y:
                        local_direction = transform.up;
                        break;

                    case Direction_enum.Z:
                        local_direction = transform.forward;
                        break;
                }

                Vector3 direction = World_direction_bool ? Direction : local_direction + Direction;

                return direction;
            }
        }

        private void OnDrawGizmos()
        {
            if (Gizmos_bool)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawLine(transform.position, transform.position + Find_out_direction * 5f);
            }
        }
    }
}