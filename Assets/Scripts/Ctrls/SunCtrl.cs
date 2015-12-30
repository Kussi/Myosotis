using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class SunCtrl {

    private static readonly Dictionary<string, int> PlayerAngle = new Dictionary<string, int>
    {
        { "Red", 90 }, { "Yellow", 0 }, { "Blue", 270 }, { "Green", 180 }
    };

    private static string SunGameObject = "Sun";
    private static Sun sun;

    public static void InitializeSun()
    {
        GameObject.Find(SunGameObject).AddComponent<Sun>();
    }
}