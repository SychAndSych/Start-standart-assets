using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sych_scripts
{
    public class Slot_equipnet_inventory_UI : Slot_inventory_UI
    {
        Vector2 Active_id_item = new Vector2(-1, -1);


        #region Методы

        /// <summary>
        /// Активировать эвент по смене оружия игрока
        /// </summary>
        void Active_item_event()
        {
            
            if (Active_id_item != null)
                Inventory_UI.Singleton_Instance.Active_item_player_event.Invoke(Active_id_item);
            else
                Inventory_UI.Singleton_Instance.Active_item_player_event.Invoke(new Vector2(-1, -1));
        }
        #endregion


        #region Публичные методы

        public override void Add_id_item(Vector2 _item_id)
        {
            Active_id_item = _item_id;

            Update_slot();
        }

        public override void Null_slot()
        {
            base.Null_slot();

            Active_id_item = new Vector2(-1, -1);

            Update_slot();
        }

        public override void Drop_item()
        {
            base.Drop_item();

            Inventory_UI.Singleton_Instance.Inventory_player.Drop_item(Active_id_item);
            Active_id_item = new Vector2(-1, -1);

            Update_slot();
        }


        public override void Destroy_item()
        {
            base.Destroy_item();

            if (Active_id_item != null)
                Active_id_item = new Vector2(-1, -1);

            Update_slot();
        }

        public override void Update_slot()
        {
            base.Update_slot();

            if (Active_id_item != null)
                Active_image_transform.GetComponent<Image>().sprite = System_configs.Singleton_instance.Get_Config_object_SO(Active_id_item).Sprite;
            else
                Active_image_transform.GetComponent<Image>().sprite = null;


            if (Backlight_image.enabled)
                Active_item_event();
        }

        public override void Change_selection(bool _activity)
        {
            base.Change_selection(_activity);

            if (_activity)
                Active_item_event();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            if (Active_id_item != null)
            {
                Inventory_UI.Singleton_Instance.Current_slot = this;
                Inventory_UI.Singleton_Instance.Activity_Hint_backlight_slot(true);
                Inventory_UI.Singleton_Instance.Active_image.GetComponent<Image>().sprite = Active_image_transform.GetComponent<Image>().sprite;
                Inventory_UI.Singleton_Instance.Active_image.gameObject.SetActive(true);
                Active_image_transform.GetComponent<Image>().sprite = null;
                Active_bool = true;
            }
        }

        #endregion


        #region Проверяющие методы
        public override Vector2 Get_id_item
        {
            get
            {
                return Active_id_item;
            }
        }
        #endregion


        #region Для тестов

        [ContextMenu("Включить для теста")]
        public void ON_test()
        {
            Change_selection(true);
        }
        #endregion
    }
}