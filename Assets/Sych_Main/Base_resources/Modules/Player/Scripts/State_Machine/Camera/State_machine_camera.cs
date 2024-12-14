namespace Sych_scripts
{
    public class State_machine_camera
    {
        public Camera_state_abstract Current_State { get; private set; }

        /// <summary>
        /// Стартовое состояние
        /// </summary>
        /// <param name="starting_State">Состояние</param>
        public void Initialize(Camera_state_abstract starting_State)
        {
            Current_State = starting_State;
            starting_State.Enter_state();
        }



        /// <summary>
        /// Сменить состояние на другоие
        /// </summary>
        /// <param name="new_State">новое состояние</param>
        public void Change_State(Camera_state_abstract new_State)
        {
            Current_State.Exit_state();

            Current_State = new_State;
            new_State.Enter_state();
        }

        /// <summary>
        /// Узнать какое текущее состояние
        /// </summary>
        public Camera_state_abstract Find_out_Current_State
        {
            get
            {
                return Current_State;
            }
        }

    }
}