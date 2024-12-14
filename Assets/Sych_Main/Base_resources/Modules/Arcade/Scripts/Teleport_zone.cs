//�������� ������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Arcade / Teleport zone")]
    [DisallowMultipleComponent]
    public class Teleport_zone : MonoBehaviour
    {
        [Tooltip("����� ���� �������������")]
        [SerializeField]
        Transform Point_finale = null;

        [Tooltip("��� �� ������ ������")]
        [SerializeField]
        bool Gizmos_mode_bool = false;

        private void OnTriggerEnter(Collider other)
        {
            Game_Method.Teleport(other.transform, Point_finale.position, true);
        }

        #region ����������� ������

        private void OnDrawGizmos()
        {

            if (Gizmos_mode_bool)
            {
                Gizmos.color = Color.yellow;

                Gizmos.DrawLine(transform.position, Point_finale.position);

                Gizmos.DrawSphere(transform.position, 1);

                Gizmos.DrawSphere(Point_finale.position, 1);
            }
        }
        #endregion
    }
}