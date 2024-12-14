//����������� ��������� �� ������ (������� �� ��������� ������ � ������� �������� ���)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Skills / Grab ledge")]
    [DisallowMultipleComponent]
    public class Grab_ledge : MonoBehaviour
    {
        #region ����������

        [Tooltip("�������� �������� �� ����� ����������")]
        [SerializeField]
        Transform Owner_transform = null;

        [Tooltip("����� ������������ � �������� ����� �������������� ������ �� ������� ����� ������")]
        [SerializeField]
        bool Direction_point_bool = false;

        [HideIf(nameof(Direction_point_bool))]
        [Tooltip("������������� �� ������������ ����� �������")]
        [SerializeField]
        Vector3 Offset = Vector3.zero;

        [HideIf(nameof(Direction_point_bool))]
        [Tooltip("����� ����������� (����� ���� ����������� ��� ������������ �����������)")]
        [SerializeField]
        Transform Forward_point = null;

        [ShowIf(nameof(Direction_point_bool))]
        [Tooltip("������������ ����� (����� ����������� �� Z � � �� ��� ������ ����������)")]
        [SerializeField]
        Transform Direction_point = null;

        [Min(0)]
        [Tooltip("��������� �������")]
        [SerializeField]
        float Distance_grab = 1f;

        [Min(0)]
        [Tooltip("����������� ����� ������� (��� �� �� �������� �� �� ������)")]
        [SerializeField]
        float Protrusion_length = 0.5f;

        [Min(0)]
        [Tooltip("������ �������")]
        [SerializeField]
        float Height_grab = 0.5f;

        [Min(0)]
        [Tooltip("��������� ������� ��� ����� ������ ����������")]
        [SerializeField]
        float Post_distance_grab = 1f;

        [Min(0)]
        [Tooltip("������ ��������� (��� �� ����� �� ��������� ������ ������� ����� ����������)")]
        [SerializeField]
        float Height_owner = 1f;

        [Tooltip("���� � �������� ���������������")]
        [SerializeField]
        LayerMask Mask = 1;

        [Tooltip("�������� �������������")]
        [SerializeField]
        float Speed_grab = 2f;

        [Tooltip("�������� �������� � ����� ������� (��� �� �������� ����� ������ �����, � �� ��� �� ������ � �������)")]
        [SerializeField]
        float Speed_lerp_connect = 15f;

        [Tooltip("����� �� ����������� ��������� �����")]
        [SerializeField]
        bool Move_final_grab_bool = true;

        [ShowIf(nameof(Move_final_grab_bool))]
        [Tooltip("��������� ����������� ����� ��������� ����� �� ���������� �� ������ ")]
        [SerializeField]
        float Distance_final_position = 1f;

        [Tooltip("����� ����� ���� �� ��� ����������.")]
        [SerializeField]
        UnityEvent Connect_grab_event = new UnityEvent();

        [Tooltip("����� ����� ����� ����������")]
        [SerializeField]
        UnityEvent Start_climb_event = new UnityEvent();

        [Tooltip("����� ����� �������� ���������� ��� ��������.")]
        [SerializeField]
        UnityEvent End_climb_event = new UnityEvent();

        [Tooltip("��� �� ������ ������")]
        [SerializeField]
        bool Gizmos_mode_bool = false;

        [Tooltip("�������� ��� ������")]
        [SerializeField]
        bool Start_active_bool = false;

        bool Active_bool = false;

        bool Connect_grab_bool = false;//���������� �� �� �� ��� ��?

        bool Climb_active_bool = false;//�������� ���������� � �����

        Coroutine Climb_coroutine = null;

        Vector3[] End_pos_climb_array = new Vector3[0];

        Vector3 Position_grab_length_contact = Vector3.zero;//����� ����� ������� (��� �� �������������� � �����).

        Vector3 Position_grab_height_contact = Vector3.zero;//����� �������� ���� ������ (��� �� ��������� �� ������).

        Vector3 Hit_normal = Vector3.zero;

        Coroutine Lerp_grab_coroutine = null;

        #endregion

        #region ��������� ������

        private void Start()
        {
            if (Start_active_bool)
                Active_bool = true;
        }

        void FixedUpdate()
        {
            if (Active_bool && !Connect_grab_bool)
            {
                if (Check_distance_grab)
                {
                    if (Check_finale_grab)
                    {
                        Grab_hold();
                    }
                }
            }
        }

        #endregion


        #region ��������� ������


        /// <summary>
        /// ���������� �� ������
        /// </summary>
        void Grab_hold()
        {
            if (Lerp_grab_coroutine == null)
                Lerp_grab_coroutine = StartCoroutine(Coroutine_lerp_grab());

            Connect_grab_event.Invoke();
            Active_bool = false;
            Connect_grab_bool = true;
        }

        IEnumerator Coroutine_lerp_grab()
        {
            Vector3 offset_position = Owner_transform.position - Find_out_position_start;//������� ��������, ��� �� ����� ��� ������ � ����� ��������

            Vector3 pos = Owner_transform.position;
            pos.y = Position_grab_height_contact.y + offset_position.y;
            pos.x = Position_grab_length_contact.x + offset_position.x;
            pos.z = Position_grab_length_contact.z + offset_position.z;

            //Owner_transform.position = pos;

            Vector3 start_pos = Owner_transform.position;

            Quaternion start_rot = Quaternion.identity;

            Quaternion end_rot = Quaternion.identity;

            if (Forward_point)//������� ����������� ����� ��� ����������� ����
            {
                start_rot = Forward_point.rotation;

                end_rot = Quaternion.LookRotation(Hit_normal * -1);
                //Forward_point.rotation = Quaternion.LookRotation(Hit_normal * -1);
            }

            float step = 0;

            while (step < 1)
            {
                yield return null;

                Owner_transform.position = Vector3.LerpUnclamped(start_pos, pos, step);

                if (Forward_point)//������� ����������� ����� ��� ����������� ����
                {
                    Forward_point.rotation = Quaternion.LerpUnclamped(start_rot, end_rot, step);
                }

                step += Speed_lerp_connect * Time.fixedDeltaTime;

                if (step > 1)
                    step = 1;

            }

            Lerp_grab_coroutine = null;
        }

        /// <summary>
        /// �������� ������������ ���
        /// </summary>
        Transform Find_out_forward_point
        {
            get
            {
                Transform result = null;

                if (Direction_point_bool)
                {
                    result = Direction_point;
                }
                else
                {
                    if (Forward_point != null)
                        result = Forward_point;
                    else
                        result = Owner_transform;
                }

                return result;
            }
        }

        /// <summary>
        /// �������� ��������� �������
        /// </summary>
        Vector3 Find_out_position_start
        {
            get
            {
                Vector3 result = Vector3.zero;

                if (Direction_point_bool)
                {
                    result = transform.position;
                }
                else
                {
                    Transform forward_point = Find_out_forward_point;

                    result = Owner_transform.position + (forward_point.forward * Offset.z + forward_point.up * Offset.y + forward_point.right * Offset.x);
                }

                return result;
            }
        }

        /// <summary>
        /// ��� ���������� ����� ��������� ������� �� ����� (����� ���� ������� ���������� ����� ���� �����������
        /// </summary>
        Vector3 Find_out_End_pos_Distance_grab
        {
            get
            {


                return Position_grab_length_contact != Vector3.zero && Active_bool ? Position_grab_length_contact : Find_out_position_start + Find_out_forward_point.forward * Distance_grab;
            }
        }

        /// <summary>
        /// ������� ����� � ������� ����� ����������� �������� ����� �������� �� �����
        /// </summary>
        Vector3 Find_out_End_move_owner
        {
            get
            {

                return Owner_transform.position + Find_out_forward_point.forward * Distance_final_position;
            }
        }

        /// <summary>
        /// ��� ���������� ����� ������ ������� �� ����� (� ������ ���������)
        /// </summary>
        Vector3 Find_out_End_pos_Height_grab
        {
            get
            {
                return Find_out_End_pos_Distance_grab + Find_out_forward_point.up * Height_grab;
            }
        }

        /// <summary>
        /// ��� ���������� ����� ����������� ����� �������
        /// </summary>
        Vector3 Find_out_End_pos_Protrusion_length
        {
            get
            {
                return Find_out_End_pos_Height_grab + Find_out_forward_point.forward * Protrusion_length;
            }
        }

        /// <summary>
        /// ��� ���������� ����� ���� ��������� ������� �� ����� (� ������ �������� ��������� � ������ �������) 
        /// </summary>
        Vector3 Find_out_End_pos_Post_distance_grab
        {
            get
            {
                return Find_out_End_pos_Protrusion_length + Find_out_forward_point.forward * Post_distance_grab;
            }
        }

        /// <summary>
        /// ��� ���������� ����� ������� ������� �� ����� (� ������ �������� ��������� � ���� ��������� � ������ �������) 
        /// </summary>
        Vector3 Find_out_End_pos_Depth_grab
        {
            get
            {
                return Find_out_End_pos_Post_distance_grab + -Find_out_forward_point.up * Height_grab;
            }
        }

        /// <summary>
        /// ��������� �� ������� ����������� ����� ����������
        /// </summary>
        bool Check_distance_grab
        {
            get
            {
                bool result = false;

                RaycastHit hit;

                Physics.Raycast(Find_out_position_start, Find_out_forward_point.forward, out hit, Distance_grab, Mask, QueryTriggerInteraction.Ignore);

                if (hit.transform != null)
                {
                    Position_grab_length_contact = hit.point;
                    Hit_normal = hit.normal;
                    result = true;
                }


                return result;
            }
        }


        /// <summary>
        /// ��������� ����� �� �� � ����� ����������
        /// </summary>
        bool Check_finale_grab
        {
            get
            {
                bool result = false;

                RaycastHit hit;

                int step_limit = 10;//���������� ������� � ����������� �� ��� �����

                float step_interval = 1f / (float)step_limit;//

                for (int x = 0; x <= step_limit; x++)
                {
                    float step = x * step_interval;

                    Vector3 step_position = Vector3.Lerp(Find_out_End_pos_Protrusion_length, Find_out_End_pos_Post_distance_grab, step);

                    Physics.Raycast(step_position, -Find_out_forward_point.up, out hit, Height_grab, Mask, QueryTriggerInteraction.Ignore);

                    if (hit.transform != null)
                    {
                        result = true;
                        Position_grab_height_contact = hit.point;
                        break;
                    }
                }


                return result;
            }
        }

        /// <summary>
        /// ���������� ��������� �� ������� � ����� �������
        /// </summary>
        /// <returns></returns>
        IEnumerator Coroutine_climb()
        {
            int limit = 1000;

            float step = 0;

            while (limit > 0 && Owner_transform.position != End_pos_climb_array[End_pos_climb_array.Length - 1])
            {
                yield return null;

                limit--;

                Owner_transform.position = Game_Bezier_curve.Get_point_Bezier(End_pos_climb_array[0], End_pos_climb_array[1], End_pos_climb_array[2], step);
                step += Speed_grab * Time.fixedDeltaTime;

                if (step > 1)
                    step = 1;

            }

            End_climb();

            Climb_coroutine = null;
        }

        #endregion


        #region ��������� ������
        /// <summary>
        /// ����� �� �������� ������
        /// </summary>
        /// <param name="_activity">��������?</param>
        public void Activity(bool _activity)
        {
            Active_bool = _activity;
        }

        /// <summary>
        /// ���������
        /// </summary>
        [ContextMenu("���������")]
        public void Climb()
        {
            if (Connect_grab_bool && !Climb_active_bool)
            {
                if (Climb_coroutine != null)
                    StopCoroutine(Climb_coroutine);

                End_pos_climb_array = new Vector3[3];
                End_pos_climb_array[0] = Owner_transform.position;
                End_pos_climb_array[1] = Find_out_position_start + Find_out_forward_point.up * Height_owner;
                End_pos_climb_array[2] = Find_out_End_move_owner + Find_out_forward_point.up * Height_owner;

                Start_climb_event.Invoke();

                Climb_active_bool = true;

                if (Move_final_grab_bool)
                    Climb_coroutine = StartCoroutine(Coroutine_climb());
            }
        }

        /// <summary>
        /// ����� ���������
        /// </summary>
        public void End_climb()
        {
            Connect_grab_bool = false;

            Climb_active_bool = false;

            End_climb_event.Invoke();
        }

        #endregion


        #region ����������� ������

        private void OnDrawGizmos()
        {

            if (Gizmos_mode_bool)
            {
                //����������� ���������
                Gizmos.color = Color.red;

                Gizmos.DrawLine(Find_out_position_start, Find_out_End_pos_Distance_grab);

                //���������� ������ �������
                Gizmos.color = Color.blue;

                Gizmos.DrawLine(Find_out_End_pos_Distance_grab, Find_out_End_pos_Height_grab);

                Gizmos.DrawLine(Find_out_End_pos_Post_distance_grab, Find_out_End_pos_Depth_grab);

                //����������� ���� ��������� ��� ���������� ������
                Gizmos.color = Color.yellow;

                Gizmos.DrawLine(Find_out_End_pos_Height_grab, Find_out_End_pos_Post_distance_grab);

                //���������� ����������� ����� ������
                Gizmos.color = Color.gray;

                Gizmos.DrawLine(Find_out_End_pos_Height_grab, Find_out_End_pos_Protrusion_length);

                Gizmos.color = Color.cyan;

                Gizmos.DrawLine(Owner_transform.position, Find_out_End_move_owner);
            }
        }
        #endregion
    }
}