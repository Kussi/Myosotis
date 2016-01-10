using System.Collections.Generic;
using UnityEngine;

public static class Initializer
{
    public static void SetupGame(string playerName, Dictionary<string, int> playerColors, int nofFigures)
    {
        FieldCtrl.InitializeFields();
        PlayerCtrl.InitializePlayers(playerColors);
        DiceCtrl.InitializeDice(((Player)PlayerCtrl.players[0]).Color);
        FigureCtrl.InitializeFigures(PlayerCtrl.players, nofFigures);
        PersonalizationCtrl.InitializePersonalization(playerName);
        FigureCtrl.PlaceFiguresOnStartPosition();

        GameCtrl.StartGame();
    }

    public static void DestroyGame()
    {
        DiceCtrl.DestroyDice();
        FieldCtrl.DestroyFields();
        FigureCtrl.DestroyFigures();
        PlayerCtrl.Reset();
        GameCtrl.Reset();
        TurnCtrl.Reset();

        PersonalizationCtrl.Reset();
    } 
}