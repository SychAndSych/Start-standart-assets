using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    public abstract class Player_Motor_abstract : MonoBehaviour
    {
        [Tooltip("Аниматор")]
        [SerializeField]
        protected Animator Anim = null;

        [Tooltip("CharacterController")]
        [SerializeField]
        protected CharacterController CharacterController_script = null;

        [Tooltip("Поворачиваем персонажа (элемент который будет поворачиваться)")]
        [SerializeField]
        protected Transform Rotation_transform = null;

        [field: Tooltip("Нету Game_administrator, переходим на ручное управление задавание настроек")]
        [field: SerializeField]
        protected bool No_Game_administrator_bool { get; private set; } = false;

        [ShowIf(nameof(No_Game_administrator_bool))]
        [Tooltip("Камера")]
        [SerializeField]
        protected Camera Cam = null;



        [Space(20)]
        [Header("Гравитация")]

        [Tooltip("Персонаж использует собственное значение гравитации. Гравитация в самом Unity по умолчанию -9.81f")]
        public float Gravity = -15.0f;

        [Tooltip("Время, необходимое для перехода в состояние падения. Полезно для спуска по лестнице (а то каждая ступенька заставит падать)")]
        public float Fall_Timeout = 0.15f;


        [field: Space(20)]

        [Tooltip("Высота, на которую может прыгнуть игрок")]
        public float JumpHeight = 1.2f;

        [Tooltip("Время, необходимое для того, чтобы снова прыгнуть. Установите 0f, чтобы снова мгновенно прыгнуть")]
        public float Jump_Timeout = 0.2f;


        bool Jump_input_bool = false;//Внешняя команда прыгать/не прыгать

        bool Jump_bool = true;//Можно прыгать или нет

        internal float _verticalVelocity = 0;//Вертикальная скорость персонажа


        float _jumpTimeoutDelta = 0;//Время проведённое в прыжке
        float _fallTimeoutDelta = 0;//Время проведённое в падаение

        float _terminalVelocity = 53.0f;//Максимальная скорость падения

        bool Grounded_bool = true;//Приземлён и стоит на земле


        //Передвижение
        protected float Speed_active = 0;

        protected float _animationBlend = 0;//Отвечает за параметр скорости передвижения в аниматоре

        protected float _targetRotation = 0.0f;
        protected float _rotationVelocity = 0;

        protected float SpeedChangeRate = 10.0f;//Ускорение и замедление (выяснить зачем)

        #region Системные методы
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


        #region Методы

        /// <summary>
        /// Прыжок и падение
        /// </summary>
        protected void Graviry_and_jump()
        {

            if (Grounded_bool && Jump_bool)
            {
                // сбросить таймер тайм-аута падения
                _fallTimeoutDelta = Fall_Timeout;

                /*
                if (Anim)
                {
                    Anim.SetBool("Jump", false);
                    Anim.SetBool("Free_fall", false);
                }
                */

                // остановить бесконечное падение нашей скорости при заземлении
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Прыжок
                if (Jump_input_bool && _jumpTimeoutDelta <= 0.0f)
                {
                    // квадратный корень из H * -2 * G = скорость, необходимая для достижения желаемой высоты
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



        #region Публичные методы

        public abstract void Move(float speed_movement_, Vector2 _direction);

        /// <summary>
        /// Переключение режима заземления
        /// </summary>
        /// <param name="_activity">На земле стоим или нет</param>
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
        /// Переключение приказа на прыжок
        /// </summary>
        /// <param name="_activity">Жмём прыгать ?</param>
        public void Activity_Jump_bool(bool _activity)
        {
            Jump_input_bool = _activity;
        }
        #endregion
    }
}