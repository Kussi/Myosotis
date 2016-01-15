using System.Collections;
using UnityEngine;

/// <summary>
/// Responsible for the all over game control
/// </summary>
public static class GameCtrl
{
    private static readonly string EndMenu = "EndMenu";

    private static bool gameIsRunning = false;
    private static bool playerFinished = false;

    private static int playerOnTurn;
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
            if (GameIsRunning) return (Player)activePlayers[playerOnTurn];
            throw new InvalidGameStateException();
        }
    }

    /// <summary>
    /// Resets the original setup
    /// </summary>
    public static void Reset()
    {
        playerOnTurn = -1;
        activePlayers = null;
        actualDiceValue = 0;
        turnCounter = 0;
        gameIsRunning = false;
    }

    /// <summary>
    /// starts a game
    /// </summary>
    public static void StartGame()
    {
        Reset();
        gameIsRunning = true;
        turnCounter = 0;
        activePlayers = new ArrayList(PlayerCtrl.players);
        StartNextTurn();
    }

    /// <summary>
    /// gets the notification that the dice has been thrown and hands
    /// it over to the players state
    /// </summary>
    /// <param name="value"></param>
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

    /// <summary>
    /// gets the notification that the player has been selected and hands
    /// over to the turn controler to execute the turn
    /// </summary>
    /// <param name="figure"></param>
    public static void Notify(Figure figure)
    {
        TurnCtrl.Execute(figure, actualDiceValue);
    }

    /// <summary>
    /// Gets the notification of the turnctrl that the turn has been
    /// completely executed and launches the next one
    /// </summary>
    public static void Notify()
    {
        StartNextTurn();
    }

    /// <summary>
    /// activates all figures that are on a regular or stair field (not on home or
    /// goal field)
    /// </summary>
    public static void ActivateReleasedFigures()
    {
        FigureCtrl.ActivateReleasedFigures(PlayerOnTurn);
    }

    /// <summary>
    /// activates all figures, except of those who are in goal
    /// </summary>
    public static void ActivateAllFigures()
    {
        FigureCtrl.ActivateAllFigures(PlayerOnTurn);
    }

    /// <summary>
    /// activates no figures and launches the next turn
    /// </summary>
    public static void ActivateNoFigure()
    {
        StartNextTurn();
    }

    /// <summary>
    /// Finishes a player who has  finished all his figures
    /// </summary>
    /// <param name="player"></param>
    public static void FinishPlayer(Player player)
    {
        int numberOfPlayersOld = activePlayers.Count;
        activePlayers.Remove(player);
        if (numberOfPlayersOld - 1 != activePlayers.Count)
            throw new InvalidGameStateException();
        if (activePlayers.Count == 0)
            FinishGame();
    }

    /// <summary>
    /// Changes the current player
    /// </summary>
    private static void ChangePlayerOnTurn()
    {
        playerOnTurn = (playerOnTurn + 1) % activePlayers.Count;
    }

    /// <summary>
    /// starts the next turn
    /// </summary>
    private static void StartNextTurn()
    {
        foreach (Player player in activePlayers)
            player.RefreshState();
        if (playerOnTurn >= 0)
            if (PlayerOnTurn.State.GetType().Equals(typeof(PlayerStateStateAllInGoal)))
            {
                FinishPlayer(PlayerOnTurn);
                playerFinished = true;
            }

        if (gameIsRunning)
        {
            if (actualDiceValue != 6 || playerFinished)
            {
                ChangePlayerOnTurn();
                playerFinished = false;
            }

            ++turnCounter;
            Debug.Log("Turn " + turnCounter + ": " + PlayerOnTurn.Color);
            DiceCtrl.StartDiceThrowingProcess(PlayerOnTurn.Color);
        }
    }

    /// <summary>
    /// finishes the whole game and displays the endmenu
    /// </summary>
    private static void FinishGame()
    {
        gameIsRunning = false;
        GameObject.Find(EndMenu).GetComponent<EndMenu>().Show();
    }
}