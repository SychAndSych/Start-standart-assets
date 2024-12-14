//???? ? ???????? ????????
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;


namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / General / Touch field")]
    [DisallowMultipleComponent]
    public class Touch_field : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerMoveHandler
    {
        [Tooltip("Инвертировать направление по оси движения Y")]//Invert Y direction
        [SerializeField]
        bool Invert_axis_Y = true;

        [Tooltip("Инвертировать направление по оси движения X")]//Invert X direction
        [SerializeField]
        bool Invert_axis_X = false;

        [Tooltip("Таймер остановка движения после того, как палец находится на одном и том же месте")]//Stop motion timer after it is located in the same place
        [SerializeField]
        float Time_stop = 0.01f;

        internal Vector2 TouchDist;

        Vector2 PointerOld;

        internal bool Pressed;
        internal int PressedIndex = -1;

        Coroutine Stop_detect_coroutine = null;

        [Tooltip("Ивент перемещения джостика по экрану")]//Joystick move event
        [SerializeField]
        UnityEvent<Vector2> Position_event = new UnityEvent<Vector2>();

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Pressed)
            {
                PointerOld = eventData.position;
                Pressed = true;
                PressedIndex = eventData.pointerId;
            }
        }

        /// <summary>
        /// ?????? ? ???????? (????????? ?????? ??? ?? ????????)
        /// </summary>
        /// <returns></returns>
        public Vector2 PlayerJoystickOutputVector()
        {
            return TouchDist ;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId == PressedIndex)
            {
                Pressed = false;
                PressedIndex = -1;
                Position_event.Invoke(Vector2.zero);
            }
            
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (eventData.pointerId == PressedIndex && Pressed)
            {
                Vector2 dir = eventData.position - PointerOld;

                dir.x *= Invert_axis_X ? 1 : -1;

                dir.y *= Invert_axis_Y ? 1 : -1;

                TouchDist = dir;

                Position_event.Invoke(TouchDist);

                PointerOld = eventData.position;

                if (Stop_detect_coroutine != null)
                    StopCoroutine(Stop_detect_coroutine);

                Stop_detect_coroutine = StartCoroutine(Coroutine_detect_stop());
            }
        }

        IEnumerator Coroutine_detect_stop()
        {
            yield return new WaitForSeconds(Time_stop);
            TouchDist = Vector2.zero;
            Position_event.Invoke(TouchDist);
        }
    }
}