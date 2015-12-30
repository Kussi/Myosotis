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
                if (!MakeOneStep(figure, i == steps - 1))
                {
                    // figure cannot walk another step because the
                    // next Field is not accessible
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
        int nextFieldIndex = actualFieldIndex + 1;

        // figure is on StairBench and can enter the stair
        if (FieldCtrl.IsStairBench(actualFieldIndex, figure))
        {
            nextFieldIndex = FieldCtrl.GetFirstStairStep(figure);
        }
        else
        {
            // figure is on RegularField with the highest index
            if (nextFieldIndex == FieldCtrl.NofRegularFields)
                nextFieldIndex = 0;
            // figure is on GoalField
            else if (nextFieldIndex == FieldCtrl.GoalFieldIndex)
                hasToGoBackwards = true;

            // figure has to go backwards
            if (hasToGoBackwards)
                nextFieldIndex = FieldCtrl.GetFirstStairStep(figure)
                    + FieldCtrl.NofStairFieldsEachPlayer - 1;
        }
        if (isLastStep) hasToGoBackwards = false;
        return FieldCtrl.PlaceFigure(figure, nextFieldIndex);
    }

    private static void AddTurn(Turn turn)
    {
        turns.Add(turn);
        if (turns.Count > MaxNofTurns) turns.Remove(turns[0]);
    }

    public static bool PlaceFigureOnTheNextField(GameFigure figure, bool isLastStep)
    {
        int actualFieldIndex = figure.Field;
        int nextFieldIndex = actualFieldIndex + 1;

        // figure is on StairBench and can enter the stair
        if (FieldCtrl.IsStairBench(actualFieldIndex, figure))
        {
            nextFieldIndex = FieldCtrl.GetFirstStairStep(figure);
        }
        else
        {
            // figure is on RegularField with the highest index
            if (nextFieldIndex == FieldCtrl.NofRegularFields)
                nextFieldIndex = 0;
            // figure is on GoalField
            else if (nextFieldIndex == FieldCtrl.GoalFieldIndex)
                hasToGoBackwards = true;

            // figure has to go backwards
            if (hasToGoBackwards)
                nextFieldIndex = GetFirstStairStep(figure) + NofStairFieldsEachPlayer - 1;

        }
        if (isLastStep) hasToGoBackwards = false;
        return PlaceFigure(figure, nextFieldIndex);
    }

}