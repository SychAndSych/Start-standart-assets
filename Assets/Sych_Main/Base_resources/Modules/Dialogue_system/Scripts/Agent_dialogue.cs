//Агент для активации 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts 
{
    [AddComponentMenu("Sych scripts / Game / Dialogue system / Agent dialogue")]
    [DisallowMultipleComponent]
    public class Agent_dialogue : MonoBehaviour, I_Interaction
    {

        [Tooltip("Диалоги")]
        [SerializeField]
        TextAsset[] Text_file_array = new TextAsset[0];

        [Tooltip("Спрайты для портрета")]
        [SerializeField]
        Sprite[] Portrait_array = new Sprite[0];

        [Tooltip("Аниматор")]
        [SerializeField]
        Animator Anim = null;

        public Interaction_enum Interaction_type => Interaction_enum.Talking;

        int Id_Text_file_active = 0;

        #region Публичные методы

        /// <summary>
        /// Изменить активный диалог (что бы сменить id активного файла диалога)
        /// </summary>
        /// <param name="_id_change"></param>
        public void Change_id_active_Text_file(int _id_change)
        {
            Id_Text_file_active = _id_change;
        }


        /// <summary>
        /// Стартовать диалог
        /// </summary>
        public void Activation_dialogue()
        {
            Dialogue_administrator.Singleton_Instance.Enter_dialogue(this);
        }


        public TextAsset Find_out_Ink_JSON
        {
            get
            {
                return Text_file_array[Id_Text_file_active];
            }
        }

        /// <summary>
        /// Проиграть эмоцию (и запустить анимацию если есть аниматор)
        /// </summary>
        /// <param name="_name_emotion"></param>
        public void Change_emotion(string _name_emotion)
        {
            Anim.Play(_name_emotion);
            //print("Меняет эмоцию на" + _name_emotion);
        }

        /// <summary>
        /// Получить портрет для данного персонажа
        /// </summary>
        public Sprite Find_out_Portrait(int _id)
        {
            return Portrait_array[_id];
        }

        public void Interaction()
        {
            Activation_dialogue();
        }


        #endregion


    }
}