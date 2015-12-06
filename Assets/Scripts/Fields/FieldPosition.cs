﻿using UnityEngine;

public class FieldPosition : MonoBehaviour {

    private GameFigure gameFigure;

    public bool IsOccupied
    {
        get { return gameFigure != null; }
    }

    public GameFigure GameFigure
    {
        get { return gameFigure; }
        set { gameFigure = value; }
    }
}