using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Lean.Pool;

namespace Sych_scripts
{
    public class Inventory_UI : Singleton<Inventory_UI>, IPointerEnterHandler, IPointerExitHandler
    {

        [Tooltip("��������� ��������� (���������� �����)")]//Inventory interface (visual part)
        [SerializeField]
        GameObject Inventory_visual = null;

        [Tooltip("��������� ������������ �������")]//Inventory of the opened object
        [SerializeField]
        GameObject Inventory_object_interact_visual = null;

        [Tooltip("�������� ��������������� ��������")]//Active dragged picture
        [field: SerializeField]
        public Transform Active_image = null;

        [Tooltip("������ �����")]
        [SerializeField]
        Slot_inventory_UI Prefab_slot = null;

        [Tooltip("������������ ����� � ������� ����� �������� ������ �������")]
        [SerializeField]
        Slot_inventory_UI[] Hint_backlight_slot_array = new Slot_inventory_UI[0];

        [Tooltip("������������ ������ ���������� ������ ��������� ������")]//Parent object containing inventory slots player
        [SerializeField]
        Transform Inventory_transform_player = null;

        /// <summary>
        /// ������ ��������� ������
        /// </summary>
        List<Slot_inventory_UI> Player_Inventory_slots_list = new List<Slot_inventory_UI>();

        [Tooltip("������������ ������ ���������� ������ ��������� ������� � ������� ���������������")]//Parent object containing the inventory cells of the object with which we interact
        [SerializeField]
        Transform Inventory_transform_interact_object = null;

        /// <summary>
        /// ������ ��������� ������� � ������� ���������������
        /// </summary>
        List<Slot_inventory_UI> Interact_object_Inventory_slots_list = new List<Slot_inventory_UI>();

        /// <summary>
        /// ��� ������ ��� ��������������
        /// </summary>
        List<Slot_inventory_UI> All_active_slot_inventory_UI_list = new List<Slot_inventory_UI>();


        [Tooltip("������������ ������ ���������� ������ �������� ��������")]//Parent object containing the inventory cells of the object with which we interact
        [SerializeField]
        Transform Inventory_transform_Equipment = null;


        [Tooltip("����� ����������")]
        [SerializeField]
        UnityEvent<bool> Active_inventory_event = new UnityEvent<bool>();

        /// <summary>
        /// ������ �������� �������
        /// </summary>
        List<Slot_inventory_UI> Equipment_Inventory_slots_list = new List<Slot_inventory_UI>();

        internal UnityEvent<Vector2> Active_item_player_event = new UnityEvent<Vector2>();//����������� ����� ������ ���������� �������� (� �������� ����� ��� ����� �������� � ����� ������)

        internal Slot_inventory_UI Current_slot = null;

        internal Slot_inventory_UI Next_slot = null;

        internal Object_inventory Inventory_player = null;

        Object_inventory Inventory_interact_object = null;

        GameObject Current_Hover = null;

        bool Drop_bool = false;

        public bool Open_inventory_bool { get; private set; } = false;

        protected override void Awake()
        {
            base.Awake();

            foreach (Slot_inventory_UI child in GetComponentsInChildren<Slot_inventory_UI>())
            {
                All_active_slot_inventory_UI_list.Add(child);
            }

            foreach (Slot_inventory_UI child in Inventory_transform_interact_object.GetComponentsInChildren<Slot_inventory_UI>())
            {
                Interact_object_Inventory_slots_list.Add(child);
            }

            foreach (Slot_inventory_UI child in Inventory_transform_player.GetComponentsInChildren<Slot_inventory_UI>())
            {
                Player_Inventory_slots_list.Add(child);
            }

            foreach (Slot_inventory_UI child in Inventory_transform_Equipment.GetComponentsInChildren<Slot_inventory_UI>())
            {
                Equipment_Inventory_slots_list.Add(child);
            }

            Active_image.gameObject.SetActive(false);
        }

        private void Start()
        {
            //Drop_force = System_configs.Singleton_Instance.Player_config.dr;

            //Inventory_player = Game_administrator.Singleton_Instance.Player_administrator.Inventory;
            Game_administrator.Singleton_Instance.Player_administrator.Player_input.Change_id_weapon_event.AddListener(Change_Equipment_slot);

            Inventory_recalculation();

            Inventory_visual.SetActive(false);

            Activity(false, false);

            Invoke(nameof(Delay_Change_Equipment_slot), 0.1f);
            
        }

        void Delay_Change_Equipment_slot()
        {
            Change_Equipment_slot(0);
        }

