using System;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public class EffectParams
{
    [Range(0, 85)]
    public int frame;
    public Color lineColor;
    public Color gridColor;
    [Range(0,1)]
    public float lineAlpha;
    [Range(0,1)]
    public float colorMaskAlpha;

    [HideInInspector] public int lineNumber;
    [HideInInspector] public int pixelStep;

    public void GetSettings()
    {
        var line = lineColor;
        line.a = (frame == 84 || frame == 85)? 0 : lineAlpha;
        lineColor = line;
        if (frame == 0)
        {
            lineNumber = 150;
            pixelStep = 0;
        }
        else if (frame == 1 || frame == 2)
        {
            lineNumber = 150;
            pixelStep = 1;
        }
        else if (frame >= 3 && frame <= 5)
        {
            lineNumber = 150;
            pixelStep = 255;
        }
        else if (frame == 6)
        {
            lineNumber = 150;
            pixelStep = 189;
        }
        else if (frame >= 7 && frame <= 17)
        {
            lineNumber = 150;
            pixelStep = 122;
        }
        else if (frame == 18)
        {
            lineNumber = 80;
            pixelStep = 93;
        }
        else if (frame >= 19 && frame <= 27)
        {
            lineNumber = 10;
            pixelStep = 64;
        }
        else if (frame == 28)
        {
            lineNumber = 10;
            pixelStep = 48;
        }
        else if (frame >= 29 && frame <= 36)
        {
            lineNumber = 10;
            pixelStep = 33;
        }
        else if (frame >= 37 && frame <= 46)
        {
            lineNumber = 10;
            pixelStep = 16;
        }
        else if (frame == 47)
        {
            lineNumber = 38;
            pixelStep = 12;
        }
        else if (frame >= 48 && frame <= 56)
        {
            lineNumber = 67;
            pixelStep = 7;
        }
        else if (frame == 57)
        {
            lineNumber = 76;
            pixelStep = 5;
        }
        else if (frame >= 58 && frame <= 65)
        {
            lineNumber = 86;
            pixelStep = 3;
        }
        else if (frame >= 66 && frame <= 83)
        {
            lineNumber = 150;
            pixelStep = 1;
        }
        else if (frame >= 84 && frame <= 85)
        {
            lineNumber = 150;
            pixelStep = 1;
        }
        lineNumber = (int)(lineNumber * 1.5f);
    }
}