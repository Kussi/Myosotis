﻿using UnityEngine;

public class GameField : AbstractGameField
{

    private const int figureCapacity = 2;

    public bool IsBarrier
    {
        get { return gameFigures[0] != null && gameFigures[1] != null; }
    }

    // Use this for initialization
    void Start()
    {
        gameFigures = new GameFigure[figureCapacity];
        index = System.Int32.Parse(gameObject.name.Substring("Field".Length));
    }

    // Update is called once per frame
    void Update()
    {

    }
}