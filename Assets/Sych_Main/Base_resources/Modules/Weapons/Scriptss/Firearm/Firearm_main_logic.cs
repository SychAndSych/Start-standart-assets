using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Weapon / Firearm")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Firearm_reload_logic))]
    [RequireComponent(typeof(Firearm_shutter_logic))]
    [RequireComponent(typeof(Charged))]
    [RequireComponent(typeof(Firearm_fire_logic))]
    public class Firearm_main_logic : Weapon_abstract, I_reload
    {
        #region Переменные


        [Tooltip("Скорострельность")]
        [SerializeField]
        float Fire_rate = 0.1f;

        bool Fire_rate_On_bool = true;//Разрешается стрелять (нужно для создания скорострельности)



        [Tooltip("Автоматический огонь (что бы можно было зажать кнопку и стрелять)")]
        [SerializeField]
        protected bool Automatical_bool = false;




        [field: Tooltip("Скрипт отвечающия за перезарядку оружия")]
        //[field: SerializeField]
        public Firearm_reload_logic Firearm_reload_logic_script { get; private set; } = null;

        [field: Tooltip("Скрипт отвечающия за дополнительные условия оружия")]
        //[field: SerializeField]
        public Firearm_shutter_logic Firearm_shutter_logic_script { get; private set; } = null;

        [field: Tooltip("Скрипт отвечающия за выстрел оружия")]
        //[field: SerializeField]
        public Firearm_fire_logic Firearm_fire_logic_script { get; private set; } = null;

        [field: Tooltip("Скрипт отвечающия за зарядку")]
        //[field: SerializeField]
        public Charged Charged_script { get; private set; } = null;

        bool Press_bool = false;//Отвечает зажата ли кнопка атаки

        Coroutine Detect_press_attack_coroutine = null;//Для работы с постоянным нажатием (что бы не была постоянная стрельба, если оружие нужно при каждом выстреле прокликивать)

        internal bool Detect_press_bool { get; private set; } = false;//Разрешается ли детектить нажатие игрока (для не автоматической стрельбы. То есть при зажатие выстрелит только 1 раз)

        float Charged_value = 0;

        #endregion


        #region Системные методы

        protected override void Awake()
        {
            base.Awake();

            Firearm_reload_logic_script = GetComponent<Firearm_reload_logic>();
            Firearm_shutter_logic_script = GetComponent<Firearm_shutter_logic>();
            Firearm_fire_logic_script = GetComponent<Firearm_fire_logic>();
            Charged_script = GetComponent<Charged>();


            Firearm_reload_logic_script.Firearm_main_logic_script = this;
        }

        private void OnEnable()
        {
            if (UI_weapon_administrator.Singleton_Instance)
            {
                UI_weapon_administrator.Singleton_Instance.Activity(Firearm_reload_logic_script.Active_ammo_bool, Firearm_reload_logic_script.Active_magazine_bool, Charged_script.Active_charged_bool);
                UI_weapon_administrator.Singleton_Instance.Activity_Aim(true);
            }
            Update_UI();
        }

        private void OnDisable()
        {
            if (UI_weapon_administrator.Singleton_Instance) 
            {
                UI_weapon_administrator.Singleton_Instance.Activity(false, false, false);
                UI_weapon_administrator.Singleton_Instance.Activity_Aim(false);
            }
        }


        void FixedUpdate()
        {
            if(Press_bool)
            Active();
        }

        #endregion


        #region Методы
        /// <summary>
        /// Активировать оружие (стартовый метод)
        /// </summary>
        [ContextMenu(nameof(Active))]
        void Active()
        {
                if (Check_fire_possible)
                {
                    if (!Charged_script.Active_charged_bool)
                    {
                    if (Automatical_bool || Detect_press) 
                    {
                        Charged_value = 1;
                        Fire();
                    }
                    }
                    else
                    {
                        if (Press_bool)
                        {
                            if (Charged_script.Charged_shot_value_active < 1)
                                Charged_script.Charger();
                            else if (Charged_script.Charged_shot_value_active >= 1 && Automatical_bool)
                            {
                            Charged_value = Charged_script.Charged_shot_value_active;
                            Fire();
                            Charged_script.Reset_method();
                            }
                        Update_UI();
                        }

                        if (!Press_bool)
                        {
                        if (Charged_script.Charged_shot_step_active > 0)
                        {
                            Charged_value = Charged_script.Charged_shot_value_active;
                            Fire();
                        }
                        Charged_script.Reset_method();
                        Update_UI();
                    }
                    }
                }
        }

        /// <summary>
        /// Выстрелить
        /// </summary>
        void Fire()
        {
            if (Charged_value != 0)
            {
                Firearm_reload_logic_script.Remove_ammo(Firearm_fire_logic_script.Check_price_ammo(Charged_value));
                Firearm_fire_logic_script.Fire(Charged_value);
                Firearm_shutter_logic_script.Shutter_reload();
                Fire_rate_Start_reset();
            }
        }

        /// <summary>
        /// Проверяет можно ли сделать выстрел (проверяет все условия)
        /// </summary>
        bool Check_fire_possible
        {
            get
            {
                bool result = false;
                
                if (Fire_rate_On_bool &&
                    (!Charged_script.Active_charged_bool && Firearm_reload_logic_script.Check_ammo(Firearm_fire_logic_script.Check_price_ammo(1)) || Charged_script.Active_charged_bool && Firearm_reload_logic_script.Check_ammo(Firearm_fire_logic_script.Check_price_ammo(Charged_script.Charged_shot_value_active))) &&
                    Firearm_shutter_logic_script.Shutter_bool)
                {
                    result = true;
                }
                return result;
            }
        }


        /// <summary>
        /// Проверяет на индивудуальность нажатия кнопки атаки (то есть использовалось ли это нажатие или это продолжения зажатой кнопки)
        /// </summary>
        bool Detect_press
        {
            get
            {
                bool result = false;

                if (Detect_press_attack_coroutine != null)
                {
                    StopCoroutine(Detect_press_attack_coroutine);
                    Detect_press_attack_coroutine = null;
                }

                Detect_press_attack_coroutine = StartCoroutine(Coroutine_press_detect_attack());

                if (!Detect_press_bool)
                {
                    Detect_press_bool = true;
                    result = true;
                }

                return result;
            }
        }

        /// <summary>
        /// Начать паузу связанную со скорострельностью
        /// </summary>
        void Fire_rate_Start_reset()
        {
            if (Fire_rate_On_bool) 
            {
                Fire_rate_On_bool = false;
                Invoke(nameof(Fire_rate_end_method), Fire_rate);
            }
        }

        /// <summary>
        /// Нужно для того, что бы игрок стрелял только при нажатие (не удержание)
        /// </summary>
        /// <returns></returns>
        IEnumerator Coroutine_press_detect_attack()
        {
            yield return null;
            Detect_press_bool = false;
        }


        /// <summary>
        /// Разрешает стрельбу после задержки (для задавания скорострельности)
        /// </summary>
        void Fire_rate_end_method()
        {
            Fire_rate_On_bool = true;
        }

        #endregion


        #region Публичные методы

        /// <summary>
        /// Обновить показания в интерфейсе
        /// </summary>
        public void Update_UI()
        {
            if (UI_weapon_administrator.Singleton_Instance)
            {
                UI_weapon_administrator UI = UI_weapon_administrator.Singleton_Instance;
                UI.Update_Ammo_value(Firearm_reload_logic_script.Active_count_ammo);
                UI.Update_Magazine_value(Firearm_reload_logic_script.Active_count_magazine);
                UI.Update_Charged_value(Charged_script.Charged_shot_value_active);
            }
        }


        public override void Attack(bool _activity)
        {
            Press_bool = _activity;
        }

        public override void End_attack()
        {
            throw new System.NotImplementedException();
        }

        public override void Activity(bool _activity)
        {
            //throw new System.NotImplementedException();
        }

        protected override void Initialized_stats()
        {
            //throw new System.NotImplementedException();
        }

        public void Reload()
        {
            Firearm_reload_logic_script.Reload();
        }
        #endregion
    }
}