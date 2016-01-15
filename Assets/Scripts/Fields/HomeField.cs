using UnityEngine;

/// <summary>
/// Representation of a homefield
/// </summary>
public class HomeField : GameFieldBase
{
    private readonly int figureCapacity = 4;
    private readonly Vector3[] positions = new Vector3[] { new Vector3(0.9f, 0, 0.9f),
        new Vector3(-0.9f, 0, 0.9f), new Vector3(0.9f, 0, -0.9f), new Vector3(-0.9f, 0, -0.9f) };

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Awake()
    {
        fieldPositions = new FieldPosition[figureCapacity];
        InitializeFieldPositions(positions);
    }

    /// <summary>
    /// Starts the figure to walk to position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="figure"></param>
    protected override void MoveFigureObject(FieldPosition position, Figure figure)
    {
        Debug.Log("Move Figure Home");
        figure.StartWalking(position.transform);
    }

    /// <summary>
    /// places a figure at the very beginning of the game (without moving the smoothly)
    /// </summary>
    /// <param name="figure"></param>
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