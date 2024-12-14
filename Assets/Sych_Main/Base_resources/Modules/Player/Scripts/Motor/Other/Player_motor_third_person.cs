//Передвижение, прыжок и гравитация от третьего лица
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    //[AddComponentMenu("Sych scripts / Game / Player / Player motor third person")]
    [DisallowMultipleComponent]
    public class Player_motor_third_person : Player_Motor_abstract
    {

        [Tooltip("Скорость поворота при движение")]
        [SerializeField]
        float Speed_rotation_character = 0.3f;

        #region Публичные методы

        /// <summary>
        /// Передвижение для персонажа от  3-го лица (когда крутим камеру отдельно,а персонаж идёт от направления куда смотрит камера)
        /// </summary>
        /// <param name="speed_movement_">Скоросить передвижения</param>
        /// <param name="_direction">Направление движения</param>
        /// <param name="_speed_rotation_character">Скорость поворота персонажа (тела)</param>
        public override void Move(float speed_movement_, Vector2 _direction)
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = speed_movement_;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(CharacterController_script.velocity.x, 0.0f, CharacterController_script.velocity.z).magnitude;

            float speedOffset = 0.1f;
            //float inputMagnitude = Character.Input.Analog_Movement_bool ? Character.Input.Move_vector.magnitude : 1f;

            float inputMagnitude = _direction.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                Speed_active = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                Speed_active = Mathf.Round(Speed_active * 1000f) / 1000f;
            }
            else
            {
                Speed_active = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_direction.x, 0.0f, _direction.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_direction != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  Cam.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(Rotation_transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    Speed_rotation_character);

                // rotate to face input direction relative to camera position
                Rotation_transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            CharacterController_script.Move(targetDirection.normalized * (Speed_active * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (Anim)
                Anim.SetFloat("Speed", _animationBlend);
            //Anim.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }


        #endregion
    }
}