using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sych_scripts;

public class Test_ricochet : MonoBehaviour
{
    [Tooltip("Количество рикошетов")]
    [SerializeField]
    int Count_ricochet = 3;

    [Tooltip("Дистанция луча")]
    [SerializeField]
    float Distance_ray = 1000f;

    [Tooltip("Отображение для игрока траектории рикошета")]
    [SerializeField]
    LineRenderer Trail = null;

    [Tooltip("Слои с которым взаимодействует луч")]
    [SerializeField]
    LayerMask Layer = 1;

    [Tooltip("Увидеть больше")]
    [SerializeField]
    bool Debug_mode_bool = false;

    private void Update()
    {
        if (Trail)
        {
            List<Vector3> point_list = Get_ricochet_points(transform.position, transform.forward);

            if (Trail.positionCount != point_list.Count)
                Trail.positionCount = point_list.Count;

            Trail.SetPositions(Game_calculator.Convert_from_List_to_Array(point_list));
        }
    }

    /// <summary>
    /// Получить точки рекошета
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

    private void OnDrawGizmos()
    {
        if (Debug_mode_bool)
        {
            List<Vector3> point_list = Get_ricochet_points(transform.position, transform.forward);

            for (int x = 0; x < point_list.Count - 1; x++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(point_list[x], point_list[x + 1]);
            }
        }
    }
}
