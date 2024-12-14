using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Pool;

namespace Sych_scripts
{
    [DisallowMultipleComponent]
    public class UI_quest : MonoBehaviour
    {
        #region ����������
        [Tooltip("����������� ����� �������")]
        [SerializeField]
        public TMP_Text Name_quest = null;

        [Tooltip("������ ����� (��� �������� �����)")]
        [SerializeField]
        UI_task_quest Prefab_task = null;

        [Tooltip("������������ ������ ��� ������������ ��������")]
        [SerializeField]
        Transform Parent_point_task = null;

        List<UI_task_quest> Task_list = new List<UI_task_quest>();//���� ��������

        Quest_SO Quest_SO_ = null;//������ � �������

        [Tooltip("������ ����������� ��������")]
        [SerializeField]
        public bool[] Task_completed_bool_array = new bool[0];
        #endregion


        #region ��������� ������
        /// <summary>
        /// �������� ������ � ������
        /// </summary>
        /// <param name="_quest_SO">������ ������</param>
        public void Add_quest_SO(Quest_SO _quest_SO)
        {
            Quest_SO_ = _quest_SO;

            if (Name_quest)
                Name_quest.text = Quest_SO_.Name_quest;

            Task_completed_bool_array = new bool[Quest_SO_.Description_Tasks_array.Length];
            Add_task();
        }

        /// <summary>
        /// �������� (�������� � �������������� ���)
        /// </summary>
        public void Clear()
        {
            Remove_all_subtask();
        }

        /// <summary>
        /// ��������� ������ �������
        /// </summary>
        /// <param name="_id_task">Id ����� ������</param>
        public void Task_win_completed(int _id_task)
        {
            if (Task_completed_bool_array[_id_task] != true)
            {
                Task_list[_id_task].Completed(true);
                Task_completed_bool_array[_id_task] = true;
            }

        }

        /// <summary>
        /// ��������� ������ ���������
        /// </summary>
        /// <param name="_id_task">Id ����� ������</param>
        public void Task_lose_completed(int _id_task)
        {
            Task_list[_id_task].Completed(false);
            Task_completed_bool_array[_id_task] = true;
        }

        /// <summary>
        /// ��������� �����������, ��� �� ��������� �� �� ��������� ��� ���������� ������ (���� �� ��, �� ������ �� �����)
        /// </summary>
        public void Completed_task()
        {
            bool result = true;

            foreach (bool completed_task in Task_completed_bool_array)
            {
                if (completed_task == false)
                {
                    result = false;
                    break;
                }
            }

            if (result)
                Quest_system.Singleton_Instance.End_win_quest(Quest_SO_);
        }

        #endregion

        #region ������
        /// <summary>
        /// �������� ���������
        /// </summary>
        void Add_task()
        {
            for (int x = 0; x < Quest_SO_.Description_Tasks_array.Length; x++)
            {
                UI_task_quest task = LeanPool.Spawn(Prefab_task, Parent_point_task);
                task.Add_quest_SO(Quest_SO_, x, this);
                Task_list.Add(task);
            }
        }

        /// <summary>
        /// �������� ���������
        /// </summary>
        void Remove_all_subtask()
        {
            for (int x = 0; x < Quest_SO_.Description_Tasks_array.Length; x++)
            {
                UI_task_quest subtask = Task_list[0];
                subtask.Clear();
                Task_list.Remove(Task_list[0]);
                LeanPool.Despawn(subtask);
            }
        }
        #endregion

    }
}
