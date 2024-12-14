//Мозг для игрока
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Brain player SM")]
    [DisallowMultipleComponent]
    public class Brain_player_SM : Brain_SM
    {

        [Tooltip("Какую логику поведения нужно будет применить, если персонаж возьмёт в руки этот предмет.")]
        [SerializeField]
        State_item_logic[] State_item_logic_array = new State_item_logic[0];

        /// <summary>
        /// Меняет состояния поведения в зависимости от активного предмета в руках
        /// </summary>
        /// <param name="_id_item"></param>
        public void Change_active_item_preparation_state(Item_Game_object_abstract _item)
        {
            bool result = false;

            if (_item != null)
            {
                for (int x = 0; x < State_item_logic_array.Length; x++)
                {
                    if (_item.Item_type == State_item_logic_array[x].Type_item)
                    {
                        Change_state(State_item_logic_array[x].Name_state_activation);
                        result = true;
                        break;
                    }
                }

                if (!result)
                    Change_state(State_item_logic_array[0].Name_state_activation);
            }
        }

        [System.Serializable]
        class State_item_logic
        {
            [Tooltip("Если будет такой тип предмета (это для сравнения с предметом)")]
            public Item_enum Type_item = Item_enum.Item;

            [Tooltip("Если условие прошло, то будет активировано это состояние поведения")]
            public string Name_state_activation = "Normal";
        }
    }
}