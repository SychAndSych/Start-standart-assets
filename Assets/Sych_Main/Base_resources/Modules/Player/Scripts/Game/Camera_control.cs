//������ ��� ������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


namespace Sych_scripts
{

    [AddComponentMenu("Sych scripts / Game / Player / Camera control")]
    [DisallowMultipleComponent]
    public class Camera_control : MonoBehaviour
    {

        [Tooltip("������ ��������� ��������� (���������������� (�����������) ������ ������ ����� ������ ���������)")]
        [SerializeField]
        Camera_state_abstract[] State_array = new Camera_state_abstract[0];

        State_machine_camera State_Machine;

        [field: Tooltip("�������� ��������")]
        [field: SerializeField]
        public float Speed_rotation { get; private set; } = 1.2f;

        [field: Tooltip("����� �� ������� ������ ������ (��� �� ����� ��������)")]
        [field: SerializeField]
        public GameObject Camera_point { get; private set; } = null;

        [field: Tooltip("������ ����������")]
        [field: SerializeField]
        public Input_player Input { get; private set; } = null;


        [Tooltip("�������������� ������� ��� ��������������� ������. ������� ��� ������ ��������� ��������� ������ � ��������������� ���������.")]
        public float CameraAngleOverride = 0.0f;

        [field: Tooltip("���� Game_administrator, ��������� �� ������ ���������� ��������� ��������")]
        [field: SerializeField]
        protected bool No_Game_administrator_bool { get; private set; } = false;

        [field: ShowIf(nameof(No_Game_administrator_bool))]
        [field: Tooltip("������")]
        [field: SerializeField]
        public Camera Cam { get; private set; } = null;

        private void Start()
        {
            if (Game_administrator.Singleton_Instance)
                Cam = Game_administrator.Singleton_Instance.Player_administrator.Cam;

            Initialized_State_machine();
        }


        private void LateUpdate()
        {
            if (State_Machine != null)
            State_Machine.Current_State.Camera_rotation();
        }


        /// <summary>
        /// ������������� ������ ���������
        /// </summary>
        protected virtual void Initialized_State_machine()
        {
            State_Machine = new State_machine_camera();

            for (int x = 0; x < State_array.Length; x++)
            {
                State_array[x].Preparation(this, State_Machine);
            }

            State_Machine.Initialize(State_array[0]);
        }


        #region ����������� ������
        /// <summary>
        /// �������� ��������� ��������� �� �����
        /// </summary>
        /// <param name="_name">��� ���������</param>
        public void Change_state(string _name)
        {
            bool result = false;

            for (int x = 0; x < State_array.Length; x++)
            {
                if (State_array[x].Name_state == _name)
                {
                    State_Machine.Change_State(State_array[x]);
                    result = true;
                    break;
                }
            }

            if (!result)
                Debug.LogError("��������� ��������� �� �����  " + _name + "  �� �������!");
        }


        /// <summary>
        /// �������� ��������� ��������� �� id
        /// </summary>
        /// <param name="_name">��� ���������</param>
        public void Change_state(int _id)
        {
            State_Machine.Change_State(State_array[_id]);
        }


        #endregion


    }
}