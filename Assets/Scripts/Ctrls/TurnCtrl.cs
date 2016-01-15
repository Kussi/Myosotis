public static class TurnCtrl
{
    private static bool hasToGoBackwards = false;
    private static bool turnIsRunning = false;
    private static Figure currentFigure;
    private static int currentStep;
    private static int lastStep;

    /// <summary>
    /// Resets the original setup
    /// </summary>
    public static void Reset()
    {
        hasToGoBackwards = false;
        turnIsRunning = false;
        currentFigure = null;
        currentStep = 0;
        lastStep = 0;
    }

    /// <summary>
    /// starts executing the actual turn
    /// </summary>
    /// <param name="figure"></param>
    /// <param name="steps"></param>
    public static void Execute(Figure figure, int steps)
    {
        int startFieldIndex = figure.Field;
        currentFigure = figure;
        figure.isMainFigure = true;

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

    /// <summary>
    /// performs one step of the actual figure (if it has to go one more)
    /// </summary>
    public static void MakeNextStep()
    {
        if(turnIsRunning && ++currentStep <= lastStep)
        {
            if(!MakeOneStep(currentFigure, currentStep == lastStep))
                GameCtrl.Notify();
        }
        else
        {
            if(FieldCtrl.IsRegularField(currentFigure.Field) && FieldCtrl.IsBarrier(currentFigure.Field))
                MediaEventHandler.Notify(currentFigure, MediaEventHandler.GameEvent.FigureRaisesBarrier, true);
            turnIsRunning = false;
            currentFigure.isMainFigure = false;
            currentFigure = null;   
            GameCtrl.Notify();
        }
    }

    /// <summary>
    /// releases a figure from it homefield to its homebench
    /// </summary>
    /// <param name="figure"></param>
    /// <returns></returns>
    private static bool ReleaseFigureFromHome(Figure figure)
    {
        bool couldBePlaced = FieldCtrl.PlaceFigureOnHomeBench(figure);
        if(couldBePlaced)
            MediaEventHandler.Notify(figure, MediaEventHandler.GameEvent.FigureReleasedFromHome, true);
        return couldBePlaced;
    }

    /// <summary>
    /// performs one step and checks the different possibilities
    /// </summary>
    /// <param name="figure"></param>
    /// <param name="isLastStep"></param>
    /// <returns></returns>
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

    /// <summary>
    /// returns a figure that has to be sent home. Null if there is no figure
    /// </summary>
    /// <param name="figure"></param>
    /// <param name="isLastStep"></param>
    /// <returns></returns>
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