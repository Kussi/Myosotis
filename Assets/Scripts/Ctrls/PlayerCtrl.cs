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

    private static readonly Dictionary<string, Color> PlayerColors = new Dictionary<string, Color>
    {
        //{ "Red", new Color(255, 0, 0) }, { "Yellow", new Color(220, 200, 0) }, { "Blue", new Color(15, 15, 255) }, { "Green", new Color(0, 153, 0) }
        { "Red", new Color(1, 0, 0) }, { "Yellow", new Color(220, 200, 0) }, { "Blue", new Color(15/255, 15/255, 1) }, { "Green", new Color(0, 153, 0) }
    };

    private static readonly int MinNofPlayers = 2;
    private static readonly int MaxNofPlayers = 4;

    public static ArrayList players = new ArrayList();

    public static ArrayList GetPlayers()
    {
        return players;
    }

    public static Color GetColor(Player player)
    {
        Color color;
        PlayerColors.TryGetValue(player.Color, out color);
        if (color == null) throw new InvalidGameStateException();
        return color;
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