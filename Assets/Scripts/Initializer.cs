using System.Collections.Generic;

/// <summary>
/// Responsible for the game initialization and destruction
/// </summary>
public static class Initializer
{
    /// <summary>
    /// Sets up the game. Summarizes all Initialize Method calls
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="playerColors"></param>
    /// <param name="nofFigures"></param>
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

    /// <summary>
    /// Destroys the game. Summarizes all Destroy and Reset Method calls
    /// </summary>
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