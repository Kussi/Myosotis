using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turn {

    private GameFigure figure;
    private int startFieldIndex;
    private int diceValue;

    public GameFigure Figure
    {
        get { return figure; }
    }

    public int Start
    {
        get { return startFieldIndex; }
    }

    public int DiceValue
    {
        get { return diceValue; }
    }

    public Turn(GameFigure figure, int startFieldIndex, int diceValue)
    {
        this.figure = figure;
        this.startFieldIndex = startFieldIndex;
        this.diceValue = diceValue;
    }
}