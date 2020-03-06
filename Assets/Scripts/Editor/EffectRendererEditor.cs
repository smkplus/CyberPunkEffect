using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(EffectRenderer))]
    public class EffectRendererEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Force Update Command Buffer")) 
                EffectRenderer.ForceUpdate();
        }
    }
}
