using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts 
{
    [AddComponentMenu("Sych scripts / Game / Weapon / Projectile / Rocket")]
    [DisallowMultipleComponent]
    public class Rocket : Projectile_abstract
    {
        [Tooltip("Эффекты взрыва")]
        [SerializeField]
        GameObject PS_explosion_prefab = null;

        [Tooltip("Будет ли реагировать на другие снаряды")]
        [SerializeField]
        bool Trigger_projectile_bool = false;

        [Tooltip("Радиус задеваемого взрыва")]
        [SerializeField]
        float Radius_explosion = 2f;

        [Tooltip("Сила взрыва")]
        [SerializeField]
        float Force_explosion = 8f;

        [Tooltip("Триггер взрыва")]
        [SerializeField]
        SphereCollider Trigger_explosion = null;

        bool Explosion_bool = false;

        [Space(20)]
        [Header("Разное")]

        [Tooltip("Режим отладки (показывает больше для Волшебников)")]
        [SerializeField]
        bool Debug_bool = false;

        #region Системные методы

        private void Awake()
        {
            Trigger_explosion.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Explosion_bool)
            {
                if (other.GetComponent<I_damage>() != null)
                {
                    other.GetComponent<I_damage>().Add_Damage(Damage, Host);

                    if (!Punch_Through_bool)
                        Explosion();
                }
                else
                {
                    if (other.GetComponent<Projectile_abstract>() && Trigger_projectile_bool)
                        Explosion();
                    else if (!other.GetComponent<Projectile_abstract>())
                        Explosion();
                }

                if (other.transform.GetComponent<Rigidbody>())
                {
                    other.transform.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position).normalized * Body.mass * Force_Repulsion);
                    //print(Body.velocity * Body.mass);
                }
            }
            else
            {
                if (other.GetComponent<I_damage>() != null)
                {
                    other.GetComponent<I_damage>().Add_Damage(Damage, Host);
                }
            }
        }
        #endregion



        #region Методы
        /// <summary>
        /// Физический взрыв
        /// </summary>
        void Explosion_impulse()
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, Radius_explosion);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(Force_explosion, explosionPos, Radius_explosion, 3.0F, ForceMode.Impulse);
            }
            Destroy(gameObject);
        }

        /// <summary>
        /// Взрыв
        /// </summary>
        void Explosion()
        {
            Explosion_bool = true;

            Instantiate(PS_explosion_prefab, transform.position, Quaternion.identity);
            Trigger_explosion.enabled = true;

            Invoke(nameof(Explosion_impulse), 0.1f);
        }
        #endregion



        #region Дополнительно
        private void OnDrawGizmos()
        {
            if (Trigger_explosion.radius != Radius_explosion)
            {
                Trigger_explosion.radius = Radius_explosion;
            }

            if (Debug_bool)
            {
                Gizmos.color = new Color(1, 0, 0, 0.2f);
                Gizmos.DrawSphere(transform.position, Radius_explosion);
            }

        }
        #endregion
    }
}