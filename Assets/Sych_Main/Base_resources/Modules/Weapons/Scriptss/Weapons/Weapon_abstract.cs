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
        /// Атака
        /// </summary>
        public abstract void Attack(bool _activity);

        /// <summary>
        /// Конец атаки
        /// </summary>
        public abstract void End_attack();

        /// <summary>
        /// Активность оружия
        /// </summary>
        /// <param name="_activity">Активирован (исользуется в атаке) ?</param>
        public abstract void Activity(bool _activity);

    }
}