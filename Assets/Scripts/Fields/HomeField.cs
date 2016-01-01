using System;
using UnityEngine;

public class HomeField : GameFieldBase
{
    private const int figureCapacity = 4;
    private readonly Vector3[] positions = new Vector3[] { new Vector3(1, 0, 1),
        new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, -1) };

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void OnEnable ()
    {
        SetFieldPositions(new FieldPosition[figureCapacity]);

        for (int i = 0; i < fieldPositions.Length; ++i)
        {
            GameObject positionObject = new GameObject();
            positionObject.name = "position" + i;
            positionObject.transform.parent = gameObject.transform;

            positionObject.transform.localPosition = positions[i];

            positionObject.AddComponent<FieldPosition>();
            fieldPositions[i] = positionObject.GetComponent<FieldPosition>();
        }
    }

    public void InitiallyPlaceFigure(GameFigure figure)
    {
        if (GameCtrl.GameIsRunning) throw new InvalidGameStateException();
        if (figure.Field != 0) throw new InvalidGameStateException();
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

    /// <summary>
    /// adjusts the positions of the figures
    /// </summary>
    protected override void RefreshPositionsAfterRemoval()
    {
        // Nothing to adjust
    }
}