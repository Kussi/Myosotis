using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class SunCtrl {

    private static string SunGameObject = "Sun";
    private static Sun sun;

    public static void InitializeSun()
    {
        GameObject.Find(SunGameObject).AddComponent<Sun>();
    }
}