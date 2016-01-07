using System.Collections;
using UnityEngine;

public static class MediaEventHandler
{
    private static ArrayList events = new ArrayList();

    public enum MediaEvent
    {
        FigureReleasedFromHome, FigureHasToGoHome, FigureStepsOnSingleTriggeredField,
        FigureStepsOnMultiTriggeredField, FigureRaisesBarrier, FigureFinishesGame, FigureEnteresStair
    };

    public static void AddEvent(MediaEvent mediaEvent)
    {
        events.Add(mediaEvent);
    }

    public static void Notify(Player player)
    {
        foreach (MediaEvent me in events)
            if (me == MediaEvent.FigureStepsOnSingleTriggeredField)
            {
                ImageCtrl.ChangeImageRandomly(player);
            }
            else if (me == MediaEvent.FigureStepsOnMultiTriggeredField)
                ImageCtrl.ChangeAllImages();
        events.Clear();
    }
}
