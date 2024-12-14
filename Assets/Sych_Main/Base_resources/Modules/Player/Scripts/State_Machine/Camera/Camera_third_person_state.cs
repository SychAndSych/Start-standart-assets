using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Camera state / Third person")]
    [DisallowMultipleComponent]
    public class Camera_third_person_state : Camera_state_abstract
    {

        [Tooltip("Нужно ли отдельно вращать по горизонтали другой объект")]
        [SerializeField]
        bool Rotation_obj_Y_bool = false;

        [ShowIf(nameof(Rotation_obj_Y_bool))]
        [Tooltip("Вращает определённый объект по горизонтали")]
        [SerializeField]
        Transform Rotation_transform = null;

        public override void Camera_rotation()
        {
            // if there is an input and camera position is not fixed
            if (Main_script.Input.Look_rotation.sqrMagnitude >= _threshold && !Lock_Camera_rotation)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = 1.0f;//IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                //if(Rotation_horizontal_bool)
                _cinemachineTargetYaw += Main_script.Input.Look_rotation.x * deltaTimeMultiplier * (Main_script.Speed_rotation * Additional_mouse_sensitivity);

                //if (Rotation_vertical_bool)
                _cinemachineTargetPitch += Main_script.Input.Look_rotation.y * deltaTimeMultiplier * (Main_script.Speed_rotation * Additional_mouse_sensitivity);
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            if (!Rotation_obj_Y_bool)
            {
                Main_script.Camera_point.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + Main_script.CameraAngleOverride,
    _cinemachineTargetYaw, 0.0f);
            }
            else
            {
                Main_script.Camera_point.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch + Main_script.CameraAngleOverride, 0, 0.0f);
                Rotation_transform.rotation = Quaternion.Euler(0, _cinemachineTargetYaw, 0.0f);
            }
        }
    }
}