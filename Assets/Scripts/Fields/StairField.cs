using UnityEngine;

/// <summary>
/// Representation of a stair field
/// </summary>
public class StairField : GameFieldBase
{
    private readonly int figureCapacity = 4;
    private readonly Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0.3F),
        new Vector3(0, 0, -0.3F), new Vector3(0, 0, 0.9F), new Vector3(0, 0, -0.9F) };

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Awake () {
        fieldPositions = new FieldPosition[figureCapacity];
        InitializeFieldPositions(positions);
    }
}