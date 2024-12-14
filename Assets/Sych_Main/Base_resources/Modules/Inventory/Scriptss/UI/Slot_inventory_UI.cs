using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Sych_scripts
{
    [DisallowMultipleComponent]
    public class Slot_inventory_UI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler
    {
        [Tooltip("јктивна€ (дочерн€€) часть слота с картинкой")]//Active (child) part of the slot with a picture
        [SerializeField]
        protected RectTransform Active_image_transform = null;

        [Tooltip("ѕодсветка слота (дочерн€€ часть)")]//Slot illumination (child)
        [SerializeField]
        protected Image Backlight_image = null;

        [Tooltip("–азрешенна€ категори€ предметов которые можно положить в €чейку (-1 значит разрешено всЄ)")]// Allowed category of items that can be put in a cell(-1 means everything is allowed)
        [field: SerializeField]
        public int[] Allowed_id_category_item_array { get; private set; } = new int[0];

        protected bool Active_bool = false;

        int Id_slot  = -1;

        Object_inventory Active_inventory = null;


        private void Awake()
        {
            Change_selection(false);
        }




        #region ѕубличные методы

        /// <summary>
        /// »нициализировать слот (задать необходимые параметры дл€ работы)
        /// </summary>
        /// <param name="_active_inventory">»нвентарь к которому относитс€ €чейка (инвентарь на объекте)</param>
        /// <param name="_id_slot">Id слота (который равен id €чейки инвентар€ на объекте)</param>
        public void Initialization_slot(Object_inventory _active_inventory, int _id_slot)
        {
            Active_inventory = _active_inventory;
            Id_slot = _id_slot;

            Update_slot();
        }


        /// <summary>
        /// ќбновить слот
        /// </summary>
        public virtual void Update_slot()
        {
            if (Active_inventory != null)
                if (Active_inventory.Item_list[Id_slot].x > -1 && Active_inventory.Item_list[Id_slot].y > -1)
                    Active_image_transform.GetComponent<Image>().sprite = System_configs.Singleton_instance.Object_category_array[(int)Active_inventory.Item_list[Id_slot].x].Object_array[(int)Active_inventory.Item_list[Id_slot].y].Sprite;
        }

        public virtual void Add_id_item(Vector2 _item)
        {
            if (Active_inventory != null)
            {
                Active_inventory.Item_list[Id_slot] = new Vector2 (_item.x, _item.y);
            }
            Update_slot();

            
        }

        public virtual void Null_slot()
        {
            if (Active_inventory != null)
            {
                Active_inventory.Item_list[Id_slot] = new Vector2(-1, -1);
            }

            Update_slot();
        }

        public virtual void Drop_item()
        {
            if (Active_inventory != null)
            {
                Active_inventory.Drop_item(Id_slot);
            }
            Update_slot();
        }

        public virtual void Destroy_item()
        {
            if (Active_inventory != null)
            {
                Active_inventory.Destroy_item(Id_slot);
                Update_slot();
            }

        }


        /// <summary>
        /// ¬кл/¬ыкл подсветки
        /// </summary>
        /// <param name="_activity"></param>
        public virtual void Change_selection(bool _activity)
        {
            Backlight_image.enabled = _activity;
        }


        /// <summary>
        /// —хватили €чейку
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if(Active_inventory != null)
            if (Active_inventory.Item_list[Id_slot].x > -1 && Active_inventory.Item_list[Id_slot].y > -1)
            {
                Inventory_UI.Singleton_Instance.Current_slot = this;
                Inventory_UI.Singleton_Instance.Activity_Hint_backlight_slot(true);
                Inventory_UI.Singleton_Instance.Active_image.GetComponent<Image>().sprite = Active_image_transform.GetComponent<Image>().sprite;
                Inventory_UI.Singleton_Instance.Active_image.gameObject.SetActive(true);
                Active_image_transform.GetComponent<Image>().sprite = null;
                Active_bool = true;
            }
        }


        /// <summary>
        /// “ащим €чейку
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (Active_bool)
            {
                Inventory_UI.Singleton_Instance.Active_image.position = Mouse.current.position.ReadValue();
            }

        }

        /// <summary>
        /// ќтпустили в €чейку
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            Inventory_UI.Singleton_Instance.Active_image.localPosition = transform.position;
            Inventory_UI.Singleton_Instance.Active_image.gameObject.SetActive(false);
            Active_bool = false;
            Inventory_UI.Singleton_Instance.Transfer_slots();

            Inventory_UI.Singleton_Instance.Activity_Hint_backlight_slot(false);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            Inventory_UI.Singleton_Instance.Next_slot = this;
        }
        #endregion


        #region ѕровер€ющие методы
        /// <summary>
        /// ”знать у слота можно ли поместить в него предмет данной категории
        /// </summary>
        /// <param name="_id_item"></param>
        /// <returns></returns>
        public bool Check_Allowed_id_category_item(int _id_item)
        {
            bool result = false;

            for (int x = 0; x < Allowed_id_category_item_array.Length; x++)
            {
                if (_id_item == Allowed_id_category_item_array[x])
                {
                    result = true;
                }

            }

            return result;
        }

        /// <summary>
        /// ѕолучить предмет дл€ проверки данных
        /// </summary>
        public virtual Vector2 Get_id_item
        {
            get
            {
                Vector2 result = new Vector2 (-1, -1);

                if(Active_inventory != null)
                result = Active_inventory.Item_list[Id_slot];

                return result;
            }
        }
        #endregion
    }
}