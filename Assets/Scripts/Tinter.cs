using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tinter
{
    private const float FadeSpeed = 1f;
    public Color Initial;
    private Color _delta = Color.black;
    
    public Tinter(Color initial)
    {
        Initial = initial;
    }

    public void Update(float dt)
    {
        _delta.r = Math.Max(0, _delta.r - dt * FadeSpeed);
        _delta.g = Math.Max(0, _delta.g - dt * FadeSpeed);
        _delta.b = Math.Max(0, _delta.b - dt * FadeSpeed);
    }

    public Color GetCurrent()
    {
        var c = Initial;
        c.r += _delta.r;
        c.g += _delta.g;
        c.b += _delta.b;
        return c;
    }

    public void Blink(Color color)
    {
        _delta.r += color.r;
        _delta.r = Math.Min(_delta.r, 2f);
        _delta.g += color.g;
        _delta.g = Math.Min(_delta.g, 2f);
        _delta.b += color.b;
        _delta.b = Math.Min(_delta.b, 2f);
    }
}