using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Player State / Archer state")]
    public class Archer_player_state : Player_abstract_state
    {
        float Speed_movement = 0;

        float Speed_sprint = 0;

        float Speed_rotation = 0;

        bool Attack_mode = false;

        Firearm_main_logic Weapon = null;

        public override void Enter_state()
        {
            base.Enter_state();
            Weapon = (Firearm_main_logic)Character.Item_in_hand_handler_script.Active_item;
        }

        public override void Logic_Update()
        {
            base.Logic_Update();

            Move();
        }


        #region Ìועמהû

        protected override void Initialized_stats()
        {
            Config_object_SO config = Character.Config;

            Speed_movement = config.Get_parameter<float>(nameof(Speed_movement));
            Speed_sprint = config.Get_parameter<float>(nameof(Speed_sprint));
            Speed_rotation = config.Get_parameter<float>(nameof(Speed_rotation));
        }

        private void Move()
        {
            float speed = Character.Input.Sprint_bool ? Speed_sprint : Speed_movement;

            if (Attack_mode && speed > Speed_movement)
                speed = Speed_movement;


            if (Character.Input.Move_vector == Vector2.zero)
                speed = 0;

            if (Attack_mode != Character.Input.Attack_bool)
            {
                Attack_mode = Character.Input.Attack_bool;

                //Weapon.Attack(Character.Input.Attack_bool);

                if (Attack_mode)
                {
                    Character.Camera_control_sc.Change_state("First_person");
                }
                else
                {
                    Character.Camera_control_sc.Change_state("Third_person");
                }
            }

            Weapon.Attack(Character.Input.Attack_bool);

            if (!Attack_mode)
            {
                Character.Player_Motor_script.Move_Third_person(speed, Character.Input.Move_vector, Speed_rotation);
            }
            else
            {
                Character.Player_Motor_script.Move_First_person(speed, Character.Input.Move_vector);
            }

        }

        #endregion
    }
}