//�������� ������ ������ � �� ������
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

        [Tooltip("������ ������ ������")]
        [SerializeField]
        Effects_class[] Effects_class_array = new Effects_class[0];



        /// <summary>
        /// �������� ����������� �������
        /// </summary>
        /// <param name="_id_effect">����� �������</param>
        public void On_effect(int _id_effect)
        {
            Activity_effects(true, _id_effect, "");
        }

        /// <summary>
        /// �������� ����������� �������
        /// </summary>
        /// <param name="_name_effect">��� ��������</param>
        public void On_effect(string _name_effect)
        {
            Activity_effects(true, -1, _name_effect);
        }

        /// <summary>
        /// ��������� ����������� �������
        /// </summary>
        /// <param name="_id_effect">����� ��������</param>
        public void Off_effect(int _id_effect)
        {
            Activity_effects(false, _id_effect, "");
        }

        /// <summary>
        /// ��������� ����������� �������
        /// </summary>
        /// <param name="_name_effect">��� ��������</param>
        public void Off_effect(string _name_effect)
        {
            Activity_effects(false, -1, _name_effect);
        }


        /// <summary>
        /// ��������� ��� �������� �������
        /// </summary>
        /// <param name="_activity">���/����</param>
        /// <param name="_id">����� id �������</param>
        /// <param name="_name">��� �������</param>
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
            [Tooltip("��������, ��� �� �� ������, ��� ��� � �� ��� ��������.")]
            public string Name = "��������";

            [Tooltip("���� ������� ������")]
            public ParticleSystem PS = null;

            [Tooltip("���������� ������ (�� �����)")]
            public VisualEffect VE = null;

            //[Tooltip("����� �� ��� �����������")]
            //public bool Loop_bool = false; 
        }
    }
}