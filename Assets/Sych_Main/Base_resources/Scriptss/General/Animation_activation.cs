//���������� �������� �� ����������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / General / Animation activation")]
    [DisallowMultipleComponent]
    public class Animation_activation : MonoBehaviour
    {
        [Tooltip("��������")]
        [SerializeField]
        Animator Anim = null;

        string Name_value = null;

        /// <summary>
        /// ��������� �������� ������ ��������
        /// </summary>
        /// <param name="_name_animation">��� ��������</param>
        public void Play_animation(string _name_animation)
        {
            Anim.Play(_name_animation);
        }

        /// <summary>
        /// ���������� bool ���������� � true
        /// </summary>
        /// <param name="_name_animation">�������� bool ����������</param>
        public void Set_bool_true(string _name_bool)
        {
            Anim.SetBool(_name_bool, true);
        }

        /// <summary>
        /// ���������� bool ���������� � false
        /// </summary>
        /// <param name="_name_animation">�������� bool ����������</param>
        public void Set_bool_false(string _name_bool)
        {
            Anim.SetBool(_name_bool, false);
        }

        /// <summary>
        /// ���������� trigger ����������
        /// </summary>
        /// <param name="_name_animation">�������� trigger ����������</param>
        public void Set_trigger(string _name_trigger)
        {
            Anim.SetTrigger(_name_trigger);
        }

        /// <summary>
        /// ��������� ��� ����������
        /// </summary>
        /// <param name="_name">��� ����������</param>
        public void Save_name(string _name)
        {
            Name_value = _name;
        }

        /// <summary>
        /// ���������� int ����������
        /// </summary>
        /// <param name="_name_animation">�������� int ����������</param>
        public void Set_int(int _value_int)
        {
            Anim.SetInteger(Name_value, _value_int);
        }

        /// <summary>
        /// ���������� float ����������
        /// </summary>
        /// <param name="_name_animation">�������� float ����������</param>
        public void Set_float(float _value_float)
        {
            Anim.SetFloat(Name_value, _value_float);
        }

        /// <summary>
        /// ��������� ������� �� �����
        /// </summary>
        /// <param name="_name">��� ��������</param>
        public void Reset_trigger_name(string _name)
        {
            Anim.ResetTrigger(_name);
        }

        /// <summary>
        /// ��������� ������� �� id
        /// </summary>
        /// <param name="_name">id ��������</param>
        public void Reset_trigger_id(int _id)
        {
            Anim.ResetTrigger(_id);
        }

    }
}