        /// <summary>
        /// ������������� ��������� ���������
        /// </summary>
        void Inventory_recalculation()
        {
            if (Inventory_player != null)
            {
                if (Inventory_player.Item_list.Count < Player_Inventory_slots_list.Count)//�������� ������ ��� ������������ ����� ��������� ������
                {
                    for (int x = 0; x < Player_Inventory_slots_list.Count; x++)
                    {
                        if (Inventory_player.Item_list.Count > x)
                            Player_Inventory_slots_list[x].gameObject.SetActive(true);
                        else
                            Player_Inventory_slots_list[x].gameObject.SetActive(false);
                    }
                }
                else if (Inventory_player.Item_list.Count > Player_Inventory_slots_list.Count)//���� ����� ������������, �� ������ �����
                {
                    int value_add = Inventory_player.Item_list.Count - Player_Inventory_slots_list.Count;

                    for (int x = 0; x < value_add; x++)
                    {
                        Slot_inventory_UI slot = Instantiate(Prefab_slot, Inventory_transform_player);
                        All_active_slot_inventory_UI_list.Add(slot);
                        Player_Inventory_slots_list.Add(slot);
                    }
                }

                for (int x = 0; x < Inventory_player.Item_list.Count; x++)//�������� �������� �� ��������� ������
                {
                    Player_Inventory_slots_list[x].Initialization_slot(Inventory_player, x);
                }
            }


            if (Inventory_interact_object != null)
            {
                if (Inventory_interact_object.Item_list.Count < Interact_object_Inventory_slots_list.Count)
                {
                    for (int x = 0; x < Interact_object_Inventory_slots_list.Count; x++)
                    {
                        if (Inventory_interact_object.Item_list.Count > x)
                            Interact_object_Inventory_slots_list[x].gameObject.SetActive(true);
                        else
                            Interact_object_Inventory_slots_list[x].gameObject.SetActive(false);
                    }
                }
                else if (Inventory_interact_object.Item_list.Count > Interact_object_Inventory_slots_list.Count)
                {
                    int value_add = Inventory_interact_object.Item_list.Count - Interact_object_Inventory_slots_list.Count;

                    for (int x = 0; x < value_add; x++)
                    {
                        Slot_inventory_UI slot = Instantiate(Prefab_slot, Inventory_transform_player);
                        All_active_slot_inventory_UI_list.Add(slot);
                        Interact_object_Inventory_slots_list.Add(slot);
                    }
                }

                for (int x = 0; x < Inventory_interact_object.Item_list.Count; x++)
                {
                    Interact_object_Inventory_slots_list[x].Initialization_slot(Inventory_interact_object, x);
                }
            }
        }


        /// <summary>
        /// ��������/��������� ���������� ����� ���������
        /// </summary>
        /// <param name="_activity"></param>
        /// <param name="interact_object"></param>
        public void Activity(bool _activity, bool interact_object)
        {
            Open_inventory_bool = _activity;
            Inventory_visual.SetActive(_activity);

            if (_activity && interact_object || !_activity)
                Inventory_object_interact_visual.SetActive(_activity);

            if (_activity)
                Inventory_recalculation();

            Active_inventory_event.Invoke(Open_inventory_bool);

            if(Game_administrator.Singleton_Instance)
            Game_administrator.Singleton_Instance.Player_control_activity(!_activity);
        }

        [ContextMenu("���/����")]
        public void Button_inventory()
        {
            Open_inventory_bool = !Open_inventory_bool;

            Activity(Open_inventory_bool, false);
        }


        public void Change_Equipment_slot(int _id)
        {
            foreach (Slot_inventory_UI slot in Equipment_Inventory_slots_list)
            {
                slot.Change_selection(false);
            }
            Equipment_Inventory_slots_list[_id].Change_selection(true);
        }


