using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Checkpoint / Checkpoint UI button")]
    [DisallowMultipleComponent]
public class Checkpoint_button : MonoBehaviour
{

    [field: Tooltip("ID ���������")]
    [field: SerializeField]
    public int Id_checkpoint { get; private set; } = 0;

    [Tooltip("����� �������� ������� ��� ���������")]
    [SerializeField]
    bool Advertisement_mode_bool = false;

    [Tooltip("����������� �����")]
    [SerializeField]
    Image UI_lock = null;

    [Tooltip("����������� �������")]
    [SerializeField]
    Image UI_advertisement = null;

    [Tooltip("������������� �����������")]
    [SerializeField]
    Image UI_block_image = null;

    [Tooltip("������������ ��� ������")]
    [SerializeField]
    bool Start_UnLock_bool = false;

    bool Unlock_bool = false;

    private void Start()
    {
        UI_advertisement.gameObject.SetActive(false);
        UI_block_image.gameObject.SetActive(true);
        UI_lock.gameObject.SetActive(true);

        if (Start_UnLock_bool)
            Unlock();

        if (UI_game_menu.Singleton_Instance.Check_unlock(Id_checkpoint))
        {
            Unlock();
        }
    }

    private void OnEnable()
    {
        if (UI_game_menu.Singleton_Instance.Check_unlock(Id_checkpoint))
        {
            Unlock();
        }

    }

    /// <summary>
    /// �������������� ����
    /// </summary>
    public void Unlock()
    {
        Unlock_bool = true;

        UI_block_image.gameObject.SetActive(false);
        UI_lock.gameObject.SetActive(false);

        if(Advertisement_mode_bool)
            UI_advertisement.gameObject.SetActive(true);
    }

    /// <summary>
    /// ������������ ����������������
    /// </summary>
    public void Active()
    {
        if (Unlock_bool)
        {
            //Game_administrator.Singleton_Instance.Teleport_checpoint(Id_checkpoint);
            UI_game_menu.Singleton_Instance.Close_menu();
        }
        
    }

}
}