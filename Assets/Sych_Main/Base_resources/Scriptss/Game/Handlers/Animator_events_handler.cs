//������ �������������� ������ � ��������� (�������� ������ ���������)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Handlers / Animator events handler")]
    [DisallowMultipleComponent]
    public class Animator_events_handler : StateMachineBehaviour
    {

        [Tooltip("��������� ������� ��� ������ ��������")]
        [SerializeField]
        bool Enter_bool = true;

        [ConditionalHide(nameof(Enter_bool), true)]
        [Tooltip("��� ������� ��� ������ ��������")]
        [SerializeField]
        string Name_event_enter = "Event_enter";

        [Tooltip("��������� ������� ��� ����� ��������")]
        [SerializeField]
        bool Exit_bool = true;

        [ConditionalHide(nameof(Exit_bool), true)]
        [Tooltip("��� ������� ��� ������ ��������")]
        [SerializeField]
        string Name_event_exit = "Event_exit";

        Animation_clips_events_handler Animation_clips_events_handler_script = null;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log(1);
            if (!Animation_clips_events_handler_script)
            {
                if (animator.GetComponent<Animation_clips_events_handler>())
                {
                    Animation_clips_events_handler_script = animator.GetComponent<Animation_clips_events_handler>();
                }
                else
                {
                    Debug.LogError("���� ������� " + nameof(Animation_clips_events_handler) + "!!!");
                }
            }

            if (Enter_bool)
                Animation_clips_events_handler_script.Activation_animation_event(Name_event_enter);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log(2);

            if (Exit_bool)
                Animation_clips_events_handler_script.Activation_animation_event(Name_event_exit);
        }

        /*
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("On Attack Update ");
        }

        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("On Attack Move ");
        }

        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("On Attack IK ");
        }
        */
    }
}