        /// <summary>
        /// ��������������, ����������� � �����������
        /// </summary>
        public void Transfer_slots()
        {
            if (Current_slot != null && Next_slot != null && Current_slot != Next_slot)
            {
                if (Next_slot.Get_id_item == null && (Next_slot.Check_Allowed_id_category_item((int)Current_slot.Get_id_item.x) || Next_slot.Allowed_id_category_item_array.Length == 0))//���������� ���� ��������
                {
                    Next_slot.Add_id_item(Current_slot.Get_id_item);
                    
                    Current_slot.Null_slot();
                }

                else if (Next_slot.Get_id_item != null && (Next_slot.Check_Allowed_id_category_item((int)Current_slot.Get_id_item.x) 
                    || Next_slot.Allowed_id_category_item_array.Length == 0) && (Current_slot.Check_Allowed_id_category_item((int)Next_slot.Get_id_item.x) 
                    || Current_slot.Allowed_id_category_item_array.Length == 0))
                {
                    bool merge_bool = false;

                    //��� ��������� ����� �� ���������� ��������
                    for (int x = 0; x < System_configs.Singleton_instance.Merge_item_array.Length; x++)
                    {

                        if (System_configs.Singleton_instance.Merge_item_array[x].First_item == null || System_configs.Singleton_instance.Merge_item_array[x].Second_item == null || System_configs.Singleton_instance.Merge_item_array[x].Final_item == null)
                        {
                            Debug.LogError("������ ������� ���������� �������� ��� �������  " + x + "  ������!");
                        }  
                        else
                        {

                            if (Next_slot.Get_id_item == System_configs.Singleton_instance.Merge_item_array[x].First_item.Index && Current_slot.Get_id_item == System_configs.Singleton_instance.Merge_item_array[x].Second_item.Index ||
                                Next_slot.Get_id_item == System_configs.Singleton_instance.Merge_item_array[x].Second_item.Index && Current_slot.Get_id_item == System_configs.Singleton_instance.Merge_item_array[x].First_item.Index)
                            {
                                Next_slot.Destroy_item();
                                Entity_abstract fin_merge_item = System_configs.Singleton_instance.Merge_item_array[x].Final_item;
                                Next_slot.Add_id_item(System_configs.Singleton_instance.Object_category_array[(int)fin_merge_item.Index.x].Object_array[(int)fin_merge_item.Index.y].Prefab_game.Index);
                                Current_slot.Destroy_item();
                                merge_bool = true;
                                break;
                            }
                        }
                    }

                    if (!merge_bool)
                    {
                        Vector2 buffer = Next_slot.Get_id_item;

                        Next_slot.Add_id_item(Current_slot.Get_id_item);
                        Current_slot.Add_id_item(buffer);
                    }
                }
                /*
                else
                {
                    print(1);
                    //��� ��������� ����� �� ���������� ��������
                    for (int x = 0; x < System_configs.Singleton_Instance.Merge_item_array.Length; x++)
                    {
                        if (Next_slot.Find_out_item == System_configs.Singleton_Instance.Merge_item_array[x].First_item && Current_slot.Find_out_item == System_configs.Singleton_Instance.Merge_item_array[x].Second_item ||
                            Next_slot.Find_out_item == System_configs.Singleton_Instance.Merge_item_array[x].Second_item && Current_slot.Find_out_item == System_configs.Singleton_Instance.Merge_item_array[x].First_item)
                        {
                            Next_slot.Destroy_item();
                            Entity_abstract fin_merge_item = System_configs.Singleton_Instance.Merge_item_array[x].Final_item;
                            Next_slot.Add_item(System_configs.Singleton_Instance.Object_category_array[(int)fin_merge_item.Index.x].Object_array[(int)fin_merge_item.Index.y].Prefab_item_to_scene.GetComponent<Item_Pick_up>());
                            Current_slot.Destroy_item();
                            break;
                        }
                    }
                }
                */
            }
            else if (Current_slot)
            {
                if (Drop_bool)
                {
                    Current_slot.Drop_item();
                }

            }

            if(Current_slot != null)
                Current_slot.Update_slot();
            if (Next_slot != null)
                Next_slot.Update_slot();

            Current_slot = null;
            Next_slot = null;
        }


        /// <summary>
        /// �������� ��������� (���������) ������� � ������� ���������������
        /// </summary>
        /// /// <param name="_inventory">������ ��������� �������</param>
        public void Add_interact_inventory_object(Object_inventory _inventory)
        {
            Inventory_interact_object = _inventory;
            Inventory_recalculation();
        }


        /// <summary>
        /// ��������/��������� ��������� ������ � ������� ����� �������� ������ �������
        /// </summary>
        /// <param name="_activity"></param>
        public void Activity_Hint_backlight_slot(bool _activity)
        {
            if (Current_slot != null && _activity)
            {
                for (int x = 0; x < Hint_backlight_slot_array.Length; x++)
                {
                    if (Hint_backlight_slot_array[x].Check_Allowed_id_category_item((int)Current_slot.Get_id_item.x))
                    {
                        Hint_backlight_slot_array[x].Change_selection(true);
                    }
                }
            }
            else
            {
                for (int x = 0; x < Hint_backlight_slot_array.Length; x++)
                {
                    Hint_backlight_slot_array[x].Change_selection(false);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                Current_Hover = eventData.pointerCurrentRaycast.gameObject;
                Drop_bool = false;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Current_Hover = null;
            Drop_bool = true;
        }
    }
}