using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Sych_scripts
{
    public abstract class State_abstract : MonoBehaviour
    {
        [field: Tooltip("Имя состояния (нужно для обращения к этому состоянию, когда переключаемся из другого)")]
        [field: SerializeField]
        public string Name_state { get; private set; } = "Name state";

        [field: ReadOnly]
        [field: SerializeField]
        public bool Active_bool { get; private set; } = false;

        [Tooltip("Ивент при старте (включение) состояние")]
        [SerializeField]
        UnityEvent Start_state_event = new UnityEvent();

        [Tooltip("Ивент при конце (выключение) состояние")]
        [SerializeField]
        UnityEvent End_state_event = new UnityEvent();

        protected Game_object_abstract Character_script;
        protected StateMachine State_Machine_script;

        bool Start_preparation_bool = false;//Подготовка (проводится 1 раз при первом запуске состояния)

        public virtual void Start_preparation_SM(Game_object_abstract character, StateMachine stateMachine)
        {
            Character_script = character;
            State_Machine_script = stateMachine;
        }

        /*
        public State_abstract(Game_character_abstract character, StateMachine stateMachine)
        {
            Character_script = character;
            State_Machine_script = stateMachine;
        }
        */

        /// <summary>
        /// Вход состояния (начало реализации)
        /// </summary>
        public virtual void Enter_state()
        {
            if (!Start_preparation_bool)
            {
                Start_preparation_bool = true;
                //Invoke(nameof(Preparation), 0.02f);
                Preparation();
            }

            Start_state_event.Invoke();

            Active_bool = true;
        }

        /// <summary>
        /// Начальная подготовка
        /// </summary>
        protected virtual void Preparation()
        {
            Initialized_stats();
        }

        /// <summary>
        /// Назначает все необходимые параметры из конфига
        /// </summary>
        protected abstract void Initialized_stats();

        /// <summary>
        /// Блок относящийся к задаванию управления (в основном от игрока)
        /// </summary>
        public virtual void Handle_Input()
        {

        }


        /// <summary>
        /// Логика с большей задержкой, чем Update (так сказать для снижения нагрузки)
        /// </summary>
        public virtual void Slow_Update()
        {

        }

        /// <summary>
        /// Логика с большей задержкой, чем Update (второй) (так сказать для снижения нагрузки)
        /// </summary>
        public virtual void Slow_Update_2()
        {

        }


        /// <summary>
        /// Логика поведения этого состояния
        /// </summary>
        public virtual void Logic_Update()
        {

        }

        /// <summary>
        /// Физика и методы которые к ней относятся (например передвижение)
        /// </summary>
        public virtual void Physics_Update()
        {

        }


        /// <summary>
        /// Выход из состояния
        /// </summary>
        public virtual void Exit_state()
        {
            Active_bool = false;

            End_state_event.Invoke();
        }
    }
}