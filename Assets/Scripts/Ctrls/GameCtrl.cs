using System;
using System.Collections;
using UnityEngine;

public static class GameCtrl
{
    private static bool gameIsRunning = false;

    private static int playerOnTurn = -1;
    private static ArrayList activePlayers;

    private static int actualDiceValue;
    private static int turnCounter;

    public static bool GameIsRunning
    {
        get { return gameIsRunning; }
    }

    public static Player PlayerOnTurn
    {
        get
        {
            if (GameIsRunning) return (Player)PlayerCtrl.players[playerOnTurn];
            throw new InvalidGameStateException();
        }
    }

    public static void StartGame()
    {
        gameIsRunning = true;
        turnCounter = 0;
        activePlayers = new ArrayList(PlayerCtrl.players);
        StartNextTurn();
    }

    public static void Notify(int value)
    {
        actualDiceValue = value;
        switch (value)
        {
            case 6:
                PlayerOnTurn.State.ThrowsRegularOrSix();
                break;
            case 5:
                PlayerOnTurn.State.ThrowsFive();
                break;
            default:
                PlayerOnTurn.State.ThrowsRegularOrSix();
                break;
        }

    }

    public static void Notify(Figure figure)
    {
        TurnCtrl.Execute(figure, actualDiceValue);
    }

    public static void Notify()
    {
        StartNextTurn();
    }

    public static void ActivateReleasedFigures()
    {
        FigureCtrl.ActivateReleasedFigures(PlayerOnTurn);
    }

    public static void ActivateAllFigures()
    {

        FigureCtrl.ActivateAllFigures(PlayerOnTurn);
    }

    public static void ActivateNoFigure()
    {
        StartNextTurn();
    }

    public static void FinishPlayer(Player player)
    {
        int numberOfPlayersOld = activePlayers.Count;
        activePlayers.Remove(player);
        if (numberOfPlayersOld - 1 != activePlayers.Count)
            throw new InvalidGameStateException();
    }

    private static void CreateTurn()
    {
        throw new NotImplementedException();
    }

    private static void ChangePlayerOnTurn()
    {
        playerOnTurn = (playerOnTurn + 1) % activePlayers.Count;
    }

    private static void StartNextTurn()
    {
        //sun
        //dice
        if (actualDiceValue != 6)
            ChangePlayerOnTurn();
        foreach(Player player in activePlayers)
            player.RefreshState();

        ++turnCounter;
        Debug.Log("Turn " + turnCounter + ": " + PlayerOnTurn.Color);
        DiceCtrl.ActivateDice(PlayerOnTurn);
    }

    private static void FinishGame()
    {
        gameIsRunning = false;
        throw new NotImplementedException();
    }
}