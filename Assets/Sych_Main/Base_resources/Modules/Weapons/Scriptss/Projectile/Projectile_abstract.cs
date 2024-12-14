using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Lean.Pool;

namespace Sych_scripts
{
    public abstract class Projectile_abstract : MonoBehaviour
    {
        [Tooltip("����")]
        [SerializeField]
        [Min(1)]
        protected int Damage = 10;

        [Tooltip("�������� �������")]
        [SerializeField]
        [Min(1)]
        protected float Speed_bullet = 150f;

        [Tooltip("���� ������������")]
        [SerializeField]
        [Min(0)]
        protected float Force_Repulsion = 5000f;

        [Tooltip("������")]
        [SerializeField]
        protected Rigidbody Body = null;

        [Tooltip("����� �� ���������������")]
        [SerializeField]
        [Min(1)]
        protected float Time_destroy = 10f;

        [Tooltip("�� ��������������� ��� �������� � ����� � ������� ���� �������� (��� ����, ��� �� ������ ������ ������ ���� � ������� ������)")]
        [SerializeField]
        protected bool Punch_Through_bool = false;



        [Space(10)]
        [Tooltip("����� ������ �� ������� �������������� ����")]
        [SerializeField]
        bool Move_Way_bool = false;

        [ShowIf(nameof(Move_Way_bool))]
        [Tooltip("������ ������������ �� ����")]
        [SerializeField]
        Movement_object_to_points Move_way_script = null;




        [Space(10)]
        [Tooltip("�������� ������� ��� �������� ���������")]
        [SerializeField]
        bool Raycast_bool = false;
        
        [ShowIf(nameof(Raycast_bool))]
        [Tooltip("��������� �������� ����� ����� �����")]
        [SerializeField]
        [Min(0.001f)]
        float Distance_raycast_detected = 0.5f;

        [ShowIf(nameof(Raycast_bool))]
        [Tooltip("���������� ���� ��� ���������� �������� �������� �������")]
        [SerializeField]
        [Min(0.001f)]
        float Distance_raycast_multiply = 0.05f;

        protected bool Raycast_active_bool = true;



        [Tooltip("���� �� �������� �������")]
        [SerializeField]
        protected TrailRenderer Trail = null;


        [Tooltip("���� � �������� ����� �����������������")]
        [SerializeField]
        protected LayerMask Layer_detected = 1;

        protected Game_character_abstract Host = null;//��� �������� ������

        protected bool Way_mode_bool = false;//������ ����� ������ �� ����


        [Tooltip("������ ������")]
        [SerializeField]
        bool Debug_mode = false;

        bool Destroy_bool = false;
        private void OnEnable()
        {
            Body.angularVelocity = Vector3.zero;
            Destroy_bool = false;
        }

        protected virtual void Start()
        {
            //Body.AddForce(transform.forward * Speed_bullet);
            Body.velocity = transform.forward * Speed_bullet;
        }

        protected virtual void FixedUpdate()
        {
            if (Raycast_bool)
            {
                if(Raycast_active_bool)
                Raycast_preparation();
            }
        }

        #region ������

        /// <summary>
        /// ���������� ������
        /// </summary>
        protected virtual void Off_rigidbody_arrow()
        {
            Body.isKinematic = true;
            Body.velocity = Vector3.zero;

            if (Trail)
            {
                Trail.time = 0.1f;
                Trail.emitting = false;
            }

            StopAllCoroutines();


        }


        /// <summary>
        /// ������� ������� �����, ��� ��������
        /// </summary>
        void Raycast_preparation()
        {
            RaycastHit hit;

            float distance_raycast = Distance_raycast_detected + Distance_raycast_multiply;

            if(Body)
            distance_raycast = Distance_raycast_detected + Distance_raycast_multiply * Body.velocity.magnitude;

            if (Physics.Raycast(transform.position, transform.forward, out hit, distance_raycast, Layer_detected))
            {
                Detect_raycast(hit);
            }
        }

        /// <summary>
        /// ��������� ������� ����� ������� �������� (����)
        /// </summary>
        /// <param name="_hit">����� ���������</param>
        protected virtual void Detect_raycast(RaycastHit _hit)
        {
            if (_hit.transform.GetComponent<I_damage>() != null)
            {
                _hit.transform.GetComponent<I_damage>().Add_Damage(Damage, null);
            }

            if (_hit.transform.GetComponent<Rigidbody>())
            {
                Rigidbody body = _hit.transform.GetComponent<Rigidbody>();

                if (body != Body) 
                {
                    //hit.transform.GetComponent<Rigidbody>().AddForce(Body.velocity * Body.mass * Force_Repulsion);
                    Vector3 direction = _hit.point - Body.transform.position;
                    body.AddForceAtPosition(direction.normalized * Force_Repulsion, _hit.point);
                }
            }

            if ((!Punch_Through_bool || Punch_Through_bool && _hit.transform.GetComponent<I_damage>() == null))
                Invoke(nameof(Destroy_method), 0.01f);
        }

        /// <summary>
        /// ���������� ������
        /// </summary>
        protected virtual void Destroy_method()
        {
            if (!Destroy_bool) {
                StopAllCoroutines();
                Destroy_bool = true;
                LeanPool.Despawn(gameObject);
                //Destroy(gameObject);
            }
        }

        /// <summary>
        /// ���������� ������ ������ �����
        /// </summary>
        /// <returns></returns>
        protected IEnumerator Destroy_coroutine()
        {
            yield return new WaitForSeconds(Time_destroy);
            Destroy_method();
        }

        #endregion


        #region ��������� ������
        public void Move_way(Vector3[] _points, float _speed)
        {
            if (Move_Way_bool)
            {
                if (Move_way_script)
                {
                    Way_mode_bool = true;
                    Move_way_script.Set_points(_points, _speed);
                }
            }
        }

        /// <summary>
        /// ������� ��������� �������
        /// </summary>
        /// <param name="_damage">������� ����</param>
        /// <param name="_bullet_flight_speed">������� �������� �����</param>
        public void Specify_settings(int _damage, float _bullet_flight_speed, float _force_Repulsion, Game_character_abstract _host)
        {
            StartCoroutine(Destroy_coroutine());
            Host = _host;
            Speed_bullet = _bullet_flight_speed;
            Damage = _damage;
            Body.velocity = transform.forward * _bullet_flight_speed;
            Force_Repulsion = _force_Repulsion;
            //Body.velocity = Vector3.zero;
            //Body.AddForce(transform.forward * _bullet_flight_speed);
        }

        public void Specify_settings(int _damage)
        {
            Specify_settings(_damage, Speed_bullet, Force_Repulsion, Host);
        }

        public void Specify_settings(float _bullet_flight_speed)
        {
            Specify_settings(Damage, _bullet_flight_speed, Force_Repulsion, Host);
        }

        public void Specify_settings(Game_character_abstract _host)
        {
            Specify_settings(Damage, Speed_bullet, Force_Repulsion, _host);
        }

        #endregion


        #region �������������
        private void OnDrawGizmosSelected()
        {
            if (Debug_mode)
            {
                float distance_raycast = Distance_raycast_detected + Distance_raycast_multiply;

                if (Body)
                    distance_raycast = Distance_raycast_detected + Distance_raycast_multiply * Body.velocity.magnitude;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + transform.forward * distance_raycast);
            }
        }
        #endregion
    }
}
