using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Navigator / Agent navigator")]
    [DisallowMultipleComponent]
    public class Agent_navigator : MonoBehaviour
    {

        /// <summary>
        /// ��������� ����� ������
        /// </summary>
        /// <param name="_target">���� ������</param>
        public void Set_target_object(Transform _target)
        {
            Navigator_abstract.Singleton_Instance.New_way(_target);
        }

        /// <summary>
        /// ��������� ����� �������
        /// </summary>
        /// <param name="_target_position">������� ����</param>
        public void Set_target_position(Vector3 _target_position)
        {
            Navigator_abstract.Singleton_Instance.New_way(_target_position);
        }

        /// <summary>
        /// ��������� ���������
        /// </summary>
        public void Off_navigator()
        {
            Navigator_abstract.Singleton_Instance.Stop_navigator();
        }
    }
}