using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLogic {

    private static Player[] player;
    private static ArrayList gameFields;
    private static Dictionary<int, StairField> stairFields;
    private static GoalField goalField;
    private static AbstractLogicState logicState;
    private static int lastDiceValue = 0;
    private static bool hasToGoBackwards = false;
    private static bool skipOneStep = false;

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

    public static void Initialize(Player[] player, ArrayList gameFields, Dictionary<int, StairField> stairFields, GoalField goalField)
    {
        GameLogic.player = player;
        GameLogic.gameFields = gameFields;
        GameLogic.stairFields = stairFields;
        GameLogic.goalField = goalField;
        GameLogic.logicState = new LogicStateThrowingDice();
        player[playerOnTurn].Dice.gameObject.GetComponent<TextMesh>().color = new Color(255F, 0F, 0F, 1F);
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

    public static GameFigure[] GetFiguresOnGameOrHomeOrStairField(GameFigure[] figures)
    {
        ArrayList result = new ArrayList();
        foreach (GameFigure figure in figures)
        {
            if (figure.Field.GetType() != typeof(GoalField))
            {
                result.Add(figure);
            }
        }
        return (GameFigure[])result.ToArray(typeof(GameFigure));
    }

    public static GameFigure[] GetFiguresOnGameOrStairField(GameFigure[] figures)
    {
        ArrayList result = new ArrayList();
        foreach (GameFigure figure in figures)
        {
            if (figure.Field.GetType() == typeof(GameField) || figure.Field.GetType() == typeof(StairField))
            {
                result.Add(figure);
            }
        }
        return (GameFigure[])result.ToArray(typeof(GameFigure));
    }

    public static void NextPlayer()
    {
        player[playerOnTurn].Dice.gameObject.GetComponent<TextMesh>().color = new Color(0F,0F,0F,1F);
        playerOnTurn = (playerOnTurn + 1) % 4;
        player[playerOnTurn].Dice.gameObject.GetComponent<TextMesh>().color = new Color(255F, 0F, 0F, 1F);

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
                if (!hasToGoBackwards)
                {
                    GoOneStep(figure, i == lastDiceValue - 1);
                }
                else
                {
                    if(!skipOneStep)
                    {
                        GoOneStepBack(figure);
                    }
                    skipOneStep = false; 
                } 
            }

            hasToGoBackwards = false;
            foreach (GameFigure gameFigure in figure.Parent.GameFigures) gameFigure.SetActive(false);
            GameLogic.State = new LogicStateThrowingDice();
        }

        figure.Parent.RefreshState();
    }

    public static void GoOneStep(GameFigure figure, bool isLastStep)
    {
        int actualPosition = figure.Field.Index;
        int nextPosition = actualPosition + 1;
        bool nextIsStairField = false;

        if(actualPosition == figure.Parent.StairBank)
        {
            nextPosition = figure.Parent.FirstStairStep;
            nextIsStairField = true;
        }
        else if(actualPosition > 100)
        {
            nextIsStairField = true;
        }
        else if(nextPosition >= gameFields.Count)
        {
            nextPosition %= gameFields.Count;
        }

        if(!nextIsStairField)
        {
            if (!((GameField)gameFields[nextPosition]).IsBarrier)
            {
                ((AbstractGameField)gameFields[actualPosition]).RemoveGameFigure(figure);
                ((AbstractGameField)gameFields[nextPosition]).PlaceGameFigure(figure);
                figure.Field = (AbstractGameField)gameFields[nextPosition];

                GameLogic.SendHome(figure, (GameField)figure.Field);
                figure.transform.position = figure.Field.transform.position;
            }
        }
        else
        {
            // im Ziel
            if(actualPosition%100 == 7 && isLastStep)
            {
                stairFields[actualPosition].RemoveGameFigure(figure);
                goalField.PlaceGameFigure(figure);
                figure.Field = goalField;
                figure.transform.position = figure.Field.transform.position;

            }
            else if(actualPosition%100 == 7)
            {
                hasToGoBackwards = true;
                skipOneStep = true;
            }
            else
            {
                if (nextPosition % 100 == 1)
                {
                    ((AbstractGameField)gameFields[actualPosition]).RemoveGameFigure(figure);
                }
                else
                {
                    stairFields[actualPosition].RemoveGameFigure(figure);
                }
                stairFields[nextPosition].PlaceGameFigure(figure);
                figure.Field = stairFields[nextPosition];
                figure.transform.position = figure.Field.transform.position;
            }
        }
    }

    public static void GoOneStepBack(GameFigure figure)
    {
        int actualPosition = figure.Field.Index;
        int nextPosition = actualPosition - 1;

        stairFields[actualPosition].RemoveGameFigure(figure);
        stairFields[nextPosition].PlaceGameFigure(figure);
        figure.Field = stairFields[nextPosition];
        figure.transform.position = figure.Field.transform.position;
    }

    private static void SendHome(GameFigure figure, GameField field)
    {
        if (!field.IsBench)
        {
            GameFigure[] figures = field.GameFigures;
            foreach (GameFigure gameFigure in figures)
            {
                if (gameFigure != null && !gameFigure.Parent.Color.Equals(figure.Parent.Color))
                {
                    GameLogic.GoHome(gameFigure);
                }
            }
        }
    }

    private static void GoHome(GameFigure figure)
    {
        bool hasSpace = false;
        HomeField[] homeFields = figure.Parent.HomeFields;
        int actualPosition = figure.Field.Index;
        HomeField nextPosition;

        for(int i = 0; i < homeFields.Length; ++i)
        {
            if(!homeFields[i].IsOccupied)
            {
                nextPosition = homeFields[i];
                ((AbstractGameField)gameFields[actualPosition]).RemoveGameFigure(figure);
                nextPosition.PlaceGameFigure(figure);
                figure.Field = nextPosition;
                figure.transform.position = nextPosition.transform.position;
                figure.Parent.RefreshState();
                hasSpace = true;
                break;
            }
        }
        if (!hasSpace) throw new UnityException();
    }
}