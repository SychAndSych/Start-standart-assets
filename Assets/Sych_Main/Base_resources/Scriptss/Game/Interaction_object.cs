//Ёлемент с которым будем взаимодействовать
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / Interaction object")]
    [DisallowMultipleComponent]
    public class Interaction_object : MonoBehaviour, I_Interaction
    {

        [Tooltip("»вент взаимодействи€")]
        [SerializeField]
        UnityEvent Interact_event = new UnityEvent();

        [Tooltip("”знать больше")]
        [SerializeField]
        bool Debug_bool = false;

        public Interaction_enum Interaction_type => Interaction_enum.Take;

        /// <summary>
        /// јктивировать взаимодействие
        /// </summary>
        public void Interaction()
        {
            Interact_event.Invoke();

            if (Debug_bool)
            {
                Debug.Log("¬заимодействуем с " + gameObject.name);
            }
        }

    }
}