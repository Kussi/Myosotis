﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLogic {

    private static Player[] player;
    private static Dictionary<int, GameFieldBase> gameFields;
    private static ILogicState state;
    private static bool hasToGoBackwards = false;
    private static int nofRegularFields = 68;

    private static int playerOnTurn = 0;

    public static Player PlayerOnTurn
    {
        get { return player[playerOnTurn]; }
        private set { player[playerOnTurn] = value; }
    }

    public static ILogicState State
    {
        get { return state; }
        set
        {
            state = value;
            Debug.Log("[GameLogic] State has been changed to " + State);
        }
    }

    public static void Initialize(Player[] player, Dictionary<int, GameFieldBase> gameFields)
    {
        GameLogic.player = player;
        GameLogic.gameFields = gameFields;
        GameLogic.State = new LogicStateThrowingDice();
        PlayerOnTurn.Dice.gameObject.GetComponent<TextMesh>().color = new Color(255F, 0F, 0F, 1F);
        Debug.Log("[GameLogic] Game has initialized successfully");
    }

    public static void ExecuteTurn(int number)
    {
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

    /// <summary>
    /// Takes an Array of GameFigures and returns all, which are currently on a RegularField
    /// </summary>
    /// <param name="figures">Regularly all GameFigures of a Player</param>
    /// <returns>All Figures, which are currently on a RegularField</returns>
    public static GameFigure[] GetFiguresOnRegularField(GameFigure[] figures)
    {
        ArrayList result = new ArrayList();
        foreach(GameFigure figure in figures)
        {
            if(figure.Field.GetType() == typeof(RegularField))
            {
                result.Add(figure);
            }
        }
        return (GameFigure[])result.ToArray(typeof(GameFigure));
    }

    /// <summary>
    /// Takes an Array of GameFigures and returns all, which are currently on a GoalField
    /// </summary>
    /// <param name="figures">Regularly all GameFigures of a Player</param>
    /// <returns>All Figures, which are currently on a GoalField</returns>
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

    /// <summary>
    /// Takes an Array of GameFigures and returns all, which are currently on a HomeField
    /// </summary>
    /// <param name="figures">Regularly all GameFigures of a Player</param>
    /// <returns>All Figures, which are currently on a HomeField</returns>
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

    /// <summary>
    /// Takes an Array of GameFigures and returns all, which are currently on a RegularField or on a HomeField or on a StairField
    /// </summary>
    /// <param name="figures">Regularly all GameFigures of a Player</param>
    /// <returns>All Figures, which are currently on a RegularField or on a HomeField or on a StairField</returns>
    private static GameFigure[] GetFiguresOnRegularOrHomeOrStairField(GameFigure[] figures)
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

    /// <summary>
    /// Takes an Array of GameFigures and returns all, which are currently on a RegularField or on a StairField
    /// </summary>
    /// <param name="figures">Regularly all GameFigures of a Player</param>
    /// <returns>All Figures, which are currently on a RegularField or on a StairField</returns>
    private static GameFigure[] GetFiguresOnRegularOrStairField(GameFigure[] figures)
    {
        ArrayList result = new ArrayList();
        foreach (GameFigure figure in figures)
        {
            if (figure.Field.GetType() == typeof(RegularField) || figure.Field.GetType() == typeof(StairField))
            {
                result.Add(figure);
            }
        }
        return (GameFigure[])result.ToArray(typeof(GameFigure));
    }

    /// <summary>
    /// Activates GameFigures from PlayerOnTurn, which are currently on a RegularField or on a HomeField or on a StairField
    /// </summary>
    public static void ActivateFiguresOnRegularOrHomeOrStairField()
    {
        GameFigure[] figures = GetFiguresOnRegularOrHomeOrStairField(PlayerOnTurn.GameFigures);
        foreach (GameFigure figure in figures) figure.SetActive(true);
    }

    /// <summary>
    /// Activates GameFigures from PlayerOnTurn, which are currently on a RegularField or on a StairField
    /// </summary>
    public static void ActivateFiguresOnRegularOrStairField()
    {
        GameFigure[] figures = GetFiguresOnRegularOrStairField(PlayerOnTurn.GameFigures);
        foreach (GameFigure figure in figures) figure.SetActive(true);
    }

    /// <summary>
    /// Activates all Figures
    /// </summary>
    /// <param name="figures">All figures, which have to be activated</param>
    private static void ActivateFigures(GameFigure[] figures)
    {
        foreach (GameFigure figure in figures) ActivateFigure(figure);
    }

    /// <summary>
    /// Deactivates all Figures
    /// </summary>
    /// <param name="figures">All figures, which have to be deactivated</param>
    private static void DeactivateFigures(GameFigure[] figures)
    {
        foreach (GameFigure figure in figures) DeactivateFigure(figure);
    }

    public static void ActivateDice()
    {
        PlayerOnTurn.Dice.SetActive(true);
    }

    public static void NextPlayer()
    {
        PlayerOnTurn.Dice.gameObject.GetComponent<TextMesh>().color = new Color(0F,0F,0F,1F);
        playerOnTurn = (playerOnTurn + 1) % 4;
        PlayerOnTurn.Dice.gameObject.GetComponent<TextMesh>().color = new Color(255F, 0F, 0F, 1F);
        Debug.Log("[GameLogic] PlayerOnTurn: " + PlayerOnTurn.Color);
    }

    public static void MoveFigure(GameFigure figure)
    {
        for(int i = 0; i < figure.Parent.Dice.Value; ++i)
        {
            GoOneStep(figure, i == figure.Parent.Dice.Value - 1);
        }
        hasToGoBackwards = false;
        FinishTurn();  
    }

    public static void GoOneStep(GameFigure figure, bool isLastStep)
    {
        int actualPosition = figure.Field.Index;
        int nextPosition = actualPosition + 1;

        // Figure stands on the StairBench and can enter its stair with this step
        if (actualPosition == figure.Parent.StairBench) 
        {
            EnterStair(figure);
        }
        
        // Figure has entered the stair and is from now on only walking on the stair steps
        else if (gameFields[actualPosition].GetType() == typeof(StairField)) 
        {
            GoOneStepOnStair(figure, isLastStep);
        }

        // Figure walks on the Regular Fields and is stepping over the last field index 
        else if(nextPosition <= nofRegularFields)
        {
            nextPosition %= nofRegularFields;

            // Next Field is NOT blocked by a barrier
            if (!((RegularField)gameFields[nextPosition]).IsBarrier)
            {
                PlaceFigureOnField(figure, nextPosition);
                SendHome(figure, nextPosition);
            }

            // Next Field is blocked by a barrier.
            else
            {
                PlaceFigureOnField(figure, actualPosition);
            }
        } 
    }

    /// <summary>
    /// Figure takes a regular step on the stair. If it reached the end
    /// and has steps left, it walks backwards, otherwise it enters the goal. 
    /// </summary>
    /// <param name="figure">figure that walks</param>
    /// <param name="isLastStep">value, if this step is the last step of this round</param>
    private static void GoOneStepOnStair(GameFigure figure, bool isLastStep)
    {
        int actualPosition = figure.Field.Index;
        int nextPosition = actualPosition + 1;

        if (hasToGoBackwards) GoOneStepBack(figure);
        else if (nextPosition % 100 == Initializer.NofStairFieldsEachPlayer)
        {
            PlaceFigureOnField(figure, Initializer.GoalFieldIndex);
            if (!isLastStep) hasToGoBackwards = true;
        }
        else
        {
            PlaceFigureOnField(figure, nextPosition);
        }
    }

    private static void EnterStair(GameFigure figure)
    {
        PlaceFigureOnField(figure, figure.Parent.FirstStairStep);
    }

    /// <summary>
    /// Figure will go one step backwards on stair
    /// </summary>
    /// <param name="figure">figure which has to walk</param>
    public static void GoOneStepBack(GameFigure figure)
    {
        int actualPosition = figure.Field.Index;
        int nextPosition = actualPosition - 1;

        PlaceFigureOnField(figure, nextPosition);
    }

    private static void SendHome(GameFigure figure, int fieldIndex)
    {
        RegularField field = (RegularField)gameFields[fieldIndex];
        if (!field.IsBench)
        {
            GameFigure[] figuresOnNextField = field.GameFigures;
            foreach (GameFigure figureOnNextField in figuresOnNextField)
            {
                if (figureOnNextField != null && !figureOnNextField.Parent.Equals(figure.Parent))
                {
                    GameLogic.GoHome(figureOnNextField);
                }
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
                PlaceFigureOnField(figure, homeFields[i]);
                hasSpace = true;
                break;
            }
        }
        if (!hasSpace) throw new InvalidGameStateException();
    }

    private static void PlaceFigureOnField(GameFigure figure, int fieldIndex)
    {
        PlaceFigureOnField(figure, gameFields[fieldIndex]);
    }

    private static void PlaceFigureOnField(GameFigure figure, GameFieldBase field)
    {
        figure.Field.RemoveGameFigure(figure);
        field.PlaceGameFigure(figure);
        figure.Parent.RefreshState();
        figure.transform.position = figure.Field.transform.position;
    }

    private static void FinishTurn()
    {
        foreach (GameFigure gameFigure in PlayerOnTurn.GameFigures) gameFigure.SetActive(false);
        GameLogic.State = new LogicStateThrowingDice();
    }

    public static void ReleaseFigure(GameFigure figure)
    {
        PlaceFigureOnField(figure, figure.Parent.HomeBench);
        FinishTurn();
    }

    private static void ActivateFigure(GameFigure figure)
    {
        figure.SetActive(true);
        Debug.Log(figure.gameObject.name + " has been activated.");
    }

    private static void DeactivateFigure(GameFigure figure)
    {
        figure.SetActive(false);
        Debug.Log(figure.gameObject.name + " has been deactivated.");
    }
}