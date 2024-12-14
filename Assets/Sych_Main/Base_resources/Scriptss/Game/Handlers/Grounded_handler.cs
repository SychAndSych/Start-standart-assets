//������ ���������� �� ��, ��� �� ��������� ����������� ����� ��� ������ (�� �� ��� �� �������� ��� ������������ ��� ������� ;) )
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Handlers / Grounded handler")]
    [DisallowMultipleComponent]
    public class Grounded_handler : MonoBehaviour
    {

        [Tooltip("����� ������ ����� � ����������")]
        public UnityEvent<bool> Grounded_bool_event = new UnityEvent<bool>();

        [Tooltip("����� ������ ����� � ���, ��� �� �� ��������� (�� ���� � �������)")]
        public UnityEvent<bool> Fall_bool_event = new UnityEvent<bool>();

        [Tooltip("����� ����������")]
        [SerializeField]
        float Time_update = 0.1f;

        [Tooltip("������� ��� �����������")]
        [SerializeField]
        float Grounded_Offset = 0.0f;

        [Tooltip("������ ����������� ��������.")]
        [SerializeField]
        float Grounded_radius = 0.15f;

        [Tooltip("����� ���� �������� ���������� � �������� �����")]
        [SerializeField]
        LayerMask Ground_layers = 1;

        bool Active_bool = true;

        [Tooltip("������ ������, ��� �����")]
        [SerializeField]
        bool Debug_mode = true;

        public bool Grounded_bool { get; private set; } = true;//����� �� �� �����

        private void Start()
        {
            StartCoroutine(Coroutine_Update());
        }

        IEnumerator Coroutine_Update()
        {

            while (Active_bool)
            {
                Grounded_check();

                yield return new WaitForSeconds(Time_update);
                
            }

        }


        /// <summary>
        /// ��������� ����� ��� ������
        /// </summary>
        private void Grounded_check()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - Grounded_Offset,
                transform.position.z);
            Grounded_bool = Physics.CheckSphere(spherePosition, Grounded_radius, Ground_layers,
                QueryTriggerInteraction.Ignore);

            Grounded_bool_event.Invoke(Grounded_bool);
            Fall_bool_event.Invoke(!Grounded_bool);
        }


        private void OnDrawGizmosSelected()
        {
            if (Debug_mode)
            {
                Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
                Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

                if (Grounded_bool) Gizmos.color = transparentGreen;
                else Gizmos.color = transparentRed;

                // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
                Gizmos.DrawSphere(
                    new Vector3(transform.position.x, transform.position.y - Grounded_Offset, transform.position.z),
                    Grounded_radius);
            }

        }

    }
}