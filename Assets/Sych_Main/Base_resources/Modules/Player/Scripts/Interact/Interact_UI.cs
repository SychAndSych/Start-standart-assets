using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sych_scripts {
    [AddComponentMenu("Sych scripts / Game / Player / UI Interact player")]
    [DisallowMultipleComponent]
    public class Interact_UI : Singleton<Interact_UI>
    {
        [Tooltip("Массив вариантов изображений")]
        [SerializeField]
        Slot_variation[] Variation_array = new Slot_variation[0];

        [Tooltip("Картинка для отображения по умолчанию (когда другие не подходят)")]
        public Sprite Sprite_image_default = null;

        [Tooltip("Картинка отображающая с чем взаимодействуем")]
        [SerializeField]
        Image Image_UI = null;

        private void Start()
        {
            Off_image();
        }

        /// <summary>
        /// Поменять картинку
        /// </summary>
        /// <param name="_image_name">Имя слота с картинкой</param>
        public void Change_image(string _image_name)
        {
            bool result = false;
            Image_UI.enabled = true;

            foreach (Slot_variation slot in Variation_array)
            {
                if (slot.Name_type.ToString() == _image_name)
                {
                    Image_UI.sprite = slot.Sprite_image;
                    result = true;
                    break;
                }
            }

            if (!result)
                Image_UI.sprite = Sprite_image_default;
        }

        /// <summary>
        /// Выключить картинку
        /// </summary>
        public void Off_image()
        {
            Image_UI.enabled = false;
        }

        [System.Serializable]
        class Slot_variation
        {
            [Tooltip("Имя вариации (для поиска)")]
            public Interaction_enum Name_type = Interaction_enum.None;

            [Tooltip("Картинка для отображения")]
            public Sprite Sprite_image = null;
        }

    }


}