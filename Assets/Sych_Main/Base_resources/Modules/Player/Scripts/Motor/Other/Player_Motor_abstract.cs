using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    public abstract class Player_Motor_abstract : MonoBehaviour
    {
        [Tooltip("��������")]
        [SerializeField]
        protected Animator Anim = null;

        [Tooltip("CharacterController")]
        [SerializeField]
        protected CharacterController CharacterController_script = null;

        [Tooltip("������������ ��������� (������� ������� ����� ��������������)")]
        [SerializeField]
        protected Transform Rotation_transform = null;

        [field: Tooltip("���� Game_administrator, ��������� �� ������ ���������� ��������� ��������")]
        [field: SerializeField]
        protected bool No_Game_administrator_bool { get; private set; } = false;

        [ShowIf(nameof(No_Game_administrator_bool))]
        [Tooltip("������")]
        [SerializeField]
        protected Camera Cam = null;



        [Space(20)]
        [Header("����������")]

        [Tooltip("�������� ���������� ����������� �������� ����������. ���������� � ����� Unity �� ��������� -9.81f")]
        public float Gravity = -15.0f;

        [Tooltip("�����, ����������� ��� �������� � ��������� �������. ������� ��� ������ �� �������� (� �� ������ ��������� �������� ������)")]
        public float Fall_Timeout = 0.15f;


        [field: Space(20)]

        [Tooltip("������, �� ������� ����� �������� �����")]
        public float JumpHeight = 1.2f;

        [Tooltip("�����, ����������� ��� ����, ����� ����� ��������. ���������� 0f, ����� ����� ��������� ��������")]
        public float Jump_Timeout = 0.2f;


        bool Jump_input_bool = false;//������� ������� �������/�� �������

        bool Jump_bool = true;//����� ������� ��� ���

        internal float _verticalVelocity = 0;//������������ �������� ���������


        float _jumpTimeoutDelta = 0;//����� ���������� � ������
        float _fallTimeoutDelta = 0;//����� ���������� � ��������

        float _terminalVelocity = 53.0f;//������������ �������� �������

        bool Grounded_bool = true;//�������� � ����� �� �����


        //������������
        protected float Speed_active = 0;

        protected float _animationBlend = 0;//�������� �� �������� �������� ������������ � ���������

        protected float _targetRotation = 0.0f;
        protected float _rotationVelocity = 0;

        protected float SpeedChangeRate = 10.0f;//��������� � ���������� (�������� �����)

        #region ��������� ������
        private void Start()
        {
            if (Game_administrator.Singleton_Instance)
                Cam = Game_administrator.Singleton_Instance.Player_administrator.Cam;
        }

        void Update()
        {
            Graviry_and_jump();
        }
        #endregion


        #region ������

        /// <summary>
        /// ������ � �������
        /// </summary>
        protected void Graviry_and_jump()
        {

            if (Grounded_bool && Jump_bool)
            {
                // �������� ������ ����-���� �������
                _fallTimeoutDelta = Fall_Timeout;

                /*
                if (Anim)
                {
                    Anim.SetBool("Jump", false);
                    Anim.SetBool("Free_fall", false);
                }
                */

                // ���������� ����������� ������� ����� �������� ��� ����������
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // ������
                if (Jump_input_bool && _jumpTimeoutDelta <= 0.0f)
                {
                    // ���������� ������ �� H * -2 * G = ��������, ����������� ��� ���������� �������� ������
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    if (Anim)
                    {
                        //Anim.SetBool("Grounded", false);
                        Anim.SetBool("Jump", true);
                    }

                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = Jump_Timeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (Anim)
                        Anim.SetBool("Free_fall", true);
                }

                // if we are not grounded, do not jump
                Jump_input_bool = false;
                Jump_bool = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
        #endregion



        #region ��������� ������

        public abstract void Move(float speed_movement_, Vector2 _direction);

        /// <summary>
        /// ������������ ������ ����������
        /// </summary>
        /// <param name="_activity">�� ����� ����� ��� ���</param>
        public void Activity_Grounded_bool(bool _activity)
        {
            Grounded_bool = _activity;

            if (Anim)
                Anim.SetBool("Grounded", Grounded_bool);


            if (_activity)
            {
                Jump_bool = true;
                if (Anim)
                {
                    Anim.SetBool("Jump", false);
                    Anim.SetBool("Free_fall", false);
                }

            }

        }


        /// <summary>
        /// ������������ ������� �� ������
        /// </summary>
        /// <param name="_activity">��� ������� ?</param>
        public void Activity_Jump_bool(bool _activity)
        {
            Jump_input_bool = _activity;
        }
        #endregion
    }
}