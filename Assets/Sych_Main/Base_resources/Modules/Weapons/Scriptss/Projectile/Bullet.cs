//Пуля
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Weapon / Projectile / Bullet")]
    [DisallowMultipleComponent]
    public class Bullet : Projectile_abstract
    {

        [Tooltip("Будет ли реагировать на другие снаряды")]
        [SerializeField]
        bool Trigger_projectile_bool = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<I_damage>() != null)
            {
                other.GetComponent<I_damage>().Add_Damage(Damage, Host);
            }

        }

        void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.GetComponent<I_damage>() != null)
            {
                collision.gameObject.GetComponent<I_damage>().Add_Damage(Damage, Host);

                if (!Punch_Through_bool)
                    Destroy_method();
            }
            else if (!Way_mode_bool)
            {
                if (collision.gameObject.GetComponent<Projectile_abstract>() && Trigger_projectile_bool)
                    Destroy_method();
                else if (!collision.gameObject.GetComponent<Projectile_abstract>())
                    Destroy_method();
            }

            if (collision.transform.GetComponent<Rigidbody>())
            {
                Rigidbody body = collision.transform.GetComponent<Rigidbody>();

                if (body != Body)
                {
                    
                    //foreach (ContactPoint contact in collision.contacts)
                    //{
                   //     body.AddForce((transform.position - contact.point).normalized * Body.mass * Force_Repulsion);
                    //}
                    
                    //print(Body.velocity * Body.mass);

                    //Vector3 direction = collision.contacts[0].point - Body.transform.position;
                    body.AddForceAtPosition(Body.velocity.normalized * Force_Repulsion, collision.contacts[0].point);

                    //body.AddForce((transform.position - collision.contacts[0].point).normalized * Body.mass * Force_Repulsion);
                }
            }
            
        }
        
    }
}