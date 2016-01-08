using System.Collections.Generic;
using UnityEngine;

public static class Initializer
{
    public static void SetupGame(string playerName, Dictionary<string, int> playerColors, int nofFigures)
    {
        FieldCtrl.InitializeFields();
        PlayerCtrl.InitializePlayers(playerColors);
        DiceCtrl.InitializeDices(PlayerCtrl.players);
        FigureCtrl.InitializeFigures(PlayerCtrl.players, nofFigures);
        SunCtrl.InitializeSun();
        BulpCtrl.InitializeBulps(PlayerCtrl.players);
        PersonalizationCtrl.InitializePersonalization(playerName);

        FigureCtrl.PlaceFiguresOnStartPosition();
        DiceCtrl.PlaceDices();

        GameCtrl.StartGame();
    }

    public static void ExitGame()
    {

    } 
}