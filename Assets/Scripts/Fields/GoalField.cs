using UnityEngine;

/// <summary>
/// Representation of the goalfield
/// </summary>
public class GoalField : GameFieldBase
{
    private readonly int figureCapacity = 16;
    public Vector3[] positions = new Vector3[]
    {
        new Vector3(-1.9f, 0, 0.9f), new Vector3(-1.4f, 0, 0.3f), new Vector3(-1.9f, 0, -0.3f),
        new Vector3(-1.4f, 0, -0.9f), new Vector3(-0.9f, 0, -1.9f), new Vector3(-0.3f, 0, -1.4f),
        new Vector3(0.3f, 0, -1.9f), new Vector3(0.9f, 0, -1.4f), new Vector3(1.9f, 0, -0.9f),
        new Vector3(1.4f, 0, -0.3f), new Vector3(1.9f, 0, 0.3f), new Vector3(1.4f, 0, 0.9f),
        new Vector3(0.9f, 0, 1.9f), new Vector3(0.3f, 0, 1.4f), new Vector3(-0.3f, 0, 1.9f),
        new Vector3(-0.9f, 0, 1.4f)
    };
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

    /// <summary>
    /// Places a figure at this field (only references, not the object itself. this
    /// is implemented in figure and figureCtrl classes.)
    /// </summary>
    /// <param name="figure">Figure that has to be moved</param>
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