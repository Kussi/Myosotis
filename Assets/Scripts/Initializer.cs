using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public static class Initializer
{
    public static void SetupGame(string playerName, int nofPlayers, int nofGameFigures)
    {
        GameFieldCtrl.InitializeGameFields();
        PlayerCtrl.InitializePlayers(nofPlayers);
        DiceCtrl.InitializeDices(PlayerCtrl.players);
        GameFigureCtrl.InitializeGameFigures(PlayerCtrl.players, nofGameFigures);
        SunCtrl.InitializeSun();
        //InitializePersonalisation(playerName);

        // place GameFigures on their HomeField
        foreach(Player player in PlayerCtrl.players)
        {
            HomeField homeField = GameFieldCtrl.GetHomeField(player);
            foreach(GameFigure figure in GameFigureCtrl.GetGameFigures(player))
            {
                homeField.PlaceGameFigure(figure);
            }
        }

        //Place dice -> diceObject.transform.localPosition = new Vector3(0, 0, 0);
        // rotate the dice towards the player
        //Vector3 diceRotation = dice.transform.eulerAngles;
        //diceRotation = new Vector3(diceRotation.x, playerAngle, diceRotation.z);
        //dice.transform.eulerAngles = diceRotation;

        GameCtrl.StartGame();
    }

    private static void InitializePersonalisation(string playerName)
    {
        ImageCtrl.InitializeImages(playerName);
        MusicCtrl.InitializeMusic(playerName);
        TextCtrl.InitializeTexts(playerName);
    }   
}