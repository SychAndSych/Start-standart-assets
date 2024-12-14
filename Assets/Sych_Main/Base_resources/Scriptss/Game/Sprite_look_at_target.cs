//Скрипт для игр, где мир в 3D, а персонажи и вещи это плоские спрайты
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / Sprite look at target")]
    [DisallowMultipleComponent]
    public class Sprite_look_at_target : MonoBehaviour
    {
        [Tooltip("Поворачиваемая часть")]
        [SerializeField]
        Transform Sprite_rotation_transform = null;

        [Tooltip("Аниматор (для задавая индекса отображаемого спрайта(перед, зад и тд)")]
        [SerializeField]
        Animator Anim = null;

        [Tooltip("Камера")]
        //[SerializeField]
        Camera Cam = null;

        private void Start()
        {
            Cam = Game_administrator.Singleton_Instance.Player_administrator.Cam;
        }

        private void Update()
        {
            if (Anim)
                Direction_anim();
        }

        private void LateUpdate()
        {
            Sprite_rotation_transform.LookAt(Cam.transform);
            Sprite_rotation_transform.eulerAngles = new Vector3(0, Sprite_rotation_transform.eulerAngles.y, 0);
        }

        /// <summary>
        /// Вычисление с какой стороны цель, что бы понять какую сторону должен показывать спрайт (перед, зад и тд)
        /// </summary>
        void Direction_anim()
        {

            float angle = Vector3.Angle(transform.position - Cam.transform.position, transform.forward);

            if (angle <= 90)
            {
                Anim.SetFloat("Direction", 0);
            }
            else
            {
                Anim.SetFloat("Direction", 1);
            }
        }


    }
}