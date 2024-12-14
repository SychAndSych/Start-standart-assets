namespace Sych_scripts
{
    public class State_machine_camera
    {
        public Camera_state_abstract Current_State { get; private set; }

        /// <summary>
        /// ��������� ���������
        /// </summary>
        /// <param name="starting_State">���������</param>
        public void Initialize(Camera_state_abstract starting_State)
        {
            Current_State = starting_State;
            starting_State.Enter_state();
        }



        /// <summary>
        /// ������� ��������� �� �������
        /// </summary>
        /// <param name="new_State">����� ���������</param>
        public void Change_State(Camera_state_abstract new_State)
        {
            Current_State.Exit_state();

            Current_State = new_State;
            new_State.Enter_state();
        }

        /// <summary>
        /// ������ ����� ������� ���������
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