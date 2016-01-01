using System.Collections;

public static class TurnCtrl
{

    private static readonly int MaxNofTurns = 5;

    private static bool hasToGoBackwards = false;

    private static ArrayList turns = new ArrayList();

    public static void Execute(GameFigure figure, int steps)
    {
        Turn turn = CreateTurn(figure, steps);
        AddTurn(turn);
        ExecuteTurn(turn);
    }

    private static Turn CreateTurn(GameFigure figure, int diceValue)
    {
        return new Turn(figure, figure.Field, diceValue);
    }

    private static void ExecuteTurn(Turn turn)
    {
        GameFigure figure = turn.Figure;
        int startFieldIndex = turn.Start;
        int steps = turn.DiceValue;

        // figure is at home and may be released
        if (FieldCtrl.IsHomeField(startFieldIndex))
        {
            if (!ReleaseFigureFromHome(figure))
            {
                // figure cannot be released because the HomeBench
                // is not accessible
            }
        }
        else // figure is not at home
        {
            for (int i = 0; i < steps; ++i)
            {
                // if next field is a barrier
                if (!MakeOneStep(figure, i == steps - 1))
                {
                    // isbarrier
                }
            }
        }
    }

    private static bool ReleaseFigureFromHome(GameFigure figure)
    {
        return FieldCtrl.PlaceFigureOnHomeBench(figure);
    }

    private static bool MakeOneStep(GameFigure figure, bool isLastStep)
    {
        int actualFieldIndex = figure.Field;
        int nextFieldIndex;

        // figure is on its StairBench and can enter the stair
        if (FieldCtrl.IsStairBench(actualFieldIndex, figure))
        {
            EnterStair(figure);
        }
        // figure is on a RegularField and moves to another one
        else if(FieldCtrl.IsRegularField(actualFieldIndex))
        {
            nextFieldIndex = FieldCtrl.GetNextRegularFieldIndex(actualFieldIndex);
            if (FieldCtrl.IsBarrier(nextFieldIndex)) return false;
            GameFigure figureToSendHome = MakeOneRegularStep(figure);
            if (figureToSendHome != null) FieldCtrl.SendFigureHome(figureToSendHome);
        }
        // figure is in Goal and has to go backwards
        else if(hasToGoBackwards && FieldCtrl.IsGoalField(actualFieldIndex))
        {
            LeaveGoal(figure);
        }
        // figure is on a StairField and has to go backwards
        else if (hasToGoBackwards)
        {
            MakeOneStairStepBackward(figure);
        }
        // figure is on the last StairStep and can enter Goal
        else if (FieldCtrl.IsLastStairStep(actualFieldIndex, figure))
        {
            EnterGoal(figure);
        }
        // figure is on a StairField and has to go forwards
        else
        {
            MakeOneStairStepForward(figure);
        }
        if (isLastStep) hasToGoBackwards = false;
        return true;
    }

    private static GameFigure MakeOneRegularStep(GameFigure figure)
    {
        return FieldCtrl.PlaceFigureOnNextRegularField(figure);
    }

    private static void EnterStair(GameFigure figure)
    {
        FieldCtrl.PlaceFigureOnFirstStairStep(figure);
    }

    private static void MakeOneStairStepForward(GameFigure figure)
    {
        FieldCtrl.PlaceFigureOnNextRegularStairStep(figure, false);
    }

    private static void MakeOneStairStepBackward(GameFigure figure)
    {
        FieldCtrl.PlaceFigureOnNextRegularStairStep(figure, true);
    }

    private static void EnterGoal(GameFigure figure)
    {
        FieldCtrl.PlaceFigureInGoal(figure);
    }

    private static void LeaveGoal(GameFigure figure)
    {
        FieldCtrl.PlaceFigureOnLastStairStep(figure);
    }

    private static void AddTurn(Turn turn)
    {
        turns.Add(turn);
        if (turns.Count > MaxNofTurns) turns.Remove(turns[0]);
    }
}