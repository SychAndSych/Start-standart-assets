using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lean.Pool;

namespace Sych_scripts
{
    public class UI_task_external_administrator : MonoBehaviour
    {
        #region ����������
        [Tooltip("��������� (��������� ����) ���� ���� ������������ ������")]
        [SerializeField]
        Transform Content_transform = null;

        [Tooltip("������ ������� (��� ����������� ����)")]
        [SerializeField]
        UI_task_quest Task_prefab = null;

        [Tooltip("�������� ������")]
        [SerializeField]
        TMP_Text Name_quest = null;

        [Tooltip("�������� ������ ����� �� ��������� (�������� ������)")]
        [SerializeField]
        string Default_name_quest = "��� �������";

        Quest_SO Quest_Data = null;

        List<UI_task_quest> UI_task_list = new List<UI_task_quest>();

        bool Init = false;//������������� ��� ���������
        #endregion

        #region ��������� ������

        private void OnEnable()
        {
            if (Quest_system.Singleton_Instance && !Init)
            {
                Quest_system.Singleton_Instance.Add_quest_event.AddListener(Add_new_quest);
                Quest_system.Singleton_Instance.Remove_quest_event.AddListener(Remove_quest);
                Quest_system.Singleton_Instance.End_task_event.AddListener(End_task);
                Init = true;
            }
        }

        private void Start()
        {
            Name_quest.text = Default_name_quest;

            if (!Init)
            {
                Quest_system.Singleton_Instance.Add_quest_event.AddListener(Add_new_quest);
                Quest_system.Singleton_Instance.Remove_quest_event.AddListener(Remove_quest);
                Quest_system.Singleton_Instance.End_task_event.AddListener(End_task);
                Init = true;
            }
        }

        private void OnDisable()
        {
            if (Init)
            {
                Quest_system.Singleton_Instance.Add_quest_event.RemoveListener(Add_new_quest);
                Quest_system.Singleton_Instance.Remove_quest_event.RemoveListener(Remove_quest);
                Quest_system.Singleton_Instance.End_task_event.RemoveListener(End_task);
                Init = false;
            }
        }

        #endregion


        #region ������
        /// <summary>
        /// �������� ������
        /// </summary>
        void Add_new_quest(Quest_SO _quest_SO)
        {
            Quest_Data = _quest_SO;

            Name_quest.text = _quest_SO.Name_quest;

            for(int x = 0; x < Quest_Data.Description_Tasks_array.Length; x++)
            {
                UI_task_quest task = LeanPool.Spawn(Task_prefab, Content_transform);

                task.Add_quest_SO(_quest_SO, x);

                UI_task_list.Add(task);
            }
        }

        /// <summary>
        /// ������ ������
        /// </summary>
        void Remove_quest(Quest_SO _quest_SO)
        {
            foreach (UI_task_quest task_class in UI_task_list)
            {
                    task_class.Clear();
                    LeanPool.Despawn(task_class);
            }

            Name_quest.text = Default_name_quest;
        }

        /// <summary>
        /// ��������� ������
        /// </summary>
        /// <param name="_quest_SO_data">������ ������ � ��� �����</param>
        /// <param name="_id_task">ID ������</param>
        /// <param name="_win">������� ��������?</param>
        void End_task(Quest_SO _quest_SO_data, int _id_task, bool _win)
        {
                    if (_win)
                        UI_task_list[_id_task].Completed(_win);
                    else
                        UI_task_list[_id_task].Completed(_win);

        }

        #endregion
    }
}