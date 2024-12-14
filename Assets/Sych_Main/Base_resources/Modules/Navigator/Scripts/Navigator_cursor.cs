using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    public class Navigator_cursor : Navigator_abstract
    {
        #region Переменные
        [Tooltip("Курсор который будем вращать")]
        [SerializeField]
        Transform Cursor_transform = null;

        [Tooltip("Если включить, то прекратит наклоняться и будет только поворачиваться по оси Y")]
        [SerializeField]
        bool X_Z_bool = false;

        [Tooltip("Будет ли скрываться курсор при приближение к цели")]
        [SerializeField]
        bool Hide_in_end_position_bool = false;

        [Tooltip("Массив материалов")]
        [SerializeField]
        Renderer[] Materials_array = new Renderer[0];

        [Tooltip("Дистанция на которой начинает и на которой полностью исчезает")]
        [SerializeField]
        Vector2 Distance_in_full_hide = new Vector2(5f, 2f);
        #endregion


        #region Системные методы
        private void Update()
        {
            if (Active_bool)
            {
                Rotation();
                Hide_update();
            }
        }
        #endregion


        #region Методы

        /// <summary>
        /// Поворот курсора
        /// </summary>
        void Rotation()
        {
            Vector3 end_position = Check_target_position;

            if (X_Z_bool)
                end_position.y = Cursor_transform.position.y;

            Cursor_transform.LookAt(end_position);
        }

        /// <summary>
        /// Обновлять скрывание курсора
        /// </summary>
        void Hide_update()
        {
            if (Hide_in_end_position_bool)
            {
                float hide_step = 1;

                float distance = Check_distance_to_target;

                if (distance <= Distance_in_full_hide.x && distance > Distance_in_full_hide.y)
                {
                    hide_step = (distance - Distance_in_full_hide.y) / (Distance_in_full_hide.x - Distance_in_full_hide.y);
                }
                else if(distance <= Distance_in_full_hide.y)
                {
                    hide_step = 0;
                }

                foreach (Renderer mat in Materials_array)
                {
                    Color default_color = mat.material.color;
                    default_color = new Color(default_color.r, default_color.g, default_color.b, hide_step);
                    mat.material.color = default_color;
                    //mat.material.SetColor("_EmissionColor", default_color * 1);
                }
            }
        }


        float Check_distance_to_target
        {
            get
            {
                return Vector3.Distance(Cursor_transform.position, Check_target_position);
            }
        }

        Vector3 Check_target_position
        {
            get
            {
                Vector3 end_position = Vector3.zero;

                switch (Type_way)
                {
                    case Enum_navigator.Object_enum:
                        end_position = Target_transform.position;
                        break;

                    default:
                        end_position = Target_position;
                        break;
                }

                return end_position;
            }
        }
        #endregion

 
    }
}