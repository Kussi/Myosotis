using UnityEngine;
using System.Collections;

public static class PersonalizationCtrl {

    private static string playerName;

    private static ArrayList states = new ArrayList() { typeof(MusicCtrl), typeof(ImageCtrl) };    

    public static string PlayerName
    {
        get { return playerName; }
    }

    public static void Reset()
    {
        MusicCtrl.Reset();
        ImageCtrl.Reset();
        TextCtrl.Reset();
        SoundCtrl.Reset();
    }

    public static void InitializePersonalization(string playerName)
    {
        PersonalizationCtrl.playerName = playerName;

        MusicCtrl.InitializeMusic(playerName);
        ImageCtrl.InitializeImages(PlayerCtrl.players);
        TextCtrl.InitializeTexts(playerName);
        SoundCtrl.InitializeSounds();
    }

    public static void Notify(System.Type type)
    {
        if (type == typeof(ImageCtrl))
            FieldCtrl.SetEventTriggers();
        if (states.Contains(type)) states.Remove(type);
        if(states.Count == 0) GameObject.Find("StartMenu").GetComponent<StartMenu>().Hide();
    }
}