//���������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Events;
using Lean.Pool;

namespace Sych_scripts
{
    //[AddComponentMenu("Sych scripts / Game / Weapon / Firearm")]
    [DisallowMultipleComponent]
    public class Firearm : Weapon_abstract, I_reload
    {
        #region ������� ����������
        [Space(10)]
        [Header("��������� ������")]
        [Tooltip("����")]
        [SerializeField]
        [Min(0)]
        protected int Damage = 1;

        [Tooltip("����������������")]
        [SerializeField]
        float Fire_rate = 0.1f;

        [Tooltip("�������� �������")]
        [SerializeField]
        float Projectile_speed = 350f;




        [Space(10)]
        [Tooltip("������ ������� ����������")]
        [SerializeField]
        bool Charged_shot_bool = false;

        [ShowIf(nameof(Charged_shot_bool))]
        [Tooltip("����� �� ������� ������� ��������� �� ������ ��������")]
        [SerializeField]
        float Charged_shot_time = 2f;

        [ShowIf(nameof(Charged_shot_bool))]
        [Tooltip("�� ������� ������ ������� ������� ��������")]
        [SerializeField]
        int Charged_shot_step = 3;

        int Charged_shot_step_active = 0;//�������� ������������ ������� ���� ������

        [ShowIf(nameof(Charged_shot_bool))]
        [Tooltip("���� ������� ����� �������� ������, �� ������� ����� ����� �������� � ����������� �� ������?")]
        [SerializeField]
        Vector2 Step_Fraction_count_projectile = new Vector2(1,5);

        [ShowIf(nameof(Charged_shot_bool))]
        [Tooltip("������� ����� ����� � ����������� �� ������?")]
        [SerializeField]
        Vector2 Step_Damage = new Vector2(1, 3);

        float Charged_shot_value_active = 0;//�������� ������� ����� ��� �������� ������������



        [Space(10)]
        [Tooltip("�������� ����� �������� ��� ��������")]
        [SerializeField]
        bool Ricochet_bool = false;

        [ShowIf(nameof(Ricochet_bool))]
        [Tooltip("���������� ���������")]
        [SerializeField]
        int Count_ricochet = 3;

        [ShowIf(nameof(Ricochet_bool))]
        [Tooltip("��������� ����")]
        [SerializeField]
        float Distance_ray = 1000f;

        [ShowIf(nameof(Ricochet_bool))]
        [Tooltip("����������� ��� ������ ���������� ��������")]
        [SerializeField]
        LineRenderer Trail = null;

        [ShowIf(nameof(Ricochet_bool))]
        [Tooltip("���� � ������� ��������������� ���")]
        [SerializeField]
        LayerMask Layer = 1;




        [Space(10)]
        [Tooltip("�������� �������")]
        [SerializeField]
        bool Spread_bool = false;

        [ShowIf(nameof(Spread_bool))]
        [Tooltip("���� �������� ��� �������� (����������� � ������������)")]
        [SerializeField]
        Vector2 Spread = new Vector2(0, 1f);

        [ShowIf(nameof(Spread_bool))]
        [Tooltip("� ����� ��������� ����������� ���� ��������")]
        [SerializeField]
        float Spread_speed_up = 0.2f;

        [ShowIf(nameof(Spread_bool))]
        [Tooltip("� ����� ��������� ����������� ���� ��������")]
        [SerializeField]
        float Spread_speed_down = 0.01f;



        [Space(10)]
        [Tooltip("�������� ����� �������� � �������������� ������� (�������� ��� ����������� �������� � ��������)")]
        [SerializeField]
        bool Use_shutter_bool = false;

        bool Shutter_bool = true;//��������� �� "������"

        [ShowIf(nameof(Use_shutter_bool))]
        [Tooltip("�������������� ����� ��, �� ��� �� ������ �� ����� �������� ����� ������������� ������� (�����, ��� ����, ��� �� �� ������� ��� � ��������������� ���������)")]
        [SerializeField]
        float Additional_time_stop_shutter_reload = 0.2f;

        [ShowIf(nameof(Use_shutter_bool))]
        [Tooltip("������� ����������� � ������ ������������� ������� (� �������� ��� ������� ��������, ������� � ����� ����������� End_shutter_reload)")]
        [SerializeField]
        UnityEvent Start_shutter_reload_event = new UnityEvent();



        [Space(10)]
        [Tooltip("�������� �������� ������")]
        [SerializeField]
        bool Fraction_shot_bool = false;

