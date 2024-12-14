using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Sych_scripts
{

    public class Navigator_NavMeshPath : Navigator_abstract
    {
        #region Переменные

        [Tooltip("Время между обновлениями")]
        [SerializeField]
        float Time_update = 1f;

        Coroutine Way_coroutine = null;

        List<Vector3> Point_list = new List<Vector3>();//Лист точек

        NavMeshPath NavMeshPath_;// путь до цели на невмеше
        #endregion



        #region Методы

        void Start_way()
        {
            if(Way_coroutine != null)
            {
                StopCoroutine(Way_coroutine);
            }

                StartCoroutine(Coroutine_way_update());
        }

        IEnumerator Coroutine_way_update()
        {
            yield return new WaitForSeconds(Time_update);
        }

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

        #endregion


        #region Публичные методы
        public override void Stop_navigator()
        {
            base.Stop_navigator();

            if (Way_coroutine != null)
            {
                StopCoroutine(Way_coroutine);
            }

            Way_coroutine = null;
        }
        #endregion


        #region Дополнительное
        private void OnDrawGizmos()
        {
            if (Debug_mode && NavMeshPath_ != null)
            {
                if (NavMeshPath_.corners.Length > 1)
                {
                    Gizmos.color = Color.red;

                    for (int i = 0; i < NavMeshPath_.corners.Length - 1; i++)
                        Gizmos.DrawLine(NavMeshPath_.corners[i], NavMeshPath_.corners[i + 1]);
                }

            }
        }
        #endregion
    }
}