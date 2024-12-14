//Скрипт для растворение через шейдер погибших сущностей
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Test / Test Dissolution")]
    public class Test_Dissolution : MonoBehaviour
    {
        [Tooltip("Материал персонажа")]
        [SerializeField]
        Renderer Mat = null;

        [Tooltip("Время растворения")]
        [SerializeField]
        float Time_destroy = 5f;

        private void Start()
        {
            StartCoroutine(Coroutine_Auto_destroy());
        }


        IEnumerator Coroutine_Auto_destroy()
        {
            float material_default_value_ = Mat.material.GetFloat("_Edge");

            float time = Time_destroy;

            yield return new WaitForSeconds(2);

            GetComponent<Effects_control>().On_effect("Death");

            while (time > 0)
            {

                time -= Time.deltaTime;
                Mat.material.SetFloat("_Edge", material_default_value_ * (time / Time_destroy));
                yield return new WaitForSeconds(Time.deltaTime);
            }

            GetComponent<Effects_control>().Off_effect("Death");
            yield return new WaitForSeconds(3);

            Destroy(gameObject);
        }
    }
}