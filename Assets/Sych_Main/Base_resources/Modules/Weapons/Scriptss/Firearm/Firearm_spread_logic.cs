using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.InputSystem;

namespace Sych_scripts
{
    [DisallowMultipleComponent]
    public class Firearm_spread_logic : MonoBehaviour
    {
        #region ����������


        [Space(10)]
        [Tooltip("�������� �������")]
        [SerializeField]
        bool Spread_bool = false;

        [ShowIf(nameof(Spread_bool))]
        [Tooltip("����� ����� ����� ������� ����� ���������� � ����� ����� ���������� ��������")]
        [SerializeField]
        float Time_rest = 1f;

        bool Rest_bool = true;

        [ShowIf(nameof(Spread_bool))]
        [Tooltip("���� �������� ��� �������� (����������� � ������������)")]
        [SerializeField]
        Vector2 Spread = new Vector2(0, 1f);

        [ShowIf(nameof(Spread_bool))]
        [Tooltip("� ����� ��������� ����������� ���� ��������")]
        [SerializeField]
        float Spread_speed_up = 0.2f;

        [ShowIf(nameof(Spread_bool))]
        [Tooltip("� ����� ��������� ����������� ���� ��������")]
        [SerializeField]
        float Spread_speed_down = 0.1f;

        [Tooltip("�������������� �� ������")]
        [SerializeField]
        LayerMask Layer = 1;

        float Spread_active = 0;//��� ������ � ���������

        Quaternion Default_rotation_Fire_point = Quaternion.identity;//����������� �������� ����� ������ ���� ��� ������

        Coroutine Rest_coroutine = null;

        #endregion

        #region ��������� ������

        private void Start()
        {
            if (Spread_bool)
                Spread_active = Spread.x;
            else
                Spread_active = 0;
        }

        private void Update()
        {
            if (Spread_bool)
            {
                if (Rest_bool)
                    Spread_down();
            }


        }
        #endregion

        #region ������
        /// <summary>
        /// �������� ��������� ����� � �������� �������
        /// </summary>
        Vector3 Aim_spread_random_point(Image _aim)
        {
            Vector3 point = _aim.rectTransform.position;//new Vector3(Random.Range(-Aim_image.rectTransform.sizeDelta.x, Aim_image.rectTransform.sizeDelta.x), Random.Range(-Aim_image.rectTransform.sizeDelta.y, Aim_image.rectTransform.sizeDelta.y), Aim_image.rectTransform.position.z);

            point.x += Random.Range(-_aim.rectTransform.sizeDelta.x / 2 * Spread_active, _aim.rectTransform.sizeDelta.x / 2 * Spread_active);
            point.y += Random.Range(-_aim.rectTransform.sizeDelta.y / 2 * Spread_active, _aim.rectTransform.sizeDelta.y / 2 * Spread_active);

            return point;
        }


        /// <summary>
        /// ��������� ������� ��������
        /// </summary>
        void Spread_up()
        {
            if (Spread_bool)
            {
                if (Rest_coroutine != null)
                    StopCoroutine(Rest_coroutine);

                Rest_coroutine = StartCoroutine(Coroutine_rest_timer());

                if (Spread_active <= Spread.y)
                    Spread_active += Spread_speed_up;
            }
        }

        /// <summary>
        /// ���������� ��������
        /// </summary>
        void Spread_down()
        {
            if (Spread_active != Spread.x)
                Spread_active = Mathf.MoveTowards(Spread_active, Spread.x, Spread_speed_down);
        }

        IEnumerator Coroutine_rest_timer()
        {
            Rest_bool = false;
            yield return new WaitForSeconds(Time_rest);
            Rest_bool = true;
            Rest_coroutine = null;
        }

        #endregion


        #region ��������� ������

        /// <summary>
        /// ����������� ������� ���� �������� ����� ������� ������� �������
        /// </summary>
        public void Spread_fire_point(Transform _fire_point)
        {
            _fire_point.transform.localRotation = Default_rotation_Fire_point;

            _fire_point.transform.eulerAngles += new Vector3(Random.Range(-Spread_active, Spread_active), Random.Range(-Spread_active, Spread_active), _fire_point.transform.localRotation.z);

            Spread_up();
        }


        /// <summary>
        /// ��������� ������� ���� �������� ���� �� ������
        /// </summary>
        /// <param name="_cam">������</param>
        /// <param name="_fire_point">����� ��������� �������</param>
        /// <returns></returns>
        public Vector3 Get_fin_point_Camera_Raycast(Camera _cam, Transform _fire_point)
        {
            Vector3 final_point = Vector3.zero;

                Vector3 point_screen_point = Vector3.zero;

                Image aim = UI_weapon_administrator.Singleton_Instance ? UI_weapon_administrator.Singleton_Instance.Aim_image : null;

            Ray ray;

            if (Spread_bool)
            {
                if (aim != null)
                {
                    point_screen_point = Aim_spread_random_point(aim);
                    ray = _cam.ScreenPointToRay(point_screen_point);
                }
                else
                {
                    //ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
                    ray = new Ray(_cam.transform.position, _cam.transform.forward);
                }
            }
            else
                ray = new Ray(_cam.transform.position, _cam.transform.forward);
            //ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());


            RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layer))
                {
                    final_point = hit.point;
                }
                else
                {
                    final_point = ray.direction * 4000f;
                }

                Vector3 direction = (final_point - _fire_point.position).normalized;
                Quaternion new_rotation = Quaternion.LookRotation(direction, _fire_point.up);

                _fire_point.transform.rotation = new_rotation;

            Spread_up();

            return final_point;
        }


        /// <summary>
        /// ��������� ������� ���� �������� ���� �� ����������� (�������� ���� ����� �������� ����, ���� �������
        /// </summary>
        /// <param name="_cam">����������� ���� ������� (���� � �� ����� ����� ��������)</param>
        /// <param name="_fire_point">����� ��������� �������</param>
        /// <returns></returns>
        public Vector3 Get_fin_point_Raycast(Vector3 _direction, Transform _fire_point)
        {
            Vector3 final_point = Vector3.zero;

            RaycastHit hit;

            if (Physics.Raycast(_fire_point.position, _direction, out hit, Mathf.Infinity, Layer))
            {
                final_point = hit.point;
            }
            else
            {
                final_point = _fire_point.position + _direction * 4000f;
            }

            Vector3 direction = (final_point - _fire_point.position).normalized;
            Quaternion new_rotation = Quaternion.LookRotation(direction, _fire_point.up);

            _fire_point.transform.rotation = new_rotation;

            Spread_up();

            return final_point;
        }

        #endregion
    }
}