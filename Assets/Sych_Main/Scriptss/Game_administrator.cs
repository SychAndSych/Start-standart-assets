//Скрипт общего управления во время игры
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    
    public class Game_administrator : Singleton<Game_administrator>
    {
        /// <summary>
        /// Событие относящихся к контролю игрока 
        /// </summary>
        /// <param name="_bool">Параметр активации</param>
        public static UnityEvent<bool> Player_control_event = new UnityEvent<bool>();

        /// <summary>
        /// Событие относящихся к активности игры
        /// </summary>
        /// <param name="_bool">Параметр активации</param>
        public static UnityEvent<bool> On_game_event = new UnityEvent<bool>();

        /// <summary>
        /// Событие относящиеся к началу игры
        /// </summary>
        public static UnityEvent Start_game_event = new UnityEvent();

        /// <summary>
        /// Событие относящиеся к паузе игры
        /// </summary>
        public static UnityEvent<bool> Pause_game_event = new UnityEvent<bool>();

        /// <summary>
        /// Событие относящиеся к концу игры
        /// </summary>
        /// <param name="_active">Параметр активации</param>
        public static UnityEvent<bool> End_game_event = new UnityEvent<bool>();


        internal Player_initialization_administrator Player_administrator { get; private set; } = null;//Скрипт игрока

        internal List<Transform> Enemy_list { get; private set; } = new List<Transform>();//Лист противников

        internal bool Start_game_bool = false;//Игра начата

        bool Player_off_control_bool = false;//Параметр показывающий, что управление персонажем отключил сам игрок, а не окончанием игры (вдруг он инвентарь открыл)

        bool Off_game_bool = false;//Игра прекращена (например проиграл, но вдруг воспользуется "воскрешением" персонажа)

        /// <summary>
        ///  Сменить активность контроля игрока над персонажем
        /// </summary>
        /// <param name="_active">Включение или выключение</param>
        public void Player_control_activity(bool _active)
        {
            if(!Off_game_bool)
            Player_control_event.Invoke(_active);

            Player_off_control_bool = !_active;
        }

        /// <summary>
        ///  Сменить активность игры
        /// </summary>
        /// <param name="_active">Включение или выключение</param>
        public void On_game_activity(bool _active)
        {
            On_game_event.Invoke(_active);

            Off_game_bool = !_active;

            if (!Player_off_control_bool)
            Player_control_event.Invoke(_active);

        }


        /// <summary>
        /// Начать игру
        /// </summary>
        public void Start_game()
        {
            Player_control_event.Invoke(true);
            Start_game_event.Invoke();
            Start_game_bool = true;
        }

        /// <summary>
        /// Поставить на паузу игру
        /// </summary>
        public void Pause_game(bool _bool)
        {
            Player_control_event.Invoke(!_bool);
            Pause_game_event.Invoke(_bool);

        }

        /// <summary>
        /// Закончить игру
        /// </summary>
        /// <param name="_win">Победа?</param>
        public void End_game(bool _win)
        {
            Player_control_event.Invoke(false);
            End_game_event.Invoke(_win);
            Start_game_bool = false;
        }

        /// <summary>
        /// Добавить администратора игрока
        /// </summary>
        /// <param name="_player_script"></param>
        public void Add_player_administrator(Player_initialization_administrator _player_script)
        {
            Player_administrator = _player_script;
        }

    }
}