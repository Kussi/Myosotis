using UnityEngine;
using System.Collections;

public static class PersonalizationCtrl {

    private static string playerName;

    public static string PlayerName
    {
        get { return playerName; }
    }

    public static void InitializePersonalization(string playerName)
    {
        PersonalizationCtrl.playerName = playerName;

        ImageCtrl.InitializeImages(PlayerCtrl.players);
        MusicCtrl.InitializeMusic();
        //TextCtrl.InitializeTexts();
            
    }

    public static void Notify()
    {
        FieldCtrl.SetEventTriggers();
    }
}