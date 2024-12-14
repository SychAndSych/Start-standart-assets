using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Movement / Rotation object")]
    [DisallowMultipleComponent]
    public class Rotation_object : MonoBehaviour
    {
        [Tooltip("Вращать по физике?")]
        [SerializeField]
        bool Physics_bool = false;

        [HideIf(nameof(Physics_bool))]
        [Tooltip("Что будем вращать")]
        [SerializeField]
        Transform Transform_target = null;

        [ShowIf(nameof(Physics_bool))]
        [Tooltip("Физика для физического вращения")]
        [SerializeField]
        Rigidbody Body = null;

        [Tooltip("Скорость поворота")]
        [SerializeField]
        float Speed_rotation = 0.1f;

        [Tooltip("Направление вращения")]
        [SerializeField]
        Vector3 Direction_rotation = new Vector3(0, 0, 1f);

        [Tooltip("Включено?")]
        [SerializeField]
        bool Active_bool = true;

        private void FixedUpdate()
        {
            if(Active_bool)
            Rotation_move();
        }

        /// <summary>
        /// Вращает объект
        /// </summary>
        void Rotation_move()
        {
            if (!Physics_bool)
            {
                if (Transform_target)
                {
                    Transform_target.Rotate(Direction_rotation * Speed_rotation * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(Direction_rotation * Speed_rotation * Time.deltaTime);
                }
            }
            else if (Physics_bool && Body)
            {
                Body.MoveRotation(Body.rotation * Quaternion.Euler(Direction_rotation * Speed_rotation * Time.fixedDeltaTime));
            }
        }
    }
}