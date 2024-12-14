//������������, ������ � ����������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Player / Player Motor")]
    [DisallowMultipleComponent]
    public class Player_Motor : MonoBehaviour
    {
        [Tooltip("������������ ������?")]
        [SerializeField]
        bool Physics_bool = false;

        [Tooltip("�������������� �� �������� (������� ��� 3-�� ����)")]
        [SerializeField]
        bool Tracking_cursor_bool = false;

        [ShowIf(nameof(Tracking_cursor_bool))]
        [Tooltip("���� ��� �������������� ����� ������������� ��������� �������")]
        [SerializeField]
        LayerMask Cursor_mask = 1;

        [HideIf(nameof(Physics_bool))]
        [Tooltip("CharacterController")]
        [SerializeField]
        CharacterController CharacterController_script = null;

        [ShowIf(nameof(Physics_bool))]
        [Tooltip("������")]
        [SerializeField]
        Rigidbody Body = null;

        [Tooltip("������������ ��������� (������� ������� ����� ��������������)")]
        [SerializeField]
        Transform Rotation_transform = null;

        [field: Tooltip("���� Game_administrator, ��������� �� ������ ���������� ��������� ��������")]
        [field: SerializeField]
        protected bool No_Game_administrator_bool { get; private set; } = false;

        [ShowIf(nameof(No_Game_administrator_bool))]
        [Tooltip("������")]
        [SerializeField]
        Camera Cam = null;

        [Tooltip("������������� ������������ �� ����� (��� ��������� �������� ����� �������������� ������ ��� ��������)")]
        [SerializeField]
        bool Auto_rotation_bool = true;

        [Min(0)]
        [Tooltip("��������� � ���������� ��� ������������")]
        [SerializeField]
        float SpeedChangeRate = 4.0f;

        [Min(0)]
        [Tooltip("����� ������ (�������������� ����� ��� ������ �� ������ ����� �� ��� ���� � ���������)")]
        [SerializeField]
        float Coyote_time = 0.2f;

        [Space(20)]
        [Header("����������")]

        [HideIf(nameof(Physics_bool))]
        [Tooltip("�������� ���������� ����������� �������� ����������. ���������� � ����� Unity �� ��������� -9.81f")]
        public float Gravity = -15.0f;

        [Min(0)]
        [HideIf(nameof(Physics_bool))]
        [Tooltip("�����, ����������� ��� �������� � ��������� �������. ������� ��� ������ �� �������� (� �� ������ ��������� �������� ������)")]
        public float Fall_Timeout = 0.15f;

        [HideIf(nameof(Physics_bool))]
        [MaxValue(0f)]
        [Tooltip("������������ �������� �������")]
        [SerializeField]
        float Max_fall_speed = -100f;



        [Space(20)]

        [Tooltip("��������� ������ �����������")]
        [SerializeField]
        bool Surface_slope_bool = false;

        [Min(0)]
        [ShowIf(nameof(Surface_slope_bool))]
        [Tooltip("��������� ���� ��� �������� ������� �����������")]
        [SerializeField]
        float Distance_check_Surface_slope = 2f;

        [ShowIf(nameof(Surface_slope_bool))]
        [Tooltip("���� ������������ ��� �����������")]
        [SerializeField]
        LayerMask Layer_Surface_slope = 1;

        [Min(0)]
        [ShowIf(nameof(Surface_slope_bool))]
        [Tooltip("����� �������� (��� ������, ��� ����� ����)")]
        [SerializeField]
        float Angle_limit = 40f;


        [field: Space(20)]
        [Min(0)]
        [ShowIf(nameof(Physics_bool))]
        [Tooltip("��������� ������������� �����, ��� �� �������� �����������")]
        [SerializeField]
        float Distance_cast_detect = 1f;

        [Min(0)]
        [ShowIf(nameof(Physics_bool))]
        [Tooltip("������ �����, ��� �� �������� �����������")]
        [SerializeField]
        float Radius_cast_detect = 1f;

        [ShowIf(nameof(Physics_bool))]
        [Tooltip("���� ��� �������������� � �������������")]
        [SerializeField]
        LayerMask Layer_cast_detect = 1;





        [field: Space(20)]

        [Min(0)]
        [ShowIf(nameof(Physics_bool))]
        [Tooltip("���� ������")]
        [SerializeField]
        float Jump_force = 40;

        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Physics_bool))]
        [Tooltip("�������� �������������� ���� ��� ������ �� ����������� ��������")]
        [SerializeField]
        bool Jump_forward_bool = false;

        
        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Physics_bool), nameof(Jump_forward_bool))]
        [Min(0)]
        [Tooltip("���� ����������� ��� ������ � ������� ��������")]
        [SerializeField]
        float Jump_forward_force = 2f;

        [Min(0)]
        [HideIf(nameof(Physics_bool))]
        [Tooltip("������, �� ������� ����� �������� �����")]
        public float JumpHeight = 1.2f;

        [Min(0)]
        [HideIf(nameof(Physics_bool))]
        [Tooltip("�����, ����������� ��� ����, ����� ����� ��������. ���������� 0f, ����� ����� ��������� ��������")]
        public float Jump_Timeout = 0.2f;


        bool Jump_bool = true;//����� ������� ��� ���

        bool Jump_input_bool = false;

        internal bool Jump_active_bool { get; private set; } = false;//����������� ���������� ������

        internal float _verticalVelocity = 0;//������������ �������� ���������

        bool Coyote_time_bool = false;//�������� �� "����� ������"

        float _jumpTimeoutDelta = 0;//����� ���������� � ������
        float _fallTimeoutDelta = 0;//����� ���������� � ��������
         
        //float _terminalVelocity = 53.0f;//������������ �������� �������

        internal bool Grounded_bool { get; private set; } = true;//�������� � ����� �� �����

        bool Gravity_bool = true;//�������� ����������

        Coroutine Delay_check_position_gravity_coroutine = null;

        Coroutine Jump_coroutine = null;

        Vector3 Old_position_gravity = Vector3.zero;

        Vector3 Old_position = Vector3.zero;

        Vector3 New_position = Vector3.zero;

        internal float Speed_active { get; private set; } = 0;//������� ��������

        //float Last_speed = 0;//������� ������������ ��������

        float _targetRotation = 0.0f;
        float _rotationVelocity = 0;

        bool Control_bool = true;

        Vector3 Direction_move = Vector3.zero;

        [Tooltip("�������� ����������� �������������� ��������")]
        [SerializeField]
        bool Gizmos_mode_bool = false;

        private void Start()
        {
            if (Game_administrator.Singleton_Instance)
                Cam = Game_administrator.Singleton_Instance.Player_administrator.Cam;

            Direction_move = Rotation_transform.forward;
        }

        void FixedUpdate()
        {
            if (Gravity_bool)
                Graviry_and_jump();
            else
                _verticalVelocity = 0;
        }

        #region ������

        /// <summary>
        /// ������ � �������
        /// </summary>
        protected void Graviry_and_jump()
        {
            if (!Physics_bool)
            {
                if (Grounded_bool && Jump_bool)
                {

                    // �������� ������ ����-���� �������
                    _fallTimeoutDelta = Fall_Timeout;

                    /*
                    if (Anim)
                    {
                        Anim.SetBool("Jump", false);
                        Anim.SetBool("Free_fall", false);
                    }
                    */

                    // ���������� ����������� ������� ����� �������� ��� ����������
                    if (_verticalVelocity < 0.0f)
                    {
                        _verticalVelocity = -2f;
                    }

                    // ������
                    if (Jump_input_bool && _jumpTimeoutDelta <= 0.0f)
                    {
                        // ���������� ������ �� H * -2 * G = ��������, ����������� ��� ���������� �������� ������
                        _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    }

                    // jump timeout
                    if (_jumpTimeoutDelta >= 0.0f)
                    {
                        _jumpTimeoutDelta -= Time.deltaTime;
                    }
                    
                }
                else
                {
                    // reset the jump timeout timer
                    _jumpTimeoutDelta = Jump_Timeout;

                    // fall timeout
                    if (_fallTimeoutDelta >= 0.0f)
                    {
                        _fallTimeoutDelta -= Time.deltaTime;

                    }

                    // if we are not grounded, do not jump
                    Jump_input_bool = false;
                    Jump_bool = false;
                }

                _verticalVelocity += Gravity * Time.deltaTime;

                
                if (_verticalVelocity < -2)
                {
                    if (Delay_check_position_gravity_coroutine == null)
                   Delay_check_position_gravity_coroutine = StartCoroutine(Coroutine_delay_check_position_gravity());
                }
                
                if (Max_fall_speed > _verticalVelocity)
                    _verticalVelocity = Max_fall_speed;

                /*
                // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
                if (_verticalVelocity < _terminalVelocity)
                {
                    _verticalVelocity += Gravity * Time.deltaTime;
                }
                */
            }
            else
            {
                if(!Grounded_bool)
                {
                    _verticalVelocity = Body.velocity.y;
                }

             }
        }

        IEnumerator Coroutine_jump()
        {

            Body.velocity = new Vector3(Body.velocity.x, 0, Body.velocity.z);

            Vector3 direction = Vector3.up * Jump_force;

            if (Jump_forward_bool)
                direction = (Vector3.up + Direction_move * Jump_forward_force) * Jump_force;

            yield return new WaitForSeconds(0.05f);
            Body.AddForce(direction, ForceMode.Impulse);

            Jump_coroutine = null;
        }

        IEnumerator Coroutine_delay_check_position_gravity()
        {
            yield return new WaitForSeconds(0.02f);
            if (_verticalVelocity < -2 && transform.position == Old_position_gravity)
            {
                _verticalVelocity = -2f;
            }
                

            Old_position_gravity = transform.position;
            Delay_check_position_gravity_coroutine = null;
        }


        /// <summary>
        /// ��������� ����������� �����������
        /// </summary>
        /// <param name="_forward">����������� ��������</param>
        /// <returns></returns>
        Vector3 Find_out_surface_slope(Vector3 _forward)
        {
            Vector3 result = _forward;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, -Vector3.up, out hit, Distance_check_Surface_slope, Layer_Surface_slope, QueryTriggerInteraction.Ignore))
            {
                Vector3 pre_result = _forward - Vector3.Dot(_forward, hit.normal) * hit.normal;

                if (Angle_limit <= Vector3.Angle(_forward, _forward - pre_result))
                    result = pre_result;
            }


            return result;
        }

        /// <summary>
        /// ������� ���������, ��� �� ���������� ����� ����� ����������� (����� ������� ������ ������)
        /// </summary>
        /// <param name="_direction">����������� ��������</param>
        /// <returns></returns>
        bool Check_stop_raycast(Vector3 _direction)
        {
            bool result = false;
            
            RaycastHit hit;

            Transform target = Rotation_transform ? Rotation_transform : transform;

            //if (Physics.SphereCast(target.position + -target.forward * 0.5f, Radius_cast_detect, _direction, out hit, Distance_cast_detect, Layer_cast_detect))
            if (Physics.SphereCast(target.position, Radius_cast_detect, _direction, out hit, Distance_cast_detect, Layer_cast_detect, QueryTriggerInteraction.Ignore))
            {
                result = true;
            }

            return result;
        }


        /// <summary>
        /// �������� �������� ������� ��� ����� ������ ��������� � ����������
        /// </summary>
        float Find_out_Accelerate_or_decelerate_speed (float _speed)
        {
                float result = _speed;

                float offset = 0.1f;

                // accelerate or decelerate to target speed
                if (_speed - offset > Speed_active || _speed + offset < Speed_active)
                {
                result = Mathf.Lerp(Speed_active, _speed,
                        Time.fixedDeltaTime * SpeedChangeRate);

                    // round speed to 3 decimal places
                    // Speed_active = Mathf.Round(Speed_active * 1000f) / 1000f;
                }

                return result;
        }

        void Delay_Coyote_time_off()
        {
            Coyote_time_bool = false;
        }

        /// <summary>
        /// ����������� ���������
        /// </summary>
        /// <param name="_direction">�����������</param>
        /// <param name="_speed">�������� ��������</param>
        void Move(Vector3 _direction, float _speed, bool _Check_stop_raycast_bool)
        {
            Speed_active = Find_out_Accelerate_or_decelerate_speed(_speed);

            if (Speed_active > 0)
            {

                if (Surface_slope_bool && _direction != Vector3.zero)
                    _direction = Find_out_surface_slope(_direction);


                    if (_direction != Vector3.zero)
                        Direction_move = _direction;
                    else
                        Direction_move = Rotation_transform.forward;


                // move the player
                if(Physics_bool)
                {

                    if (!_Check_stop_raycast_bool || !Check_stop_raycast(_direction))
                    {
                        Vector3 direction_speed = _direction * Speed_active * Time.fixedDeltaTime;

                        Old_position = new Vector3(transform.position.x, 0, transform.position.z);

                        transform.position += direction_speed;

                        New_position = new Vector3(transform.position.x, 0, transform.position.z);
                    }

                    //Body.MovePosition(Body.position + targetDirection * Speed_active * Time.fixedDeltaTime);
                    //Body.MovePosition(Body.position + targetDirection * Speed_active * Time.fixedDeltaTime);
                    //Body.AddForce(targetDirection * Speed_active, ForceMode.Impulse);
                    //Body.velocity = targetDirection * Speed_active +
                    //                 new Vector3(Body.velocity.x, Body.velocity.y, Body.velocity.z);
                }
            }

            if (!Physics_bool)
                CharacterController_script.Move(_direction * (Speed_active * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        }

        Vector3 Find_out_direction_cursor
        {
            get
            {
                Vector3 result = Vector3.zero;
                Vector3 mousePosition = Vector3.zero;
                Ray mouseRaycast;
                RaycastHit hit;

                //bool ray_detect_bool = false;

                if (Cam.orthographic)
                {
                    mousePosition = Cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                }
                else
                {
                    mousePosition.x = Mouse.current.position.ReadValue().x;
                    mousePosition.y = Mouse.current.position.ReadValue().y;//Cam.pixelHeight - Mouse.current.position.ReadValue().y;
                    mousePosition.z = Vector3.Distance(Cam.transform.position, Rotation_transform.position);//Cam.nearClipPlane;

                    mouseRaycast = Cam.ScreenPointToRay(mousePosition);

                    if (Physics.Raycast(mouseRaycast, out hit, Mathf.Infinity, Cursor_mask))
                    {
                        //ray_detect_bool = true;
                        mousePosition = hit.point;
                        mousePosition.y = Rotation_transform.position.y;
                    }
                    else
                    {
                        mousePosition = Cam.ScreenToWorldPoint(mousePosition);
                    }

                }

                mousePosition.y = Rotation_transform.position.y;
                result = mousePosition - Rotation_transform.position;
                //print(mousePosition);
               result.Normalize();

                return result;
            }
        }

        #endregion



        #region ��������� ������

        /// <summary>
        /// ������������ ������ ����������
        /// </summary>
        /// <param name="_activity">�� ����� ����� ��� ���</param>
        public void Activity_Grounded_bool(bool _activity)
        {
            if (!_activity && Grounded_bool && !Coyote_time_bool && !Jump_active_bool)
            {
                Coyote_time_bool = true;
                Invoke(nameof(Delay_Coyote_time_off), Coyote_time);
            }

            if (_verticalVelocity <= 0 && _activity)
            {
                Grounded_bool = true;

                Jump_bool = true;

                Jump_active_bool = false;
            }
            else
            {
                Grounded_bool = false;
            }      
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void Jump()
        {
            if (Jump_bool && !Jump_active_bool && (Grounded_bool || Coyote_time_bool))
            {
                Jump_active_bool = true;

                if (Physics_bool && Jump_coroutine == null)
                    Jump_coroutine = StartCoroutine(Coroutine_jump());
            }

        }

        /// <summary>
        /// ��������
        /// </summary>
        public void Jump(bool _activity)
        {
            Jump_input_bool = _activity;
        }

        /// <summary>
        /// ������������ ��������� �� �������
        /// </summary>
        /// <param name="_activity">��� ������� ?</param>
        public void Activity_Jump_bool(bool _activity)
        {
            if (Jump_bool = !_activity)
            {
                Jump_bool = _activity;
            }

        }

        /// <summary>
        /// ��������� � ���������� ����������
        /// </summary>
        /// <param name="_activity">���� ���������� ?</param>
        public void Activity_Gravity_bool(bool _activity)
        {
            Gravity_bool = _activity;

            if (Physics_bool)
            {
                if (_activity)
                {
                    Body.useGravity = true;
                    Body.isKinematic = false;
                }
                else
                {
                    Body.velocity = Vector3.zero;
                    Body.useGravity = false;
                    Body.isKinematic = true;
                }

            }
                
        }

        /// <summary>
        /// ��������� � ���������� ���������� �������������
        /// </summary>
        /// <param name="_activity">����� ������?</param>
        public void Activity_control_bool(bool _activity)
        {
            Control_bool = _activity;
        }

        /// <summary>
        /// ������������� ��������� ��������
        /// </summary>
        /// <param name="_height">�� ����� ������</param>
        public void Forced_Jump(float _height)
        {
            _verticalVelocity = Mathf.Sqrt(_height * -2f * Gravity);
            Jump_active_bool = true;
        }



        /// <summary>
        /// ��������������� � �����
        /// </summary>
        /// <param name="_position">������� (�������� �������)</param>
        public void Forced_Teleport(Vector3 _position)
        {
            Activity_Gravity_bool(false);
            Activity_control_bool(false);

            if (Physics_bool)
                Body.velocity = Vector3.zero;

            transform.position = _position;

            Invoke(nameof(Character_active_delay), 0.1f);
        }

        /// <summary>
        /// �������� �� ����������������
        /// </summary>
        void Character_active_delay()
        {
            Activity_Gravity_bool(true);
            Activity_control_bool(true);
        }

        /// <summary>
        /// ������������ ��� ��������� ��  3-�� ���� (����� ������ ����� ��������,� �������� ��� �� ����������� ���� ������� ������)
        /// </summary>
        /// <param name="speed_movement_">��������� ������������</param>
        /// <param name="_direction">����������� ��������</param>
        /// <param name="_speed_rotation_character">�������� �������� ��������� (����)</param>
        public void Move_Third_person(float speed_movement_, Vector2 _direction, float _speed_rotation_character)
        {
            if (Control_bool)
            {

                _direction = _direction.magnitude > 1 ? _direction.normalized : _direction;

                float speed_actual = speed_movement_ * _direction.magnitude;

                // normalise input direction
                Vector3 inputDirection = new Vector3(_direction.x, 0.0f, _direction.y).normalized;

                // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                // if there is a move input rotate player when the player is moving
                if (_direction != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      Cam.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(Rotation_transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        _speed_rotation_character);

                    if (!Auto_rotation_bool)
                    {
                        // rotate to face input direction relative to camera position
                        if(Tracking_cursor_bool)
                            Rotation_transform.rotation = Quaternion.LookRotation(Find_out_direction_cursor);
                        else
                            Rotation_transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                    }
                }

                if(Auto_rotation_bool)
                {
                    if (Tracking_cursor_bool)
                        Rotation_transform.rotation = Quaternion.RotateTowards(Rotation_transform.rotation, Quaternion.LookRotation(Find_out_direction_cursor), _speed_rotation_character);
                    else
                        Rotation_transform.rotation = Quaternion.RotateTowards(Rotation_transform.rotation, Quaternion.Euler(0.0f, _targetRotation, 0.0f), _speed_rotation_character);
                }
                


                Vector3 targetDirection = _direction != Vector2.zero ? Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward : inputDirection;

                Move(targetDirection, speed_actual, true);
                // update animator if using character
                //Anim.SetFloat("Speed", _animationBlend / Speed_active);
                //Anim.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        /// <summary>
        /// ������������ ��� ��������� ��  1-�� ����
        /// </summary>
        /// <param name="speed_movement_">��������� ������������</param>
        /// <param name="_direction">����������� ��������</param>
        public void Move_First_person(float speed_movement_, Vector2 _direction)
        {
            if (Control_bool)
            {
                _direction = _direction.magnitude > 1 ? _direction.normalized : _direction;

                Vector3 moveDirectionForward = transform.forward * _direction.y;
                Vector3 moveDirectionSide = transform.right * _direction.x;

                Vector3 direction = (moveDirectionForward + moveDirectionSide).normalized;

                float speed_actual = speed_movement_ * _direction.magnitude;

                Move(direction, speed_actual, true);


            }
        }

        /// <summary>
        /// ������������ �� ������������ ����������� (�� ����� ��� ������������ ��������)
        /// </summary>
        /// <param name="speed_movement_">��������� ������������</param>
        /// <param name="_direction">����������� ��������</param>
        /// <param name="_vertical_bool">��������� �������� �����������</param>
        /// <param name="_horizontal_bool">��������� �������� �������������</param>
        public void Move_on_vertical_surfaces(float speed_movement_, Vector2 _direction, bool _vertical_bool, bool _horizontal_bool)
        {
            if (Control_bool)
            {
                _direction = _direction.magnitude > 1 ? _direction.normalized : _direction;

                float speed_actual = speed_movement_ * _direction.magnitude;

                Vector3 move_vertical = _vertical_bool ? transform.up * _direction.y : Vector3.zero;
                Vector3 move_horizontal = _horizontal_bool ? transform.right * _direction.x : Vector3.zero;

                Vector3 direction = (move_vertical + move_horizontal).normalized;

                Move(direction, speed_actual, false);
            }
        }

        #endregion


        #region ��������������
        private void OnDrawGizmos()
        {
            if (Gizmos_mode_bool)
            {
                if (Physics_bool)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(transform.position, -Vector3.up * Distance_check_Surface_slope);
                }

                if (Surface_slope_bool) 
                {
                    Gizmos.color = Color.yellow;
                    Transform target = Rotation_transform ? Rotation_transform : transform;
                    Vector3 direction = Direction_move != Vector3.zero ? Direction_move : target.forward;

                    Gizmos.DrawRay(target.position, direction * Distance_cast_detect);
                    Gizmos.DrawSphere(target.position + direction * Distance_cast_detect, Radius_cast_detect);
                }
            }
            
        }
        #endregion
    }
}