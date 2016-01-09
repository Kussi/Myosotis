using System.Collections;
using UnityEngine;

public static class MediaEventHandler
{
    public enum MediaEvent
    {
        FigureReleasedFromHome, FigureHasToGoHome, FigureStepsOnSingleTriggeredField,
        FigureStepsOnMultiTriggeredField, FigureRaisesBarrier, FigureFinishesGame, FigureEnteresStair
    };

    public static void Notify(Figure figure, MediaEvent mediaEvent, bool isLastStep)
    {
        Player player = FigureCtrl.GetPlayer(figure);

        switch(mediaEvent)
        {
            case MediaEvent.FigureStepsOnSingleTriggeredField:
                if(isLastStep) ImageCtrl.ChangeImageRandomly(player);
                break;
            case MediaEvent.FigureStepsOnMultiTriggeredField:
                if(isLastStep) ImageCtrl.ChangeAllImages();
                break;
            case MediaEvent.FigureRaisesBarrier:
                Debug.LogWarning("Barrier");
                if(isLastStep) TextCtrl.ShowRandomText(figure);
                break;
            default:
                break;
        }
    }
}