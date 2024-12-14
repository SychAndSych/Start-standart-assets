using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Camera state / First person")]
    [DisallowMultipleComponent]
    public class Camera_first_person_state : Camera_state_abstract
    {
        [Tooltip("Тушка самого персонажа для поворота")]
        [SerializeField]
        Transform Character_rotation = null;


        public override void Camera_rotation()
        {
            // if there is an input and camera position is not fixed
            if (Main_script.Input.Look_rotation.sqrMagnitude >= _threshold && !Lock_Camera_rotation)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = 1.0f;//IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += Main_script.Input.Look_rotation.x * deltaTimeMultiplier * (Main_script.Speed_rotation * Additional_mouse_sensitivity);

                _cinemachineTargetPitch += Main_script.Input.Look_rotation.y * deltaTimeMultiplier * (Main_script.Speed_rotation * Additional_mouse_sensitivity);
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);//
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);


            // Cinemachine will follow this target
            Main_script.Camera_point.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + Main_script.CameraAngleOverride,
              Character_rotation.transform.rotation.eulerAngles.y, 0.0f);

            Character_rotation.transform.rotation = Quaternion.Euler(0,
      _cinemachineTargetYaw, 0.0f);

        }
    }
}