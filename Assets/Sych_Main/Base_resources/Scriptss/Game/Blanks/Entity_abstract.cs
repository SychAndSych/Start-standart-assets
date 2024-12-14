//Стартовая особенность для всех игровых сущностей
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [SelectionBase]
    public abstract class Entity_abstract : MonoBehaviour
    {
        [field: Space(5)]
        [field: Header("Настройки объекта")]

        [field: Tooltip("Id предмета для того, что бы ссылаться на его характеристики")]
        [field: SerializeField]
        public Vector2 Index { get; private set; } = new Vector2(-1, 0);

        protected Transform My_transform { get; private set; } = null;//Трансформ объекта 

        internal Config_object_SO Config { get; private set; } = null;

        protected virtual void Awake()
        {
            if (Index.x > -1 && Index.y > -1)
                Config = Game_calculator.Get_cofig_object(Index);

            Initialized_stats();

            My_transform = transform;
        }

        /// <summary>
        /// Назначает все необходимые параметры из конфига
        /// </summary>
        protected abstract void Initialized_stats();

    }
}