using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;

public static class PlayerCtrl {

    private static readonly Dictionary<string, int> PlayerAngles = new Dictionary<string, int>
    {
        { "Red", 180 }, { "Yellow", 270 }, { "Blue", 0 }, { "Green", 90 }
    };

    private static readonly int MinNofPlayers = 2;
    private static readonly int MaxNofPlayers = 4;

    public static ArrayList players = new ArrayList();

    public static ArrayList GetPlayers()
    {
        return players;
    }

    /// <summary>
    /// Initializing the given number of Players
    /// </summary>
    /// <param name="nofPlayers">number of players, that will participate the game</param>
    public static void InitializePlayers(int nofPlayers)
    {
        switch (nofPlayers)
        {
            case 2:
                players.Add(new Player("Red", GetPlayerAngle("Red")));
                players.Add(new Player("Blue", GetPlayerAngle("Blue")));
                break;
            case 3:
                players.Add(new Player("Red", GetPlayerAngle("Red")));
                players.Add(new Player("Yellow", GetPlayerAngle("Yellow")));
                players.Add(new Player("Blue", GetPlayerAngle("Blue")));
                break;
            case 4:
                players.Add(new Player("Red", GetPlayerAngle("Red")));
                players.Add(new Player("Yellow", GetPlayerAngle("Yellow")));
                players.Add(new Player("Blue", GetPlayerAngle("Blue")));
                players.Add(new Player("Green", GetPlayerAngle("Green")));
                break;
            default:
                throw new ArgumentException("game can only be played with "
                + MinNofPlayers + " to " + MaxNofPlayers + " players.");
        }

        Debug.Log(ToString());
    }

    private static int GetPlayerAngle(string color)
    {
        int angle;
        PlayerAngles.TryGetValue(color, out angle);
        return angle;
    }

    private static new string ToString()
    {
        StringBuilder result = new StringBuilder(players.Count + " players initialized.");
        result.Append(" [");

        object last = players[players.Count - 1];
        foreach (Player player in players)
        {
            result.Append(player.Color);
            if (!player.Equals(last)) result.Append(", ");
        }

        result.Append("]");

        return result.ToString();
    }
}