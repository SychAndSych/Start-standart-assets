//Сохранение настроек в PlayerPrefs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    /// <summary>
    /// Тип параметров с цифровым значением float
    /// </summary>
    public enum Type_parameter_value_float
    {
        Sound_music,
        Sound_effect,
        Mouse_sensitivity
    }

    /// <summary>
    /// Тип параметров с цифровым значением int
    /// </summary>
    public enum Type_parameter_value_int
    {
        Language,
        Coins
    }

    /// <summary>
    /// Тип параметров с bool значением
    /// </summary>
    public enum Type_parameter_bool
    {
        Alert_bool,
        Vibration_bool,
        Quest_completed_bool,
        Checkpoint_bool
    }


    /// <summary>
    /// Тип параметров с vector3 значением
    /// </summary>
    public enum Type_parameter_vector3
    {
        Position
    }

    public static class Save_PlayerPrefs
    {

        static bool Initialization_saving_bool;//Параметр который определяет, было ли первоначальное сохранение (что бы выставить все параметры "по умолчанию" правильно)


        /// <summary>
        /// Инициализировать и выставить нужные параметры "по умолчанию" на нужное значение
        /// </summary>
        static void Initialization_saving()
        {
            string name = nameof(Initialization_saving_bool);
            PlayerPrefs.SetString(name, "Da");

            //Выставляем нужные параметры
            Save_parameter(Type_parameter_bool.Alert_bool, true);
            Save_parameter(Type_parameter_bool.Vibration_bool, true);
            Save_parameter(Type_parameter_value_float.Sound_music, 1);
            Save_parameter(Type_parameter_value_float.Sound_effect, 1);
            Save_parameter(Type_parameter_value_float.Mouse_sensitivity, 1);
            //


            PlayerPrefs.Save();
        }

        /// <summary>
        /// Сохранить параметр с цифровым значением float
        /// </summary>
        /// <param name="_parameter">Тип параметра</param>
        /// <param name="_value">Значение</param>
        public static void Save_parameter(Type_parameter_value_float _parameter, float _value)
        {

            string name = _parameter.ToString();

            PlayerPrefs.SetFloat(name, _value);

            PlayerPrefs.Save();
        }


        /// <summary>
        /// Сохранить параметр с float значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        /// <param name="_index">Номер ID</param>
        /// <param name="_value">Значение</param>
        public static void Save_parameter(Type_parameter_value_float _parameter, int _index, float _value)
        {
            string name = _parameter.ToString() + "_" + _index.ToString();

            PlayerPrefs.SetFloat(name, _value);

            PlayerPrefs.Save();
        }


        /// <summary>
        /// Сохранить параметр с цифровым значением int
        /// </summary>
        /// <param name="_parameter">Тип параметра</param>
        /// <param name="_value">Значение</param>
        public static void Save_parameter(Type_parameter_value_int _parameter, int _value)
        {
            string name = _parameter.ToString();

            PlayerPrefs.SetInt(name, _value);

            PlayerPrefs.Save();
        }


        /// <summary>
        /// Сохранить параметр с int значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        /// <param name="_index">Номер ID</param>
        /// <param name="_value">Значение</param>
        public static void Save_parameter(Type_parameter_value_int _parameter, int _index, int _value)
        {
            string name = _parameter.ToString() + "_" + _index.ToString();

            PlayerPrefs.SetInt(name, _value);

            PlayerPrefs.Save();
        }


        /// <summary>
        /// Сохранить параметр c bool значением
        /// </summary>
        /// <param name="_parameter">Тип параметра</param>
        /// <param name="_bool">Значение да или нет</param>
        public static void Save_parameter(Type_parameter_bool _parameter, bool _bool)
        {
            int value = 0;

            string name = _parameter.ToString();

            value = _bool ? 1 : 0;

            PlayerPrefs.SetInt(name, value);

            PlayerPrefs.Save();
        }

		
		/// <summary>
        /// Сохранить параметр c bool значением
        /// </summary>
        /// <param name="_parameter">Тип параметра</param>
        /// <param name="_bool">Значение да или нет</param>
        /// <param name="_name_id">ID имя (не номер) для сохранения</param>
        public static void Save_parameter(Type_parameter_bool _parameter, string _name_id, bool _bool)
        {
            int value = 0;

            string name = _parameter.ToString() + "_" + _name_id;

            value = _bool ? 1 : 0;

            PlayerPrefs.SetInt(name, value);

            PlayerPrefs.Save();
        }
		
		

        /// <summary>
        /// Сохранить параметр с bool значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        /// <param name="_index">Номер ID</param>
        /// <param name="_value">Значение да или нет</param>
        public static void Save_parameter(Type_parameter_bool _parameter, int _index, bool _bool)
        {
            int value = 0;

            string name = _parameter.ToString() + "_" + _index.ToString();

            value = _bool ? 1 : 0;

            PlayerPrefs.SetInt(name, value);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// Сохранить параметр с vector3 значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        /// <param name="_vector">Сохраняемый вектор</param>
        public static void Save_parameter(Type_parameter_vector3 _parameter, Vector3 _vector)
        {
            string name_X = _parameter.ToString() + "_" + "X";
            string name_Y = _parameter.ToString() + "_" + "Y";
            string name_Z = _parameter.ToString() + "_" + "Z";

            PlayerPrefs.SetFloat(name_X, _vector.x);
            PlayerPrefs.SetFloat(name_Y, _vector.y);
            PlayerPrefs.SetFloat(name_Z, _vector.z);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// Сохранить параметр с vector3 значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        /// <param name="_vector">Сохраняемый вектор</param>
        /// <param name="_name_id">Имя ID (имя параметра)</param>
        public static void Save_parameter(Type_parameter_vector3 _parameter, Vector3 _vector, string _name_id)
        {
            string name_X = _parameter.ToString() + "_" + _name_id + "_" + "X";
            string name_Y = _parameter.ToString() + "_" + _name_id + "_" + "Y";
            string name_Z = _parameter.ToString() + "_" + _name_id + "_" + "Z";

            PlayerPrefs.SetFloat(name_X, _vector.x);
            PlayerPrefs.SetFloat(name_Y, _vector.y);
            PlayerPrefs.SetFloat(name_Z, _vector.z);

            PlayerPrefs.Save();
        }











        /// <summary>
        /// Узнать значение параметра float
        /// </summary>
        /// <param name="_parameter">Тип параметра</param>
        /// <returns></returns>
        public static float Know_parameter(Type_parameter_value_float _parameter)
        {
            if (!PlayerPrefs.HasKey(nameof(Initialization_saving_bool)))
                Initialization_saving();

            string name = _parameter.ToString();

            return PlayerPrefs.GetFloat(name);
        }


        /// <summary>
        /// Узнать параметр с float значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        /// <param name="_index">Номер ID</param>
        /// <returns></returns>
        public static float Know_parameter(Type_parameter_value_float _parameter, int _index)
        {
            if (!PlayerPrefs.HasKey(nameof(Initialization_saving_bool)))
                Initialization_saving();

            string name = _parameter.ToString() + "_" + _index.ToString();

            return PlayerPrefs.GetFloat(name);
        }


        /// <summary>
        /// Узнать значение параметра int
        /// </summary>
        /// <param name="_parameter">Тип параметра</param>
        /// <returns></returns>
        public static int Know_parameter(Type_parameter_value_int _parameter)
        {
            if (!PlayerPrefs.HasKey(nameof(Initialization_saving_bool)))
                Initialization_saving();

            string name = _parameter.ToString();

            return PlayerPrefs.GetInt(name);
        }



        /// <summary>
        /// Узнать параметр с int значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        /// <param name="_index">Номер ID</param>
        /// <returns></returns>
        public static int Know_parameter(Type_parameter_value_int _parameter, int _index)
        {
            if (!PlayerPrefs.HasKey(nameof(Initialization_saving_bool)))
                Initialization_saving();

            string name = _parameter.ToString() + "_" + _index.ToString();

            return PlayerPrefs.GetInt(name);
        }






        /// <summary>
        /// Узнать активность параметра
        /// </summary>
        /// <param name="_parameter">Тип параметра</param>
        /// <returns></returns>
        public static bool Know_parameter(Type_parameter_bool _parameter)
        {
            if (!PlayerPrefs.HasKey(nameof(Initialization_saving_bool)))
                Initialization_saving();

            bool bool_ = false;

            string name = _parameter.ToString();

            bool_ = PlayerPrefs.GetInt(name) == 1 ? true : false;


            return bool_;
        }


        /// <summary>
        /// Узнать параметр с bool значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        /// <param name="_index">Номер ID</param>
        /// <returns></returns>
        public static bool Know_parameter(Type_parameter_bool _parameter, int _index)
        {
            if (!PlayerPrefs.HasKey(nameof(Initialization_saving_bool)))
                Initialization_saving();

            bool bool_ = false;

            string name = _parameter.ToString() + "_" + _index.ToString();

            bool_ = PlayerPrefs.GetInt(name) == 1 ? true : false;

            return bool_;
        }
		
		
		/// <summary>
        /// Узнать параметр с bool значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        /// <param name="_name_id">ID имя (не номер) (не номер) для загрузки</param>
        /// <returns></returns>
        public static bool Know_parameter(Type_parameter_bool _parameter, string _name_id)
        {
            if (!PlayerPrefs.HasKey(nameof(Initialization_saving_bool)))
                Initialization_saving();

            bool bool_ = false;

            string name = _parameter.ToString() + "_" + _name_id;

            bool_ = PlayerPrefs.GetInt(name) == 1 ? true : false;

            return bool_;
        }

        /// <summary>
        /// Узнать параметр с vector3 значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        public static Vector3 Know_parameter(Type_parameter_vector3 _parameter)
        {
            Vector3 result = Vector3.zero;

            string name_X = _parameter.ToString() + "_" + "X";
            string name_Y = _parameter.ToString() + "_" + "Y";
            string name_Z = _parameter.ToString() + "_" + "Z";

            result.x = PlayerPrefs.GetFloat(name_X);
            result.y = PlayerPrefs.GetFloat(name_Y);
            result.z = PlayerPrefs.GetFloat(name_Z);

            return result;
        }

        /// <summary>
        /// Узнать параметр с vector3 значением
        /// </summary>
        /// <param name="_parameter">Параметр</param>
        /// <param name="_name_id">Имя ID (имя параметра)</param>
        public static Vector3 Know_parameter(Type_parameter_vector3 _parameter, string _name_id)
        {
            Vector3 result = Vector3.zero;

            string name_X = _parameter.ToString() + "_" + _name_id + "_" + "X";
            string name_Y = _parameter.ToString() + "_" + _name_id + "_" + "Y";
            string name_Z = _parameter.ToString() + "_" + _name_id + "_" + "Z";

            result.x = PlayerPrefs.GetFloat(name_X);
            result.y = PlayerPrefs.GetFloat(name_Y);
            result.z = PlayerPrefs.GetFloat(name_Z);

            return result;
        }



    }
}