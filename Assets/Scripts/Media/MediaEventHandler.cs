using System.Collections;
using UnityEngine;

public static class MediaEventHandler
{
    private static ArrayList events = new ArrayList();

    public enum MediaEvent
    {
        FigureReleasedFromHome, FigureHasToGoHome, FigureStepsOnTriggeredField,
        FigureRaisesBarrier, FigureFinishesGame, FigureEnteresStair
    };

    public static void AddEvent(MediaEvent mediaEvent)
    {
        events.Add(mediaEvent);
    }

    public static void Notify()
    {
        foreach (MediaEvent me in events)
            Debug.LogError(me);
        events.Clear();
    }
}
