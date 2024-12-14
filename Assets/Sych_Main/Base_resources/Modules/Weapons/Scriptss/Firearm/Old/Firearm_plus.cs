using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    //[AddComponentMenu("Sych scripts / Game / Weapon / Firearm plus")]
    [DisallowMultipleComponent]
    public class Firearm_plus : Firearm
    {

        [Tooltip("Префаб супер пули")]
        [SerializeField]
        Projectile_abstract Super_Bullet_prefab = null;


        protected void Fire_id(int _id_attack)
        {
            if (gameObject.activeSelf)
            {
                switch (_id_attack)
                {
                    case 0:
                        //Fire();
                        break;

                    case 1:
                        Fire_super_1();
                        break;

                    case 2:
                        Fire_super_2();
                        break;

                    case 3:
                        Fire_super_3();
                        break;

                    case 4:
                        Fire_super_4();
                        break;
                    default:
                        //Fire_normal();
                        break;
                }

                Attack_mode_id = 0;
            }
        }


        /// <summary>
        /// Стреляет несколько раз подряд с минимальной задержкой
        /// </summary>
        void Fire_super_1()
        {
            StartCoroutine(Coroutine_super_attack());
        }

        IEnumerator Coroutine_super_attack()
        {

            for (int x = 0; x < 15; x++)
            {
                //Fire_normal();

                yield return new WaitForSeconds(0.01f);
            }

        }

        /// <summary>
        /// Стреояет горизольтальным веером стрел
        /// </summary>
        void Fire_super_2()
        {
            Vector3 save_rotation = Fire_point.eulerAngles;

            for (int x = 0; x < 6; x++)
            {
                Fire_point.rotation = Quaternion.Euler(save_rotation + new Vector3(0, 2 * x, 0));
                //Fire_normal();
            }

            for (int x = 1; x < 6; x++)
            {
                Fire_point.rotation = Quaternion.Euler(save_rotation + new Vector3(0, -1 * x, 0));
                //Fire_normal();
            }
        }

        void Fire_super_3()
        {
            if (Damage > 0)
                Instantiate(Super_Bullet_prefab, Fire_point.position, Fire_point.rotation).Specify_settings(Damage);
            else
                Instantiate(Super_Bullet_prefab, Fire_point.position, Fire_point.rotation);
        }

        void Fire_super_4()
        {
            float density = 0.3f;

            Spawn_arrow_super_attack_4(new Vector3(0, 0, 0));

            for (int x = 1; x < 5; x++)
            {
                for (int z = 1; z < 5; z++)
                {
                    Spawn_arrow_super_attack_4(new Vector3(density * x, 0, 0));
                    Spawn_arrow_super_attack_4(new Vector3(-density * x, 0, 0));
                    Spawn_arrow_super_attack_4(new Vector3(0, 0, density * z));
                    Spawn_arrow_super_attack_4(new Vector3(0, 0, -density * z));
                }
            }

            for (int x = 1; x < 5; x++)
            {
                for (int z = 1; z < 5; z++)
                {
                    Spawn_arrow_super_attack_4(new Vector3(density * x, 0, density * z));
                    Spawn_arrow_super_attack_4(new Vector3(-density * x, 0, density * z));
                    Spawn_arrow_super_attack_4(new Vector3(density * x, 0, -density * z));
                    Spawn_arrow_super_attack_4(new Vector3(-density * x, 0, -density * z));
                }
            }

        }

        void Spawn_arrow_super_attack_4(Vector3 _position_addination)
        {
            Vector3 position_attack = (Finale_point + (Vector3.up * (Random.Range(10, 15)))) + _position_addination;

            Projectile_abstract bulet = null;

            bulet = Instantiate(Projectile_prefab, position_attack, Quaternion.Euler(90, 0, 0));

            if (Damage > 0)
                bulet.Specify_settings(Damage);
        }

    }
}