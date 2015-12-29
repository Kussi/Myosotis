using UnityEngine;
using System.Collections;
using System;

public static class SunCtrl {

    private static readonly int RedlightAngle = 90;
    private static readonly int YellowlightAngle = 0;
    private static readonly int BluelightAngle = 270;
    private static readonly int GreenlightAngle = 180;

    private static string SunGameObject = "Sun";
    private static Sun sun;

    public static int GetPlayerLightAngle(string color)
    {
        switch (color)
        {
            case "red":
            case "Red":
                return RedlightAngle;
            case "yellow":
            case "Yellow":
                return YellowlightAngle;
            case "blue":
            case "Blue":
                return BluelightAngle;
            case "green":
            case "Green":
                return GreenlightAngle;
            default:
                throw new ArgumentException(color + " is not a valid player color.");
        }
    }

    public static void InitializeSun()
    {
        GameObject.Find(SunGameObject).AddComponent<Sun>();
    }
}