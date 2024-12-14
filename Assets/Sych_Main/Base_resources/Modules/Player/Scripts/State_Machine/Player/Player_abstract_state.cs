using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    public abstract class Player_abstract_state : State_abstract
    {

        protected Player_character Character;

        [field: Tooltip("Нету Game_administrator, переходим на ручное управление задавание настроек")]
        [field: SerializeField]
        protected bool No_Game_administrator_bool { get; private set; } = false;

        [ShowIf(nameof(No_Game_administrator_bool))]
        [Tooltip("Камера")]
        [SerializeField]
        protected Camera Cam = null;

        protected override void Preparation()
        {
            Character = (Player_character)Character_script;
            base.Preparation();

            if (Game_administrator.Singleton_Instance)
                Cam = Game_administrator.Singleton_Instance.Player_administrator.Cam;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}