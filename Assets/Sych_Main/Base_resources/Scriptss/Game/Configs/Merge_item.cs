using UnityEngine;
using NaughtyAttributes;

namespace Sych_scripts
{
    [System.Serializable]
    public class Merge_item
    {
        [field: SerializeField]
        public string Name { get; private set; } = "Название крафта";

        [field: ShowAssetPreview]
        [field: Tooltip("Первый предмет для смешивания")]
        [field: SerializeField]
        public Entity_abstract First_item { get; private set; } = null;

        [field: ShowAssetPreview]
        [field: Tooltip("Второй предмет для смешивания")]
        [field: SerializeField]
        public Entity_abstract Second_item { get; private set; } = null;

        [field: ShowAssetPreview]
        [field: Tooltip("Какой предмет получаем в итоге")]
        [field: SerializeField]
        public Entity_abstract Final_item { get; private set; } = null;


    }
}