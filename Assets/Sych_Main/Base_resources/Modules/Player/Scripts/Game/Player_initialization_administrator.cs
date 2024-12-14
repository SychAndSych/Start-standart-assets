//Инициализация и коннект нужных скриптов игрока (например для Game_administrator и для интерфейса)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Player initialization administrator")]
    [DisallowMultipleComponent]
    public class Player_initialization_administrator : MonoBehaviour
    {

        [field: Tooltip("Скрипт игрока")]
        [field: SerializeField]
        public Game_character_abstract Player_sc { get; private set; } = null;

        [field: Tooltip("Скрипт управления игрока")]
        [field: SerializeField]
        public Input_player Player_input { get; private set; } = null;

        [field: Tooltip("Камера")]
        [field: SerializeField]
        public Camera Cam { get; private set; } = null;

        [field: Tooltip("Скрипт смены оружия")]
        [field: SerializeField]
        public Item_in_hand_handler Item_in_hand_handler_script { get; private set; } = null;

        [field: Tooltip("Здоровье")]
        [field: SerializeField]
        public Health Player_health { get; private set; } = null;


        #region Системные методы
        private void Awake()
        {
            if (Game_administrator.Singleton_Instance)
                Game_administrator.Singleton_Instance.Add_player_administrator(this);
        }

        private void Start()
        {
            if (Player_health)
            {
                Player_health.Harm_event.AddListener(Change_health);
                Player_health.Heal_event.AddListener(Change_health);
                Player_health.Harm_event.AddListener(Damage_add);
                Player_health.Dead_event.AddListener(Death);
            }
        }

        #endregion

        #region Methods
        void Change_health()
        {
            Player_interface_UI.Singleton_Instance.Player_Health_info((float)Player_health.Health_active / (float)Player_health.Health_default);
        }


        void Damage_add()
        {
            if (Player_health.Alive_bool)
            {
                Player_interface_UI.Singleton_Instance.Damage_anim_effect();
            }
        }

        void Death()
        {
            Game_administrator.Singleton_Instance.End_game(false);
        }
        #endregion
    }
}