//������������ ����� (�� �� ������ ������� ������ ������ ��������)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Weapon / Explosion")]
    public class Explosion_script : MonoBehaviour
    {
        [Tooltip("��������� ����")]
        [SerializeField]
        int Damage = 10;

        [Tooltip("������� ������")]
        [SerializeField]
        GameObject PS_explosion_prefab = null;

        [Tooltip("������ ����������� ������")]
        [SerializeField]
        float Radius_explosion = 12f;

        [Tooltip("���� ������")]
        [SerializeField]
        float Force_explosion = 24f;

        [Tooltip("��������� ������ ��� �����������")]
        [SerializeField]
        bool Through_mode_bool = true;

        [HideIf(nameof(Through_mode_bool))]
        [Tooltip("����������� ������� ��������� �����")]
        [SerializeField]
        LayerMask Obstacles_layer = 1;

        Game_character_abstract Host = null;//��� ��� ������� �����

        [Space(20)]
        [Header("������")]

        [Tooltip("����� ������� (���������� ������ ��� �����������)")]
        [SerializeField]
        bool Debug_bool = false;


        /// <summary>
        /// �����
        /// </summary>
        [ContextMenu(nameof(Explosion))]
        public void Explosion()
        {
            /*
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, Radius_explosion, transform.forward, out hit))
            {
                print(1);
                if (hit.transform.GetComponent<I_damage>() != null && (Through_mode_bool || Check_no_obstacles(hit.transform) && !Through_mode_bool))
                {
                    print(2);
                    hit.transform.GetComponent<I_damage>().Add_Damage(Damage, Host);
                }
            }
            */

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, Radius_explosion);
            foreach (Collider hit in colliders)
            {
                if (hit.transform.GetComponent<I_damage>() != null && (Through_mode_bool || Check_no_obstacles(hit.transform) && !Through_mode_bool))
                {
                    hit.transform.GetComponent<I_damage>().Add_Damage(Damage, Host);
                }
            }

            if (PS_explosion_prefab)
            Instantiate(PS_explosion_prefab, transform.position, Quaternion.identity);

            Invoke(nameof(Explosion_impulse), 0.01f);
        }


        /// <summary>
        /// ���������� �����
        /// </summary>
        void Explosion_impulse()
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, Radius_explosion);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null && (Through_mode_bool || Check_no_obstacles(hit.transform) && !Through_mode_bool))
                    rb.AddExplosionForce(Force_explosion, explosionPos, Radius_explosion, 3.0F, ForceMode.Impulse);
            }
            Destroy(gameObject);
        }

        /// <summary>
        /// ��������� ����� �� ������ �� ������������ ������� ����� �� ����� ������
        /// </summary>
        /// <param name="_point_target">����</param>
        /// <returns></returns>
        bool Check_no_obstacles (Transform _point_target)
        {
                bool result = Physics.Linecast(transform.position, _point_target.position, Obstacles_layer);

                return !result;
        }

        private void OnDrawGizmos()
        {

            if (Debug_bool)
            {

                Gizmos.color = new Color(1, 0, 0, 0.4f);
                Gizmos.DrawSphere(transform.position, Radius_explosion);
            }

        }
    }
}