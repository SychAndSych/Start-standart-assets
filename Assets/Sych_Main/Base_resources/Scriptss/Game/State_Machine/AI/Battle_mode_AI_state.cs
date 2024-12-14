using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / AI / AI State / Battle state")]
    public class Battle_mode_AI_state : State_abstract
    {

        [Tooltip("Время паузы между атаками")]
        [SerializeField]
        Vector2 Pause_attack_time = new Vector2(0.5f, 2f);

        [Tooltip("Дистанция атаки")]
        [SerializeField]
        float Distance_attack = 0.2f;

        [Tooltip("Скорость движения назад")]
        [SerializeField]
        float Speed_back = 0;

        float Time_pause_attack = 0;

        bool Move_bool = false;

        bool Additional_rotation_look_bool = false;

        float Size_target = 0;

        float My_size = 0;

        private AI_character Character;

        Game_object_abstract Active_target = null;

        public override void Enter_state()
        {
            base.Enter_state();
            Character.Agent.New_target_move(Character.Target);
            Move_bool = true;
            Time_pause_attack = 0;
        }

        protected override void Preparation()
        {
            base.Preparation();

            Character = (AI_character)Character_script;
        }

        public override void Slow_Update()
        {
            base.Slow_Update();

            if(Active_target != Character.Target && Character.Target != null)
            {
                if (Character.Target.GetComponent<Game_object_abstract>())
                {
                    Active_target = Character.Target.GetComponent<Game_object_abstract>();
                    Character.Agent.New_target_move(Active_target.transform);
                }

            }
            

            if (Move_bool)
            {
                if (Active_target.GetComponent<Health>().Alive_bool) 
                {
                    
                    if (Vector3.Distance(Character.transform.position, new Vector3(Active_target.transform.position.x, Character.transform.position.y, Active_target.transform.position.z)) <= Distance_attack + Size_target + My_size)//if (Character.Agent.Find_out_Remaining_distance(Character.transform.position, Character.Target.position) <= Distance_attack)
                    {
                        if (Time_pause_attack <= 0 && Character.Check_look_rotation(Active_target.transform))
                        {
                            Attack();
                            Additional_rotation_look_bool = false;
                        }
                        else
                        {
                            Change_move(false);
                            Additional_rotation_look_bool = true;
                        }

                    }
                    else
                    {
                            Change_move(true);
                    }
                }
                else
                {
                    Change_move(false);
                    Character.Agent.Off_all_coroutine();
                }
            }


        }

        public override void Slow_Update_2()
        {
            base.Slow_Update_2();

            if (Active_target)
            {
                My_size = (Character.Collider_size.bounds.size.x / 2 + Character.Collider_size.bounds.size.z / 2) / 2;

                Size_target = (Active_target.Collider_size.bounds.size.x / 2 + Active_target.Collider_size.bounds.size.z / 2) / 2;
                Character.Agent.NavMeshAgent_.stoppingDistance = Size_target + My_size;

            }
        }

        public override void Logic_Update()
        {
            base.Logic_Update();

            if (Time_pause_attack > 0)
                Pause_attack();

            if(Additional_rotation_look_bool)
                Additional_rotation_look();
        }

        /// <summary>
        /// Отдыхаем и ждём
        /// </summary>
        void Pause_attack()
        {
            if (Time_pause_attack > 0)
            {
                Time_pause_attack -= Time.deltaTime;
            }

        }

        void Change_move(bool _bool)
        {
            Character.Agent.Stop_move_activity(!_bool);
        }

        void Attack()
        {
            Move_bool = false;

            Character.Agent.Stop_move_activity(true);
        }

        /// <summary>
        /// Продолжаем путь
        /// </summary>
        public void End_attack()
        {
            Time_pause_attack = Random.Range(Pause_attack_time.x, Pause_attack_time.y);

            Move_bool = true;
        }

        protected override void Initialized_stats()
        {
            //throw new System.NotImplementedException();
        }

        #region Дополнительные методы
        /// <summary>
        /// Дополнительный доворот в сторону цели
        /// </summary>
        public void Additional_rotation_look()
        {
            var direction = ( new Vector3(Active_target.transform.position.x, Character.transform.position.y, Active_target.transform.position.z) - Character.transform.position).normalized;

            Character.transform.rotation = Quaternion.RotateTowards(Character.transform.rotation, Quaternion.LookRotation(direction), 6.8f);
        }

        /// <summary>
        /// Дополнительное движение назад, что бы не стоять вплотную к цели
        /// </summary>
        public void Additional_move_back()
        {
            if ((Vector3.Distance(Active_target.transform.position, Character.transform.position)) < Distance_attack * 0.6f)
            {
                transform.position -= transform.forward * (Speed_back * Time.deltaTime);
            }
        }
        #endregion

    }
}