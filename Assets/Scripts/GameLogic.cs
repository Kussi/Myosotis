using System.Collections;
using UnityEngine;

public static class GameLogic {

    private static Player[] player;
    private static ArrayList gameFields;
    private static AbstractLogicState logicState;
    private static int lastDiceValue = 0;

    private static int playerOnTurn = 4;

    public static Player PlayerOnTurn
    {
        get { return player[playerOnTurn]; }
    }

    public static AbstractLogicState State
    {
        get { return logicState; }
        set { logicState = value; }
    }

    public static void Initialize(Player[] player, ArrayList gameFields)
    {
        GameLogic.player = player;
        GameLogic.gameFields = gameFields;
        GameLogic.logicState = new LogicStateThrowingDice();
    }

    public static void ExecuteTurn(int number)
    {
        lastDiceValue = number;

        if (number == 5)
        {
            PlayerOnTurn.State.ThrowsFive();
        }
        else if (number == 6)
        {
            PlayerOnTurn.State.ThrowsSix();
        }
        else
        {
            PlayerOnTurn.State.ThrowsRegular(number);
        }
    }

    public static GameFigure[] GetFiguresOnGameField(GameFigure[] figures)
    {
        ArrayList result = new ArrayList();
        foreach(GameFigure figure in figures)
        {
            if(figure.Field.GetType() == typeof(GameField))
            {
                result.Add(figure);
            }
        }
        return (GameFigure[])result.ToArray(typeof(GameFigure));
    }

    public static GameFigure[] GetFiguresOnGameOrHomeField(GameFigure[] figures)
    {
        ArrayList result = new ArrayList();
        foreach (GameFigure figure in figures)
        {
            if (figure.Field.GetType() == typeof(GameField) || figure.Field.GetType() == typeof(HomeField))
            {
                result.Add(figure);
            }
        }
        return (GameFigure[])result.ToArray(typeof(GameFigure));
    }

    public static GameFigure[] GetFiguresOnGoalField(GameFigure[] figures)
    {
        ArrayList result = new ArrayList();
        foreach (GameFigure figure in figures)
        {
            if (figure.Field.GetType() == typeof(GoalField))
            {
                result.Add(figure);
            }
        }
        return (GameFigure[])result.ToArray(typeof(GameFigure));
    }

    public static GameFigure[] GetFiguresOnHomeField(GameFigure[] figures)
    {
        ArrayList result = new ArrayList();
        foreach (GameFigure figure in figures)
        {
            if (figure.Field.GetType() == typeof(HomeField))
            {
                result.Add(figure);
            }
        }
        return (GameFigure[])result.ToArray(typeof(GameFigure));
    }

    public static void NextPlayer()
    {
        playerOnTurn = (playerOnTurn + 1) % 4;
    }

    public static void Move(GameFigure figure)
    {
        if(figure.Field.GetType() == typeof(HomeField))
        {
            figure.transform.position = ((GameField)gameFields[figure.Parent.HomeBank-1]).transform.position;
            foreach (GameFigure gameFigure in figure.Parent.GameFigures) gameFigure.SetActive(false);
            GameLogic.State = new LogicStateThrowingDice();
        }
        else
        {
            // TODO
        }

        figure.Parent.RefreshState();
    }
}