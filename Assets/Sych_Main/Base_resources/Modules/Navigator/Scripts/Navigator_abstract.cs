using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{

    public abstract class Navigator_abstract : Singleton<Navigator_abstract>
    {

        protected enum Enum_navigator
        {
            Object_enum,
            Position_enum
        }

        #region ����������

        [Tooltip("���������� �����")]
        [SerializeField]
        GameObject Visual = null;

        protected Transform Target_transform = null;//���� �� ������� �����  ���������� ���������

        protected Vector3 Target_position = Vector3.zero;//�������� ������� �� ������� �����  ���������� ���������

        protected Enum_navigator Type_way = Enum_navigator.Position_enum;

        protected bool Active_bool = false;

        #endregion


        #region ��������� ������
        private void Start()
        {
            Stop_navigator();
        }
        #endregion


        #region ������
        protected virtual void Start_method()
        {
            Active_bool = true;
            Invoke(nameof(Delay_active_mesh), 0.1f);
        }

        void Delay_active_mesh()
        {
            Visual.SetActive(true);
        }

        #endregion


        #region ��������� ������
        /// <summary>
        /// ������ ������
        /// </summary>
        /// <param name="_target_transform">���� ������</param>
        public void New_way(Transform _target_transform)
        {
            Target_transform = _target_transform;
            Type_way = Enum_navigator.Object_enum;
            Start_method();
        }

        /// <summary>
        /// ������ ������
        /// </summary>
        /// <param name="_target_position">���� �������</param>
        public void New_way(Vector3 _target_position)
        {
            Target_position = _target_position;
            Type_way = Enum_navigator.Position_enum;
            Start_method();
        }

        /// <summary>
        /// ���������� ���������
        /// </summary>
        public virtual void Stop_navigator()
        {
            Active_bool = false;
            Visual.SetActive(false);
        }
        #endregion
    }
}