        [ShowIf(nameof(Fraction_shot_bool))]
        [Tooltip("���������� �������� � �����")]
        [SerializeField]
        int Fraction_count_projectile = 5;




        [Space(10)]
        [Tooltip("�������� ������� (�� ����, ������ �� ����� �������� ����������)")]
        [SerializeField]
        bool Ammo_bool = false;

        [ShowIf(nameof(Ammo_bool))]
        [Tooltip("���������� �������� � ����� ������")]
        [SerializeField]
        int Max_count_ammo = 20;

        int Active_count_ammo = 0;

        [field: ShowIf(nameof(Ammo_bool))]
        [field: Tooltip("����������� �� 1 �������")]
        [field: SerializeField]
        protected bool Single_ammo_reload_bool { get; private set; } = false;

        [ShowIf(nameof(Single_ammo_reload_bool))]
        [Tooltip("������� ����������� � ����� ����������� �� 1 ������� (������� ������, ������� � ����� ����������� End_reload)")]
        [SerializeField]
        UnityEvent End_Single_ammo_reload_event = new UnityEvent();

        [ShowIf(nameof(Ammo_bool))]
        [Tooltip("�������� �������� �������� ��� ������ (��� �� ����� ���� ������������ ����������)")]
        [SerializeField]
        bool Magazine_bool = false;

        [ShowIf(nameof(Magazine_bool))]
        [Tooltip("������� ����� ��������� �������� ������� � ����� ������")]
        [SerializeField]
        int Max_count_magazine = 10;

        int Active_count_magazine = 0;

        [Tooltip("���� ������������ ����")]
        [SerializeField]
        [Min(0)]
        protected float Force_Repulsion = 5000f;

        [ShowIf(nameof(Use_shutter_bool))]
        [Tooltip("�������������� ����� ��, �� ��� �� ������ �� ����� �������� ����� ����������� (�����, ��� ����, ��� �� �� ������� ��� � ��������������� ���������)")]
        [SerializeField]
        float Additional_time_stop_reload = 0.2f;

        [ShowIf(nameof(Magazine_bool))]
        [Tooltip("������� ����������� � ������ ����������� (� �������� ��� ������� ��������, ������� � ����� ����������� End_reload)")]
        [SerializeField]
        UnityEvent Start_reload_event = new UnityEvent();

        bool Reload_bool = true;//�����������


        [Space(10)]
        [Tooltip("������ ����")]
        [SerializeField]
        protected Projectile_abstract Projectile_prefab = null;

        [Tooltip("��� ������� ���� ������� (�� �����������)")]
        [SerializeField]
        Game_character_abstract Host = null;

        #endregion


        #region �������������� ����������
        [Tooltip("����� ������ ����")]
        [SerializeField]
        protected Transform Fire_point = null;

        [Tooltip("�������������� ����� (��� �� ����� ���� ������ ������ � ��������)")]
        [SerializeField]
        protected bool Automatical_bool = false;

        [Tooltip("������� ����� ������ �� � ����������� ������, � � �������� �������")]
        [SerializeField]
        bool Aim_image_bool = true;

        #endregion


        #region ���������� ��� ������ ������ �������
        bool Press_bool = false;

        bool Fire_On_bool = true;//����������� �������� (����� ��� �������� ����������������)

        float Spread_active = 0;//��� ������ � ���������

        Coroutine Detect_press_attack_coroutine = null;//��� ������ � ���������� �������� (��� �� �� ���� ���������� ��������, ���� ������ ����� ��� ������ �������� ������������)

        bool Detect_press_bool = false;//����������� �� ��������� ������� ������ (��� �� �������������� ��������. �� ���� ��� ������� ��������� ������ 1 ���)

        Quaternion Default_rotation_Fire_point = Quaternion.identity;//����������� �������� ����� ������ ���� ��� ������

        protected int Attack_mode_id = 0;//��� ������������ ���� ����� ������

        [field: Tooltip("���� Game_administrator, ��������� �� ������ ���������� ��������� ��������")]
        [field: SerializeField]
        protected bool No_Game_administrator_bool { get; private set; } = false;

        [ShowIf(nameof(No_Game_administrator_bool))]
        [Tooltip("������")]
        [SerializeField]
        Camera Cam = null;

        protected Vector3 Finale_point = Vector3.zero;//����� ���� �������� �������
        #endregion



        #region ��������� ������

