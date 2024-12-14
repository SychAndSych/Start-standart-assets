using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Checkpoint / Checkpoint object")]
    [DisallowMultipleComponent]
    public class Checkpoint : MonoBehaviour
    {
        [field: Tooltip("Номер чекпоинта (нужно для приоритета)")]
        [field: SerializeField]
        public int ID { get; private set; } = 0;

        [field: Tooltip("Точка для куда телепортироваться")]
        [field: SerializeField]
        public Transform Teleport_point = null;

    bool Activation_bool = false;

    private void Start()
    {

        Invoke(nameof(Delay_preparation_unlock), 0.1f);
    }

    private void OnTriggerEnter(Collider other)
        {
        if (other.GetComponent<Player_character>()) 
        {
            Unlock();
        }
    }

    void Delay_preparation_unlock()
    {
        Add();

        if (Save_PlayerPrefs.Know_parameter(Type_parameter_bool.Checkpoint_bool, ID))
            Unlock();
    }

    void Add()
    {
        if (!Activation_bool)
        {
            //Game_administrator.Singleton_Instance.Add_Checkpoint_list(this);
            Activation_bool = true;
        }
    }

    void Unlock()
    {
        UI_game_menu.Singleton_Instance.Unlock_slot(ID);

        if (!Save_PlayerPrefs.Know_parameter(Type_parameter_bool.Checkpoint_bool, ID))
        {
            //if (GA_Manager.Singleton_Instance)
            //    GA_Manager.Singleton_Instance.OnCheckpointUnlock(ID);

            Save_PlayerPrefs.Save_parameter(Type_parameter_bool.Checkpoint_bool, ID, true);
        }
    }

    }
}