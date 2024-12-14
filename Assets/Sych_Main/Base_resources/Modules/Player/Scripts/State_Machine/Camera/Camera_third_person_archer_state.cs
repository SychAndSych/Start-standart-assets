using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Camera state / Third person archer")]
    [DisallowMultipleComponent]
    public class Camera_third_person_archer_state : Camera_state_abstract
    {
        [Tooltip("Тушка самого персонажа для поворота")]
        [SerializeField]
        Transform Character_rotation = null;

        [Tooltip("Скорость поворота персонажа за камерой")]
        [SerializeField]
        float Speed_character_rotation = 15f;


        public override void Camera_rotation()
        {
            // if there is an input and camera position is not fixed
            if (Main_script.Input.Look_rotation.sqrMagnitude >= _threshold && !Lock_Camera_rotation)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = 1.0f;//IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += Main_script.Input.Look_rotation.x * deltaTimeMultiplier * Main_script.Speed_rotation;

                _cinemachineTargetPitch += Main_script.Input.Look_rotation.y * deltaTimeMultiplier * Main_script.Speed_rotation;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);//
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            Quaternion rot_cam = Main_script.Cam.transform.rotation;
            rot_cam.x = 0;
            rot_cam.z = 0;
            Character_rotation.transform.rotation = Quaternion.RotateTowards(Character_rotation.transform.rotation, rot_cam, Speed_character_rotation);

            // Cinemachine will follow this target
            Main_script.Camera_point.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + Main_script.CameraAngleOverride,
     _cinemachineTargetYaw, 0.0f);

        }
    }
}