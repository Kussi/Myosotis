using UnityEngine;
using System.Collections;

/// <summary>
/// Responsible for all of the personalized Ctrls. Sums up all of these
/// </summary>
public static class PersonalizationCtrl {

    private static string playerName;

    private static ArrayList states = new ArrayList() { typeof(MusicCtrl), typeof(ImageCtrl) };    

    public static string PlayerName
    {
        get { return playerName; }
    }

    /// <summary>
    /// Resets the original setup
    /// </summary>
    public static void Reset()
    {
        MusicCtrl.Reset();
        ImageCtrl.Reset();
        TextCtrl.Reset();
        SoundCtrl.Reset();
    }

    /// <summary>
    /// initialized personalization
    /// </summary>
    /// <param name="playerName"></param>
    public static void InitializePersonalization(string playerName)
    {
        PersonalizationCtrl.playerName = playerName;

        MusicCtrl.InitializeMusic(playerName);
        ImageCtrl.InitializeImages(PlayerCtrl.players);
        TextCtrl.InitializeTexts(playerName);
        SoundCtrl.InitializeSounds();
    }

    /// <summary>
    /// checks whether the personalized media are ready and if yes, hides the startmenu / splashscreen
    /// </summary>
    /// <param name="type"></param>
    public static void Notify(System.Type type)
    {
        if (type == typeof(ImageCtrl))
            FieldCtrl.SetEventTriggers();
        if (states.Contains(type)) states.Remove(type);
        if(states.Count == 0) GameObject.Find("StartMenu").GetComponent<StartMenu>().Hide();
    }
}