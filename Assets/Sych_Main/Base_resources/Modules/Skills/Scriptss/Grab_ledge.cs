//Способность хвататься за уступы (ставить на отдельный объект в области хватания рук)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Skills / Grab ledge")]
    [DisallowMultipleComponent]
    public class Grab_ledge : MonoBehaviour
    {
        #region Переменные

        [Tooltip("Владелец которого мы будем перемещать")]
        [SerializeField]
        Transform Owner_transform = null;

        [Tooltip("Будет использовать в качестве точки ориентирования объект на котором висит скрипт")]
        [SerializeField]
        bool Direction_point_bool = false;

        [HideIf(nameof(Direction_point_bool))]
        [Tooltip("Корректировка на расположение точки отсчёта")]
        [SerializeField]
        Vector3 Offset = Vector3.zero;

        [HideIf(nameof(Direction_point_bool))]
        [Tooltip("Точка направления (вдруг есть вращающийся меш показывающий направление)")]
        [SerializeField]
        Transform Forward_point = null;

        [ShowIf(nameof(Direction_point_bool))]
        [Tooltip("Направляющая точка (задаёт направление по Z и с неё идёт отсчёт расстояния)")]
        [SerializeField]
        Transform Direction_point = null;

        [Min(0)]
        [Tooltip("Дистанция захвата")]
        [SerializeField]
        float Distance_grab = 1f;

        [Min(0)]
        [Tooltip("Необходимая длина выступа (что бы не хватался за всё подряд)")]
        [SerializeField]
        float Protrusion_length = 0.5f;

        [Min(0)]
        [Tooltip("Высота захвата")]
        [SerializeField]
        float Height_grab = 0.5f;

        [Min(0)]
        [Tooltip("Дистанция захвата для более тонких вычислений")]
        [SerializeField]
        float Post_distance_grab = 1f;

        [Min(0)]
        [Tooltip("Высота владельца (что бы особо не проходить сквозь объекты когда забираемся)")]
        [SerializeField]
        float Height_owner = 1f;

        [Tooltip("Слои с которыми взаимодействуем")]
        [SerializeField]
        LayerMask Mask = 1;

        [Tooltip("Скорость скарабкивания")]
        [SerializeField]
        float Speed_grab = 2f;

        [Tooltip("Скорость смещения к точке захвата (что бы персонаж занял нужную точку, а не был от уступа в стороне)")]
        [SerializeField]
        float Speed_lerp_connect = 15f;

        [Tooltip("Будет ли передвигать персонажа вперёд")]
        [SerializeField]
        bool Move_final_grab_bool = true;

        [ShowIf(nameof(Move_final_grab_bool))]
        [Tooltip("Дальность перемещения вперёд персонажа когда он забирается по уступу ")]
        [SerializeField]
        float Distance_final_position = 1f;

        [Tooltip("Ивент когда есть за что схватиться.")]
        [SerializeField]
        UnityEvent Connect_grab_event = new UnityEvent();

        [Tooltip("Ивент когда начал забираться")]
        [SerializeField]
        UnityEvent Start_climb_event = new UnityEvent();

        [Tooltip("Ивент когда закончил забираться или спрыгнул.")]
        [SerializeField]
        UnityEvent End_climb_event = new UnityEvent();

        [Tooltip("Что бы видеть больше")]
        [SerializeField]
        bool Gizmos_mode_bool = false;

        [Tooltip("Включает при старте")]
        [SerializeField]
        bool Start_active_bool = false;

        bool Active_bool = false;

        bool Connect_grab_bool = false;//Зацепились ли мы за что то?

        bool Climb_active_bool = false;//Персонаж забирается и занят

        Coroutine Climb_coroutine = null;

        Vector3[] End_pos_climb_array = new Vector3[0];

        Vector3 Position_grab_length_contact = Vector3.zero;//Точка перед игроком (что бы примагнититься к стене).

        Vector3 Position_grab_height_contact = Vector3.zero;//Точка верхнего края уступа (что бы выравнить по высоте).

        Vector3 Hit_normal = Vector3.zero;

        Coroutine Lerp_grab_coroutine = null;

        #endregion

        #region Системные методы

        private void Start()
        {
            if (Start_active_bool)
                Active_bool = true;
        }

        void FixedUpdate()
        {
            if (Active_bool && !Connect_grab_bool)
            {
                if (Check_distance_grab)
                {
                    if (Check_finale_grab)
                    {
                        Grab_hold();
                    }
                }
            }
        }

        #endregion


        #region Приватные Методы


        /// <summary>
        /// Схватиться за выступ
        /// </summary>
        void Grab_hold()
        {
            if (Lerp_grab_coroutine == null)
                Lerp_grab_coroutine = StartCoroutine(Coroutine_lerp_grab());

            Connect_grab_event.Invoke();
            Active_bool = false;
            Connect_grab_bool = true;
        }

        IEnumerator Coroutine_lerp_grab()
        {
            Vector3 offset_position = Owner_transform.position - Find_out_position_start;//Позиция смещения, что бы зацеп был именно в точке контакта

            Vector3 pos = Owner_transform.position;
            pos.y = Position_grab_height_contact.y + offset_position.y;
            pos.x = Position_grab_length_contact.x + offset_position.x;
            pos.z = Position_grab_length_contact.z + offset_position.z;

            //Owner_transform.position = pos;

            Vector3 start_pos = Owner_transform.position;

            Quaternion start_rot = Quaternion.identity;

            Quaternion end_rot = Quaternion.identity;

            if (Forward_point)//Повернём вращающуюся часть для корректного вида
            {
                start_rot = Forward_point.rotation;

                end_rot = Quaternion.LookRotation(Hit_normal * -1);
                //Forward_point.rotation = Quaternion.LookRotation(Hit_normal * -1);
            }

            float step = 0;

            while (step < 1)
            {
                yield return null;

                Owner_transform.position = Vector3.LerpUnclamped(start_pos, pos, step);

                if (Forward_point)//Повернём вращающуюся часть для корректного вида
                {
                    Forward_point.rotation = Quaternion.LerpUnclamped(start_rot, end_rot, step);
                }

                step += Speed_lerp_connect * Time.fixedDeltaTime;

                if (step > 1)
                    step = 1;

            }

            Lerp_grab_coroutine = null;
        }

        /// <summary>
        /// Получить направляющую ось
        /// </summary>
        Transform Find_out_forward_point
        {
            get
            {
                Transform result = null;

                if (Direction_point_bool)
                {
                    result = Direction_point;
                }
                else
                {
                    if (Forward_point != null)
                        result = Forward_point;
                    else
                        result = Owner_transform;
                }

                return result;
            }
        }

        /// <summary>
        /// Получить стартовую позицию
        /// </summary>
        Vector3 Find_out_position_start
        {
            get
            {
                Vector3 result = Vector3.zero;

                if (Direction_point_bool)
                {
                    result = transform.position;
                }
                else
                {
                    Transform forward_point = Find_out_forward_point;

                    result = Owner_transform.position + (forward_point.forward * Offset.z + forward_point.up * Offset.y + forward_point.right * Offset.x);
                }

                return result;
            }
        }

        /// <summary>
        /// Для вычисления конца дистанции захвата за уступ (конец луча который определяет перед нами поверхность
        /// </summary>
        Vector3 Find_out_End_pos_Distance_grab
        {
            get
            {


                return Position_grab_length_contact != Vector3.zero && Active_bool ? Position_grab_length_contact : Find_out_position_start + Find_out_forward_point.forward * Distance_grab;
            }
        }

        /// <summary>
        /// Дальняя точка к которой будет перемещатся персонаж когда заберётся на уступ
        /// </summary>
        Vector3 Find_out_End_move_owner
        {
            get
            {

                return Owner_transform.position + Find_out_forward_point.forward * Distance_final_position;
            }
        }

        /// <summary>
        /// Для вычисления конца высоты захвата за уступ (с учётом дистанции)
        /// </summary>
        Vector3 Find_out_End_pos_Height_grab
        {
            get
            {
                return Find_out_End_pos_Distance_grab + Find_out_forward_point.up * Height_grab;
            }
        }

        /// <summary>
        /// Для вычисления конца необходимой длины выступа
        /// </summary>
        Vector3 Find_out_End_pos_Protrusion_length
        {
            get
            {
                return Find_out_End_pos_Height_grab + Find_out_forward_point.forward * Protrusion_length;
            }
        }

        /// <summary>
        /// Для вычисления конца пост дистанции захвата за уступ (с учётом основной дистанции и высоты захвата) 
        /// </summary>
        Vector3 Find_out_End_pos_Post_distance_grab
        {
            get
            {
                return Find_out_End_pos_Protrusion_length + Find_out_forward_point.forward * Post_distance_grab;
            }
        }

        /// <summary>
        /// Для вычисления конца глубины захвата за уступ (с учётом основной дистанции и пост дистанции и высоты захвата) 
        /// </summary>
        Vector3 Find_out_End_pos_Depth_grab
        {
            get
            {
                return Find_out_End_pos_Post_distance_grab + -Find_out_forward_point.up * Height_grab;
            }
        }

        /// <summary>
        /// Проверить на наличие поверхности перед персонажем
        /// </summary>
        bool Check_distance_grab
        {
            get
            {
                bool result = false;

                RaycastHit hit;

                Physics.Raycast(Find_out_position_start, Find_out_forward_point.forward, out hit, Distance_grab, Mask, QueryTriggerInteraction.Ignore);

                if (hit.transform != null)
                {
                    Position_grab_length_contact = hit.point;
                    Hit_normal = hit.normal;
                    result = true;
                }


                return result;
            }
        }


        /// <summary>
        /// Проверить можем ли мы в итоге зацепиться
        /// </summary>
        bool Check_finale_grab
        {
            get
            {
                bool result = false;

                RaycastHit hit;

                int step_limit = 10;//Количество попыток и промежутков на всю длину

                float step_interval = 1f / (float)step_limit;//

                for (int x = 0; x <= step_limit; x++)
                {
                    float step = x * step_interval;

                    Vector3 step_position = Vector3.Lerp(Find_out_End_pos_Protrusion_length, Find_out_End_pos_Post_distance_grab, step);

                    Physics.Raycast(step_position, -Find_out_forward_point.up, out hit, Height_grab, Mask, QueryTriggerInteraction.Ignore);

                    if (hit.transform != null)
                    {
                        result = true;
                        Position_grab_height_contact = hit.point;
                        break;
                    }
                }


                return result;
            }
        }

        /// <summary>
        /// Перемещаем владельца на позицию в конце подъёма
        /// </summary>
        /// <returns></returns>
        IEnumerator Coroutine_climb()
        {
            int limit = 1000;

            float step = 0;

            while (limit > 0 && Owner_transform.position != End_pos_climb_array[End_pos_climb_array.Length - 1])
            {
                yield return null;

                limit--;

                Owner_transform.position = Game_Bezier_curve.Get_point_Bezier(End_pos_climb_array[0], End_pos_climb_array[1], End_pos_climb_array[2], step);
                step += Speed_grab * Time.fixedDeltaTime;

                if (step > 1)
                    step = 1;

            }

            End_climb();

            Climb_coroutine = null;
        }

        #endregion


        #region Публичные методы
        /// <summary>
        /// Будет ли работать сейчас
        /// </summary>
        /// <param name="_activity">Включить?</param>
        public void Activity(bool _activity)
        {
            Active_bool = _activity;
        }

        /// <summary>
        /// Забраться
        /// </summary>
        [ContextMenu("Забраться")]
        public void Climb()
        {
            if (Connect_grab_bool && !Climb_active_bool)
            {
                if (Climb_coroutine != null)
                    StopCoroutine(Climb_coroutine);

                End_pos_climb_array = new Vector3[3];
                End_pos_climb_array[0] = Owner_transform.position;
                End_pos_climb_array[1] = Find_out_position_start + Find_out_forward_point.up * Height_owner;
                End_pos_climb_array[2] = Find_out_End_move_owner + Find_out_forward_point.up * Height_owner;

                Start_climb_event.Invoke();

                Climb_active_bool = true;

                if (Move_final_grab_bool)
                    Climb_coroutine = StartCoroutine(Coroutine_climb());
            }
        }

        /// <summary>
        /// Конец залезания
        /// </summary>
        public void End_climb()
        {
            Connect_grab_bool = false;

            Climb_active_bool = false;

            End_climb_event.Invoke();
        }

        #endregion


        #region Проверяющие методы

        private void OnDrawGizmos()
        {

            if (Gizmos_mode_bool)
            {
                //Отображение дистанции
                Gizmos.color = Color.red;

                Gizmos.DrawLine(Find_out_position_start, Find_out_End_pos_Distance_grab);

                //Отображает высоту захвата
                Gizmos.color = Color.blue;

                Gizmos.DrawLine(Find_out_End_pos_Distance_grab, Find_out_End_pos_Height_grab);

                Gizmos.DrawLine(Find_out_End_pos_Post_distance_grab, Find_out_End_pos_Depth_grab);

                //Отображение пост дистанции для вычисления уступа
                Gizmos.color = Color.yellow;

                Gizmos.DrawLine(Find_out_End_pos_Height_grab, Find_out_End_pos_Post_distance_grab);

                //Отображает необходимую длину уступа
                Gizmos.color = Color.gray;

                Gizmos.DrawLine(Find_out_End_pos_Height_grab, Find_out_End_pos_Protrusion_length);

                Gizmos.color = Color.cyan;

                Gizmos.DrawLine(Owner_transform.position, Find_out_End_move_owner);
            }
        }
        #endregion
    }
}