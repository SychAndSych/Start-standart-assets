//Отвечает за смену состояний поведения
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / Brain SM")]
    [DisallowMultipleComponent]
    public class Brain_SM : MonoBehaviour
    {
        #region Переменные
        [field: Tooltip("Ссылка на основной скрипт персонажа (это может быть даже предмет!)")]
        [field: SerializeField]
        protected Game_object_abstract Main_character { get; private set; } = null;

        [Tooltip("Массив состояний поведения (инициализируется (запускается) всегда первым самое первое состояние)")]
        [SerializeField]
        State_abstract[] State_array = new State_abstract[0];

        protected StateMachine State_Machine { get; private set; } = null;

        [Tooltip("Время обновления для медленного Update")]
        [SerializeField]
        float Time_slow_update = 0.4f;

        [Tooltip("Время обновления для медленного Update_2 (второй)")]
        [SerializeField]
        float Time_slow_update_2 = 0.5f;

        Coroutine Slow_update_coroutine = null;
        Coroutine Slow_update_coroutine_2 = null;

        protected bool Alive_bool = true;

        #endregion


        #region MonoBehaviour Callbacks
        protected virtual void Start()
        {
            Initialized_State_machine();

        }


        protected virtual void Update()
        {
            if (Alive_bool && State_Machine.Current_State != null)
            {
                State_Machine.Current_State.Handle_Input();
                State_Machine.Current_State.Logic_Update();
            }

        }

        protected virtual void FixedUpdate()
        {
            if (Alive_bool && State_Machine.Current_State != null)
            {
                State_Machine.Current_State.Physics_Update();
            }
        }
        #endregion


        #region Методы

        /// <summary>
        /// Инициализация машины состояния
        /// </summary>
        protected virtual void Initialized_State_machine()
        {
            State_Machine = new StateMachine();
            
            Invoke(nameof(Delay_Initialized_State_machine), 0.1f);

        }

        /// <summary>
        /// Задержка второй части Инициализации машины состояния
        /// </summary>
        void Delay_Initialized_State_machine()
        {
            if (State_array.Length > 0)
            {
                for (int x = 0; x < State_array.Length; x++)
                {
                    State_array[x].Start_preparation_SM(Main_character, State_Machine);
                }

                State_Machine.Initialize(State_array[0]);
            }
            else
            {
                if (Main_character != null)
                    Debug.LogError("Не заданы состояния поведения! " + "(примерно объект называется  " + Main_character.name + "  )");
                else
                    Debug.LogError("Не указан скрипт самого персонажа!");
            }

            if (State_Machine.Current_State != null)
            {
                Slow_update_coroutine = StartCoroutine(Coroutine_Slow_update());
                Slow_update_coroutine_2 = StartCoroutine(Coroutine_Slow_update_2());
            }
        }


        IEnumerator Coroutine_Slow_update()
        {
            while (true)
            {
                if (Alive_bool && State_Machine.Current_State != null)
                {
                    State_Machine.Current_State.Slow_Update();
                }

                yield return new WaitForSeconds(Time_slow_update);
            }

        }

        IEnumerator Coroutine_Slow_update_2()
        {
            while (true)
            {
                if (Alive_bool && State_Machine.Current_State != null)
                {
                    State_Machine.Current_State.Slow_Update_2();
                }

                yield return new WaitForSeconds(Time_slow_update_2);
            }

        }
        #endregion


        #region Управляющие методы
        /// <summary>
        /// Изменить состояние поведения по имени
        /// </summary>
        /// <param name="_name">Имя состояния</param>
        public void Change_state(string _name)
        {
            bool result = false;

            for (int x = 0; x < State_array.Length; x++)
            {
                if (State_array[x].Name_state == _name)
                {
                    if (State_Machine.Current_State != State_array[x])
                        State_Machine.Change_State(State_array[x]);

                    result = true;
                    break;
                }
            }

            if(!result)
            Debug.LogError("Состояние поведения по имени  " + _name + "  не найдено!");
        }

        /// <summary>
        /// Изменить состояние поведения по id
        /// </summary>
        /// <param name="_name">Имя состояния</param>
        public void Change_state(int _id)
        {
            if(State_array.Length - 1 < _id || _id < 0)
            Debug.LogError("Состояние поведения по id  " + _id + "  не существует!");

            if(State_Machine.Current_State != State_array[_id])
            State_Machine.Change_State(State_array[_id]);
        }


        /// <summary>
        /// Выключить состояния
        /// </summary>
        public void Off_state()
        {
            State_Machine.Change_State(null);
        }


        /// <summary>
        /// Жизни закончились
        /// </summary>
        public virtual void Dead()
        {
            StopAllCoroutines();

            Alive_bool = false;

            print("Мозг всё.");

        }

        #endregion
    }
}
