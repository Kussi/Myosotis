using System.Collections;

public static class TurnCtrl
{
    private static readonly int MaxNofTurns = 5;

    private static bool hasToGoBackwards = false;
    private static bool turnIsRunning = false;
    private static Figure currentFigure;
    private static int currentStep;
    private static int lastStep;

    public static void Reset()
    {
        hasToGoBackwards = false;
        turnIsRunning = false;
        currentFigure = null;
        currentStep = 0;
        lastStep = 0;
    }

    public static void Execute(Figure figure, int steps)
    {
        int startFieldIndex = figure.Field;
        currentFigure = figure;

        // figure is at home and may be released
        if (FieldCtrl.IsHomeField(startFieldIndex, figure))
        {
            if (!ReleaseFigureFromHome(figure))
                GameCtrl.Notify();
        }
        else // figure is not at home
        {
            turnIsRunning = true;
            currentStep = 0;
            lastStep = steps;
            MakeNextStep();

        }
    }

    public static void MakeNextStep()
    {
        if(turnIsRunning && ++currentStep <= lastStep)
        {
            if(!MakeOneStep(currentFigure, currentStep == lastStep))
                GameCtrl.Notify();
        }
        else
        {
            if(FieldCtrl.IsBarrier(currentFigure.Field))
                MediaEventHandler.Notify(currentFigure, MediaEventHandler.GameEvent.FigureRaisesBarrier, true);
            turnIsRunning = false;
            GameCtrl.Notify();
        }
    }

    private static bool ReleaseFigureFromHome(Figure figure)
    {
        bool couldBePlaced = FieldCtrl.PlaceFigureOnHomeBench(figure);
        if(couldBePlaced)
            MediaEventHandler.Notify(figure, MediaEventHandler.GameEvent.FigureReleasedFromHome, true);
        return couldBePlaced;
    }

    private static bool MakeOneStep(Figure figure, bool isLastStep)
    {
        int actualFieldIndex = figure.Field;
        int nextFieldIndex;

        // figure is on its StairBench and can enter the stair
        if (FieldCtrl.IsStairBench(actualFieldIndex, figure))
        {
            MediaEventHandler.Notify(figure, MediaEventHandler.GameEvent.FigureEnteresStair, isLastStep);
            EnterStair(figure);
        }
        // figure is on a RegularField and moves to another one
        else if(FieldCtrl.IsRegularField(actualFieldIndex))
        {
            nextFieldIndex = FieldCtrl.GetNextRegularFieldIndex(actualFieldIndex);
            if (FieldCtrl.IsBarrier(nextFieldIndex)) return false;
            if(FieldCtrl.IsSingleEventTrigger(nextFieldIndex))
                MediaEventHandler.Notify(figure, MediaEventHandler.GameEvent.FigureStepsOnSingleTriggeredField, isLastStep);
            else if(FieldCtrl.IsMultiEventTrigger(nextFieldIndex))
                MediaEventHandler.Notify(figure, MediaEventHandler.GameEvent.FigureStepsOnMultiTriggeredField, isLastStep);
            Figure figureToSendHome = MakeOneRegularStep(figure, isLastStep);
            if (figureToSendHome != null)
            {
                MediaEventHandler.Notify(figure, MediaEventHandler.GameEvent.FigureHasToGoHome, isLastStep);
                FieldCtrl.MoveFigureHome(figureToSendHome);
            }
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
            if (!isLastStep) hasToGoBackwards = true;
            MediaEventHandler.Notify(figure, MediaEventHandler.GameEvent.FigureEnteresGoal, isLastStep);
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

    private static Figure MakeOneRegularStep(Figure figure, bool isLastStep)
    {
        return FieldCtrl.PlaceFigureOnNextRegularField(figure, isLastStep);
    }

    private static void EnterStair(Figure figure)
    {
        FieldCtrl.PlaceFigureOnFirstStairStep(figure);
    }

    private static void MakeOneStairStepForward(Figure figure)
    {
        FieldCtrl.PlaceFigureOnNextRegularStairStep(figure, false);
    }

    private static void MakeOneStairStepBackward(Figure figure)
    {
        FieldCtrl.PlaceFigureOnNextRegularStairStep(figure, true);
    }

    private static void EnterGoal(Figure figure)
    {
        FieldCtrl.PlaceFigureInGoal(figure);
    }

    private static void LeaveGoal(Figure figure)
    {
        FieldCtrl.PlaceFigureOnLastStairStep(figure);
    }
}