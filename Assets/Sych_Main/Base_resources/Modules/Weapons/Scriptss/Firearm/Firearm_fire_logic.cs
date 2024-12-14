using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Lean.Pool;
using UnityEngine.Events;

namespace Sych_scripts
{
    [RequireComponent(typeof(Firearm_spread_logic))]
    [DisallowMultipleComponent]
    public class Firearm_fire_logic : MonoBehaviour
    {
        #region Переменные

        [Space(10)]
        [Header("Параметры оружия")]

        [Tooltip("Включить режим в котором будет зависить параметры от степени зарядки оружия")]
        [SerializeField]
        bool Charged_bool = false;

        [HideIf(nameof(Charged_bool))]
        [Tooltip("Урон")]
        [SerializeField]
        [Min(0)]
        protected int Damage = 1;

        [ShowIf(nameof(Charged_bool))]
        [Tooltip("Урон зависящий от Зарядки оружия")]
        [SerializeField]
        Vector2 Damage_charged = Vector2.one;

        [HideIf(nameof(Charged_bool))]
        [Tooltip("Скорость снаряда")]
        [SerializeField]
        float Projectile_speed = 40f;

        [ShowIf(nameof(Charged_bool))]
        [Tooltip("Скорость снаряда зависящий от Зарядки оружия")]
        [SerializeField]
        Vector2 Projectile_speed_charged = new Vector2(1, 40);

        [HideIf(nameof(Charged_bool))]
        [Tooltip("Сколько тратит патронов за выстрел")]
        [SerializeField]
        int Remove_ammo_value = 1;

        [ShowIf(nameof(Charged_bool))]
        [Tooltip("Сколько тратит патронов за выстрел зависящий от Зарядки оружия")]
        [SerializeField]
        Vector2 Remove_ammo_value_charged = Vector2.one;


        [HideIf(nameof(Charged_bool))]
        [Tooltip("Количество снарядов в выстреле")]
        [SerializeField]
        int Fraction_count_projectile = 1;

        [ShowIf(nameof(Charged_bool))]
        [Tooltip("Количество снарядов в выстреле зависящий от Зарядки оружия")]
        [SerializeField]
        Vector2 Fraction_count_projectile_charged = Vector2.one;

        [HideIf(nameof(Charged_bool))]
        [Tooltip("Сила отбрасывания цели")]
        [SerializeField]
        [Min(0)]
        protected float Force_Repulsion = 5000f;

        [ShowIf(nameof(Charged_bool))]
        [Tooltip("Сила отбрасывания цели зависящий от Зарядки оружия")]
        [SerializeField]
        Vector2 Force_Repulsion_charged = new Vector2(500f, 5000f);


        [Space(10)]
        [Tooltip("Префаб пули")]
        [SerializeField]
        protected Projectile_abstract Projectile_prefab = null;

        [Tooltip("Кто владеет этим оружием (не обязательно)")]
        [SerializeField]
        Game_character_abstract Host = null;

        [field: Tooltip("Точка спавна пули")]
        [field: SerializeField]
        public Muzzle_class[] Muzzle_array { get; private set; } = new Muzzle_class[1];

        int Id_active_muzzle = 0;

        [field: Tooltip("Нету Game_administrator, переходим на ручное управление задавание настроек")]
        [field: SerializeField]
        protected bool No_Game_administrator_bool { get; private set; } = false;

        [Tooltip("Поворачивать точку стрельбы в сторону камеры (нужно для корректной стрельбы от первого лица)")]
        [SerializeField]
        protected bool Loock_at_camera_rotatin_bool = true;

        [ShowIf(nameof(No_Game_administrator_bool))]
        [Tooltip("Камера")]
        [SerializeField]
        Camera Cam = null;

        protected Vector3 Finale_point = Vector3.zero;//Точка куда попадает выстрел


        [field: Tooltip("Скрипт отвечающия за разброс при выстреле оружия")]
        //[field: SerializeField]
        public Firearm_spread_logic Firearm_spread_logic_script { get; private set; } = null;


        float Value_charged = 1;

        float Step_charged = 1;

        #endregion


        #region Системные методы
        private void Awake()
        {
            Firearm_spread_logic_script = GetComponent<Firearm_spread_logic>();
        }

        private void Start()
        {
            if (Game_administrator.Singleton_Instance)
                Cam = Game_administrator.Singleton_Instance.Player_administrator.Cam;
        }

        #endregion


        #region Методы

        void Fire_method()
        {
            if (Charged_bool)
            {
                Damage =  Mathf.RoundToInt(Mathf.Lerp(Damage_charged.x, Damage_charged.y, Value_charged));
                Fraction_count_projectile = Mathf.RoundToInt(Mathf.Lerp(Fraction_count_projectile_charged.x, Fraction_count_projectile_charged.y, Value_charged));
                Projectile_speed = Mathf.RoundToInt(Mathf.Lerp(Projectile_speed_charged.x, Projectile_speed_charged.y, Value_charged));
                Force_Repulsion = Mathf.RoundToInt(Mathf.Lerp(Force_Repulsion_charged.x, Force_Repulsion_charged.y, Value_charged));
            }


            for (int x = 0; x < Fraction_count_projectile; x++)
            {
                if(Loock_at_camera_rotatin_bool)
                    Muzzle_array[Id_active_muzzle].Fire_point.LookAt(Firearm_spread_logic_script.Get_fin_point_Camera_Raycast(Cam, Muzzle_array[Id_active_muzzle].Fire_point));

                Projectile_abstract projectile = LeanPool.Spawn(Projectile_prefab, Muzzle_array[Id_active_muzzle].Fire_point.position, Muzzle_array[Id_active_muzzle].Fire_point.rotation);
                projectile.Specify_settings(Damage, Projectile_speed, Force_Repulsion, Host);
            }

            Muzzle_array[Id_active_muzzle].Fire_event.Invoke();
            Id_active_muzzle = Id_active_muzzle + 1 < Muzzle_array.Length ? Id_active_muzzle + 1 : 0;
        }

        #endregion


        #region Публичные методы

        /// <summary>
        /// Выстрелить
        /// </summary>
        public void Fire()
        {
            Value_charged = 1;
            Fire_method();
        }

        public void Fire(float _value_charged)
        {
            Value_charged = _value_charged;
            Fire_method();
        }

        public void Fire(int _step_charged)
        {
            Step_charged = _step_charged;
            Fire_method();
        }


        /// <summary>
        /// Узнать сколько патронов будет тратить оружие за выстрел
        /// </summary>
        public int Check_price_ammo (float _value_charged)
        {
                int result = 1;

                if (Charged_bool)
                    result = Mathf.RoundToInt(Mathf.Lerp(Remove_ammo_value_charged.x, Remove_ammo_value_charged.y, _value_charged));
                else
                    result = Remove_ammo_value;

                return result;
        }
        #endregion


        #region
        [System.Serializable]
        public class Muzzle_class
        {
            [field: Tooltip("Точка спавна пули")]
            [field: SerializeField]
            public Transform Fire_point { get; private set; } = null;

            [Tooltip("Ивент когда происходит выстрел из этого дула")]
            [SerializeField]
            public UnityEvent Fire_event = new UnityEvent();
        }

        #endregion
    }
}