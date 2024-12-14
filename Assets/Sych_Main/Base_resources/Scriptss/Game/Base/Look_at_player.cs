//Следит за игроком
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Base / Look at player")]
    [DisallowMultipleComponent]
    public class Look_at_player : MonoBehaviour
    {
        [Tooltip("Если включить, то прекратит наклоняться и будет только поворачиваться по оси Y")]
        [SerializeField]
        bool X_Z_bool = true;

        private Transform target;

        private void Start()
        {
            target = Camera.main.transform;// Game_administrator.Singleton_Instance.Player_sc.transform;
        }

        private void LateUpdate()
        {
            Vector3 end_position = target.position;

            if (X_Z_bool)
                end_position.y = transform.position.y;

            transform.LookAt(end_position);
        }
    }
}