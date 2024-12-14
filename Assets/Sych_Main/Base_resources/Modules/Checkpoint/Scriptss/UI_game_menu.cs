using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Checkpoint / Checkpoint UI Menu")]
    [DisallowMultipleComponent]
public class UI_game_menu : Singleton<UI_game_menu>
{
    [Tooltip("Меню отображения уровней")]
    [SerializeField]
    GameObject Menu_obj = null;

    [Tooltip("Массив слотов чекпоинтов")]
    [SerializeField]
    Checkpoint_button[] Checkpoint_slot_array = new Checkpoint_button[0];

    bool[] Unlock_fix = new bool[0];

    private void Start()
    {
        Unlock_fix = new bool[Checkpoint_slot_array.Length];
    }

    public void Unlock_slot(int _id)
    {
        for (int x = 0; x < Checkpoint_slot_array.Length; x++)
        {
            if(Checkpoint_slot_array[x].Id_checkpoint == _id)
            {
                Checkpoint_slot_array[x].Unlock();
                Unlock_fix[x] = true;
                break;
            }

        }
    }

    public void Close_menu()
    {
        Menu_obj.SetActive(false);
    }

    public bool Check_unlock(int _id)
    {
        bool result = false;

        for (int x = 0; x < Checkpoint_slot_array.Length; x++)
        {
            if (Checkpoint_slot_array[x].Id_checkpoint == _id)
            {
                result = Unlock_fix[x];
                break;
            }

        }

        return result;
    }

}
}