//��������� ����������� � �������������� ������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / Change lock cursor")]
    [DisallowMultipleComponent]
    public class Change_lock_cursor : MonoBehaviour
    {
        [SerializeField]
        bool Start_lock = true;

        void Start()
        {
            if (Start_lock)
                Game_Player.Cursor_player(false);
        }

        [ContextMenu("������������� � ������")]
        public void Lock()
        {
            Game_Player.Cursor_player(false);
        }

        [ContextMenu("�������������� � ��������")]
        public void UnLock()
        {
            Game_Player.Cursor_player(true);
        }

    }
}