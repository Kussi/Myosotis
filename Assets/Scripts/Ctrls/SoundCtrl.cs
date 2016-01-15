using UnityEngine;

/// <summary>
/// Controls the sounds
/// </summary>
public static class SoundCtrl
{
    private static readonly string SoundObject = "Main Camera";

    private static Sound sound;
    private static bool isAvailable = false;

    /// <summary>
    /// initializes all sounds
    /// </summary>
    public static void InitializeSounds()
    {
        sound = GameObject.Find(SoundObject).GetComponent<Sound>();
        isAvailable = true;
    }

    /// <summary>
    /// Plays an applause sound
    /// </summary>
    public static void PlayApplaus()
    {
        if(isAvailable) sound.PlayApplause();
    }

    /// <summary>
    /// Plays an step sound
    /// </summary>
    public static void PlayStep()
    {
        if (isAvailable) sound.PlayStep();
    }

    /// <summary>
    /// Plays an dice sound
    /// </summary>
    public static void PlayDice()
    {
        if (isAvailable) sound.PlayDice();
    }

    /// <summary>
    /// Resets the original setup
    /// </summary>
    public static void Reset()
    {
        sound = null;
        isAvailable = false;
    }
}