using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Player State / First person Move state")]
    public class First_person_movement_Player_state : Player_abstract_state
    {
        float Speed_movement = 0;

        float Speed_sprint = 0;

        public override void Slow_Update()
        {
            base.Slow_Update();
        }

        public override void Physics_Update()
        {
            base.Physics_Update();

            if (Character.Control_bool)
                Move();
        }

        void Move()
        {
            float speed = Character.Input.Sprint_bool ? Speed_sprint : Speed_movement;

            if (Character.Input.Move_vector == Vector2.zero)
                speed = 0;

            Character.Player_Motor_script.Move_First_person(speed, Character.Input.Move_vector);

        }


        protected override void Initialized_stats()
        {
            Config_object_SO config = Character.Config;

            Speed_movement = config.Get_parameter<float>(nameof(Speed_movement));
            Speed_sprint = config.Get_parameter<float>(nameof(Speed_sprint));
        }
    }
}