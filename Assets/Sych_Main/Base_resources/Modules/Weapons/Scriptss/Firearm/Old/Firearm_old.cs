//���������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    //[AddComponentMenu("Sych scripts / Game / Weapon / Firearm")]
    //[DisallowMultipleComponent]
    public class Firearm_old : Weapon_abstract
    {
        [Tooltip("����")]
        [SerializeField]
        [Min(0)]
        protected int Damage = 1;

        [Tooltip("����������������")]
        [SerializeField]
        float Fire_rate = 0.1f;

        bool Fire_On_bool = true;

        [Tooltip("������ ����")]
        [SerializeField]
        protected Projectile_abstract Projectile_prefab = null;

        [Tooltip("����� ������ ����")]
        [SerializeField]
        protected Transform Fire_point = null;

        [Tooltip("�������������� ����� (��� �� ����� ���� ������ ������ � ��������)")]
        [SerializeField]
        protected bool Automatical_bool = false;

        bool Attack_bool = false;

        [Tooltip("������� � ����������� �� ������� UI �������� ������� ")]
        [SerializeField]
        bool Aim_image_spread_bool = true;

        [HideIf(nameof(Aim_image_spread_bool))]
        [Tooltip("������� �������� (���������������� ��������� ����������� �������� ���� ��������� �������� Fire_point)")]
        [SerializeField]
        float Spread = 0;

        float Spread_active = 0;//��� ������ � ���������

        Coroutine Detect_press_attack_coroutine = null;

        Quaternion Default_rotation_Fire_point = Quaternion.identity;//����������� �������� ����� ������ ���� ��� ������

        protected int Attack_mode_id = 0;

        Transform Target = null;//���� �����

        Camera Cam = null;

        protected Vector3 Finale_point = Vector3.zero;//����� ���� �������� �������

        private void Start()
        {
            Cam = Game_administrator.Singleton_Instance.Player_administrator.Cam;
            Spread_active = Spread;
            Default_rotation_Fire_point = Fire_point.localRotation;
        }

        protected void Fire_normal()
        {

            if (Damage > 0)
                Instantiate(Projectile_prefab, Fire_point.position, Fire_point.rotation).Specify_settings(Damage);
            else
                Instantiate(Projectile_prefab, Fire_point.position, Fire_point.rotation);
        }


        /// <summary>
        /// ����������
        /// </summary>
        [ContextMenu(nameof(Fire))]
        public void Fire()
        {
            if (Aim_image_spread_bool)
            {
                Vector3 point_screen_point = Vector3.zero;

                if (Game_HC_UI.Singleton_Instance)
                {
                    //point_screen_point = Game_HC_UI.Singleton_Instance.Aim_spread_random_point;
                }    
                else if (Player_interface_UI.Singleton_Instance)
                {
                    //point_screen_point = Player_interface_UI.Singleton_Instance.Aim_spread_random_point;
                }
                    
                Ray ray = Cam.ScreenPointToRay(point_screen_point);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Finale_point = hit.point;
                    //print(hit.transform.name);
                }
                else
                {
                    Finale_point = ray.direction * 4000f;
                }
                Fire_point.transform.LookAt(Finale_point, Fire_point.up);
            }


            Fire_id(Attack_mode_id);

        }

        /// <summary>
        /// ������� ������� �������� ��������� ������ ����-����� (��� �������)
        /// </summary>
        protected virtual void Fire_id(int _id_attack)
        {
            if (gameObject.activeSelf)
            {
                switch (_id_attack)
                {
                    default:
                        Fire_normal();
                        break;
                }

                Attack_mode_id = 0;
            }
        }

        /// <summary>
        /// ���������� � ��������� ����� ��� �������
        /// </summary>
        /// <param name="_damage"></param>
        public void Fire(int _damage)
        {
            Damage = _damage;
            Fire();
        }

        /// <summary>
        /// �������� �� �����������
        /// </summary>
        /// <param name="_point_end">�������� ����� (���� �������)</param>
        public void Fire(Vector3 _point_end)
        {
            Fire_point.transform.LookAt(_point_end, Fire_point.up);
            Fire_point.transform.eulerAngles += new Vector3(Random.Range(-Spread_active, Spread_active), Random.Range(-Spread_active, Spread_active), Fire_point.transform.localRotation.z);

            Fire();
        }

        /// <summary>
        /// �������� �� ����������� � ��������� �����
        /// </summary>
        /// <param name="_point_end">�������� ����� (���� �������)</param>
        /// <param name="_damage">����</param>
        public void Fire(Vector3 _point_end, int _damage)
        {
            Damage = _damage;
            Fire(_point_end);
        }

        /// <summary>
        /// ������� � ����� ���������
        /// </summary>
        /// <param name="_force_spead">���� �������� (�� 0 �� 1)</param>
        public void Fire(float _force_spead)
        {
            Spread_active = Spread * _force_spead;
            Fire_point.transform.localRotation = Default_rotation_Fire_point;
            Fire_point.transform.eulerAngles += new Vector3(Random.Range(-Spread_active, Spread_active), Random.Range(-Spread_active, Spread_active), Fire_point.transform.localRotation.z);
            Fire();
        }

        /// <summary>
        /// ������� � ����� ��������� � ��������� �����
        /// </summary>
        /// <param name="_force_spead">���� �������� (�� 0 �� 1)</param>
        /// <param name="_damage">����</param>
        public void Fire(float _force_spead, int _damage)
        {
            Damage = _damage;
            Fire(_force_spead);
        }

        /// <summary>
        /// ������� �� ����������� � � ����� ��������
        /// </summary>
        /// <param name="_point_end">�������� ����� (���� �������)</param>
        /// <param name="_force_spead">���� �������� (�� 0 �� 1)</param>
        public void Fire(Vector3 _point_end, float _force_spead)
        {
            Spread_active = Spread * _force_spead;
            Fire(_point_end);
        }

        /// <summary>
        /// ������� �� �����������, � ����� �������� � ���
        /// </summary>
        /// <param name="_point_end">�������� ����� (���� �������)</param>
        /// <param name="_force_spead">���� �������� (�� 0 �� 1)</param>
        /// <param name="_damage">����</param>
        public void Fire(Vector3 _point_end, float _force_spead, int _damage)
        {
            Damage = _damage;
            Fire(_point_end, _force_spead);
        }


        /// <summary>
        /// ������� ����� �����
        /// </summary>
        /// <param name="_id_mode">����� �����</param>
        public void Mode_attack(int _id_mode)
        {
            Attack_mode_id = _id_mode;
        }

        /// <summary>
        /// ����� ����
        /// </summary>
        /// <param name="_target">����</param>
        public void New_target(Transform _target)
        {
            Target = _target;
        }

        public override void Attack(bool _activity)
        {
            if (Detect_press_attack_coroutine != null)
            {
                StopCoroutine(Detect_press_attack_coroutine);
                Detect_press_attack_coroutine = null;
            }

            Detect_press_attack_coroutine = StartCoroutine(Coroutine_press_detect_attack());

            
            if (Fire_On_bool)
            {

                if (Automatical_bool || !Attack_bool)
                {
                    Fire();

                    Attack_bool = true;
                    Fire_On_bool = false;
                    Invoke(nameof(Fire_rate_method), Fire_rate);
                }
            }

        }

        IEnumerator Coroutine_press_detect_attack()//����� ��� ����, ��� �� ����� ������� ������ ��� ������� (�� ���������)
        {
            yield return null;
            Attack_bool = false;
        }

        void Fire_rate_method()
        {
            Fire_On_bool = true;
        }

        public override void End_attack()
        {
            throw new System.NotImplementedException();
        }

        public override void Activity(bool _activity)
        {
            //throw new System.NotImplementedException();
        }

        protected override void Initialized_stats()
        {
            //throw new System.NotImplementedException();
        }
    }
}