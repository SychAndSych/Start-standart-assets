using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    public abstract class Weapon_abstract : Item_Game_object_abstract
    {

        private void OnEnable()
        {
            Activity(false);
        }

        /// <summary>
        /// �����
        /// </summary>
        public abstract void Attack(bool _activity);

        /// <summary>
        /// ����� �����
        /// </summary>
        public abstract void End_attack();

        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <param name="_activity">����������� (����������� � �����) ?</param>
        public abstract void Activity(bool _activity);

    }
}