//��������� �� ������ ��������

namespace Sych_scripts
{
    public interface I_Pick_up
    {
        /// <summary>
        /// ��������� ������
        /// </summary>
        /// <param name="_host_inventory">��������� ������������</param>
        /// <param name="_item">����������� �������</param>
        public void Add_inventory(Object_inventory _host_inventory, Item_Pick_up _item)
        {
            _host_inventory.Add_item(_item);
        }
    }
}