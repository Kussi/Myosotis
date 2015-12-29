using UnityEngine;
using System.Collections;
using System;
using System.Text;

public static class PlayerCtrl {

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
                players.Add(new Player("red"));
                players.Add(new Player("blue"));
                break;
            case 3:
                players.Add(new Player("red"));
                players.Add(new Player("yellow"));
                players.Add(new Player("blue"));
                break;
            case 4:
                players.Add(new Player("red"));
                players.Add(new Player("yellow"));
                players.Add(new Player("blue"));
                players.Add(new Player("green"));
                break;
            default:
                throw new ArgumentException("game can only be played with "
                + MinNofPlayers + " to " + MaxNofPlayers + " players.");
        }

        Debug.Log(ToString());
    }

    private static new string ToString()
    {
        StringBuilder result = new StringBuilder(players.Count + " players initialized.");
        result.Append(" [");

        object last = players[players.Count - 1];
        foreach (Player player in players)
        {
            result.Append(player.Name);
            if (!player.Equals(last)) result.Append(", ");
        }

        result.Append("]");

        return result.ToString();
    }
}