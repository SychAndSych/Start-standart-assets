//���������� ����������� �� ��, ��� � �������� ����� ������ ������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Inventory / Character / Active equipment handler")]
    [DisallowMultipleComponent]
    public class Active_equipment_handler : MonoBehaviour
    {

        [Tooltip("����� ������� � ���, ����� ������� ��� ������ � �������� ����� � ������")]
        [SerializeField]
        UnityEvent<Entity_abstract> Item_active_event = new UnityEvent<Entity_abstract>(); 

        private void Start()
        {
            Inventory_UI.Singleton_Instance.Active_item_player_event.AddListener(Active_item);
        }

        /// <summary>
        /// ����� ������� ��� ������ � �������� ����� ������
        /// </summary>
        /// <param name="_id_item">�������</param>
        void Active_item(Vector2 _id_item)
        {
            Item_active_event.Invoke(System_configs.Singleton_instance.Get_Config_object_SO(_id_item).Prefab_game);
        }

    }
}
