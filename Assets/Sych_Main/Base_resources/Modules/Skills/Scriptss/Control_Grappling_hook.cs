using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts 
{
    [AddComponentMenu("Sych scripts / Game / Skills / Control Grappling hook")]
    [DisallowMultipleComponent]
    public class Control_Grappling_hook : MonoBehaviour
    {
        [Tooltip("������ ������")]
        //[SerializeField]
        Camera Cam = null;

        [Tooltip("������ �����-�����")]
        [SerializeField]
        Grappling_hook Grappling_hook_script = null;

        [Tooltip("� ��� ��������������� ? (����� ��� ������������� ����)")]
        [SerializeField]
        LayerMask Layer_mask = 1;

        bool Active_bool = false;

        private void Start()
        {
            Cam = Game_administrator.Singleton_Instance.Player_administrator.Cam;
        }

        [ContextMenu("��������� ����-�����")]
        public void Activation()
        {
            Active_bool = !Active_bool;

            if (!Grappling_hook_script.Active_bool)
            {
                Ray ray = new Ray(Cam.transform.position, Cam.transform.forward);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Grappling_hook_script.Max_distance_hook, Layer_mask))
                {
                    if (hit.transform != null)
                    {
                        Vector3 direction = hit.point - transform.position;
                        direction.Normalize();

                        Grappling_hook_script.Activation(transform.position, direction);
                    }
                }

            }
            else
            {
                Grappling_hook_script.Cancel();
            }


        }

    }
}