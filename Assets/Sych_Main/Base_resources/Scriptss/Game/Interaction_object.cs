//������� � ������� ����� �����������������
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

        [Tooltip("����� ��������������")]
        [SerializeField]
        UnityEvent Interact_event = new UnityEvent();

        [Tooltip("������ ������")]
        [SerializeField]
        bool Debug_bool = false;

        public Interaction_enum Interaction_type => Interaction_enum.Take;

        /// <summary>
        /// ������������ ��������������
        /// </summary>
        public void Interaction()
        {
            Interact_event.Invoke();

            if (Debug_bool)
            {
                Debug.Log("��������������� � " + gameObject.name);
            }
        }

    }
}