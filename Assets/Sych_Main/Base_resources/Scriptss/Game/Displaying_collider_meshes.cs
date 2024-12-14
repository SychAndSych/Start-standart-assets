//Отображение мешей невидимых коллайдеров (которые должны быть выключены в игре для отображения)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Other / Displaying collider meshes")]
    [DisallowMultipleComponent]
    public class Displaying_collider_meshes : MonoBehaviour
    {

        [Tooltip("Рендер мешей")]
        [SerializeField]
        private Renderer[] Renderer_mesh_array = new Renderer[0];

        [Tooltip("Цвет меша")]
        [SerializeField]
        private Color Color_mesh = new Color(0.8f, 0f, 0f, 0.5f);

        bool Play_bool = false;//Игра запущена

        private void Start()
        {
            Play_bool = true;

            for (int x = 0; x < Renderer_mesh_array.Length; x++)
            {
                Renderer_mesh_array[x].enabled = false;
            }
        }

        private void OnDrawGizmos()
        {
            if (!Play_bool)
            {

                if (Renderer_mesh_array.Length > 0 && Renderer_mesh_array[0] != null && (Renderer_mesh_array[0].material.color != Color_mesh || Renderer_mesh_array[0].enabled == false))
                {
                    for (int x = 0; x < Renderer_mesh_array.Length; x++)
                    {
                        if (Renderer_mesh_array[x] != null && Renderer_mesh_array[x].enabled == false)
                        {
                            Renderer_mesh_array[x].enabled = true;
                            Renderer_mesh_array[x].material.color = Color_mesh;
                        }
                    }
                }


            }
        }

    }
}