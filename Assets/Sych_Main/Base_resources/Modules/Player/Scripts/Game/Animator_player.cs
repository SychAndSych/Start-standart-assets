//�������� ������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Animator player")]
    [DisallowMultipleComponent]
    public class Animator_player : MonoBehaviour
    {
        [Tooltip("��������")]
        [SerializeField]
        Animator Anim = null;

        [Tooltip("������ ������ ������")]
        [SerializeField]
        Player_Motor Motor_script = null;

        private void Update()
        {
            Anim.SetFloat("Speed", Motor_script.Speed_active);

            Anim.SetFloat("Fall", Motor_script._verticalVelocity);

            Anim.SetBool("Grounded", Motor_script.Grounded_bool);

            Anim.SetBool("Jump", Motor_script.Jump_active_bool);
        }
    }
}