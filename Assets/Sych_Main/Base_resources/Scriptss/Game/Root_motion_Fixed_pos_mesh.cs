//Фиксирует и смещает основное тело объекта к мешу который ушёл из-за RootMotion анимации
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / Root motion Fixed pos mesh")]
    [DisallowMultipleComponent]
    public class Root_motion_Fixed_pos_mesh : MonoBehaviour
    {

        [Tooltip("Меш игрока")]
        [field: SerializeField]
        public Transform Mesh { get; private set; } = null;

        protected void LateUpdate()
        {
            if (Mesh.localPosition != Vector3.zero)
            {

                transform.position = Mesh.position;

                Mesh.localPosition = Vector3.zero;
                Mesh.localRotation = Quaternion.identity;
            }
        }
    }
}