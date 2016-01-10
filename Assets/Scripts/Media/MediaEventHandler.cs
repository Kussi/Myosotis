using System.Collections;
using UnityEngine;

public static class MediaEventHandler
{
    public enum GameEvent
    {
        FigureReleasedFromHome, FigureHasToGoHome, FigureStepsOnSingleTriggeredField,
        FigureStepsOnMultiTriggeredField, FigureRaisesBarrier, FigureEnteresGoal, FigureEnteresStair,
        
    };

    public enum SoundEvent
    {
        FigureMakesStep, DiceFallsOnGround
    };

    public static void Notify(Figure figure, GameEvent gameEvent, bool isLastStep)
    {
        Player player = FigureCtrl.GetPlayer(figure);
        switch (gameEvent)
        {
            
            case GameEvent.FigureStepsOnSingleTriggeredField:
                if(isLastStep) ImageCtrl.ChangeImageRandomly(player);
                break;
            case GameEvent.FigureStepsOnMultiTriggeredField:
                if(isLastStep) ImageCtrl.ChangeAllImages();
                break;
            case GameEvent.FigureRaisesBarrier:
                if(isLastStep) TextCtrl.ShowRandomText(figure);
                break;
            case GameEvent.FigureEnteresGoal:
                if (isLastStep) SoundCtrl.PlayApplaus();
                break;
            default:
                break;
        }
    }

    public static void Notify(SoundEvent soundEvent)
    {
        switch(soundEvent)
        {
            case SoundEvent.DiceFallsOnGround:
                SoundCtrl.PlayDice();
                break;
            case SoundEvent.FigureMakesStep:
                SoundCtrl.PlayStep();
                break;
            default:
                break;
        }
    }
}