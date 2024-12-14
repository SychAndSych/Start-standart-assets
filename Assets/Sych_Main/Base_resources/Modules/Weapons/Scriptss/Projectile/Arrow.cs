using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Weapon / Projectile / Arrow")]
    [DisallowMultipleComponent]
    public class Arrow : Projectile_abstract
    {
        [Space(20)]

        [SerializeField]
        float Velocity_multiply = 4;

        [SerializeField]
        float Angular_velocity_multiply = 0.2f;

        [HideIf(nameof(No_time_destroy))]
        [Tooltip("Время до самоуничтожения после застревания стрелы")]
        [SerializeField]
        [Min(0.001f)]
        float Time_destroy_arrow = 20f;

        [Tooltip("Выключает таймер самоуничтожения для застрявшей стрелы")]
        [SerializeField]
        bool No_time_destroy = false;

        bool Active_bool = true;//Активность стрелы (летит ли она куда то сейчас)

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if(Active_bool)
            Torque();
        }

        void Torque()
        {
            Vector3 cross = Vector3.Cross(transform.forward, Body.velocity.normalized);

            Body.AddTorque(cross * Body.velocity.magnitude * Velocity_multiply);
            Body.AddTorque((-Body.angularVelocity + Vector3.Project(Body.angularVelocity, transform.forward)) * Body.velocity.magnitude * Angular_velocity_multiply);
        }

        protected override void Off_rigidbody_arrow()
        {
            base.Off_rigidbody_arrow();

            Active_bool = false;

            if (!No_time_destroy)
            {
                Time_destroy = Time_destroy_arrow;
                StartCoroutine(Destroy_coroutine());
            }


        }

        protected override void Destroy_method()
        {

        }

        protected override void Detect_raycast(RaycastHit _hit)
        {
            base.Detect_raycast(_hit);

            if (!Punch_Through_bool || _hit.transform.GetComponent<I_damage>() == null)
            {
                Off_rigidbody_arrow();

                transform.position = _hit.point;

                //transform.rotation = Quaternion.LookRotation(-hit.normal);//Поворот по нормале сопрокосновения

                transform.SetParent(_hit.transform, true);


                Raycast_active_bool = false;
            }
        }
    }
}