//Набор параметров для персонажей
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Sych_scripts
{

    //[RequireComponent(typeof(Health))]//Атрибут добавляющий здоровье
    [DisallowMultipleComponent]
    public abstract class Game_character_abstract : Game_object_abstract
    {
        #region Variables


#pragma warning disable 0649

        [field: Space(20)]
        [field: Header("Настройки персонажа")]

        [field: Tooltip("К какой фракции относится персонаж (к какой команде относится персонаж)")]
        [field: SerializeField]
        public string Fraction_name { get; private set; } = "Team_name";

        [field: Tooltip("Мозг который отвечает за состояния поведения")]
        [field: SerializeField]
        public Brain_SM Brain_script { get; private set; } = null;

        //[FormerlySerializedAs(oldName: "Health_active")] // При переименование поля, можно создать ссылку на старое имя, что бы не переделывать (нужно не забыть using UnityEngine.Serialization;)
        [field: Foldout("Не обязательное")]
        [field: Tooltip("Голова (если есть)")]
        [field: SerializeField]
        public Transform Head { get; private set; } = null;

        public bool Control_bool { get; private set; } = true;//Контролирует ли игрок персонажа

        internal UnityEvent<bool> Control_event = new UnityEvent<bool>();//Эвент который говорит, можем ли мы сейчас управлять

        protected bool Alive_bool = true;

        #endregion



        #region Управляющие методы


        /// <summary>
        /// Включить/отключить контроль персонажем
        /// </summary>
        /// <param name="_active">Активность</param>
        public virtual void Active_control(bool _active)
        {
            Control_bool = _active;
            Control_event.Invoke(_active);
        }

        /// <summary>
        /// Жизни закончились
        /// </summary>
        public virtual void Dead()
        {
            Active_control(false);

            StopAllCoroutines();

            Alive_bool = false;

            print("Я всё.");

        }

        #endregion

    }
}