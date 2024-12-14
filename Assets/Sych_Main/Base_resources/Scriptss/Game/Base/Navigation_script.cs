//Для поиска пути по NavMesh и прочие проверки с ним
//Точка поиска пути должна быть выше земли на которой NavMesh, чем указана высота при герерации NavMesh
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / Navigation navmesh")]
    [DisallowMultipleComponent]
    public class Navigation_script : MonoBehaviour
    {
        NavMeshPath NavMeshPath_;// путь до цели на невмеше

        [Tooltip("Узнать больше")]
        [SerializeField]
        bool Debug_mode = false;

        void Start()
        {
            NavMeshPath_ = new NavMeshPath();
        }


        /// <summary>
        /// Создать случайную точку для передвижения к ней
        /// </summary>
        public Vector3 Nav_random_point_target(float _radius)
        {
            bool get_correct_point_bool = false;//Сгенерировалась ли корректная точка (до которой можно добраться)
            Vector3 test_new_point = Vector3.zero;

            int step_alert = 0;//Сколько раз понадобилось для решения

            while (!get_correct_point_bool)
            {
                NavMeshHit nav_hit;

                NavMesh.SamplePosition(Random.insideUnitSphere * _radius + transform.position, out nav_hit, _radius, NavMesh.AllAreas);
                test_new_point = nav_hit.position;

                if (Check_path_comlete(test_new_point))
                    get_correct_point_bool = true;
                else if (step_alert > 100)
                {
                    get_correct_point_bool = true;
                    Debug.LogError("Фатальная ошибка, не смог найти путь!((");
                }

                step_alert++;
            }

            return test_new_point;
        }


        /// <summary>
        /// Проверить, можно ли дойти до этой точки
        /// </summary>
        /// <param name="_target">Конечная точка</param>
        /// <returns>Результат</returns>
        protected bool Check_path_comlete(Vector3 _target)
        {
            bool result_bool = false;

            NavMesh.CalculatePath(transform.position, _target, NavMesh.AllAreas, NavMeshPath_);//Заполнить путь corners

            if (NavMeshPath_.status == NavMeshPathStatus.PathComplete)
            {
                result_bool = true;
            }

            return result_bool;
        }


        private void OnDrawGizmos()
        {
            if (Debug_mode && NavMeshPath_ != null)
            {
                if(NavMeshPath_.corners.Length > 1)
                {
                    Gizmos.color = Color.red;

                    for (int i = 0; i < NavMeshPath_.corners.Length - 1; i++)
                        Gizmos.DrawLine(NavMeshPath_.corners[i], NavMeshPath_.corners[i + 1]);
                }

            }
        }

    }
}