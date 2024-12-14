using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / AI / AI State / Peaceful state")]
    public class Peaceful_AI_mode_state : State_abstract
    {
        [Tooltip("������ ����������� ������")]
        [SerializeField]
        float Radius_detect  = 10f;

        [Tooltip("��� ���������� (���� ����� ��������� AI � ����� �����")]
        [SerializeField]
        string[] Fraction_enemy_array = new string[1] { "Player" };

        [Tooltip("����� ����� �� ����� ������������ ����� �������")]
        [SerializeField]
        Vector2 Pause_walk_time  = new Vector2(2, 8);

        [Tooltip("������������� ����� ����-����")]
        [SerializeField]
        bool Auto_random_walk_bool = true;

        float Time_pause = 0;

        Vector3 End_point;

        bool Move_bool = false;


        private AI_character Character;


        public override void Enter_state()
        {
            base.Enter_state();
        }

        
        protected override void Preparation()
        {
            base.Preparation();
            Character = (AI_character)Character_script;

            if (Auto_random_walk_bool)
                Random_walk();
        }

        public override void Exit_state()
        {
            base.Exit_state();
        }

        public override void Slow_Update()
        {
            base.Slow_Update();

            if (Move_bool)
            {

                if (Character.Agent.Find_out_Remaining_distance(Character.transform.position, End_point) <= 0.5f)
                {
                    Move_bool = false;

                    Time_pause = Random.Range(Pause_walk_time.x, Pause_walk_time.y);
                }
            }


            Detect_enemy();
        }



        public override void Logic_Update()
        {
            base.Logic_Update();

            if (!Move_bool)
            {
                Pause();
            }
        }


        /// <summary>
        /// ���� ������
        /// </summary>
        void Detect_enemy()
        {
            Collider[] check_array = Physics.OverlapSphere(Character.transform.position, Radius_detect, 1 << 0);



            for (int x = 0; x < check_array.Length; x++)
            {
                for (int tag_x = 0; tag_x < Fraction_enemy_array.Length; tag_x++)
                {
                  if(check_array[x].GetComponent<Game_character_abstract>())
                    if (check_array[x].GetComponent<Game_character_abstract>().Fraction_name == Fraction_enemy_array[tag_x])
                    {
                        Character.Target = check_array[x].transform;
                        Character.Brain_script.Change_state("Battle");
                        break;
                    }
                }
                //����� ����� ������������� �� ������������� (� �� �����, �� ����� ���������� � ������ �����������
            }
        }


        /// <summary>
        /// �������� � ���
        /// </summary>
        void Pause()
        {
            if (Time_pause > 0)
            {
                Time_pause -= Time.deltaTime;
            }
            else
            {
                Random_walk();
            }
        }

        /// <summary>
        /// ��������� ����� � ��� � ���
        /// </summary>
        void Random_walk()
        {
            Character.Agent.Stop_move_activity(false);
            End_point = Character.Agent.Nav_random_point_target(50);
            Invoke(nameof(Delay_move_ON), 0.1f);
        }

        void Delay_move_ON()
        {
            Move_bool = true;
        }

        protected override void Initialized_stats()
        {
            
        }


        #region �������������
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 1, 0.2f);
            Gizmos.DrawSphere(transform.position, Radius_detect);
        }
        #endregion
    }
}