        private void OnEnable()
        {

            UI_weapon_administrator UI = UI_weapon_administrator.Singleton_Instance;

            UI.Activity(Ammo_bool, Magazine_bool, Charged_shot_bool);
            UI.Activity_Aim(true);
            Preparation_reload();
            Preparation_shutter();
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

        private void Start()
        {
            if (Game_administrator.Singleton_Instance)
                Cam = Game_administrator.Singleton_Instance.Player_administrator.Cam;

            Default_rotation_Fire_point = Fire_point.localRotation;

            if (Spread_bool)
                Spread_active = Spread.x;
            else
                Spread_active = 0;

            if (Ammo_bool)
            {
                Active_count_ammo = Max_count_ammo;
                Active_count_magazine = Max_count_magazine;
            }

            Update_UI();
        }

        private void Update()
        {
            if (Spread_bool)
            {
                if (!Detect_press_bool)
                    Spread_down();
            }

            if (Ricochet_bool) 
            {
                if (Trail)
                {
                    List<Vector3> point_list = Get_ricochet_points(transform.position, transform.forward);

                    if (Trail.positionCount != point_list.Count)
                        Trail.positionCount = point_list.Count;

                    Trail.SetPositions(Game_calculator.Convert_from_List_to_Array(point_list));
                }
            }
        }

        #endregion


        #region ������
        /// <summary>
        /// ������������ ������ (��������� �����)
        /// </summary>
        [ContextMenu(nameof(Active))]
        void Active()
        {

            if (Charged_shot_bool)
            {
                if (Detect_press && Detect_press_bool)
                {
                    if(Charged_shot_value_active < 1)
                    Charger();
                    else if (Charged_shot_value_active >= 1 && Automatical_bool)
                    {
                        Fire();
                        Charged_shot_step_active = 0;
                        Charged_shot_value_active = 0;
                    }
                }

                if (!Press_bool)
                {
                    if(Charged_shot_step_active > 0)
                    Fire();

                    Charged_shot_step_active = 0;
                    Charged_shot_value_active = 0;
                }

                Update_UI();
            }
            else
            {
                Fire();
            }
        }

        /// <summary>
        /// ������� ��������
        /// </summary>
        void Charger()
        {
            if(Charged_shot_time >= 1)
                Charged_shot_value_active += Time.deltaTime / Charged_shot_time;
            else
                Charged_shot_value_active += (1 - Charged_shot_value_active);

            Charged_shot_value_active = Mathf.Clamp(Charged_shot_value_active, 0f, 1f);
            Update_UI();

            Charged_shot_step_active = Mathf.RoundToInt((float)Charged_shot_step * Charged_shot_value_active);
        }


        /// <summary>
        /// �������� ����� ��������
        /// </summary>
        /// <returns></returns>
        List<Vector3> Get_ricochet_points(Vector3 _start_point, Vector3 _start_direction)
        {
            List<Vector3> point_list = new List<Vector3>();

            RaycastHit hit = new RaycastHit();

            Vector3 start_point = _start_point;

            Vector3 end_point = _start_point + _start_direction * Distance_ray;

            Vector3 direction = transform.forward;

            point_list.Add(_start_point);

            for (int x = 0; x < Count_ricochet; x++)
            {
                if (Physics.Raycast(start_point, direction, out hit, Distance_ray, Layer))
                {
                    end_point = hit.point;

                    start_point = hit.point;
                    direction = Vector3.Reflect(direction, hit.normal);

                    point_list.Add(hit.point);
                }
                else
                {
                    end_point = start_point + direction * Distance_ray;

                    point_list.Add(end_point);

                    break;
                }
            }

            return point_list;
        }


        /// <summary>
        /// ����������
        /// </summary>
        void Fire()
        {
            if (Check_fire_possible && Reload_bool)
            {
                if (Fraction_shot_bool)
                {
                    int count = Charged_shot_bool ? Mathf.CeilToInt(Mathf.Lerp((float)Step_Fraction_count_projectile.x, (float)Step_Fraction_count_projectile.y, Charged_shot_value_active)) : Fraction_count_projectile;

                    for (int x = 0; x < count; x++)
                    {
                        Spawn_projectile_and_spread();
                    }
                }
                else
                {
                    Spawn_projectile_and_spread();
                }

                if (Spread_bool)
                    Spread_active = Mathf.MoveTowards(Spread_active, Spread.y, Spread_speed_up);

                if (Use_shutter_bool && Shutter_bool)
                {
                    Shutter_bool = false;
                    Shutter_reload();
                }

                Active_count_ammo--;
                Update_UI();

                if (Magazine_bool)
                {
                    if (Active_count_ammo <= 0)
                    {
                        Reload();
                    }
                }

            }
        }


        /// <summary>
        /// ������� ������
        /// </summary>
        void Spawn_projectile_and_spread()
        {
            List<Vector3> ricochet_points_list = new List<Vector3>();

            if (Aim_image_bool)
            {
                Vector3 point_screen_point = Vector3.zero;

                Image aim = UI_weapon_administrator.Singleton_Instance.Aim_image;

                point_screen_point = Aim_spread_random_point(aim);

                Ray ray = Cam.ScreenPointToRay(point_screen_point);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Finale_point = hit.point;
                }
                else
                {
                    Finale_point = ray.direction * 4000f;
                }

                Vector3 direction = (Finale_point - Fire_point.position).normalized;
                Quaternion new_rotation = Quaternion.LookRotation(direction, Fire_point.up);

                Fire_point.transform.rotation = new_rotation;

                if (Ricochet_bool)
                    ricochet_points_list = Get_ricochet_points(Fire_point.position, direction);
            }
            else
            {
                Fire_point.transform.localRotation = Default_rotation_Fire_point;

                if(Spread_bool)
                Fire_point.transform.eulerAngles += new Vector3(Random.Range(-Spread_active, Spread_active), Random.Range(-Spread_active, Spread_active), Fire_point.transform.localRotation.z);

                if (Ricochet_bool)
                    ricochet_points_list = Get_ricochet_points(Fire_point.position, Fire_point.forward);
            }

            int damage = Charged_shot_bool ? Mathf.CeilToInt(Mathf.Lerp((float)Step_Damage.x, (float)Step_Damage.y, Charged_shot_value_active)) : Damage;

            if(!Ricochet_bool)
               LeanPool.Spawn(Projectile_prefab, Fire_point.position, Fire_point.rotation).Specify_settings(damage, Projectile_speed, Force_Repulsion, Host);
            else
            {
              Bullet projectile = (Bullet)LeanPool.Spawn(Projectile_prefab, Fire_point.position, Fire_point.rotation);
                projectile.Specify_settings(damage, 0, Force_Repulsion, Host);
                projectile.Move_way(Game_calculator.Convert_from_List_to_Array(ricochet_points_list), Projectile_speed);

            }

        }


