//Скрипт который позволяет составить пошаговые рывки, которые будут воспроизведены автоматически
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Lean.Pool;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;

namespace Sych_scripts
{
    public class ShockBlast : MonoBehaviour
    {

        #region Переменные

        [Tooltip("Владелец скила (кого будет передвигать)")]
        [SerializeField]
        Transform Owner = null;

        [Tooltip("Максимальная дистанция отрезка рывка")]
        [SerializeField]
        float Max_distance = 10f;

        [Tooltip("Префаб для установке точки")]
        [SerializeField]
        GameObject Prefab_point = null;

        [SerializeField]
        Camera Cam = null;

        [Tooltip("Слой для взаимодействия луча")]
        [SerializeField]
        LayerMask Detect_layer = 1;

        [Tooltip("Скорость передвижение")]
        [SerializeField]
        float Speed_move = 2f;

        [Tooltip("Количество устанавливаемых точек")]
        [SerializeField]
        int Max_point = 3;

        [Tooltip("Визуализация линии")]
        [SerializeField]
        LineRenderer Line = null;


        [Tooltip("Ивент начала")]
        [SerializeField]
        UnityEvent Start_event = new UnityEvent();

        [Tooltip("Ивент окончания")]
        [SerializeField]
        UnityEvent End_event = new UnityEvent();



        List<Vector3> Position_point_list = new List<Vector3>();

        List<GameObject> Point_spawned_list = new List<GameObject>();

        bool Active_bool = false;

        bool Process_bool = false;

        Vector3 Mouse_pos = Vector3.zero;
        #endregion

        #region Системные методы

        private void Start()
        {
            if(Line)
            Line.positionCount = 0;
        }

        void Update()
        {
            if (Active_bool && Max_point > Position_point_list.Count)
            {
                if(Line)
                Line.SetPosition(Position_point_list.Count + 1, Find_out_point_pos(Line.GetPosition(Position_point_list.Count)));
            }
        }
        #endregion

        #region Методы
        Vector3 Find_out_point_pos()
        {
            Vector3 result = Vector3.zero;
            Vector3 mousePosition = Vector3.zero;
            Ray mouseRaycast;
            RaycastHit hit;

            mousePosition.x = Mouse.current.position.ReadValue().x;
            mousePosition.y = Mouse.current.position.ReadValue().y;//Cam.pixelHeight - Mouse.current.position.ReadValue().y;
            mousePosition.z = Cam.nearClipPlane;

            mouseRaycast = Cam.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(mouseRaycast, out hit, Mathf.Infinity, Detect_layer))
            {
                //ray_detect_bool = true;
                result = hit.point;
                result.y += 0.2f;
            }

            return result;
        }

        Vector3 Find_out_point_pos(Vector3 _last_pos)
        {
            Vector3 result = Find_out_point_pos();

            _last_pos.y = result.y;

            if(Vector3.Distance(result, _last_pos) > Max_distance)
            {
                Vector3 direction = (result - _last_pos).normalized;

                result = _last_pos + direction * Max_distance;
            }

            return result;
        }

        IEnumerator Coroutine_move()
        {
            Process_bool = true;

            bool active_bool = true;

            int id_point = 0;

            if (Line)
                Line.positionCount = 0;

            while (active_bool)
            {

                yield return new WaitForFixedUpdate();

                Owner.position = Vector3.MoveTowards(Owner.position, Position_point_list[id_point], Speed_move);

                if(Owner.position == Position_point_list[id_point] && id_point < Position_point_list.Count - 1)
                {
                    id_point++;
                }
                else if (Owner.position == Position_point_list[id_point] &&  id_point >= Position_point_list.Count - 1)
                {
                    active_bool = false;
                }

            }

            End_preparation();
        }

        void End_preparation()
        {

            Process_bool = false;

            Position_point_list.Clear();

            Line.positionCount = 0;

            End_event.Invoke();
        }
        #endregion


        #region Публичные методы

        public void On_end_Off()
        {
            if (!Process_bool)
            {
                if (!Active_bool)
                {
                    Start_method();
                }
                else
                {
                    End_method();
                }
            }
        }

        /// <summary>
        /// Включить работу
        /// </summary>
        public void Start_method()
        {
            if (!Active_bool)
            {
                Active_bool = true;

                Start_event.Invoke();

                if (Line)
                {
                    Line.positionCount = 2;
                    Line.SetPosition(0, Owner.position);
                }
                    
            }
        }

        /// <summary>
        /// Закончить работу
        /// </summary>
        public void End_method()
        {
            if (!Process_bool)
            {
                Active_bool = false;

                if(Prefab_point)
                for (int x = Point_spawned_list.Count - 1; x > -1; x--)
                {
                    LeanPool.Despawn(Point_spawned_list[x]);
                    Point_spawned_list.RemoveAt(x);
                }

                if (Position_point_list.Count > 0)
                    StartCoroutine(Coroutine_move());
                else
                    End_preparation();
            }

        }

        /// <summary>
        /// Убрать поставленные точки
        /// </summary>
        public void Return_point()
        {
            if (Active_bool)
            {
                if (Position_point_list.Count > 0)
                {
                    Position_point_list.RemoveAt(Position_point_list.Count - 1);

                    if (Line)
                    {
                        Line.positionCount = Position_point_list.Count + 2;
                    }
                        

                    if (Prefab_point)
                    {
                        LeanPool.Despawn(Point_spawned_list[Point_spawned_list.Count - 1]);
                        Point_spawned_list.RemoveAt(Point_spawned_list.Count - 1);
                    }
                        
                }
            }
        }

        /// <summary>
        /// Установить точку куда будем двигаться
        /// </summary>
        public void Set_point()
        {
            if (Active_bool && Max_point > Position_point_list.Count)
            {
                Vector3 current_pos = Position_point_list.Count > 0 ? Position_point_list[Position_point_list.Count - 1] :  Owner.position;


                Vector3 new_point = Find_out_point_pos(current_pos);


                if (new_point != Vector3.zero)
                {
                    Position_point_list.Add(new_point);

                    if(Prefab_point)
                    Point_spawned_list.Add(LeanPool.Spawn(Prefab_point, new_point, Quaternion.identity));
                }

                if (Line && Max_point > Position_point_list.Count)
                {
                    Line.positionCount = 2 + Position_point_list.Count;
                    Line.SetPosition(1 + Position_point_list.Count, Position_point_list[Position_point_list.Count - 1]);
                }

            }
        }

        #endregion

    }
}