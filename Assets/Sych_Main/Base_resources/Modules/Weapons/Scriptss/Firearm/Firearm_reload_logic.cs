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
        #region ѕеременные

        [field: Space(10)]
        [field: Tooltip("¬ключить патроны (то есть, оружие не будет стрел€ть бесконечно)")]
        [field: SerializeField]
        public bool Active_ammo_bool = false;

        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_ammo_bool))]
        [Tooltip(" оличество патронов в самом оружие")]
        [SerializeField]
        int Max_count_ammo = 20;

        internal int Active_count_ammo  = 0;

        [field: ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_ammo_bool))]
        [field: Tooltip("ѕерезар€дка по 1 патрону")]
        [field: SerializeField]
        protected bool Single_ammo_reload_bool = false;

        //[ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Single_ammo_reload_bool))]
        [Tooltip("—обытие относ€щиес€ к концу перезар€дки по 1 патрону (магазин полный, котора€ в конце задействует End_reload)")]
        [SerializeField]
        UnityEvent End_Single_ammo_reload_event = new UnityEvent();


        [field: ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_ammo_bool))]
        [field: Tooltip("¬ключить магазины патронов дл€ оружи€ (что бы можно было перезар€жать магазинами)")]
        [field: SerializeField]
        public bool Active_magazine_bool = false;

        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_magazine_bool), nameof(Active_ammo_bool))]
        [SerializeField]
        int Max_count_magazine = 10;

        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_magazine_bool), nameof(Active_ammo_bool))]
        [Tooltip("1 магазин равен полному боезапасу (если нет, то из показател€ магазина будет вычитатьс€ столько патронов, сколько нехватает в оружие)")]
        [SerializeField]
        bool Magazine_full_ammo_bool = false;

        internal int Active_count_magazine = 0;

        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_magazine_bool), nameof(Active_ammo_bool))]
        [Tooltip("ƒополнительное врем€ на, то что бы оружие не сразу стрел€ло после перезар€дки (нужно, дл€ того, что бы не словить баг с преждевременным выстрелом)")]
        [SerializeField]
        float Additional_time_stop_reload = 0.2f;

        //[ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Active_magazine_bool), nameof(Active_ammo_bool))]
        [Tooltip("—обытие относ€щиес€ к началу перезар€дки (в основном дл€ запуска анимации, котора€ в конце задействует End_reload)")]
        [SerializeField]
        UnityEvent Start_reload_event = new UnityEvent();

        internal Firearm_main_logic Firearm_main_logic_script = null;//√лавный св€зующий скрипт оружи€

        bool Reload_active_bool = false;//ѕерезар€жаетс€
        #endregion


        #region —истемные методы

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


        #region ћетоды

        /// <summary>
        /// ѕерепроверить на сбивчивость перезар€дки
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
        /// ƒополнительное врем€ на, то что бы оружие не сразу стрел€ло после перезар€дки(нужно, дл€ того, что бы не словить баг с преждевременным выстрелом)
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


        #region ѕубличные методы


        /// <summary>
        /// Ќачало перезар€дки
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
        /// ƒобавить дополнительные патроны/ количество магазинов
        /// </summary>
        /// <param name="_ammo"> оличество патронов</param>
        public void Add_magazine(int _ammo)
        {
            Active_count_magazine += _ammo;
            Active_count_magazine = Mathf.Clamp(Active_count_magazine, 0, Max_count_magazine);
            Firearm_main_logic_script.Update_UI();
        }

        /// <summary>
        /// ¬ычесть патроны
        /// </summary>
        /// <param name="_ammo"> оличество вычитаемых патронов</param>
        public void Remove_ammo(int _ammo)
        {
            Active_count_ammo -= _ammo;

            if (Active_count_ammo <= 0)
                Reload();

            Firearm_main_logic_script.Update_UI();
        }

        /// <summary>
        /// ѕровер€ет наличи€ патронов в оружие
        /// </summary>
        /// <param name="_cost_shot">—тоимость количества патронов за выстрел</param>
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
        ///  онец перезар€дки, можно начислить патроны
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
        ///  онец ƒозар€дки 1 патрона
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