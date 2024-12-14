using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    public class Anticipate_ricochet : MonoBehaviour
    {
        [field: Space(10)]
        [field: Tooltip("�������� ����� �������� ��� ��������")]
        [field: SerializeField]
        public bool Ricochet_bool { get; private set; } = false;

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


        private void Update()
        {

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

        /// <summary>
        /// �������� ����� ��������
        /// </summary>
        /// <returns></returns>
        public List<Vector3> Get_ricochet_points(Vector3 _start_point, Vector3 _start_direction)
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
    }
}