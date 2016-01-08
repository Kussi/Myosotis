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

        MusicCtrl.InitializeMusic();
        ImageCtrl.InitializeImages(PlayerCtrl.players);
        //TextCtrl.InitializeTexts();
            
    }

    public static void Notify()
    {
        FieldCtrl.SetEventTriggers();
    }
}