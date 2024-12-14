//���������� ���� ��� ��������������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Inventory / Character / Interact inventory and item")]
    [DisallowMultipleComponent]
    public class Interact_inventory_and_item : MonoBehaviour
    {
        [Tooltip("����� �� ������� ���� (����� ��������� ��� �� ������)")]
        [SerializeField]
        bool First_person_mode_bool = true;

        [HideIf(nameof(First_person_mode_bool))]
        [Tooltip("����� ������� �������� �� ����������� ���������� ����")]
        [SerializeField]
        Transform Forward_transform = null;

        [Tooltip("��������� ��������������")]
        [SerializeField]
        float Distance = 6f;

        [Tooltip("������ �������������� (��������� ������� ���)")]
        [SerializeField]
        float Radius_interact = 0.5f;

        [Tooltip("���� ��� ��������������")]
        [SerializeField]
        LayerMask Layer = 1;

        [Tooltip("������ ���������")]
        [SerializeField]
        Object_inventory Inventory_script = null;

        [Tooltip("������")]
        [SerializeField]
        Camera Cam = null;

        [Tooltip("������� ������")]
        [SerializeField]
        bool Debug_bool = false;

        /// <summary>
        /// ��������������
        /// </summary>
        void Interact()
        {
            RaycastHit hit;

            Ray ray = First_person_mode_bool ? Cam.ScreenPointToRay(Mouse.current.position.ReadValue()) : new Ray(Forward_transform.position, Forward_transform.forward);

            bool result_bool = false;
            if (Physics.SphereCast(ray, Radius_interact, out hit, Distance, Layer))
            {
                if (hit.transform.GetComponent<I_Pick_up>() != null && hit.transform.GetComponent<Item_Pick_up>())
                {
                    hit.transform.GetComponent<I_Pick_up>().Add_inventory(Inventory_script, hit.transform.GetComponent<Item_Pick_up>());
                    result_bool = true;
                }
                else if (hit.transform.GetComponent<I_open_object_inventory>() != null && hit.transform.GetComponent<Object_inventory>())
                {
                    if (!Inventory_UI.Singleton_Instance.Open_inventory_bool) 
                    {
                        if (hit.transform.GetComponent<Object_inventory>().Interact_inventory_bool)
                        {
                            hit.transform.GetComponent<I_open_object_inventory>().Open_inventory_object(hit.transform.GetComponent<Object_inventory>());
                            result_bool = true;
                        }
                    }
                }
            }

            if (Inventory_UI.Singleton_Instance.Open_inventory_bool && !result_bool)
                Inventory_UI.Singleton_Instance.Button_inventory();
        }


        private void OnDrawGizmosSelected()
        {
            if (Debug_bool)
            {
                Gizmos.color = Color.yellow;

                Vector3 start_pos = First_person_mode_bool ? Cam.transform.position : Forward_transform.position;

                Vector3 direction = First_person_mode_bool ?  Cam.transform.forward : Forward_transform.forward;

                Vector3 end_pos = start_pos + direction * Distance;

                Gizmos.DrawLine(start_pos, end_pos);

                Gizmos.DrawWireSphere(end_pos, Radius_interact);
            }
        }

    }
}