using System.Collections;
using UnityEngine;

public static class GameLogic {

    private static Player[] player;
    private static ArrayList gameFields;
    private static AbstractLogicState logicState;
    private static int lastDiceValue = 0;

    private static int playerOnTurn = 0;

    public static Player PlayerOnTurn
    {
        get { return player[playerOnTurn]; }
    }

    public static AbstractLogicState State
    {
        get { return logicState; }
        set { logicState = value; }
    }

    public static int LastDiceValue
    {
        get { return lastDiceValue; }
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
            figure.Field.RemoveGameFigure(figure);
            figure.Field = (GameField)gameFields[figure.Parent.HomeBank];
            figure.Field.PlaceGameFigure(figure);

            figure.transform.position = figure.Field.transform.position;
            
            foreach (GameFigure gameFigure in figure.Parent.GameFigures) gameFigure.SetActive(false);
            GameLogic.State = new LogicStateThrowingDice();
        }
        else
        {
            for(int i = 0; i < lastDiceValue; ++i)
            {
                GoOneStep(figure);
            }

            foreach (GameFigure gameFigure in figure.Parent.GameFigures) gameFigure.SetActive(false);
            GameLogic.State = new LogicStateThrowingDice();
        }

        figure.Parent.RefreshState();
    }

    public static void GoOneStep(GameFigure figure)
    {
        int actualPosition = figure.Field.Index;
        int nextPosition = (actualPosition + 1) % gameFields.Count;

        ((AbstractGameField)gameFields[actualPosition]).RemoveGameFigure(figure);
        ((AbstractGameField)gameFields[nextPosition]).PlaceGameFigure(figure);
        figure.Field = (AbstractGameField)gameFields[nextPosition];

        if(figure.Field.GetType() == typeof(GameField))
        {
            GameLogic.SendHome(figure, (GameField)figure.Field);
        }

        figure.transform.position = figure.Field.transform.position;
    }

    private static void SendHome(GameFigure figure, GameField field)
    {
        GameFigure[] figures = field.GameFigures;
        foreach (GameFigure gameFigure in figures)
        {
            if(gameFigure != null && !gameFigure.Parent.Color.Equals(figure.Parent.Color))
            {
                GameLogic.GoHome(gameFigure);
            }
        }
    }

    private static void GoHome(GameFigure figure)
    {
        bool hasSpace = false;
        HomeField[] homeFields = figure.Parent.HomeFields;
        for(int i = 0; i < homeFields.Length; ++i)
        {
            if(!homeFields[i].IsOccupied)
            {
                figure.transform.position = homeFields[i].transform.position;
                hasSpace = true;
                break;
            }
        }
        if (!hasSpace) throw new UnityException();
    }
}