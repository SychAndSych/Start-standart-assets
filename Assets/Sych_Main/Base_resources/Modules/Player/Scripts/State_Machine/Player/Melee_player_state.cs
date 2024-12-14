using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Player State / Melee state")]
    public class Melee_player_state : Player_abstract_state
    {
        float Speed_movement = 0;

        float Speed_sprint = 0;

        bool Active_attack_bool = false;//Начали атаку

        bool Detect_press_bool = false;

        #region Методы

        public override void Enter_state()
        {
            base.Enter_state();
        }


        public override void Logic_Update()
        {
            base.Logic_Update();

            Move();
        }

        protected override void Initialized_stats()
        {
            Config_object_SO config = Character.Config;

            Speed_movement = config.Get_parameter<float>(nameof(Speed_movement));
            Speed_sprint = config.Get_parameter<float>(nameof(Speed_sprint));
        }

        private void Move()
        {
            float speed = 0;

            if (!Active_attack_bool)
                speed = Character.Input.Sprint_bool ? Speed_sprint : Speed_movement;

            if (Character.Input.Move_vector == Vector2.zero)
                speed = 0;


            Character.Player_Motor_script.Move_First_person(speed, Character.Input.Move_vector);

            if (!Character.Input.Attack_bool)
            {
                Detect_press_bool = false;
            }
                

                if (Character.Input.Attack_bool && !Detect_press_bool && !Active_attack_bool)
                {
                    Detect_press_bool = true;
                    Attack();
                }

        }

        void Attack()
        {
            //тут
            if (Active_bool)
            {
                Active_attack_bool = true;
                //тут
            }
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Позволяет не зациклить в бесконечность последовательность ударов, а определять игроку нужно ли продолжать комбо ударов дальше
        /// </summary>
        public void Reset_combo()
        {
            if (Active_bool)
            {
                Active_attack_bool = false;
                //тут
            }
        }

        /// <summary>
        /// Конец атаки (завершённая атака)
        /// </summary>
        public void End_attack()
        {
            if (Active_bool)
            {
                Active_attack_bool = false;
                //тут
            }
        }

        #endregion
    }
}