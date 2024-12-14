using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{

    [AddComponentMenu("Sych scripts / Game / Player / Interact player")]
    [DisallowMultipleComponent]
    public class Interact_player : MonoBehaviour
    {
        #region Переменные

        [Tooltip("Режим рейкаста из камеры")]
        [SerializeField]
        bool Camera_mode_bool = false;

        [ShowIf(nameof(Camera_mode_bool))]
        [Tooltip("Камера игрока")]
        [SerializeField]
        Camera Cam = null;

        [HideIf(nameof(Camera_mode_bool))]
        [Tooltip("Объект из которого будет выпускаться луч и который будет задавать направление")]
        [SerializeField]
        Transform Transform_forward = null;

        [HideIf(nameof(Camera_mode_bool))]
        [Tooltip("Дополнительное смещение")]
        [SerializeField]
        Vector3 Offset_position = Vector3.zero;

        [Tooltip("Дальность взаимодействия")]
        [SerializeField]
        float Distance_interact = 10f;

        [Tooltip("Радиус взаимодействия взаимодействия (зона взаимодействия становится больше")]
        [SerializeField]
        float Radius_interact = 0.5f;

        [Tooltip("С кем взаимодействуем ? (нужно для пробрасывания луча)")]
        [SerializeField]
        LayerMask Layer_mask = 1;

        [Tooltip("Включает ли переодичную проверку с чем может взаимодействовать игрок")]
        [SerializeField]
        bool Check_interact_mode_bool = true;

        [ShowIf(nameof(Check_interact_mode_bool))]
        [Tooltip("Проверять можно ли взаимодействовать")]
        [SerializeField]
        float Time_check = 1f;

        [Tooltip("Увидеть больше")]
        [SerializeField]
        bool Debug_bool = false;

        #endregion


        #region Системные методы
        private void Start()
        {
            if(Check_interact_mode_bool)
            StartCoroutine(Coroutine_update_check_interaction());
        }
        #endregion


        #region Методы

        /// <summary>
        /// Пустить луч и получить объект
        /// </summary>
        RaycastHit Find_out_hit_interaction
        {
            get
            {
                //Ray ray = First_person_mode_bool ? Cam.ScreenPointToRay(Mouse.current.position.ReadValue()) : new Ray(Forward_transform.position, Forward_transform.forward);
                Ray ray = Camera_mode_bool ? new Ray(Cam.transform.position, Cam.transform.forward) : new Ray(Transform_forward.position + Offset_position, Transform_forward.forward);

                RaycastHit hit;

                bool one_ray_interact = false;

                if (Physics.Raycast(ray, out hit, Distance_interact, Layer_mask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform != null)
                    {
                        if (hit.transform.GetComponent<I_Interaction>() != null)
                        {
                            //hit.transform.GetComponent<I_Interaction>().Interaction();
                            one_ray_interact = true;
                        }

                    }
                }

                if (!one_ray_interact)
                {
                    /*
                    if (Physics.SphereCast(ray, Radius_interact, out hit, Distance_interact, Layer_mask, QueryTriggerInteraction.Ignore))
                    {
                        if (hit.transform != null)
                        {
                            print(02 + " " + hit.transform.name);
                            if (hit.transform.GetComponent<I_Interaction>() != null)
                            {
                                hit.transform.GetComponent<I_Interaction>().Interaction();
                                print(2 + " " + hit.transform.name);
                            }

                        }

                    }
                    */

                    RaycastHit[] hits = Physics.SphereCastAll(ray, Radius_interact, Distance_interact, Layer_mask, QueryTriggerInteraction.Ignore);

                    foreach (RaycastHit hit_active in hits)
                    {
                        if (hit_active.transform != null)
                        {
                            if (hit_active.transform.GetComponent<I_Interaction>() != null)
                            {
                                if (hit.transform == null || Vector3.Distance(transform.position, hit.transform.position) > Vector3.Distance(transform.position, hit_active.transform.position))
                                    hit = hit_active;
                            }
                        }
                    }
                }

                return hit;
            }

        }

        /// <summary>
        /// Обновлять и проверять перед собой объекты с которыми можно взаимодействовать
        /// </summary>
        /// <returns></returns>
        IEnumerator Coroutine_update_check_interaction()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(Time_check);

                RaycastHit hit = Find_out_hit_interaction;

                if(hit.transform != null)
                {
                    Interact_UI.Singleton_Instance.Change_image(hit.transform.GetComponent<I_Interaction>().Find_out_type_object);
                }
                else
                {
                    Interact_UI.Singleton_Instance.Off_image();
                }
            }
        }

        #endregion


        #region Публичные методы

        /// <summary>
        /// Пустить луч, что бы найти с чем взаимодействовать
        /// </summary>
        public void Activation_interact()
        {
            RaycastHit hit = Find_out_hit_interaction;

            if (hit.transform != null && hit.transform.GetComponent<I_Interaction>() != null)
                hit.transform.GetComponent<I_Interaction>().Interaction();
        }

        #endregion


        #region Проверочные методы

        private void OnDrawGizmos()
        {
            if (Debug_bool)
            {
                Gizmos.color = Color.yellow;

                Vector3 start_pos = Camera_mode_bool ? Cam.transform.position : Transform_forward.position + Offset_position;

                Vector3 direction = Camera_mode_bool ? Cam.transform.forward : Transform_forward.forward;

                Vector3 end_pos = start_pos + direction * Distance_interact;

                Gizmos.DrawLine(start_pos, end_pos);

                Gizmos.DrawWireSphere(end_pos, Radius_interact);
            }
        }

        #endregion

    }
}