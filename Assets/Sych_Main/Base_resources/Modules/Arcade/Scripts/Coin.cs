using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Arcade / Coin")]
    [DisallowMultipleComponent]
    public class Coin : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player_character>())
            {
                Give();
            }
        }

        /// <summary>
        /// Подобрать
        /// </summary>
        void Give()
        {
            //Game_administrator.Singleton_Instance.Add_coin();
            LeanPool.Despawn(gameObject);
        }

    }
}