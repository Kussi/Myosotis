using System;
using UnityEngine;

public class HomeField : GameFieldBase
{
    private readonly int figureCapacity = 4;
    private readonly Vector3[] positions = new Vector3[] { new Vector3(0.9f, 0, 0.9f),
        new Vector3(-0.9f, 0, 0.9f), new Vector3(0.9f, 0, -0.9f), new Vector3(-0.9f, 0, -0.9f) };

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Awake ()
    {
        fieldPositions = new FieldPosition[figureCapacity];
        InitializeFieldPositions(positions);
    }

    protected override void MoveFigureObject(FieldPosition position, Figure figure)
    {
        Debug.Log("Move Figure Home");
        figure.transform.position = position.transform.position;
    }

    public void InitiallyPlaceFigure(Figure figure)
    {
        if (GameCtrl.GameIsRunning) throw new InvalidGameStateException();
        if (IsOccupied) throw new InvalidGameStateException();

        foreach (FieldPosition fieldPosition in fieldPositions)
        {
            if (!fieldPosition.IsOccupied)
            {
                AdjustReferences(fieldPosition, figure);
                figure.gameObject.transform.position = fieldPosition.gameObject.transform.position;
                break;
            }
        }
    }
}