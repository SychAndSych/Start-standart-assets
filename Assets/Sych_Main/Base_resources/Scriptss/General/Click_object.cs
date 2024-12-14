using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / General / Click object")]
    [DisallowMultipleComponent]
    public class Click_object : MonoBehaviour
    {

        [Tooltip("Ивент когда кликаем по объекту")]
        [SerializeField]
        UnityEvent Click_event = new UnityEvent();

        private void OnMouseDown()
        {
            Click_event.Invoke();
        }
    }
}