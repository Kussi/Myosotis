using System.Collections.Generic;
using UnityEngine;

public class GoalField : GameFieldBase
{
    private readonly int figureCapacity = 16;
    public Vector3[] positions;
    private FieldPosition[] redPositions = new FieldPosition[4];
    private FieldPosition[] yellowPositions = new FieldPosition[4];
    private FieldPosition[] bluePositions = new FieldPosition[4];
    private FieldPosition[] greenPositions = new FieldPosition[4];

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Awake()
    {
        fieldPositions = new FieldPosition[figureCapacity];
        InitializeFieldPositions(positions);

        for (int i = 0; i < 4; ++i)
        {
            redPositions[i] = fieldPositions[i];
            yellowPositions[i] = fieldPositions[i + 4];
            bluePositions[i] = fieldPositions[i + 8];
            greenPositions[i] = fieldPositions[i + 12];
        }
    }

    public override void PlaceFigure(Figure figure)
    {
        FieldPosition[] fieldPositions;
        if (FigureCtrl.GetPlayer(figure).Color.ToLower().Equals("red"))
            fieldPositions = redPositions;
        else if (FigureCtrl.GetPlayer(figure).Color.ToLower().Equals("yellow"))
            fieldPositions = yellowPositions;
        else if (FigureCtrl.GetPlayer(figure).Color.ToLower().Equals("blue"))
            fieldPositions = bluePositions;
        else if (FigureCtrl.GetPlayer(figure).Color.ToLower().Equals("green"))
            fieldPositions = greenPositions;
        else throw new InvalidGameStateException();

        foreach (FieldPosition fieldPosition in fieldPositions)
        {
            if (!fieldPosition.IsOccupied)
            {
                AdjustReferences(fieldPosition, figure);
                MoveFigureObject(fieldPosition, figure);
                break;
            }
        }
    }
}