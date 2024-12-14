using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [RequireComponent(typeof(Player_initialization_administrator))]
    //[RequireComponent(typeof(Item_in_hand_handler))]
    [RequireComponent(typeof(Input_player))]
    //[RequireComponent(typeof(Grounded_handler))]
    //[RequireComponent(typeof(Camera_control))]
    [AddComponentMenu("Sych scripts / Game / Player / Player character")]
    [DisallowMultipleComponent]
    public class Player_character : Game_character_abstract
    {
        #region Переменные

        [field: Tooltip("Управление камерой")]
        [field: SerializeField]
        public Camera_control Camera_control_sc { get; private set; } = null;

        [field: Tooltip("Скрипт для собственной гравитации + прыжок")]
        [field: SerializeField]
        public Player_Motor Player_Motor_script = null;

        [field: Tooltip("Для смены оружия")]
        [field: SerializeField]
        public Item_in_hand_handler Item_in_hand_handler_script { get; private set; } = null;

        /*
        [Tooltip("Как быстро персонаж поворачивается лицом в направлении движения")]
        [Range(0.0f, 0.3f)]
        [SerializeField]
        float RotationSmoothTime = 0.12f;
        */

        [Tooltip("Скрипт отвечающий за принятие нажатия кнопок игрока")]
        [field: SerializeField]
        public Input_player Input { get; private set; } = null;

        #endregion

        #region MonoBehaviour Callbacks

        protected void Start()
        {
            //Game_Player.Cursor_player(false);
        }

        #endregion

        #region Методы

        /// <summary>
        /// Включить нужный вариант поведения в зависимости от взятого оружия
        /// </summary>
        void Preparation_brain(Item_enum _item_type)
        {
            switch (_item_type)
            {
                case Item_enum.Bow:
                    Brain_script.Change_state("Archer");
                    break;

                default:
                    Brain_script.Change_state("Normal");
                    break;
            }

            
        }

        protected override void Initialized_stats()
        {
        }

        #endregion

        #region Управляющие методы



        #endregion

        #region Публичные Методы

        #endregion

        #region Дополнительно

        #endregion
    }
}