        /// <summary>
        /// �������� ��������� ����� � �������� �������
        /// </summary>
        public Vector3 Aim_spread_random_point(Image _aim)
        {
            Vector3 point = _aim.rectTransform.position;//new Vector3(Random.Range(-Aim_image.rectTransform.sizeDelta.x, Aim_image.rectTransform.sizeDelta.x), Random.Range(-Aim_image.rectTransform.sizeDelta.y, Aim_image.rectTransform.sizeDelta.y), Aim_image.rectTransform.position.z);

            point.x += Random.Range(-_aim.rectTransform.sizeDelta.x / 2 * Spread_active, _aim.rectTransform.sizeDelta.x / 2 * Spread_active);
            point.y += Random.Range(-_aim.rectTransform.sizeDelta.y / 2 * Spread_active, _aim.rectTransform.sizeDelta.y / 2 * Spread_active);

            return point;
        }


        /// <summary>
        /// ��������� ����� �� ������� �������
        /// </summary>
        bool Check_fire_possible
        {
            get
            {
                bool result = false;

                //Detect_press();

                if (Detect_press && Fire_On_bool)
                {

                    //if (Automatical_bool || !Detect_press_bool)
                        if (!Use_shutter_bool || Use_shutter_bool && Shutter_bool) 
                        {
                            if (!Ammo_bool)
                            {
                                result = true;
                            }
                            else if (Check_ammo(1))
                            {
                                result = true;
                            }
                        }
                        //��� ������ ���� Detect_press_bool = true;
                        Fire_On_bool = false;
                        Invoke(nameof(Fire_rate_method), Fire_rate);
                }
                return result;
            }
        }

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

                if ((Automatical_bool || Charged_shot_bool) || !Detect_press_bool)
                {
                    Detect_press_bool = true;
                    result = true;
                }
                
