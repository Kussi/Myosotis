using System;
using System.Collections;
using UnityEngine;

public class StairField : GameFieldBase
{
    private const int figureCapacity = 4;
    private readonly Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0.3F),
        new Vector3(0, 0, -0.3F), new Vector3(0, 0, 0.9F), new Vector3(0, 0, -0.9F) };

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void OnEnable () {
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

    /// <summary>
    /// adjusts the positions of the figures
    /// </summary>
    protected override void RefreshPositionsAfterRemoval()
    {
        ArrayList gameFigures = new ArrayList();
        for(int i = 0; i < fieldPositions.Length; ++i)
        {
            GameFigure gameFigure = fieldPositions[i].GameFigure;
            if (gameFigure != null) gameFigures.Add(gameFigure);
            fieldPositions[i].GameFigure = null;
        }

        for(int i = 0; i < gameFigures.Count; ++i)
        {
            fieldPositions[i].GameFigure = (GameFigure)gameFigures[i];
        }
    }
}