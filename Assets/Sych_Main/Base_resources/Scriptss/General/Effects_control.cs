//Контроль систем частиц и их запуск
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / General / Effects control")]
    [DisallowMultipleComponent]
    public class Effects_control : MonoBehaviour
    {

        [Tooltip("Список систем частиц")]
        [SerializeField]
        Effects_class[] Effects_class_array = new Effects_class[0];



        /// <summary>
        /// Включить определённые эффекты
        /// </summary>
        /// <param name="_id_effect">Номер частицы</param>
        public void On_effect(int _id_effect)
        {
            Activity_effects(true, _id_effect, "");
        }

        /// <summary>
        /// Включить определённые эффекты
        /// </summary>
        /// <param name="_name_effect">Имя эффектов</param>
        public void On_effect(string _name_effect)
        {
            Activity_effects(true, -1, _name_effect);
        }

        /// <summary>
        /// Отключить определённые эффекты
        /// </summary>
        /// <param name="_id_effect">Номер эффектов</param>
        public void Off_effect(int _id_effect)
        {
            Activity_effects(false, _id_effect, "");
        }

        /// <summary>
        /// Отключить определённые эффекты
        /// </summary>
        /// <param name="_name_effect">Имя эффектов</param>
        public void Off_effect(string _name_effect)
        {
            Activity_effects(false, -1, _name_effect);
        }


        /// <summary>
        /// Отключает или включает эффекты
        /// </summary>
        /// <param name="_activity">Вкл/Выкл</param>
        /// <param name="_id">Номер id эффекта</param>
        /// <param name="_name">Имя эффекта</param>
        void Activity_effects(bool _activity, int _id, string _name)
        {
            if (_id >= 0)
            {
                if (Effects_class_array[_id].PS != null)
                {
                    if (_activity)
                        Effects_class_array[_id].PS.Play();
                    else
                        Effects_class_array[_id].PS.Stop();
                }


                if (Effects_class_array[_id].VE != null)
                {
                    if (_activity)
                        Effects_class_array[_id].VE.Play();
                    else
                        Effects_class_array[_id].VE.Stop();
                }

            }

            if (_name != "")
            {
                for (int id_effect = 0; id_effect < Effects_class_array.Length; id_effect++)
                {
                    if (Effects_class_array[id_effect].Name == _name)
                    {
                        if (Effects_class_array[id_effect].PS != null)
                        {
                            if (_activity)
                                Effects_class_array[id_effect].PS.Play();
                            else
                                Effects_class_array[id_effect].PS.Stop();
                        }


                        if (Effects_class_array[id_effect].VE != null)
                        {
                            if (_activity)
                                Effects_class_array[id_effect].VE.Play();
                            else
                                Effects_class_array[id_effect].VE.Stop();
                        }
                    }
                }
            }

        }



        [System.Serializable]
        public class Effects_class
        {
            [Tooltip("Название, что бы не забыть, что это и за что отвечает.")]
            public string Name = "Название";

            [Tooltip("Сама система частиц")]
            public ParticleSystem PS = null;

            [Tooltip("Визуальный эффект (от графа)")]
            public VisualEffect VE = null;

            //[Tooltip("Будет ли она зацикленная")]
            //public bool Loop_bool = false; 
        }
    }
}