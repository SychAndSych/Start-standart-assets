//Скрипт для смены оружия у персонажа
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Handlers / Item in hand handler")]
    [DisallowMultipleComponent]
    public class Item_in_hand_handler : MonoBehaviour
    {

        [Tooltip("Поменять тип предмета")]
        public UnityEvent<Entity_abstract> Change_type_item_event = new UnityEvent<Entity_abstract>();

        [Tooltip("Массив предметов которые держит персонаж")]
        [SerializeField]
        Item_Game_object_abstract[] Item_hands_array = new Item_Game_object_abstract[0];

        internal Item_Game_object_abstract Active_item { get; private set; } = null;//Предмет который сейчас в руках

        private void Awake()
        {
            foreach (Entity_abstract item in Item_hands_array)
            {
                item.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            Inventory_UI.Singleton_Instance.Active_item_player_event.AddListener(Change_hands_item);
        }

        /// <summary>
        /// Поменять (включить) предмет в руках персонажа
        /// </summary>
        /// <param name="_id_item">id (номер) предмета для поиска</param>
        public void Change_hands_item(Vector2 _item)
        {

            if (Active_item != null)
                Active_item.gameObject.SetActive(false);

            if (_item != null)
                for (int x = 0; x < Item_hands_array.Length; x++)
                {
                    if (Item_hands_array[x].Index == _item)
                    {
                        Active_item = Item_hands_array[x];
                        Active_item.gameObject.SetActive(true);
                        Change_type_item_event.Invoke(System_configs.Singleton_instance.Get_Config_object_SO(_item).Prefab_game);
                        break;
                    }
                }

        }

        /*

        /// <summary>
        /// Получить скрипт предмета который есть у игрока
        /// </summary>
        /// <param name="_id_item">id (номер) предмета для поиска</param>
        /// <returns></returns>
        public Game_object_abstract Get_object_script(Vector2 _id_item)
        {
            Game_object_abstract result = null;

            for (int x = 0; x < Item_hands_array.Length; x++)
            {
                if (Item_hands_array[x].Index == _id_item)
                {
                    result = Item_hands_array[x];
                    break;
                }
            }

            return result;
        }
        */
    }
}