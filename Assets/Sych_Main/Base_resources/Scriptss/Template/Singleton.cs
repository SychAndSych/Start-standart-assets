//Шаблон для того, что бы наследующий скрипт был доступен во всей сцене
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [DisallowMultipleComponent]
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
         static T Singleton_instance { get; set; } = null;

        protected virtual void Awake()
        {
            if (!Singleton_instance)
            {
                Singleton_instance = (T)(object)this;//FindObjectOfType<T>();
            }
            else if(Singleton_instance != this)
            {
                //Debug.LogWarning("Обнаружен ещё один экземпляр " + this + "!" + "(по этому он был удалён, который был на объекте " + gameObject.name + " .)");
                Debug.LogWarning("Обнаружен ещё один экземпляр " + this + " и он по этому удалён! (а первоначальный находится на " + Singleton_instance.name + " )");
                Destroy(this);
            }
        }


        public static T Singleton_Instance
        {
            get
            {

                if (!Singleton_instance)
                {
                    Singleton_instance = FindObjectOfType<T>();//FindObjectOfType<T>();
                }

                return Singleton_instance;
            }
        }


        /*
        public static T Singleton_Instance
        {
            get
            {
                
                if (Singleton_instance == null)
                {
                    Singleton_instance = FindObjectOfType<T>();
                }
                
                return Singleton_instance;
            }
        }
        */
    }
}