using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Lean.Pool;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / Destroy GameObject")]
    [DisallowMultipleComponent]
    public class Destroy_object : MonoBehaviour
    {

        [Tooltip("�������� ������� � ��������")]
        [SerializeField]
        bool Timer_bool = true;

        [ShowIf(nameof(Timer_bool))]
        [Tooltip("����� ����� ������� ����� ��������� ������")]
        [SerializeField]
        float Time_destroy = 5f;

        [Tooltip("���������� ��� ���������� � ��� (Pool)")]
        [SerializeField]
        bool Destroy_no_pool = false;

        private void OnEnable()
        {
            if(Timer_bool)
            StartCoroutine(Coroutine_timer());
        }

        private void OnParticleSystemStopped()
        {
            Destroy_obj();
        }

        IEnumerator Coroutine_timer()
        {
            yield return new WaitForSeconds(Time_destroy);
            Destroy_obj();

        }

        /// <summary>
        /// ���������� ������ (����������)
        /// </summary>
        public void Destroy_obj()
        {
            StopAllCoroutines();

            if (Destroy_no_pool)
                Destroy(gameObject);
            else
                LeanPool.Despawn(gameObject);
        }

    }
}