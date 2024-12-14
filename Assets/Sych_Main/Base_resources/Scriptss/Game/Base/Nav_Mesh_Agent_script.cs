//Скрипт отвечающий за работу с NavMeshAgent и имеет заготовленные команды
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / NavMeshAgent script")]
    [RequireComponent(typeof(NavMeshAgent))]
    [DisallowMultipleComponent]
    public class Nav_Mesh_Agent_script : MonoBehaviour
    {
        #region Перменные

        [Tooltip("Как близко нужно подойти к цели (если выбран режим преследования)")]
        [SerializeField]
        float Distance_target = 2f;

        [Tooltip("Аниматор для задавания скорости передвижения")]
        [SerializeField]
        Animator Anim = null;

        [Tooltip("Эвент, дошёл до цели которую преследовал")]
        [SerializeField]
        UnityEvent Distance_target_event = new UnityEvent();

        NavMeshPath NavMeshPath_;// путь до цели на невмеше

        Coroutine Coroutine_Update_target = null;//Коррутина для обновления слежения за целью (если она передвигается)

        Transform Target = null;//Цель

        Transform My_transform = null;

        internal NavMeshAgent NavMeshAgent_ = null;

        #endregion

        #region CallBack Методы

        protected void Awake()
        {
            NavMeshPath_ = new NavMeshPath();
            NavMeshAgent_ = GetComponent<NavMeshAgent>();
        }

        protected void Start()
        {
            My_transform = transform;
            NavMeshAgent_.avoidancePriority = Random.Range(4, 50);
        }

        private void LateUpdate()
        {
            if(Anim)
            Anim.SetFloat("Speed_movement", NavMeshAgent_.velocity.magnitude);
        }

        #endregion


        #region Управляющие методы

        /// <summary>
        /// Назначить новую цель
        /// </summary>
        /// <param name="_target">Цель</param>
        public virtual void New_target_move(Transform _target)
        {
                Target = _target;
                if (Coroutine_Update_target != null)
                    StopCoroutine(Update_target_coroutine());

                Coroutine_Update_target = StartCoroutine(Update_target_coroutine());
        }

        public virtual void New_target_move(Vector3 _target)
        {
            NavMeshAgent_.SetDestination(_target);
        }

        public void Off_all_coroutine()
        {
            StopAllCoroutines();
        }

        IEnumerator Update_target_coroutine()//Обновление реакции ИИ
        {

            while (Target)
            {
                if (Vector3.Distance(Target.position, My_transform.position) > Distance_target)
                {
                    if (NavMeshAgent_.enabled)
                        New_target_move(Target.position);
                    else
                        break;
                }
                else
                {
                    Distance_target_event.Invoke();
                }
                yield return new WaitForSeconds(0.4f);
            }

        }


        /// <summary>
        /// Создать случайную точку для передвижения к ней
        /// </summary>
        public Vector3 Nav_random_point_target(float _radius)
        {
            bool get_correct_point_bool = false;//Сгенерировалась ли корректная точка (до которой можно добраться)
            Vector3 test_new_point = Vector3.zero;

            bool fatal_error = false;
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
                    fatal_error = true;
                    Debug.Log("Фатальная ошибка, не смог найти путь!((");
                }

                step_alert++;
            }

            if (!fatal_error)
            {
                New_target_move(test_new_point);
            }

            return test_new_point;
        }



        /// <summary>
        /// Остановить движение или продолжить
        /// </summary>
        /// <param name="_activity">Остановить?</param>
        public void Stop_move_activity(bool _activity)
        {
            NavMeshAgent_.isStopped = _activity;

            if (_activity)
            {
                if (Coroutine_Update_target != null)
                    StopCoroutine(Update_target_coroutine());

                NavMeshAgent_.velocity = new Vector3(NavMeshAgent_.velocity.x * 0.2f, NavMeshAgent_.velocity.y, NavMeshAgent_.velocity.z * 0.2f); //new Vector3(0, NavMeshAgent_.velocity.y, 0);
            }

        }
        #endregion


        #region Проверяющие методы
        /// <summary>
        /// Проверить, можно ли дойти до этой точки
        /// </summary>
        /// <param name="_target">Конечная точка</param>
        /// <returns>Результат</returns>
        protected bool Check_path_comlete(Vector3 _target)
        {
            bool result_bool = false;

            //if(NavMeshAgent_.isOnNavMesh)
            NavMeshAgent_.CalculatePath(_target, NavMeshPath_);

            if (NavMeshPath_.status == NavMeshPathStatus.PathComplete)
            {
                result_bool = true;
            }

            return result_bool;
        }

        /*            //Придумать решение для NavMeshAgent_.remainingDistance
        /// <summary>
        /// Узнать расстояние между двумя точками
        /// </summary>
        /// <param name="_target_1"></param>
        /// <param name="_target_2"></param>
        /// <returns></returns>
        public float Find_out_Remaining_distance(Vector3 _target_1, Vector3 _target_2)
        {
            float result_distance = 0;

            Vector3[] corners = NavMeshAgent_.path.corners;
            
            if (corners.Length > 2)
            {
                for (int x = 1; x < corners.Length; x++)
                {
                    Vector2 previous = new Vector2(corners[x - 1].x, corners[x - 1].z);
                    Vector2 current = new Vector2(corners[x].x, corners[x].z);

                    result_distance += Vector2.Distance(previous, current);
                }
            }
            
            else
            {
                result_distance = NavMeshAgent_.remainingDistance;

                if (result_distance == 0 && Vector3.Distance(_target_1, _target_2) > NavMeshAgent_.stoppingDistance + 1)
                {
                    result_distance = Vector3.Distance(_target_1, _target_2);
                    Debug.LogError("Не смог сразу определить растояние, нужно исправление!");
                }

            }
            
            return result_distance;
        }
        */

        /// <summary>
        /// Узнать расстояние между двумя точками
        /// </summary>
        /// <param name="_target_1"></param>
        /// <param name="_target_2"></param>
        /// <returns></returns>
        public float Find_out_Remaining_distance(Vector3 _target_1, Vector3 _target_2)
        {
            float result_distance = 0;

            Vector3[] corners = NavMeshAgent_.path.corners;

                for (int x = 1; x < corners.Length; x++)
                {
                    Vector2 previous = new Vector2(corners[x - 1].x, corners[x - 1].z);
                    Vector2 current = new Vector2(corners[x].x, corners[x].z);

                    result_distance += Vector2.Distance(previous, current);
                }

                if (result_distance == 0 && Vector3.Distance(_target_1, _target_2) > NavMeshAgent_.stoppingDistance + 1)
                {
                    result_distance = Vector3.Distance(_target_1, _target_2);
                    Debug.LogError(transform.name + "  Не смог сразу определить растояние, нужно исправление!");
                }

            return result_distance;
        }

        #endregion
    }
}
