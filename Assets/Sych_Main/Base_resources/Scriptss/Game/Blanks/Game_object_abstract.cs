//����������� ��� ������� ��������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    public abstract class Game_object_abstract : Entity_abstract
    {
        [field: Tooltip("��������� ������� (������ ��� ��������� ������� �� ���� ������) (����� � ������ ������� ��� ������ ��������)")]
        [field: SerializeField]
        public Collider Collider_size { get; private set; } = null;

        [field: Foldout("�� ������������")]
        [Tooltip("��������� ������ (���� ����)")]
        [field: SerializeField]
        public Rigidbody Body { get; private set; } = null;//������
    }
}