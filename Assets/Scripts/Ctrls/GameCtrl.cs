using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameCtrl {

    private static bool gameIsRunning = false;

    private static int playerOnTurn = -1;

    private static bool diceActivated = false;
    private static bool figureActivated = false;

    public static bool GameIsRunning
    {
        get { return gameIsRunning; }
    }

    public static Player PlayerOnTurn
    {
        get
        {
            if(GameIsRunning) return (Player)PlayerCtrl.players[playerOnTurn];
            return null;
        }
    }

    public static void StartGame()
    {
        gameIsRunning = true;

        PlaceFiguresAtHome();
        StartNextTurn();
    }

    public static void notify(Dice dice)
    {
        throw new NotImplementedException();
    }

    public static void notify(GameFigure figure)
    {
        throw new NotImplementedException();
    }

    private static void PlaceFiguresAtHome()
    {
        throw new NotImplementedException();
    }

    private static void ActivateDice()
    {
        throw new NotImplementedException();
    }

    private static void ActivateFigures()
    {
        throw new NotImplementedException();
    }

    private static void GoForward(GameFigure figure, int nofSteps)
    {
        throw new NotImplementedException();
    }

    private static void GoHome(GameFigure figure)
    {
        throw new NotImplementedException();
    }

    private static void LeaveHome(GameFigure figure)
    {
        throw new NotImplementedException();
    }

    private static void CreateTurn()
    {
        throw new NotImplementedException();
    }

    private static void ExecuteTurn()
    {
        throw new NotImplementedException();
    }

    private static void ChangePlayerOnTurn()
    {
        throw new NotImplementedException();
    }

    private static void StartNextTurn()
    {
        throw new NotImplementedException();
    }

    private static void FinishGame()
    {
        gameIsRunning = false;
        throw new NotImplementedException();
    }   
}