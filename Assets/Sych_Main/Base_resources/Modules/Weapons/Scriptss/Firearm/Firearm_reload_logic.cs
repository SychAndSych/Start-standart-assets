using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using NaughtyAttributes;

namespace Sych_scripts
{
    [DisallowMultipleComponent]
    public class Firearm_reload_logic : MonoBehaviour
    {
        #region ����������

        [field: Space(10)]
        [field: Tooltip("�������� ������� (�� ����, ������ �� ����� �������� ����������)")]
        [field: SerializeField]
        public bool Active_ammo_bool = false;

        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_ammo_bool))]
        [Tooltip("���������� �������� � ����� ������")]
        [SerializeField]
        int Max_count_ammo = 20;

        internal int Active_count_ammo  = 0;

        [field: ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_ammo_bool))]
        [field: Tooltip("����������� �� 1 �������")]
        [field: SerializeField]
        protected bool Single_ammo_reload_bool = false;

        //[ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Single_ammo_reload_bool))]
        [Tooltip("������� ����������� � ����� ����������� �� 1 ������� (������� ������, ������� � ����� ����������� End_reload)")]
        [SerializeField]
        UnityEvent End_Single_ammo_reload_event = new UnityEvent();


        [field: ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_ammo_bool))]
        [field: Tooltip("�������� �������� �������� ��� ������ (��� �� ����� ���� ������������ ����������)")]
        [field: SerializeField]
        public bool Active_magazine_bool = false;

        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_magazine_bool), nameof(Active_ammo_bool))]
        [SerializeField]
        int Max_count_magazine = 10;

        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_magazine_bool), nameof(Active_ammo_bool))]
        [Tooltip("1 ������� ����� ������� ��������� (���� ���, �� �� ���������� �������� ����� ���������� ������� ��������, ������� ��������� � ������)")]
        [SerializeField]
        bool Magazine_full_ammo_bool = false;

        internal int Active_count_magazine = 0;

        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_magazine_bool), nameof(Active_ammo_bool))]
        [Tooltip("�������������� ����� ��, �� ��� �� ������ �� ����� �������� ����� ����������� (�����, ��� ����, ��� �� �� ������� ��� � ��������������� ���������)")]
        [SerializeField]
        float Additional_time_stop_reload = 0.2f;

        //[ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_magazine_bool), nameof(Active_ammo_bool))]
        [Tooltip("������� ����������� � ������ ����������� (� �������� ��� ������� ��������, ������� � ����� ����������� End_reload)")]
        [SerializeField]
        UnityEvent Start_reload_event = new UnityEvent();

        internal Firearm_main_logic Firearm_main_logic_script = null;//������� ��������� ������ ������

        bool Reload_active_bool = false;//��������������
        #endregion


        #region ��������� ������

        private void OnEnable()
        {
            Preparation_reload();
        }

        private void Awake()
        {
            if (Active_ammo_bool)
            {
                Active_count_ammo = Max_count_ammo;
                Active_count_magazine = Max_count_magazine;
            }
        }

        #endregion


        #region ������

        /// <summary>
        /// ������������� �� ����������� �����������
        /// </summary>
        void Preparation_reload()
        {
            if (Reload_active_bool)
            {
                    Reload_active_bool = false;
                if(Active_count_ammo <= 0)
                {
                    Reload();
                }

            }
        }

        /// <summary>
        /// �������������� ����� ��, �� ��� �� ������ �� ����� �������� ����� �����������(�����, ��� ����, ��� �� �� ������� ��� � ��������������� ���������)
        /// </summary>
        void Additional_time_reload_method()
        {
            Reload_active_bool = false;

            if (Firearm_main_logic_script.Firearm_shutter_logic_script.Shutter_bool)
            {
                Firearm_main_logic_script.Firearm_shutter_logic_script.Shutter_reload();
            }
        }

        #endregion


        #region ��������� ������


        /// <summary>
        /// ������ �����������
        /// </summary>
        public void Reload()
        {
            if (Active_count_magazine > 0 && Active_count_ammo < Max_count_ammo && Active_magazine_bool && !Reload_active_bool)
            {
                Reload_active_bool = true;
                Start_reload_event.Invoke();
            }
        }

        /// <summary>
        /// �������� �������������� �������/ ���������� ���������
        /// </summary>
        /// <param name="_ammo">���������� ��������</param>
        public void Add_magazine(int _ammo)
        {
            Active_count_magazine += _ammo;
            Active_count_magazine = Mathf.Clamp(Active_count_magazine, 0, Max_count_magazine);
            Firearm_main_logic_script.Update_UI();
        }

        /// <summary>
        /// ������� �������
        /// </summary>
        /// <param name="_ammo">���������� ���������� ��������</param>
        public void Remove_ammo(int _ammo)
        {
            Active_count_ammo -= _ammo;

            if (Active_count_ammo <= 0)
                Reload();

            Firearm_main_logic_script.Update_UI();
        }

        /// <summary>
        /// ��������� ������� �������� � ������
        /// </summary>
        /// <param name="_cost_shot">��������� ���������� �������� �� �������</param>
        /// <returns></returns>
        public bool Check_ammo(int _cost_shot)
        {
            bool result = false;

            if (Active_count_ammo >= _cost_shot || !Active_ammo_bool)
            {
                result = true;
            }
            else if (Active_magazine_bool)
            {
                if (Active_count_magazine > 0)
                {
                    Reload();
                }
            }

            return result;
        }


        /// <summary>
        /// ����� �����������, ����� ��������� �������
        /// </summary>
        public void End_reload()
        {
            if (Active_count_magazine > 0)
            {
                if (!Magazine_full_ammo_bool)
                {
                    Active_count_ammo = Max_count_ammo;
                    Active_count_magazine--;
                }
                else
                {
                    int missing_ammo = Max_count_ammo - Active_count_ammo;

                    if(Active_count_magazine >= missing_ammo)
                    {
                        Active_count_ammo += missing_ammo;
                        Active_count_magazine -= missing_ammo;
                    }
                    else
                    {
                        int value_reload = missing_ammo - Active_count_ammo;
                        Active_count_ammo += value_reload;
                        Active_count_magazine -= value_reload;
                    }

                }

                Invoke(nameof(Additional_time_reload_method), Additional_time_stop_reload);
                Firearm_main_logic_script.Update_UI();
            }
        }


        /// <summary>
        /// ����� ��������� 1 �������
        /// </summary>
        public void End_Add_single_ammo_reload()
        {
            if (Active_count_ammo < Max_count_ammo && Active_count_magazine > 0)
            {
                Active_count_ammo++;
                Active_count_magazine--;
                Reload_active_bool = false;
                //Reload_bool = true;
                Firearm_main_logic_script.Firearm_shutter_logic_script.Early_reset();

                Firearm_main_logic_script.Update_UI();
            }

            if (Active_count_ammo >= Max_count_ammo || Active_count_magazine <= 0)
            {

                End_Single_ammo_reload_event.Invoke();

                if (Firearm_main_logic_script.Firearm_shutter_logic_script.Shutter_bool)
                    Firearm_main_logic_script.Firearm_shutter_logic_script.Shutter_reload();

                //Reload_bool = true;
            }

        }
        #endregion
    }
}