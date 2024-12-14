using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Player State / Third person Move state")]
    public class Third_person_movement_Player_state : Player_abstract_state
    {

        [SerializeField]
        float Speed_movement = 8;

        [SerializeField]
        float Speed_sprint = 25;

        [SerializeField]
        float Speed_rotation = 0.1f;

        public override void Physics_Update()
        {
            base.Physics_Update();

            if (Character.Control_bool)
                Move();
        }



        #region Методы

        private void Move()
        {
            float speed = Character.Input.Sprint_bool? Speed_sprint : Speed_movement;

                if (Character.Input.Move_vector == Vector2.zero)
                speed = 0;

            Character.Player_Motor_script.Move_Third_person(speed, Character.Input.Move_vector, Speed_rotation);
        }

        protected override void Initialized_stats()
        {
            Config_object_SO config = Character.Config;

            //Speed_movement = config.Get_parameter<float>(nameof(Speed_movement));
            //Speed_sprint = config.Get_parameter<float>(nameof(Speed_sprint));
            //Speed_rotation = config.Get_parameter<float>(nameof(Speed_rotation));
        }

        #endregion



        #region Дополнительно


        #endregion
    }
}