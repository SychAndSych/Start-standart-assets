using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Weapon / Melee")]
    [DisallowMultipleComponent]
    public class Melee : Weapon_abstract
    {

        bool No_stop_attack_bool = false;

        //[Tooltip("Эффект от удара")]
        //[SerializeField]
        //MeleeWeaponTrail MeleeWeaponTrail_script = null;

        [Tooltip("Ивент старт атаки")]
        [SerializeField]
        UnityEvent Attack_start_event = new UnityEvent();

        [Tooltip("Ивент конца атаки")]
        [SerializeField]
        UnityEvent Attack_end_event = new UnityEvent();

        [Tooltip("Ивент следующей атаки атаки")]
        [SerializeField]
        UnityEvent Next_attack_event = new UnityEvent();

        [Tooltip("Ивент ресета атаки")]
        [SerializeField]
        UnityEvent Reset_attack_event = new UnityEvent();

        private void OnEnable()
        {
            UI_weapon_administrator.Singleton_Instance.Activity(false, false, false);
            UI_weapon_administrator.Singleton_Instance.Activity_Aim(false);
        }

        private void OnDisable()
        {
            if (UI_weapon_administrator.Singleton_Instance) 
            {
                UI_weapon_administrator.Singleton_Instance.Activity(false, false, false);
                UI_weapon_administrator.Singleton_Instance.Activity_Aim(false);
            }
        }

        public override void Attack(bool _activity)
        {
            Attack_start_event.Invoke();

            if (!No_stop_attack_bool)
            {
                Next_attack_event.Invoke();

                No_stop_attack_bool = true;
            }

        }

        public override void End_attack()
        {
            if (!No_stop_attack_bool)
                Attack_end_event.Invoke();
        }

        public virtual void Reset_combo()
        {
            No_stop_attack_bool = false;

            Reset_attack_event.Invoke();
        }

        public override void Activity(bool _activity)
        {
            //MeleeWeaponTrail_script.Activity_emit(_activity);
        }

        protected override void Initialized_stats()
        {
            //throw new System.NotImplementedException();
        }
    }
}