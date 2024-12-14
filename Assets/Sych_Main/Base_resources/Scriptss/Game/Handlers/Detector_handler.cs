//Скрипт для детекции чего либо с нужным тегом
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Handlers / Detector handler")]
    public class Detector_handler : MonoBehaviour
    {

        [Tooltip("Размер триггера")]
        [SerializeField]
        float Radius = 4f;

        [Tooltip("Через какой интервал включает детектор для поиска")]
        [SerializeField]
        float Timer_update = 2;

        [Tooltip("Смещение")]
        [SerializeField]
        Vector3 OffSet = Vector3.zero;

        [Tooltip("Массив искоммых объектов")]
        [SerializeField]
        string[] Tag_array = new string[1] { "Untagged" };

        [Tooltip("Слои подлежащие для взаимодействия")]
        [SerializeField]
        LayerMask Layer = 1 << 0;

        [Tooltip("Результат (показывает кого нашёл детектор)")]
        [SerializeField]
        UnityEvent<Transform> Detect_result_event = new UnityEvent<Transform>();

        [Tooltip("Видеть больше")]
        [SerializeField]
        bool Debug_bool = false;

        [ShowIf(nameof(Debug_bool))]
        [Tooltip("Цвет отображаемой зоны поиска")]
        [SerializeField]
        Color Color_debug = new Color(0, 0, 1, 0.2f);

        private void Start()
        {
            StartCoroutine(Coroutine_Update());
        }

        IEnumerator Coroutine_Update()
        {
            while (true)
            {
                Detect();
                yield return new WaitForSeconds(Timer_update);
            }

        }


        void Detect()
        {
            Collider[] check_array = Physics.OverlapSphere(transform.position + OffSet, Radius, Layer);

            for (int x = 0; x < check_array.Length; x++)
            {
                for (int l = 0; l < Tag_array.Length; l++)
                {
                    if (check_array[x].tag == Tag_array[l])
                    {
                        Detect_result_event.Invoke(transform);
                        break;
                    }
                }
            }
        }


        private void OnDrawGizmos()
        {
            if (Debug_bool)
            {
                Gizmos.color = Color_debug;
                Gizmos.DrawSphere(transform.position + OffSet, Radius);
            }

        }

    }
}