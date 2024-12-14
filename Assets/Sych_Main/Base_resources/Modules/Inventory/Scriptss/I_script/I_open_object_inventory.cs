//Открыть инвентарь объекта

namespace Sych_scripts {
    public interface I_open_object_inventory
    {
        /// <summary>
        /// Открыть инвентарь объекта
        /// </summary>
        public void Open_inventory_object(Object_inventory _inventory)
        {
            Inventory_UI.Singleton_Instance.Add_interact_inventory_object(_inventory);
            Inventory_UI.Singleton_Instance.Activity(true, true);
        }
    }
}
