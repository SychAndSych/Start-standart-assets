//Обработчик реагирующий на то, что в активном слоте выбран объект
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

        [Tooltip("Эвент говорит о том, какой предмет был выбран в активном слоте у игрока")]
        [SerializeField]
        UnityEvent<Entity_abstract> Item_active_event = new UnityEvent<Entity_abstract>(); 

        private void Start()
        {
            Inventory_UI.Singleton_Instance.Active_item_player_event.AddListener(Active_item);
        }

        /// <summary>
        /// Какой предмет был выбран в активном слоте игрока
        /// </summary>
        /// <param name="_id_item">предмет</param>
        void Active_item(Vector2 _id_item)
        {
            Item_active_event.Invoke(System_configs.Singleton_instance.Get_Config_object_SO(_id_item).Prefab_game);
        }

    }
}
