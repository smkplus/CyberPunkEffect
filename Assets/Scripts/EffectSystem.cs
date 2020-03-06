using System.Collections.Generic;
using UnityEngine;

public class EffectSystem
{
    private static EffectSystem _instance;

    public static EffectSystem Instance => _instance ?? (_instance = new EffectSystem());

    private const int Height = 3;
    private static bool _updateParamsTexture;
    private static Texture2D _paramsTexture;

    private static void GetNewParamsTexture()
    {
        _paramsTexture = new Texture2D(SubscribedObjects.Count, Height, TextureFormat.ARGB32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };
    }

    public static Texture2D ParamsTexture
    {
        get
        {
            if (_updateParamsTexture)
            {
                if (_paramsTexture)
                    _paramsTexture.Resize(SubscribedObjects.Count, Height);
                else
                    GetNewParamsTexture();
                _updateParamsTexture = false;
            }

            _paramsTexture.SetPixels(GetAmounts());
            _paramsTexture.Apply();
            return _paramsTexture;
        }
    }

    public static readonly List<EffectObject> SubscribedObjects = new List<EffectObject>();

    public static void Add(EffectObject o)
    {
        if (!SubscribedObjects.Contains(o))
            SubscribedObjects.Add(o);
        _updateParamsTexture = true;
        EffectRenderer.ForceUpdate();
    }

    public static void Remove(EffectObject o)
    {
        SubscribedObjects.Remove(o);
        _updateParamsTexture = true;
        EffectRenderer.ForceUpdate();
    }

    private static Color[] GetAmounts()
    {
        var width = SubscribedObjects.Count;
        var o = new Color[width * Height];
        for (var i = 0; i < width; i++)
        {
            var e = SubscribedObjects[i].effectParams;
            e.GetSettings();
            o[i] = new Color(e.lineNumber / 150f, e.pixelStep / 255f, e.lineAlpha, e.colorMaskAlpha);
            o[i + width] = e.lineColor;
            o[i + width * 2] = e.gridColor;
        }

        return o;
    }
}