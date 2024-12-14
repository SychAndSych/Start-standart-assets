//Передвигает объект по точкам
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Sych_scripts
{
    enum Movement_enum
    {
        Last_end,//Дойти до последней точки и остановится
        Loop,//Зациклить движение от первой к последней и заново (перейдёт от последней к первой)
        Ping_pong//Зациклить передвижение от первой к последней и обратно
    }

    [AddComponentMenu("Sych scripts / Game / Movement / Movement object to points")]
    [DisallowMultipleComponent]
    public class Movement_object_to_points : MonoBehaviour
    {
        [Tooltip("Вращать по физике?")]
        [SerializeField]
        bool Physics_bool = false;

        [ShowIf(nameof(Physics_bool))]
        [Tooltip("Перемещает по физике (если вдруг нужно)")]
        [SerializeField]
        Rigidbody Body = null;

        [Tooltip("Точки движения")]
        [SerializeField]
        Transform[] Points_array = new Transform[0];

        Vector3[] Points_vector_array = new Vector3[0];

        [Tooltip("Скорость")]
        [SerializeField]
        public float Speed = 0.01f;

        [Tooltip("Тип логики передвижения между точками")]
        [SerializeField]
        Movement_enum Type_movement = Movement_enum.Last_end;



        [Tooltip("Стартует сразу при запуске")]
        [SerializeField]
        bool Start_movement_bool = false;

        [Tooltip("Объект который будем передвигать. (Если False, то двигаем, то на чём висит скрипт)")]
        [SerializeField]
        bool Target_object_bool = false;

        [ShowIf(nameof(Target_object_bool))]
        [Tooltip("Объект который будем передвигать")]
        [SerializeField]
        Transform Target_object_transform = null;

        [Tooltip("Поворачивает объект в сторону движения")]
        [SerializeField]
        bool Rotation_direct_way_bool = false;

        [ShowIf(nameof(Rotation_direct_way_bool))]
        [Tooltip("Объект который будем передвигать")]
        [SerializeField]
        float Speed_rotation = 0.8f;

        [Tooltip("Будет останавливаться каждый раз когда дойдёт до новой точки")]
        [SerializeField]
        bool Stop_next_point_bool = false;

        [Tooltip("Событие когда достиг конечной точки")]
        [SerializeField]
        UnityEvent End_point_event = new UnityEvent();


        [Foldout("Внутриние параметры(не трогать, если не нужно")]
        [Space(20)]

        [Tooltip("Увидеть больше")]
        [SerializeField]
        bool Gizmos_mode_bool = false;

        [ShowIf(nameof(Gizmos_mode_bool))]
        [Foldout("Внутриние параметры(не трогать, если не нужно")]
        [Tooltip("Меш объекта")]
        [SerializeField]
        Mesh Object_mesh = null;

        [ShowIf(nameof(Gizmos_mode_bool))]
        [Foldout("Внутриние параметры(не трогать, если не нужно")]
        [Tooltip("Расположение на линии")]
        [Range(0f, 1f)]
        [SerializeField]
        float Step = 0;


        bool Active_bool = false;

        Vector3 Start_point = Vector3.zero;

        Quaternion Start_rotation = Quaternion.identity;

        Quaternion End_rotation = Quaternion.identity;

        Vector3 End_point = Vector3.zero;

        int id_end_point = 1;

        bool Reverse_movement_bool = false;//Движение в обратном направление

        #region Системные методы

        private void Start()
        {
            if (Start_movement_bool)
            {
                Active_bool = true;

                if (Points_array.Length >= 2)
                {
                    Start_point = Points_array[0].position;
                    End_point = Points_array[1].position;
                }
                else
                {
                    Active_bool = false;
                    Debug.Log("Недостаточно точек для передвижения!");
                }
            }
        }

            private void FixedUpdate()
        {
            if (Active_bool)
            {
                if (Movement_object())
                {
                    if(Points_vector_array.Length > 0)
                        Next_way_vector();
                    else
                        Next_way_transform();
                }
            }
        }

        #endregion


        #region Методы

        /// <summary>
        /// Назначить новые точки для передвижения
        /// </summary>
        void Next_way_transform()
        {
            if (Stop_next_point_bool)
                Active_bool = false;

            Step = 0;

            if(Rotation_direct_way_bool)
            Start_rotation = Target_object_bool ? Target_object_transform.rotation : transform.rotation;

            if (Points_array.Length - 1 > id_end_point && !Reverse_movement_bool || 0 < id_end_point && Reverse_movement_bool)
            {
                if (!Reverse_movement_bool)
                {
                    id_end_point++;
                    
                    Start_point = Points_array[id_end_point - 1].position;
                    End_point = Points_array[id_end_point].position;
                }
                else
                {
                    id_end_point--;

                    Start_point = Points_array[id_end_point + 1].position;
                    End_point = Points_array[id_end_point].position;
                }

            }
            else
            {
                End_point_event.Invoke();

                switch (Type_movement)
                {
                    case Movement_enum.Last_end:
                        Active_bool = false;
                        break;

                    case Movement_enum.Loop:
                        Start_point = Points_array[id_end_point].position;
                        End_point = Points_array[0].position;
                        id_end_point = 0;
                        break;

                    case Movement_enum.Ping_pong:
                        if (!Reverse_movement_bool)
                        {
                            Start_point = Points_array[id_end_point].position;
                            End_point = Points_array[id_end_point - 1].position;

                            id_end_point = id_end_point - 1;

                            Reverse_movement_bool = true;
                        }
                        else
                        {
                            Start_point = Points_array[0].position;
                            End_point = Points_array[id_end_point + 1].position;

                            id_end_point = id_end_point + 1;

                            Reverse_movement_bool = false;
                        }
                        break;
                }
            }
        }



        void Next_way_vector()
        {
            if (Stop_next_point_bool)
                Active_bool = false;

            Step = 0;

            if(Rotation_direct_way_bool)
            Start_rotation = Target_object_bool ? Target_object_transform.rotation : transform.rotation;

            if (Points_vector_array.Length - 1 > id_end_point && !Reverse_movement_bool || 0 < id_end_point && Reverse_movement_bool)
            {
                if (!Reverse_movement_bool)
                {
                    id_end_point++;

                    Start_point = Points_vector_array[id_end_point - 1];
                    End_point = Points_vector_array[id_end_point];
                }
                else
                {
                    id_end_point--;

                    Start_point = Points_vector_array[id_end_point + 1];
                    End_point = Points_vector_array[id_end_point];
                }

            }
            else
            {
                End_point_event.Invoke();

                switch (Type_movement)
                {
                    case Movement_enum.Last_end:
                        Active_bool = false;
                        break;

                    case Movement_enum.Loop:
                        Start_point = Points_vector_array[id_end_point];
                        End_point = Points_vector_array[0];
                        id_end_point = 0;
                        break;

                    case Movement_enum.Ping_pong:
                        if (!Reverse_movement_bool)
                        {
                            Start_point = Points_vector_array[id_end_point];
                            End_point = Points_vector_array[id_end_point - 1];

                            id_end_point = id_end_point - 1;

                            Reverse_movement_bool = true;
                        }
                        else
                        {
                            Start_point = Points_vector_array[0];
                            End_point = Points_vector_array[id_end_point + 1];

                            id_end_point = id_end_point + 1;

                            Reverse_movement_bool = false;
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Передвижение и спрос
        /// </summary>
        /// <returns>Мы приехали?</returns>
        bool Movement_object()
        {
            bool result = false;

            /*
                if (Step < 1)
                {
                    Step += Speed;

                }
                else if (Step >= 1)
                {
                    result = true;
                }
            */

            Transform target = Target_object_bool ? Target_object_transform : transform;


            if (Rotation_direct_way_bool)
            {
                End_rotation = Quaternion.LookRotation(End_point - target.position);
                target.rotation = Quaternion.RotateTowards(target.rotation, End_rotation, Speed_rotation);
            }
                

            //target.position = Vector3.Lerp(Start_point, End_point, Step);
            
            if(!Physics_bool)
                target.position = Vector3.MoveTowards(target.position, End_point, Speed * Time.deltaTime);
            else
                Body.MovePosition( Vector3.MoveTowards(target.position, End_point, Speed));
            //Body.position = Vector3.MoveTowards(target.position, End_point, Speed);

            if (target.position == End_point)
                result = true;
            
            return result;
        }

        #endregion


        #region Публичные методы

        /// <summary>
        /// Вкл/Выкл
        /// </summary>
        /// <param name="_activity">Активность</param>
        public void Activity(bool _activity)
        {
            Active_bool = _activity;
        }

        /// <summary>
        /// Задать точки пути
        /// </summary>
        /// <param name="_points">Массив точек</param>
        public void Set_points(Vector3[] _points, float _speed)
        {
            Points_vector_array = _points;

            if (Points_vector_array.Length >= 2)
            {
                Speed = _speed;
                Start_point = Points_vector_array[0];
                End_point = Points_vector_array[1];
                Active_bool = true;
            }
            else
            {
                Active_bool = false;
                Debug.Log("Недостаточно точек для передвижения!");
            }

        }

        #endregion


        #region Проверяющие методы

        private void OnDrawGizmos()
        {

            if (Gizmos_mode_bool) 
            {
                Gizmos.color = Color.red;

                switch (Type_movement)
                {
                    case Movement_enum.Loop:
                        for (int x = 0; x < Points_array.Length - 1; x++)
                        {
                            Gizmos.DrawLine(Points_array[x].position, Points_array[x + 1].position);
                        }

                        Gizmos.DrawLine(Points_array[Points_array.Length - 1].position, Points_array[0].position);
                        break;

                    default:
                        for (int x = 0; x < Points_array.Length - 1; x++)
                        {
                            Gizmos.DrawLine(Points_array[x].position, Points_array[x+1].position);
                        }
                        break;
                }


                if (Object_mesh)
                {
                    int sigment_number = 20;

                    Vector3 preveuse_point = Start_point;

                    for (int x = 0; x < sigment_number; x++)
                    {
                        float perimeter = (float)x / sigment_number;
                        Vector3 point = Vector3.Lerp(Start_point, End_point, perimeter);
                        Gizmos.DrawLine(preveuse_point, point);
                        preveuse_point = point;
                    }

                    Vector3 pos = Vector3.Lerp(Start_point, End_point, Step);

                    Gizmos.DrawMesh(Object_mesh, pos, transform.rotation, transform.localScale - new Vector3(-0.001f, -0.001f, -0.001f));

                    Gizmos.DrawMesh(Object_mesh, Start_point, transform.rotation, transform.localScale * 0.8f);
                    Gizmos.DrawMesh(Object_mesh, End_point, transform.rotation, transform.localScale * 0.8f);
                }
            }
        }
        #endregion

    }
}