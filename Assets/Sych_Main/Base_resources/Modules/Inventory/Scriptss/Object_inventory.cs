//��������� ������� (��� ������)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lean.Pool;

namespace Sych_scripts {
    [AddComponentMenu("Sych scripts / Game / Inventory / Object / Object inventory")]
    [DisallowMultipleComponent]
    public class Object_inventory : MonoBehaviour, I_open_object_inventory
    {

        [field: Tooltip("���������� ��������� ������")]
        [field: SerializeField]
        public int Slots_limit { get; private set; } = 10;

        [Tooltip("��� ��������� � ��������� � ���������/�������")]//What is in the inventory of the character / object
        public List<Vector2> Item_list = new List<Vector2>();

        [Tooltip("����� ������������ ��������")]
        [SerializeField]
        Transform Point_drop_item = null;

        [field: Tooltip("����� �� ������ ����������������� � ��������� (�������� ������� ������ � ���������� � ��)?")]
        [field: SerializeField]
        public bool Interact_inventory_bool { get; private set; } = false;

        [Tooltip("�������� ��������� ������")]
        [SerializeField]
        bool Player_bool = false;

        #region ������ Callback
        private void Awake()
        {
            Preparation();
        }

        private void Start()
        {
            if (Player_bool)
                Only_player();
        }

        #endregion


        #region ������
        /// <summary>
        /// ����������
        /// </summary>
        void Preparation()
        {
            for (int x = Item_list.Count; x < Slots_limit; x++)
            {
                Item_list.Add(new Vector2(-1, -1));
            }

        }

        #endregion


        #region ��������� ������
        /// <summary>
        /// ������, ���� �� ��������� ����� � ���������
        /// </summary>
        public bool Find_out_Free_space
        {
            get
            {
                bool result = false;

                foreach (Vector2 slot in Item_list)
                {
                    if (slot.x < 0 || slot.y < 0)
                    {
                        result = true;
                        break;
                    }

                }

                return result;
            }
        }


        /// <summary>
        /// ������� ���� ��������� "�������� ��������� ������"
        /// </summary>
        public void Only_player()
        {
            Inventory_UI.Singleton_Instance.Inventory_player = this;
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void Activation()
        {
            if (Interact_inventory_bool)
            {
                Inventory_UI.Singleton_Instance.Add_interact_inventory_object(this);
                Inventory_UI.Singleton_Instance.Activity(true, true);
            }
        }

        #endregion


        #region ����������� ������

        /// <summary>
        /// �������� ������ � ���������
        /// </summary>
        /// <param name="_item">����������� ������</param>
        public void Add_item(Item_Pick_up _item)
        {

            if (Find_out_Free_space)
            {
                for (int x = 0; x < Item_list.Count; x++)
                {
                    if (Item_list[x].x < 0 || Item_list[x].y < 0)
                    {
                        Item_list[x] = _item.Index;
                        LeanPool.Despawn(_item);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// �������� �������
        /// </summary>
        /// <param name="_id_item">������� ������� ����������</param>
        public void Drop_item(Vector2 _id_item)
        {

            if (_id_item != null)
            {
                Game_object_abstract current_item = LeanPool.Spawn((Game_object_abstract)System_configs.Singleton_instance.Get_Config_object_SO(_id_item.x, _id_item.y).Prefab_game);

                /*
                if (PrefabUtility.GetPrefabAssetType(current_item) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(current_item) == PrefabAssetType.Variant)
                {
                    
                }
                else if (PrefabUtility.GetPrefabAssetType(current_item) != PrefabAssetType.NotAPrefab)
                {
                    Debug.LogError("��� �� ����� � ��������!");
                }
                */
                current_item.gameObject.SetActive(true);

                if (current_item.Body != null)
                {
                    current_item.Body.velocity = Vector3.zero;
                    current_item.Body.angularVelocity = Vector3.zero;
                    current_item.Body.AddForce(Game_administrator.Singleton_Instance.Player_administrator.Cam.transform.forward.normalized * 700f);
                }

                current_item.transform.position = Point_drop_item.position;
            }
        }


        /// <summary>
        /// �������� ������� ����� id �����
        /// </summary>
        /// <param name="_item">����� ����� � ������� ������������ ������</param>
        public void Drop_item(int _id_slot)
        {

            if (Item_list[_id_slot] != null)
            {
                Drop_item(Item_list[_id_slot]);

                Item_list[_id_slot] = new Vector2(-1, -1);
            }
        }



        /// <summary>
        /// ���������� �������
        /// </summary>
        /// <param name="_item">����� ����� � ������� ������������ ������</param>
        public void Destroy_item(int _id_slot)
        {

            if (Item_list[_id_slot] != null)
            {
                /*
                if (PrefabUtility.GetPrefabAssetType(Item_list[_id_slot]) != PrefabAssetType.Regular && PrefabUtility.GetPrefabAssetType(Item_list[_id_slot]) != PrefabAssetType.Variant)
                {
                    //print(Lean.Pool.LeanPool.Links.Remove(Item_list[_id_slot].gameObject));
                    LeanPool.Despawn(Item_list[_id_slot].gameObject);
                }
                */

                Item_list[_id_slot] = new Vector2(-1, -1);
            }
        }


        #endregion
    }
}
