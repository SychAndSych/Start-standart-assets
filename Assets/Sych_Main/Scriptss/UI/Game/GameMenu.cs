//Скрипт для игрового меню во время паузы
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Sych_scripts
{
    public class GameMenu : Singleton<GameMenu>
    {


        [Tooltip("Игровое меню")]
        [SerializeField]
        GameObject Game_menu_canvas = null;

        [Tooltip("Включено ли игровое меню")]
        public bool Game_menu_bool = false;

        [Header("Название сцены которая ведёт в главное меню")]
        [SerializeField]
        string Scene_main_menu = "Main_menu";

        [Header("Заранее записаная загрузка уровня")]
        [SerializeField]
        string Load_scene_name = "Menu";

        [Tooltip("Скрипт загрузки")]
        [SerializeField]
        Load_scene Load_scene_script = null;

        [Tooltip("Ссылки на игровые вкладки")]
        [SerializeField]
        List<GameObject> Bookmark = new List<GameObject>();

        [Tooltip("Ивент активности меню")]
        [SerializeField]
        UnityEvent<bool> Active_menu_event = new UnityEvent<bool>();


        /// <summary>
        /// Включение и отключение игрового меню
        /// </summary>
        void OnButton_game_menu()
        {

            if (!Game_menu_bool)
            {
                Game_menu_activity(true);
            }
            else
            {
                bool null_bookmark = true;

                for (int x = 0; x < Bookmark.Count; x++)
                {
                    if (Bookmark[x].activeSelf)
                    {
                        null_bookmark = false;
                        Bookmark[x].SetActive(false);
                    }
                }
                if (null_bookmark)
                {
                    Game_menu_activity(false);
                }
            }

        }


        /// <summary>
        /// Включение/Выключение игрового меню
        /// </summary>
        /// <param name="_active">Включить и отключить менюшку</param>
        void Game_menu_activity(bool _active)
        {
            Game_administrator.Singleton_Instance.Player_control_activity(!_active);
            Game_menu_bool = _active;
            Game_menu_canvas.SetActive(_active);

            Active_menu_event.Invoke(_active);
        }


        /// <summary>
        /// Продолжить дальше (выключить меню)
        /// </summary>
        public void Continue()
        {
            Game_menu_activity(false);
            Time.timeScale = 1;

        }


        /// <summary>
        /// Перезагрузить сцену (уровень)
        /// </summary>
        public void Restart_scene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }


        /// <summary>
        /// Выход в главное меню (загрузить сцену с главным меню)
        /// </summary>
        public void Load_main_menu()
        {
            Time.timeScale = 1;
            Load_scene_script.Load_start(Scene_main_menu);
            Game_menu_activity(false);
        }


        /// <summary>
        /// Загрузить указанную тут сцену (уровень)
        /// </summary>
        public void Load_scene()
        {
            Load_scene_script.Load_start(Load_scene_name);
            Time.timeScale = 1;
            Game_menu_activity(false);
        }


        /// <summary>
        /// Загрузить указанную сцену (уровень)
        /// </summary>
        /// <param name="_id_scene">Номер сцены</param>
        public void Load_scene(int _id_scene)
        {
            Load_scene_script.Load_start(_id_scene);
            Time.timeScale = 1;
            Game_menu_activity(false);
        }


        /// <summary>
        /// Загрузить указанную сцену (уровень)
        /// </summary>
        /// <param name="_name_scene">Имя сцены</param>
        public void Load_scene(string _name_scene)
        {
            Load_scene_script.Load_start(_name_scene);
            Time.timeScale = 1;
            Game_menu_activity(false);
        }



        /// <summary>
        /// Загрузить следующую сцену
        /// </summary>
        public void Next_scene()
        {
            int id_scene = SceneManager.GetActiveScene().buildIndex + 1;

            if (SceneManager.sceneCountInBuildSettings > id_scene)
            {
                Load_scene_script.Load_start(id_scene);
            }
            else
            {
                //Debug.Log("Сцены закончились, возвращаемся в главное меню.");
                //Load_main_menu();
                Load_scene_script.Load_start(0);//Начинаем с нулевой сценой
            }

        }


        /// <summary>
        /// Выключить игру
        /// </summary>
        public void Exit_game()
        {
            Application.Quit();
        }


    }
}