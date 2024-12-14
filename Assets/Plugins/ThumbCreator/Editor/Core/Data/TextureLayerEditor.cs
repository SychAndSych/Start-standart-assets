using ThumbCreator.Core;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(TextureLayer))]

public class TextureLayerEditor : Editor
{
    TextureLayer _target;
    void OnEnable()
    {
        //_target = (TextureLayer)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
#endif