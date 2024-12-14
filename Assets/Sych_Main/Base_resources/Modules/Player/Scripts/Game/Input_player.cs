using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Input player")]
    [DisallowMultipleComponent]
    public class Input_player : MonoBehaviour
    {
        [ReadOnly]
        public Vector2 Move_vector;

        [ReadOnly]
        public Vector2 Look_rotation;

        [ReadOnly]
        public bool Sprint_bool;

        [ReadOnly]
        public bool Analog_Movement_bool;//Пересмотреть на нужность

        [ReadOnly]
        public bool Jump_bool;

        [ReadOnly]
        public bool Attack_bool;

        [ReadOnly]
        [SerializeField]
        bool Activity_inventory_bool = false;

        bool Player_control_bool = true;//Игрок может играть

        bool On_game_bool = true;

        [Tooltip("Ивент переключение предметов в руках персонажа между двумя")]
        public UnityEvent Change_two_weapon_event = new UnityEvent();

        [Tooltip("Ивент переключение предметов в руках персонажа по клавишам (1,2,3,4,5 и тд)")]
        public UnityEvent<int> Change_id_weapon_event = new UnityEvent<int>();

        [Tooltip("Ивент переключение предметов в руках персонажа по сролу")]
        public UnityEvent<bool> Change_scroll_weapon_event = new UnityEvent<bool>();

        [Tooltip("Ивент когда персонаж пытается взаимодействовать (обычно E)")]
        public UnityEvent Interact_event = new UnityEvent();

        [Tooltip("Ивент когда персонаж перезаряжается (обычно R)")]
        public UnityEvent Reload_event = new UnityEvent();

        [Tooltip("Ивент когда персонаж прыгает")]
        public UnityEvent Jump_event = new UnityEvent();

        [Tooltip("Ивент когда игрок нажимает на клавишу прыжка")]
        public UnityEvent<bool> Jump_input_event = new UnityEvent<bool>();

        [Tooltip("Ивент когда игрок нажимает на клавишу атаки")]
        public UnityEvent<bool> Attack_bool_event = new UnityEvent<bool>();

        private void Awake()
        {
            Game_administrator.Player_control_event.AddListener(Change_Player_control);
            Game_administrator.On_game_event.AddListener(Change_On_game);
        }

        private void OnDisable()
        {
            Move_vector = Vector3.zero;
            
            Look_rotation = Vector3.zero;
        }

        /// <summary>
        /// Переключение доступа игрока к управлению
        /// </summary>
        /// <param name="_activity"></param>
        void Change_Player_control(bool _activity)
        {
            Player_control_bool = _activity;

            if (!_activity)
            {
                Look_rotation = Vector2.zero;
                Move_vector = Vector2.zero;
                Jump_bool = false;
                Attack_bool = false;
                Sprint_bool = false;
            }
        }

        /// <summary>
        /// Переключение понимания, что игра сейчас активна (а то вдруг ты проиграл или игра стоит на паузе)
        /// </summary>
        /// <param name="_activity"></param>
        void Change_On_game(bool _activity)
        {
            On_game_bool = _activity;

            if(!_activity)
                Inventory_UI.Singleton_Instance.Activity(false, false);
        }

        public void OnMove(Vector2 _value)
        {
            if (Player_control_bool)
                Move_vector = _value;
        }

        public void OnMove(InputValue _value)
        {
            if(Player_control_bool)
            Move_vector = _value.Get<Vector2>();
        }

        public void OnSprint(InputValue _value)
        {
            if (Player_control_bool)
                Sprint_bool = _value.isPressed;
        }

        public void OnAttack(InputValue _value)
        {
            if (Player_control_bool)
            {
                Attack_bool = _value.isPressed;

                Attack_bool_event.Invoke(Attack_bool);
            }
        }

        public void OnReload()
        {
            Reload_event.Invoke();
        }

        public void OnChange_two_weapon()
        {
            Change_two_weapon_event.Invoke();
        }

        public void OnChange_scroll_weapon(InputValue _value)
        {
            if (_value.Get<Vector2>().y > 0)
                Change_scroll_weapon_event.Invoke(true);
            else if(_value.Get<Vector2>().y < 0)
                Change_scroll_weapon_event.Invoke(false);
        }

        public void OnChange_id_weapon(InputValue _value)
        {
            print(_value.Get());
            Change_id_weapon_event.Invoke((int)_value.Get<float>());
        }

        public void OnLook_rotation_camera(Vector2 _value)
        {
            if (Player_control_bool)
                Look_rotation = _value;
        }

        public void OnLook_rotation_camera(InputValue _value)
        {
            if (Player_control_bool)
                Look_rotation = _value.Get<Vector2>();
        }

        public void OnJump(InputValue _value)
        {
            if (Player_control_bool)
            {
                Jump_bool = _value.isPressed;

                Jump_input_event.Invoke(Jump_bool);

                if (Jump_bool)
                Jump_event.Invoke();
            }
                
        }

        public void Activity_jump(bool _activity)
        {
            if (Player_control_bool)
            {
                Jump_bool = _activity;

                if (Jump_bool)
                    Jump_event.Invoke();
            }
        }

        public void OnInteract()
        {
            Interact_event.Invoke();
        }

        public void OnInventory()
        {
            if (On_game_bool)
            {
                Activity_inventory_bool = !Activity_inventory_bool;

                Inventory_UI.Singleton_Instance.Button_inventory();
            }
        }

    }
}