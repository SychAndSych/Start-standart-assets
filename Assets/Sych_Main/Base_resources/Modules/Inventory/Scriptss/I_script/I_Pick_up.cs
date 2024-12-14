//Интерфейс на подбор предмета

namespace Sych_scripts
{
    public interface I_Pick_up
    {
        /// <summary>
        /// Подобрать объект
        /// </summary>
        /// <param name="_host_inventory">Инвентарь подбирающего</param>
        /// <param name="_item">Добавляемый предмет</param>
        public void Add_inventory(Object_inventory _host_inventory, Item_Pick_up _item)
        {
            _host_inventory.Add_item(_item);
        }
    }
}