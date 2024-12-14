//Поведение на вертикальных лестницах
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Player State / Vertical ladder Move state")]
    public class Vertical_ladder_Player_state : Player_abstract_state
    {
        [SerializeField]
        float Speed_movement = 0.1f;

        public override void Physics_Update()
        {
            base.Physics_Update();

            if (Character.Control_bool)
                Move();
        }



        #region Методы

        private void Move()
        {
            float speed = Speed_movement;

            if (Character.Input.Move_vector == Vector2.zero)
                speed = 0;

            Character.Player_Motor_script.Move_on_vertical_surfaces(speed, Character.Input.Move_vector, true, false);
        }

        protected override void Initialized_stats()
        {
            Config_object_SO config = Character.Config;
        }

        #endregion
    }
}