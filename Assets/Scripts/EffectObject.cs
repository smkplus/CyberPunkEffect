using System;
using UnityEngine;

[ExecuteInEditMode]
public class EffectObject : MonoBehaviour
{
    private static Material _material;
    public static Material Mat
    {
        get
        {
            if(!_material)
                _material = new Material(Shader.Find("Hidden/OutlineEffectObject"));
            return _material;
        }
    }

    public EffectParams effectParams;

    public void OnEnable()
    {
        EffectSystem.Add(this);
    }

    public void Start()
    {
        EffectSystem.Add(this);
    }

    public void OnDisable()
    {
        EffectSystem.Remove(this);
    }

}