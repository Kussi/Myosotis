using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class GameLogic {

    private static Player[] players;
    private static Dictionary<int, GameFieldBase> gameFields;
    private static ILogicState state;
    private static bool hasToGoBackwards = false;
    private static int nofRegularFields = 68;

    private static int playerOnTurn = 0;

    public static Player PlayerOnTurn
    {
        get { return players[playerOnTurn]; }
        private set { players[playerOnTurn] = value; }
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

    /// <summary>
    /// Initializes the Game
    /// </summary>
    /// <param name="players">all Players that will play the game</param>
    /// <param name="gameFields">all GameFields of the game</param>
    public static void Initialize(Player[] players, Dictionary<int, GameFieldBase> gameFields)
    {
        GameLogic.players = players;
        GameLogic.gameFields = gameFields;
        GameLogic.State = new LogicStateThrowingDice();
        GameLogic.PlaceAllFiguresAtHome();


        GameObject diceTexture = GameObject.Find(PlayerOnTurn.Dice.gameObject.name + "/default");
        Material[] mats = new Material[2];
        mats[0] = Resources.Load("Materials/DiceOnTurn", typeof(Material)) as Material;
        mats[1] = diceTexture.GetComponent<MeshRenderer>().materials[1];
        diceTexture.GetComponent<MeshRenderer>().materials = mats;

        Debug.Log("[GameLogic] Game has initialized successfully");
    }

    /// <summary>
    /// Executes Turn, according to the diced number. This Method is calld from
    /// the Dice itself.
    /// </summary>
    /// <param name="number">diced value</param>
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
    public static GameFigure[] GetFiguresOnRegularOrStairField(GameFigure[] figures)
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

    /// <summary>
    /// Activates the Dice of the PlayerOnTurn and therefore makes it touchable
    /// </summary>
    public static void ActivateDice()
    {
        PlayerOnTurn.Dice.SetActive(true);
    }

    /// <summary>
    /// Changes the PlayerOnTurn
    /// </summary>
    public static void NextPlayer()
    {
        GameObject diceTexture = GameObject.Find(PlayerOnTurn.Dice.gameObject.name + "/default");
        Material[] mats = new Material[2];
        mats[0] = Resources.Load("Materials/DiceRegular", typeof(Material)) as Material;
        mats[1] = diceTexture.GetComponent<MeshRenderer>().materials[1];
        diceTexture.GetComponent<MeshRenderer>().materials = mats;

        // if the current player entered the goal field with his last figure 
        if (PlayerOnTurn.State.GetType() == typeof(PlayerStateStateAllInGoal))
        {
            Player[] temp = new Player[players.Length - 1];
            int index = 0;
            for (int i = 0; i < players.Length; ++i)
            {
                if (i != playerOnTurn) temp[index++] = players[i];
            }
            players = temp;
        }
        else playerOnTurn += 1;

        playerOnTurn %= players.Length;

        GameObject sun = GameObject.Find("Sun");
        sun.GetComponent<Sun>().Angle = PlayerOnTurn.LightAngle;

        diceTexture = GameObject.Find(PlayerOnTurn.Dice.gameObject.name + "/default");
        mats = new Material[2];
        mats[0] = Resources.Load("Materials/DiceOnTurn", typeof(Material)) as Material;
        mats[1] = diceTexture.GetComponent<MeshRenderer>().materials[1];
        diceTexture.GetComponent<MeshRenderer>().materials = mats;

        Debug.Log("[GameLogic] PlayerOnTurn: " + PlayerOnTurn.Color);
    }

    /// <summary>
    /// Controls the Movement of a figure and the diced number. For each steps to be done
    /// GoOneStep is called.
    /// </summary>
    /// <param name="figure">figure, that will be moved</param>
    public static void MoveFigure(GameFigure figure)
    {
        for(int i = 0; i < figure.Parent.Dice.Value; ++i)
        {
            GoOneStep(figure, i == figure.Parent.Dice.Value - 1);
            Debug.Log("Done one step " + i);
        }
        hasToGoBackwards = false;
        FinishTurn();  
    }

    /// <summary>
    /// Moves a figure one step, if the game situation allows it
    /// </summary>
    /// <param name="figure">figure, that will be moved</param>
    /// <param name="isLastStep">value, whether this is the last step of
    /// the diced number or not</param>
    public static void GoOneStep(GameFigure figure, bool isLastStep)
    {
        int actualPosition = figure.Field.Index;
        int nextPosition = actualPosition + 1;

        // Figure stands on the StairBench and can enter its stair with this step
        if (actualPosition == figure.Parent.StairBenchIndex) 
        {
            EnterStair(figure);
        }
        
        // Figure has entered the stair and is from now on only walking on the stair steps
        else if (gameFields[actualPosition].GetType() == typeof(StairField) 
            || gameFields[actualPosition].GetType() == typeof(GoalField)) 
        {
            GoOneStepOnStair(figure, isLastStep);
        }

        // Figure walks on the Regular Fields 
        else if(nextPosition <= nofRegularFields)
        {
            // if it steps over the last regular field index
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
    /// <param name="figure">figure, that walks</param>
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

    /// <summary>
    /// figure can walk on its first stair step
    /// </summary>
    /// <param name="figure">figure that walks</param>
    private static void EnterStair(GameFigure figure)
    {
        PlaceFigureOnField(figure, figure.Parent.FirstStairStepIndex);
    }

    /// <summary>
    /// Figure will go one step backwards on stair
    /// </summary>
    /// <param name="figure">figure that has to walk</param>
    public static void GoOneStepBack(GameFigure figure)
    {
        int actualPosition = figure.Field.Index;
        int nextPosition;

        if(actualPosition == Initializer.GoalFieldIndex)
        {
            nextPosition = figure.Parent.FirstStairStepIndex + 6;
        }
        else
        {
            nextPosition = actualPosition - 1;
        }
        
        PlaceFigureOnField(figure, nextPosition);
    }

    /// <summary>
    /// figures on the field with index fieldIndex will be sent home, if
    /// they are figures of another Player
    /// </summary>
    /// <param name="figure">figure, that sends the others home</param>
    /// <param name="fieldIndex">field, which is affected</param>
    private static void SendHome(GameFigure figure, int fieldIndex)
    {
        RegularField field = (RegularField)gameFields[fieldIndex];
        if (!field.IsBench)
        {
            GameFigure[] figuresOnNextField = field.GetGameFigures();
            foreach (GameFigure figureOnNextField in figuresOnNextField)
            {
                if (figureOnNextField != null && !figureOnNextField.Parent.Equals(figure.Parent))
                {
                    GameLogic.GoHome(figureOnNextField);
                }
            }
        }
    }

    /// <summary>
    /// Moves a figure to one of its HomeFields
    /// </summary>
    /// <param name="figure">figure, that goes home</param>
    private static void GoHome(GameFigure figure)
    {
        PlaceFigureOnField(figure, figure.Parent.HomeFieldIndex);
    }

    /// <summary>
    /// Moves a figure to another GameField. Figure is entered in the figure Array
    /// of the Field and the field in the field attribute of the figure.
    /// </summary>
    /// <param name="figure">figure, that will be moved</param>
    /// <param name="fieldIndex">target field</param>
    private static void PlaceFigureOnField(GameFigure figure, int fieldIndex)
    {
        PlaceFigureOnField(figure, gameFields[fieldIndex]);
    }

    /// <summary>
    /// Moves a figure to another GameField. Figure is entered in the figure Array
    /// of the Field and the field in the field attribute of the figure.
    /// </summary>
    /// <param name="figure">figure, that will be moved</param>
    /// <param name="field">target field</param>
    private static void PlaceFigureOnField(GameFigure figure, GameFieldBase nextField)
    {
        Debug.Log(figure + " will be placed on field " + nextField);
        GameFieldBase actualField = figure.Field;
        actualField.RemoveGameFigure(figure);
        nextField.PlaceGameFigure(figure);
    }

    /// <summary>
    /// Deactivates all GameFigures and changes the GameLogicState
    /// </summary>
    private static void FinishTurn()
    {
        foreach (GameFigure gameFigure in PlayerOnTurn.GameFigures) gameFigure.SetActive(false);
        foreach (Player player in players) player.RefreshState();
        GameLogic.State = new LogicStateThrowingDice();
    }

    /// <summary>
    /// Releases a figure from its home, if there is no barrier on the HomeBench
    /// </summary>
    /// <param name="figure">figure, that will be released</param>
    public static void ReleaseFigure(GameFigure figure)
    {
        int homeBench = figure.Parent.HomeBenchIndex;

        // If there is a barrier on the HomeBench, no figure can be released
        if(!((RegularField)gameFields[homeBench]).IsBarrier)
        {
            PlaceFigureOnField(figure, figure.Parent.HomeBenchIndex);
            FinishTurn();
        }
    }

    /// <summary>
    /// Activates a figure and therefore makes it touchable
    /// </summary>
    /// <param name="figure">figure, that is affected</param>
    private static void ActivateFigure(GameFigure figure)
    {
        figure.SetActive(true);
        Debug.Log(figure.gameObject.name + " has been activated.");
    }

    /// <summary>
    /// Deactivates a figure and therefore makes it untouchable
    /// </summary>
    /// <param name="figure">figure, that is affected</param>
    private static void DeactivateFigure(GameFigure figure)
    {
        figure.SetActive(false);
        Debug.Log(figure.gameObject.name + " has been deactivated.");
    }

    /// <summary>
    /// changes the positions of all figures to their homefield
    /// </summary>
    private static void PlaceAllFiguresAtHome()
    {
        foreach(Player player in players)
        {
            foreach(GameFigure gameFigure in player.GameFigures)
            {
                gameFields[player.HomeFieldIndex].PlaceGameFigure(gameFigure);
            }
        }
    }
}