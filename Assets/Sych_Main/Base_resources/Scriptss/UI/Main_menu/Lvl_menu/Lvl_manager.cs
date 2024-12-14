//Массив выбираемых уровней
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
	public class Lvl_manager : MonoBehaviour
	{

		[Tooltip("Уровни")]
		[SerializeField]
		Lvl_class[] Lvl_array = new Lvl_class[0];

		[Tooltip("Скрипт для загрузки сцены")]
		[SerializeField]
		Load_scene Load_scene_script = null;

		int N_lvl = 1;

		int Id_load_lvl = 0;

        #region Стартовые методы
        private void Start()
        {
			Change_lvl(0);
		}

        #endregion


        #region Методы
        /// <summary>
        /// Сменить уровень
        /// </summary>
        /// <param name="_next">Id выбираемого уровня из массива</param>
        void Change_lvl(int _id_load)
        {
			
			if (_id_load > -1 && _id_load < Lvl_array.Length) 
			{
				N_lvl = _id_load;

				for (int x = 0; x < Lvl_array.Length; x++)
				{
					Lvl_array[x].UI_lvl.gameObject.SetActive(false);
				}

				Lvl_array[N_lvl].UI_lvl.gameObject.SetActive(true);

				Id_load_lvl = Lvl_array[N_lvl].Id_lvl;
			}
            else
            {
				Debug.Log("Слота под уровень по Id  " + _id_load + "  не существует!");
            }
		}

		/// <summary>
		/// Сменить уровень
		/// </summary>
		/// <param name="_next">Переключить вперёд или назад?</param>
		void Change_lvl(bool _next)
		{
			if (_next)
			{
				N_lvl++;
				if (N_lvl > Lvl_array.Length - 1)
					N_lvl = 0;
			}
			else
			{
				N_lvl--;
				if (N_lvl < 0)
					N_lvl = Lvl_array.Length - 1;
			}

			Change_lvl(N_lvl);
		}
			#endregion

			#region Публичные методы
			/// <summary>
			/// Перелестнуть вперёд
			/// </summary>
			public void Next_lvl()
		{
			Change_lvl(true);
		}

		/// <summary>
		/// Перелестнуть назад
		/// </summary>
		public void Previous_lvl()
		{
			Change_lvl(false);
		}

		/// <summary>
		/// Начать загрузку уровня
		/// </summary>
		public void Load_lvl()
        {
			Load_scene_script.Load_start(Id_load_lvl);
        }
        #endregion
    }

    [System.Serializable]
	class Lvl_class
    {
		[Tooltip("Активируемый визуал уровня")]
		[SerializeField]
		public GameObject UI_lvl = null;

		[Tooltip("Id уровня для загрузки")]
		[SerializeField]
		public int Id_lvl = 0;
    }

}