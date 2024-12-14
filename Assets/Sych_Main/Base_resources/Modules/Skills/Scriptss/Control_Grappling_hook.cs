using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts 
{
    [AddComponentMenu("Sych scripts / Game / Skills / Control Grappling hook")]
    [DisallowMultipleComponent]
    public class Control_Grappling_hook : MonoBehaviour
    {
        [Tooltip("Камера игрока")]
        //[SerializeField]
        Camera Cam = null;

        [Tooltip("Скрипт крюка-кошки")]
        [SerializeField]
        Grappling_hook Grappling_hook_script = null;

        [Tooltip("С кем взаимодействуем ? (нужно для пробрасывания луча)")]
        [SerializeField]
        LayerMask Layer_mask = 1;

        bool Active_bool = false;

        private void Start()
        {
            Cam = Game_administrator.Singleton_Instance.Player_administrator.Cam;
        }

        [ContextMenu("Запустить крюк-кошку")]
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