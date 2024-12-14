//Особенности для предметов (включая оружия которое носит персонаж)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{

    public enum Item_enum
    {
        Item,
        Melee,
        Pistol,
        Rifle,
        Bow
    }

    public abstract class Item_Game_object_abstract : Game_object_abstract
    {
        [Tooltip("Тип объекта")]
        [field: SerializeField]
        public Item_enum Item_type { get; private set; } = Item_enum.Item;

    }
}