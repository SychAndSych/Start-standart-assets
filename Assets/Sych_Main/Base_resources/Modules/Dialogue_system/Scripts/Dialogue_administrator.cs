// Для работы нужен Ink.Unity.Integration
// Need Ink.Unity.Integration to work
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using NaughtyAttributes;
using UnityEngine.Events;
using Ink.UnityIntegration;

namespace Sych_scripts
{

    enum Langue_enum
    {
        Ru,
        Eng
    }

    public class Dialogue_administrator : Singleton<Dialogue_administrator>
    {

        #region Переменные

        [Tooltip("Интерфейс диалогов")]
        [SerializeField]
        GameObject Show_UI = null;

        [Tooltip("Поле показывающая текст")]
        [SerializeField]
        TMP_Text Text_UI = null;

        [Tooltip("Имя персонажа (кто говорит)")]
        [SerializeField]
        TMP_Text Name_character_UI = null;

        [Tooltip("Портрет персонажа (кто говорит)")]
        [SerializeField]
        Sprite Portrait_character_UI = null;

        [Tooltip("Кнопки выбора")]
        [SerializeField]
        Button_choice[] Button_choice_array = new Button_choice[0];

        [Tooltip("Кнопка продолжить")]
        [SerializeField]
        Button Button_continue = null;

        [Tooltip("Файл Ink которые хранит в себе переменные")]
        [SerializeField]
        InkFile Global_Ink_file = null;

        [Foldout("Other")]

        [Tooltip("Сам текст для запуска")]
        [SerializeField]
        TextAsset Text_JSON = null;

        [Tooltip("Будет ли текст набираться постеменно, а не мгновенно")]
        [SerializeField]
        bool Printing_text_mode_bool = true;

        [ShowIf(nameof(Printing_text_mode_bool))]
        [Tooltip("Время с которым будет пропечатываться текст")]
        [SerializeField]
        float Printing_text_time = 0.01f;

        Coroutine Printing_text_coroutine = null;

        [Foldout("Other")]
        [Tooltip("Стартует диалог сразу")]
        [SerializeField]
        bool Start_bool = false;

        [Tooltip("Ивент старта диалога")]
        [SerializeField]
        UnityEvent Start_dialogue_event = new UnityEvent();

        [Tooltip("Ивент конца диалога")]
        [SerializeField]
        UnityEvent End_dialogue_event = new UnityEvent();

        Agent_dialogue Agent_dialogue_script = null;

        [Tooltip("Стартовать квест")]
        [SerializeField]
        UnityEvent<int> Start_quest_event = new UnityEvent<int>();

        [Tooltip("Закончить квест")]
        [SerializeField]
        UnityEvent<int> End_quest_event = new UnityEvent<int>();

        [Tooltip("Получить награду")]
        [SerializeField]
        UnityEvent<int> Reward_event = new UnityEvent<int>();

        /// <summary>
        /// Активен ли диалог
        /// </summary>
        internal bool Active_mode_bool { get; private set; } = false;

        Story Current_story = null;

        Save_variables_dialogue Save_variables_dialogue_script;

        string active_text = "";
        #endregion



        #region Системные методы

        protected override void Awake()
        {
            base.Awake();
            Save_variables_dialogue_script = new Save_variables_dialogue(Global_Ink_file.filePath);
        }

        private void Start()
        {
            if (Start_bool)
            {
                Current_story = new Story(Text_JSON.text);

                On_dialogue();
            }
            else
                Change_activity(false);
        }

        #endregion


        #region Методы

        /// <summary>
        /// Включить или отключить меню диалогов
        /// </summary>
        /// <param name="_activity"></param>
        void Change_activity(bool _activity)
        {
            Show_UI.gameObject.SetActive(_activity);

            Active_mode_bool = _activity;
        }

        /// <summary>
        /// Начать диалог
        /// </summary>
        void On_dialogue()
        {

            Save_variables_dialogue_script.Start_listening(Current_story);
            Change_activity(true);
            Choice_all_off();
            Continue_story();
            Start_dialogue_event.Invoke();
        }

        /// <summary>
        /// Закончить диалог
        /// </summary>
        void Off_dialogue()
        {

            Save_variables_dialogue_script.End_listening(Current_story);
            Change_activity(false);
            Agent_dialogue_script = null;
            active_text = "";
            End_dialogue_event.Invoke();
        }

        //Обновить кнопки с вариантами ответов
        void Choice_update()
        {
            List<Choice> current_choices = Current_story.currentChoices;

            if (current_choices.Count > Button_choice_array.Length)
            {
                Debug.LogWarning("Вариантов ответа куда больше, чем кнопок!");
            }

            Choice_all_off();

            if (current_choices.Count > 0)
            {
                Button_continue.enabled = false;

                for (int id_button = 0; id_button < current_choices.Count; id_button++)
                {
                    Button_choice_array[id_button].Button_obj.SetActive(true);
                    Button_choice_array[id_button].Text_UI.text = current_choices[id_button].text;
                }
            }
            else
            {
                Button_continue.enabled = true;
            }
        }

