using UnityEngine;
using System.Collections;

public static class GameLogic {

    private static Player[] player;
    private static ArrayList gameFields;

    private static int playerOnTurn = 0;

    public static void Initialize(Player[] player, ArrayList gameFields)
    {
        GameLogic.player = player;
        GameLogic.gameFields = gameFields;

        player[playerOnTurn].Play();
    }
}