                return result;
            }
        }

        /// <summary>
        /// ��������� ������� �������� � ������
        /// </summary>
        /// <param name="_cost_shot">��������� ���������� �������� �� �������</param>
        /// <returns></returns>
        bool Check_ammo(int _cost_shot)
        {
            bool result = false;

            if(Active_count_ammo >= _cost_shot)
            {
                result = true;
            }
            else if (Magazine_bool)
            {
                if (Active_count_magazine > 0)
                {
                    Reload();
                }
            }

            return result;
        }

        /// <summary>
        /// ���������� ��������
        /// </summary>
        void Spread_down()
        {
            if(Spread_active != Spread.x)
            Spread_active = Mathf.MoveTowards(Spread_active, Spread.x, Spread_speed_down);
        }

        IEnumerator Coroutine_press_detect_attack()//����� ��� ����, ��� �� ����� ������� ������ ��� ������� (�� ���������)
        {
            yield return null;
            Detect_press_bool = false;
        }


        /// <summary>
        /// ������������� �� ����������� �����������
        /// </summary>
        void Preparation_reload()
        {
            if (!Reload_bool)
            {
                Reload_bool = true;

                if (Active_count_ammo <= 0)
                {
                    Reload();
                }

            }
        }

        /// <summary>
        /// ��������� ���������������� (����� ��������� �� ���� ������)
        /// </summary>
        void Preparation_shutter()
        {
            if (Use_shutter_bool)
            {
                if (!Shutter_bool)
                {
                    Shutter_reload();
                }
            }
        }

        /// <summary>
        /// ��������� �������� ����� �������� (��� ��������� ����������������)
        /// </summary>
        void Fire_rate_method()
        {
            Fire_On_bool = true;
        }

        /// <summary>
        /// �������������� ����� ��, �� ��� �� ������ �� ����� �������� ����� ������������� ������� (�����, ��� ����, ��� �� �� ������� ��� � ��������������� ���������)
        /// </summary>
        void Additional_time_shutter_reload_method()
        {
            Shutter_bool = true;
        }

        /// <summary>
        /// �������������� ����� ��, �� ��� �� ������ �� ����� �������� ����� �����������(�����, ��� ����, ��� �� �� ������� ��� � ��������������� ���������)
        /// </summary>
        void Additional_time_reload_method()
        {
            Reload_bool = true;

            if (Use_shutter_bool && !Shutter_bool)
            {
                Shutter_reload();
            }
        }

        /// <summary>
        /// ������ ������������� �������
        /// </summary>
        void Shutter_reload()
        {
            Start_shutter_reload_event.Invoke();
        }


        /// <summary>
        /// ������ �����������
        /// </summary>
        void Reload()
        {
            if (Active_count_magazine > 0 && Magazine_bool && Active_count_ammo < Max_count_ammo && Reload_bool)
            {
                Reload_bool = false;
                Start_reload_event.Invoke();
            }
        }

        /// <summary>
        /// �������� ��������� � ����������
        /// </summary>
        void Update_UI()
        {
            UI_weapon_administrator UI = UI_weapon_administrator.Singleton_Instance;
            UI.Update_Ammo_value(Active_count_ammo);
            UI.Update_Magazine_value(Active_count_magazine);
            UI.Update_Charged_value(Charged_shot_value_active);
        }

        #endregion


        #region ��������� ������

        /// <summary>
        /// ������� ����� �����
        /// </summary>
        /// <param name="_id_mode">����� �����</param>
        public void Mode_attack(int _id_mode)
        {
            Attack_mode_id = _id_mode;
        }

        public override void Attack(bool _activity)
        {
            Press_bool = _activity;
            Active();
        }

        /// <summary>
        /// ����� �����������, ����� ��������� �������
        /// </summary>
        public void End_reload()
        {
            if (Active_count_magazine > 0)
            {
                Active_count_ammo = Max_count_ammo;
                Active_count_magazine--;
                Invoke(nameof(Additional_time_reload_method), Additional_time_stop_reload);
                Update_UI();
            }
        }

        /// <summary>
        /// ����� ������������� "�������"
        /// </summary>
        public void End_reload_shutter()
        {
            Invoke(nameof(Additional_time_shutter_reload_method), Additional_time_stop_shutter_reload);
        }

        /// <summary>
        /// ����� ��������� 1 �������
        /// </summary>
        public void End_Add_single_ammo_reload()
        {
            if (Active_count_ammo >= Max_count_ammo || Active_count_magazine <= 0)
            {
                End_Single_ammo_reload_event.Invoke();
                Shutter_reload();
                Reload_bool = true;
            }
            else if (Active_count_ammo < Max_count_ammo && Active_count_magazine > 0)
            {
                Active_count_ammo++;
                Active_count_magazine--;

                Reload_bool = true;
                Shutter_bool = true;
                Update_UI();
            }

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

        void I_reload.Reload()
        {
            Reload();
        }
        #endregion
    }
}