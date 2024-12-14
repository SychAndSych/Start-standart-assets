using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / AI / AI character")]
    [DisallowMultipleComponent]
    public class AI_character : Game_character_abstract
    {
        #region Переменные
        [field: Space(20)]
        [field: Header("Настройки ИИ")]

        [field: Tooltip("Скрипт навигации")]
        [field: SerializeField]
        public Nav_Mesh_Agent_script Agent { get; private set; } = null;

        internal Transform Target = null;//Цель

        #endregion


        #region Методы

        protected override void Initialized_stats()
        {
            //Speed_back = Config.Get_parameter<float>(nameof(Speed_back));
            //Agent.NavMeshAgent_.speed = Config.Get_parameter<float>("Speed");
            //Agent.NavMeshAgent_.angularSpeed = Config.Get_parameter<float>("Rotation");
        }


        #endregion


        #region Публичные методы
        /// <summary>
        /// Погиб
        /// </summary>
        public override void Dead()
        {
            Agent.Stop_move_activity(true);
            Agent.NavMeshAgent_.enabled = false;

            base.Dead();

        }
        #endregion

        #region Управляющие мотоды

        public void New_movement(Vector3 _position)
        {
            Brain_script.Off_state();
            Agent.Off_all_coroutine();
            Agent.Stop_move_activity(false);
            Agent.New_target_move(_position);
        }


        #endregion

        #region Проверяющие методы
        /// <summary>
        /// Проверить визуально на наличие препятсвий от целей
        /// </summary>
        /// <param name="_target">Цель</param>
        /// <returns>Ответ</returns>
        public bool Check_visual(Transform _target)
        {
            bool result = false;

            Ray ray = new Ray(Head.position, My_transform.forward);

            RaycastHit hit;

            if (Physics.Linecast(Head.position, _target.position, out hit))
            {
                if (hit.transform.tag != "Player")
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Проверить, повёрнут ли в сторону цели
        /// </summary>
        /// <param name="_target">Цель</param>
        /// <returns>Ответ</returns>
        public bool Check_look_rotation(Transform _target)
        {
            bool result = false;

            Vector3 direction = new Vector3(_target.position.x, My_transform.position.y, _target.position.z) - My_transform.position;
            Quaternion qua = Quaternion.LookRotation(direction.normalized);

            if (Quaternion.Angle(My_transform.rotation, qua) <= 20f)
            {
                result = true;
            }

            return result;
        }


        /// <summary>
        /// Проверяет есть ли препятствие между 2-мя точками
        /// </summary>
        /// <param name="_target_1">1 Точка</param>
        /// <param name="_target_2">2 Точка</param>
        /// <returns>Результат</returns>
        public bool Check_no_obstacle_in_way(Vector3 _target_1, Vector3 _target_2)
        {
            bool result_bool = true;

            NavMeshHit NavMeshHit_;//Для проверки препятствий на невмеше

            if (NavMesh.Raycast(_target_1, _target_2, out NavMeshHit_, NavMesh.AllAreas))
            {
                result_bool = false;
            }

            return result_bool;
        }


        #endregion

    }
}