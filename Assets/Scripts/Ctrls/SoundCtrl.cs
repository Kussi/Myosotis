using UnityEngine;
using System.Collections;

public static class SoundCtrl
{

    private static readonly string SoundObject = "Main Camera";

    private static Sound sound;
    private static bool isAvailable = false;

    public static void InitializeSounds()
    {
        sound = GameObject.Find(SoundObject).GetComponent<Sound>();
        isAvailable = true;
    }

    public static void PlayApplaus()
    {
        if(isAvailable) sound.PlayApplause();
    }

    public static void PlayStep()
    {
        if (isAvailable) sound.PlayStep();
    }

    public static void PlayDice()
    {
        if (isAvailable) sound.PlayDice();
    }
}