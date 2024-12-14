using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Movement / Rotation object")]
    [DisallowMultipleComponent]
    public class Rotation_object : MonoBehaviour
    {
        [Tooltip("������� �� ������?")]
        [SerializeField]
        bool Physics_bool = false;

        [HideIf(nameof(Physics_bool))]
        [Tooltip("��� ����� �������")]
        [SerializeField]
        Transform Transform_target = null;

        [ShowIf(nameof(Physics_bool))]
        [Tooltip("������ ��� ����������� ��������")]
        [SerializeField]
        Rigidbody Body = null;

        [Tooltip("�������� ��������")]
        [SerializeField]
        float Speed_rotation = 0.1f;

        [Tooltip("����������� ��������")]
        [SerializeField]
        Vector3 Direction_rotation = new Vector3(0, 0, 1f);

        [Tooltip("��������?")]
        [SerializeField]
        bool Active_bool = true;

        private void FixedUpdate()
        {
            if(Active_bool)
            Rotation_move();
        }

        /// <summary>
        /// ������� ������
        /// </summary>
        void Rotation_move()
        {
            if (!Physics_bool)
            {
                if (Transform_target)
                {
                    Transform_target.Rotate(Direction_rotation * Speed_rotation * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(Direction_rotation * Speed_rotation * Time.deltaTime);
                }
            }
            else if (Physics_bool && Body)
            {
                Body.MoveRotation(Body.rotation * Quaternion.Euler(Direction_rotation * Speed_rotation * Time.fixedDeltaTime));
            }
        }
    }
}