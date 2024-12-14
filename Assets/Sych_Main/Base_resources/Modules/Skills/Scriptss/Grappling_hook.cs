using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Skills / Grappling hook")]
    [DisallowMultipleComponent]
    public class Grappling_hook : MonoBehaviour
    {
        [Tooltip("Объект который по идее выпускает канат и будет на нём раскачиватся")]
        [SerializeField]
        Transform Main_transform = null;

        [Tooltip("Объект который находится на конце верёвки крюка-кошки (сам крюк)")]
        [SerializeField]
        Transform Hook_transform = null;

        [Tooltip("Точка из которой будет запущен крюк-кошка")]
        [SerializeField]
        Transform Start_point_hook = null;

        [Tooltip("Скорость полёта крюка при запуске")]
        [SerializeField]
        float Speed_move_hook = 0.8f;

        [Tooltip("Максимальная дистанция верёвки")]
        [SerializeField]
        internal float Max_distance_hook = 20f;

        [Tooltip("Верёвка подтягивает хозяина к крюку")]
        [SerializeField]
        bool Auto_Pull_up_bool = true;

        [ShowIf(nameof(Auto_Pull_up_bool))]
        [Tooltip("Сила с которой верёвка подтягивает владельца (или предмет если он легче и меньше) (на самом деле это увеличение силы со временем)")]
        [SerializeField]
        float Pull_up_force = 0.4f;

        [Tooltip("Слои с которыми будем взаимодействовать")]
        [SerializeField]
        LayerMask Layer = 1;

        [Tooltip("Отображение верёвки")]
        [SerializeField]
        LineRenderer LineRenderer_component = null;

        [Tooltip("Пружинистость")]
        [SerializeField]
        float Spring_rope = 10f;

        [Tooltip("Жёсткость верёвки (на сколько хорошо растягивается)")]
        [SerializeField]
        float Damper_rope = 10f;

        float Active_distance = 0f;

        Transform Target_transform = null;//Объект в который попал канат (крюк-кошка)

        internal bool Active_bool = false;//В действие

        bool Pull_up_detect_bool = false;//Конечная точка зацеплена за что-то

        Vector3 final_hook_position = Vector3.zero;//Финальная позиция крюка куда он летит

        bool Move_hook_bool = false;

        bool Move_back_hook_bool = false;

        bool Move_Pull_up_bool = false;

        SpringJoint SpringJoint_component = null;

        #region Системные методы

        private void OnEnable()
        {
            Reset_script();
        }

        private void OnDisable()
        {
            Reset_script();
        }

        private void Update()
        {
            if (Active_bool)
            {
                if (Move_hook_bool)
                    Move_hook();
                else if (!Move_hook_bool && Move_back_hook_bool)
                    Move_back_hook();
                else if (!Move_hook_bool && Move_Pull_up_bool)
                    Move_Pull_up();

                Uprate_line_render();
            }
        }
        #endregion


        #region Методы

        /// <summary>
        /// Сбросить параметры на исходную
        /// </summary>
        void Reset_script()
        {
            Active_bool = false;
            Pull_up_detect_bool = false;
            Move_back_hook_bool = false;
            Move_hook_bool = false;
            Move_Pull_up_bool = false;
            Hook_transform.position = Start_point_hook.position;
            Active_distance = 0;

            Invoke(nameof(Delay_reset), 0.1f);

            if(Hook_transform)
            Hook_transform.SetParent(transform);

            if (SpringJoint_component)
                Destroy(SpringJoint_component);
        }

        void Delay_reset()
        {
            if (LineRenderer_component)
            {
                LineRenderer_component.enabled = false;
            }
        }

        /// <summary>
        /// Отрисовать верёвку
        /// </summary>
        void Uprate_line_render()
        {
            if (LineRenderer_component) 
            {
                if(!LineRenderer_component.enabled)
                LineRenderer_component.enabled = true;

                if (LineRenderer_component.positionCount > 1)
                    LineRenderer_component.positionCount = 2;

                LineRenderer_component.SetPosition(0, Main_transform.position);
                LineRenderer_component.SetPosition(1, Hook_transform.position);
            }
        }


        /// <summary>
        /// Полёт хука до конечной точки
        /// </summary>
        void Move_hook()
        {
            Hook_transform.position = Vector3.MoveTowards(Hook_transform.position, final_hook_position, Speed_move_hook);

            Hook_transform.LookAt(final_hook_position);

            Active_distance = Vector3.Distance(Start_point_hook.position, Hook_transform.position);

            if (Hook_transform.position == final_hook_position || Active_distance >= Max_distance_hook)
            {
                Move_hook_bool = false;

                if(!Pull_up_detect_bool)
                Move_back_hook_bool = true;
                else
                {
                    Move_Pull_up_bool = true;
                    Connect_hook(Target_transform);
                }
            }
        }

        /// <summary>
        /// Крюк прикрепился к цели
        /// </summary>
        /// <param name="_target">Цель</param>
        void Connect_hook(Transform _target)
        {
            Target_transform = _target;
            Add_spring_joint();
        }

        /// <summary>
        /// Полёт хука обратно на изначальную точку
        /// </summary>
        void Move_back_hook()
        {
            Hook_transform.position = Vector3.MoveTowards(Hook_transform.position, Start_point_hook.position, Speed_move_hook);

            Hook_transform.LookAt(final_hook_position);

            if (Hook_transform.position == Start_point_hook.position)
            {
                Reset_script();
            }
        }

        /// <summary>
        /// Хозяин подтягивается к зацепленному крюку
        /// </summary>
        void Move_Pull_up()
        {
            //Main_transform.position = Vector3.MoveTowards(Main_transform.position, final_hook_position, Pull_up_force);

            if (SpringJoint_component && Auto_Pull_up_bool)
                SpringJoint_component.maxDistance -= Pull_up_force;

            if (Main_transform.position == final_hook_position)
            {
                Reset_script();
            }
        }

        /// <summary>
        /// Добавить компонент пружины
        /// </summary>
        void Add_spring_joint()
        {
            SpringJoint_component = Main_transform.gameObject.AddComponent<SpringJoint>();
            SpringJoint_component.autoConfigureConnectedAnchor = false;
            SpringJoint_component.connectedAnchor = final_hook_position;
            SpringJoint_component.maxDistance = Active_distance;
            SpringJoint_component.minDistance = Auto_Pull_up_bool ? 0 : Active_distance;
            SpringJoint_component.damper = Damper_rope;
            SpringJoint_component.spring = Spring_rope;
        }

        #endregion


        #region Публичные методы

        [ContextMenu("Запустить для теста (в качестве точки запуска будет Main_transform)")]
        public void Start_method_test()
        {
            Activation(Main_transform.position, Main_transform.forward);
        }

        /// <summary>
        /// Активировать
        /// </summary>
        public void Activation(Vector3 _start_point, Vector3 _direction)
        {

            Hook_transform.position = Start_point_hook.position;

            Uprate_line_render();

            RaycastHit hit;

            if (Physics.Raycast(_start_point, _direction, out hit, Max_distance_hook, Layer))
            {
                final_hook_position = hit.point;
                Pull_up_detect_bool = true;
                Target_transform = hit.transform;
            }
            else
            {
                final_hook_position = _start_point + _direction * Max_distance_hook;
                Pull_up_detect_bool = false;
                Target_transform = null;
            }

            Hook_transform.SetParent(null);

            Move_hook_bool = true;
            Active_bool = true;

        }

        /// <summary>
        /// Отменить действие
        /// </summary>
        [ContextMenu("Отменить действие (прекращает подтягивать и возвращает крюк)")]
        public void Cancel()
        {
            if (Active_bool) 
            {
                Move_back_hook_bool = true;
                Move_hook_bool = false;
                Move_Pull_up_bool = false;

                if (SpringJoint_component)
                    Destroy(SpringJoint_component);
            }
        }

        #endregion
    }
}