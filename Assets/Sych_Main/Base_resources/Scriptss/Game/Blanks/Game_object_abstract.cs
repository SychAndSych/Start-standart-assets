//Особенности для игровых объектов
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    public abstract class Game_object_abstract : Entity_abstract
    {
        [field: Tooltip("Коллайдер размера (обычно это коллайдер колизии на весь объект) (нужно в первую очередь для замера размеров)")]
        [field: SerializeField]
        public Collider Collider_size { get; private set; } = null;

        [field: Foldout("Не обязательное")]
        [Tooltip("Компонент физики (если есть)")]
        [field: SerializeField]
        public Rigidbody Body { get; private set; } = null;//Физика
    }
}