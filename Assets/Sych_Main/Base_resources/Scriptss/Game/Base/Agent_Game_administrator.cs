using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / Agent Game administrator")]
    [DisallowMultipleComponent]
    public class Agent_Game_administrator : MonoBehaviour
    {
        
        /// <summary>
        /// ����� �� ����� ��������� ����������?
        /// </summary>
        /// <param name="_change"></param>
        public void Player_control_change(bool _change)
        {
            Game_administrator.Player_control_event.Invoke(_change);
        }

        /// <summary>
        /// �������� ��������� �������
        /// </summary>
        /// <param name="_change"></param>
        public void Cursor_change(bool _change)
        {
            Game_Player.Cursor_player(_change);
        }
    }
}