        /// <summary>
        /// Выключить все кнопки
        /// </summary>
        void Choice_all_off()
        {
            foreach (Button_choice button in Button_choice_array)
            {
                button.Button_obj.SetActive(false);
            }
        }


        /// <summary>
        /// Улавливает все теги и приводит в действие по тегу
        /// </summary>
        /// <param name="_current_tags"></param>
        void Handle_tags(List<string> _current_tags)
        {
            foreach (string tag in _current_tags)
            {
                string[] split_tag = tag.Split(':');

                if (split_tag.Length != 2)
                {
                    Debug.LogError("Значение тега и ключа в купе имеют более 2-х значений!  " + tag);
                }

                string tag_key = split_tag[0].Trim();
                string tag_value = split_tag[1].Trim();

                switch (tag_key)
                {
                    case "Emotion":
                        Agent_dialogue_script.Change_emotion(tag_value);
                        print("Эмоция  " + tag_value);
                        break;
                    case "Portrait":
                        Portrait_character_UI = Agent_dialogue_script.Find_out_Portrait(int.Parse(tag_value));
                        print("Портрет  " + tag_value);
                        break;
                    case "Speaker_name":
                        Name_character_UI.text = tag_value;
                        print("Имя говорящего  " + tag_value);
                        break;
                    case "Quest_start":
                        Start_quest_event.Invoke(int.Parse(tag_value));
                        print("Начало квеста  " + tag_value);
                        break;
                    case "Quest_end":
                        End_quest_event.Invoke(int.Parse(tag_value));
                        print("Конец квеста  " + tag_value);
                        break;
                    case "Reward":
                        Reward_event.Invoke(int.Parse(tag_value));
                        print("Получить награду  " + tag_value);
                        break;
                    case "Change_id_dialogue":
                        Agent_dialogue_script.Change_id_active_Text_file(int.Parse(tag_value));
                        print("Изменил id диалога  " + tag_value);
                        break;
                    default:
                        Debug.Log("Такого тега нет!  " + tag_value);
                        break;
                }
            }


        }


        /// <summary>
        /// Пропечатывание текста
        /// </summary>
        /// <param name="_line"></param>
        /// <returns></returns>
        IEnumerator Coroutine_pronting_text(string _line)
        {
            Text_UI.text = "";

            bool is_adding_rich_text_tag = false;

            Button_continue.enabled = true; 

            foreach (char letter in _line.ToCharArray())
            {
                if (letter == '<' || is_adding_rich_text_tag)
                {
                    is_adding_rich_text_tag = true;
                    Text_UI.text += letter;

                    if (letter == '>')
                    {
                        is_adding_rich_text_tag = false;
                    }
                }
                else
                {
                    Text_UI.text += letter;
                    yield return new WaitForSecondsRealtime(Printing_text_time);
                }
            }
            Choice_update();
            Printing_text_coroutine = null;
        }

        #endregion


        #region Публичные методы
        /// <summary>
        /// Стартовать диалог
        /// </summary>
        /// <param name="_ink_JSON">Файл с диалогами</param>
        public void Enter_dialogue(Agent_dialogue _agent_dialogue)
        {
            Agent_dialogue_script = _agent_dialogue;

            Current_story = new Story(_agent_dialogue.Find_out_Ink_JSON.text);

            On_dialogue();
        }

        /// <summary>
        /// Продолжить диалог (без указания варианта ответа)
        /// </summary>
        public void Continue_story()
        {
            Choice_all_off();
            Button_continue.enabled = false;

            if (Printing_text_coroutine == null)
            {
                if (Current_story.canContinue)
                {
                    active_text = Current_story.Continue();

                    if (Printing_text_mode_bool)
                    {

                        Printing_text_coroutine = StartCoroutine(Coroutine_pronting_text(active_text));
                    }
                    else
                    {
                        Text_UI.text = active_text;
                        Choice_update();
                    }

                    Handle_tags(Current_story.currentTags);
                }
                else
                {
                    Off_dialogue();
                }
            }
            else
            {
                StopCoroutine(Printing_text_coroutine);
                Printing_text_coroutine = null;
                Button_continue.enabled = false;
                Text_UI.text = active_text;
                Choice_update();
            }
        }

        /// <summary>
        /// Продолжить со сделанным выбором
        /// </summary>
        /// <param name="_choice_id"></param>
        public void Continue_story(int _choice_id)
        {
            Current_story.ChooseChoiceIndex(_choice_id);
            Continue_story();
        }

        #endregion


        #region Прочее
        [System.Serializable]
        class Button_choice
        {
            [Tooltip("Сама кнопка")]
            [SerializeField]
            public GameObject Button_obj = null;

            [Tooltip("Текст кнопки ответа")]
            [SerializeField]
            public TMP_Text Text_UI = null;
        }
        #endregion

    }


}