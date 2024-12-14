using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    public abstract class Camera_state_abstract : MonoBehaviour
    {

        [field: Tooltip("Имя состояния")]
        [field: SerializeField]
        public string Name_state { get; private set; } = "Name state";

        [Tooltip("На сколько градусов можно поднять камеру")]
        [SerializeField]
        protected float TopClamp = 70.0f;

        [Tooltip("На сколько градусов можно опустить камеру")]
        [SerializeField]
        protected float BottomClamp = -30.0f;

        [Tooltip("Для фиксации положения камеры по всем осям")]
        [SerializeField]
        protected bool Lock_Camera_rotation = false;

        [Tooltip("Объект для камеры (на нём находится CinemachineVirtualCamera)")]
        [SerializeField]
        Transform GameObject_CinemachineVirtualCamera = null;


        [field: ShowNonSerializedField]
        protected bool Active_bool { get; private set; } = false;

        // cinemachine
        protected float _cinemachineTargetYaw;
        protected float _cinemachineTargetPitch;

        protected const float _threshold = 0.01f;


        protected Camera_control Main_script;
        protected State_machine_camera State_Machine_script;

        bool Start_preparation_bool = false;//Подготовка



        protected float Additional_mouse_sensitivity = 1;//дополнительная настройка пользователя чувствительности мыши

        void OnEnable()
        {
            Additional_mouse_sensitivity = Check_Mouse_sensitivity();

            if (Setting_menu.Singleton_Instance)
            {
                    Setting_menu.Singleton_Instance.Mouse_sensitivity_d += Change_Mouse_sensitivity;
            }
        }

        private void OnDisable()
        {
            if (Setting_menu.Singleton_Instance)
            {
                    Setting_menu.Singleton_Instance.Mouse_sensitivity_d -= Change_Mouse_sensitivity;
            }
        }

        public void Preparation(Camera_control character, State_machine_camera stateMachine)
        {
            Main_script = character;
            State_Machine_script = stateMachine;
        }

        /// <summary>
        /// Узнать уровень чувствительности мыши
        /// </summary>
        /// <param name="_add_individual_volume">Дополнительная корректирующая чувствительность мыши</param>
        /// <returns></returns>
        float Check_Mouse_sensitivity()
        {
            float result = 1;

            result = Save_PlayerPrefs.Know_parameter(Type_parameter_value_float.Mouse_sensitivity);

            return result;
        }

        /// <summary>
        /// Изменение чувствительности мыши
        /// </summary>
        /// <param name="_value">Значение чувствительности</param>
        void Change_Mouse_sensitivity(float _value)
        {
            Additional_mouse_sensitivity = _value;
        }

        /// <summary>
        /// Поворот камеры
        /// </summary>
        public abstract void Camera_rotation();


        protected static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }


        /// <summary>
        /// Вход состояния (начало реализации)
        /// </summary>
        public virtual void Enter_state()
        {
            if (!Start_preparation_bool)
            {
                Start_preparation_bool = true;
                Preparation();
            }

            Active_bool = true;

            GameObject_CinemachineVirtualCamera.gameObject.SetActive(true);

            _cinemachineTargetYaw = Main_script.Cam.transform.eulerAngles.y;

            float angle = Main_script.Cam.transform.eulerAngles.x;
            angle = Mathf.Repeat(angle + 180, 360) - 180;
            _cinemachineTargetPitch = angle;
        }

        /// <summary>
        /// Начальная подготовка
        /// </summary>
        protected virtual void Preparation()
        {
            GameObject_CinemachineVirtualCamera.gameObject.SetActive(false);
        }

        /// <summary>
        /// Выход из состояния
        /// </summary>
        public virtual void Exit_state()
        {
            Active_bool = false;

            GameObject_CinemachineVirtualCamera.gameObject.SetActive(false);
        }

    }
}