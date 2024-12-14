//Передвижение, прыжок и гравитация от первого лица
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    //[AddComponentMenu("Sych scripts / Game / Player / Player motor first person")]
    [DisallowMultipleComponent]
    public class Player_motor_first_person : Player_Motor_abstract
    {
        /// <summary>
        /// Передвижение для персонажа от  1-го лица
        /// </summary>
        /// <param name="speed_movement_">Скоросить передвижения</param>
        /// <param name="_direction">Направление движения</param>
        public override void Move(float speed_movement_, Vector2 _direction)
        {
            float speed = speed_movement_;

            Vector3 moveDirectionForward = transform.forward * _direction.y;
            Vector3 moveDirectionSide = transform.right * _direction.x;

            Vector3 direction = (moveDirectionForward + moveDirectionSide).normalized;

            Vector3 inputDirection = new Vector3(_direction.x, 0.0f, _direction.y).normalized;

            CharacterController_script.Move(direction * (speed * Time.deltaTime) +
                     new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }
    }
}