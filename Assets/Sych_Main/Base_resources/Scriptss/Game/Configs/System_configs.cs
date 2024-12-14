using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    /*
    public enum Category_object
    {
        Item, Armor, Unit
    }
    */

    [CreateAssetMenu(fileName = "System_configs", menuName = "Sych_SO / Config / New_System_Configs", order = 0)]
    public class System_configs : ScriptableObject
    {
        #region Singleton
        private static System_configs instance;
        public static System_configs Singleton_instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load("System_configs") as System_configs;

                    if (instance == null)
                    {
                        Debug.LogError("System_configs Singleton_instance is null");
                    }

                }

                return instance;
            }
        }
        #endregion

        #region Переменные
        //[Foldout("Категории объектов")]
        //public Config_object Item, Armor, Unit;

        [Tooltip("Конфиги объектов")]
        public Config_object[] Object_category_array = new Config_object[0];

        [Tooltip("Конфиги по смешиванию предметов")]
        public Merge_item[] Merge_item_array = new Merge_item[0];

        //[NamedArrayAttribute(new string[] { "Neutral", "Happy", "Sad" })]
        //public Config_object_SO[] fd;
        #endregion

        #region Публичные методы
        /// <summary>
        /// Получить данные объекта
        /// </summary>
        /// <param name="_id_Object_category">id категории</param>
        /// <param name="_id_Object">id объекта в категории</param>
        /// <returns></returns>
        public Config_object_SO Get_Config_object_SO (int _id_Object_category, int _id_Object)
        {
                return Object_category_array[_id_Object_category].Object_array[_id_Object];
        }

        public Config_object_SO Get_Config_object_SO(float _id_Object_category, float _id_Object)
        {
            return Object_category_array[(int)_id_Object_category].Object_array[(int)_id_Object];
        }

        public Config_object_SO Get_Config_object_SO(Vector2 id_x_and_y_object)
        {
            return Object_category_array[(int)id_x_and_y_object.x].Object_array[(int)id_x_and_y_object.y];
        }
        #endregion
    }
}
