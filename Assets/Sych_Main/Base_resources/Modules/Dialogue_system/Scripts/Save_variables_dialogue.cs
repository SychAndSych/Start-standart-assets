using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

namespace Sych_scripts
{

    public class Save_variables_dialogue
    {

        Dictionary<string, Ink.Runtime.Object> Variables;


        #region Методы

        /// <summary>
        /// Изменить значение переменной
        /// </summary>
        /// <param name="_name_variable"></param>
        /// <param name="_value"></param>
        void Variable_changed(string _name_variable, Ink.Runtime.Object _value_variable)
        {
            if (Variables.ContainsKey(_name_variable))
            {
                Variables.Remove(_name_variable);
                Variables.Add(_name_variable, _value_variable);
            }
        }

        void Variable_to_story(Story _story)
        {
            foreach (KeyValuePair<string, Ink.Runtime.Object> variable in Variables)
            {
                _story.variablesState.SetGlobal(variable.Key, variable.Value);
            }
        }
        #endregion


        #region Публичные методы
        //Начать слежение за историей
        public void Start_listening(Story _story_text)
        {
            Variable_to_story(_story_text);
            _story_text.variablesState.variableChangedEvent += Variable_changed;
        }

        //Отменить слежение за историей
        public void End_listening(Story _story_text)
        {
            _story_text.variablesState.variableChangedEvent -= Variable_changed;
        }
        #endregion


        #region Публичная инициализации
        public Save_variables_dialogue(string _globals_file_path)
        {
            //Компиляция истории
            string ink_file_contents = File.ReadAllText(_globals_file_path);
            Ink.Compiler compiler = new Ink.Compiler(ink_file_contents);
            Story global_variables_story = compiler.Compile();

            //Инициализация
            Variables = new Dictionary<string, Ink.Runtime.Object>();
            foreach (string name in global_variables_story.variablesState)
            {
                Ink.Runtime.Object value = global_variables_story.variablesState.GetVariableWithName(name);
                Variables.Add(name, value);
                Debug.Log("Инициализация глобальной переменной для диалоговой системы: " + name + " = " + value);
            }
        }
        #endregion
    }
}
