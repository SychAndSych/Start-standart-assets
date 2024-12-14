//Триггер который детектит наносимый урон и передаёт основному скрипту здоровья
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Battle / HurtBox")]
    [DisallowMultipleComponent]
    public class HurtBox : MonoBehaviour, I_damage
    {

        [Tooltip("Основной скрипт здоровья")]
        [SerializeField]
        Health Main_health = null;

        [Tooltip("Есть попадание! Что будем делать ? (нужно для дополнительных действий, например если этот скрипт повесить на Кнопку)")]
        [SerializeField]
        UnityEvent Hit_event = new UnityEvent();

        Health I_damage.Main_health => Main_health;

        public void Add_Main_health(Health _health_script)//Сделано для автоматического скрипта который создаёт на кукле части повреждения (например Ragdoll_script)
        {
            if(!Main_health)
            Main_health = _health_script;
        }

        void I_damage.Add_damage(int _damage, Game_character_abstract _killer)
        {
            if (Main_health)
                Main_health.Damage_add(_damage, _killer);

            Hit_event.Invoke();
        }
    }
}