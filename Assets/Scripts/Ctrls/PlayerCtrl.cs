using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public static class PlayerCtrl
{

    private static readonly Dictionary<string, int> PlayerAngles = new Dictionary<string, int>
    {
        { "red", 180 }, { "yellow", 270 }, { "blue", 0 }, { "green", 90 }
    };

    private static readonly Dictionary<string, Color> PlayerColors = new Dictionary<string, Color>
    {
        //{ "Red", new Color(255, 0, 0) }, { "Yellow", new Color(220, 200, 0) }, { "Blue", new Color(15, 15, 255) }, { "Green", new Color(0, 153, 0) }
        { "red", new Color(1, 0, 0) }, { "yellow", new Color(220, 200, 0) }, { "blue", new Color(15/255, 15/255, 1) }, { "green", new Color(0, 153, 0) }
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
        PlayerColors.TryGetValue(player.Color.ToLower(), out color);
        if (color == null) throw new InvalidGameStateException();
        return color;
    }

    /// <summary>
    /// Initializing the given number of Players
    /// </summary>
    /// <param name="nofPlayers">number of players, that will participate the game</param>
    public static void InitializePlayers(Dictionary<string, int> playerColors)
    {
        foreach (string playerColor in playerColors.Keys)
        {
            int marker;
            playerColors.TryGetValue(playerColor, out marker);
            players.Add(new Player(playerColor, GetPlayerAngle(playerColor, marker)));
        }
        Debug.Log(ToString());
    }

    private static int GetPlayerAngle(string color, int marker)
    {
        int angle;
        PlayerAngles.TryGetValue(color.ToLower(), out angle);
        return marker == 0 ? angle : (angle + 90) % 360;
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