using UnityEngine;

public class GoalField : GameFieldBase
{
    private readonly int figureCapacity = 16;
    private readonly Vector3[] positions = new Vector3[] { new Vector3(0, 0, -0.2F),
        new Vector3(-0.32F, 0, 0.65F), new Vector3(0.18F, 0, -1.02F), new Vector3(-1.04F, 0, 0.01F),
        new Vector3(0.99F, 0, -0.23F), new Vector3(-0.98F, 0, -1), new Vector3(0.68F, 0, 0.73F),
        new Vector3(-0.44F, 0, -1.82F), new Vector3(-0.52F, 0, 1.79F), new Vector3(1.38F, 0, -1.26F),
        new Vector3(1.69F, 0, 0.84F), new Vector3(-1.47F, 0, 1), new Vector3(0.62F, 0, -1.82F),
        new Vector3(0.85F, 0, 1.65F), new Vector3(1.7F, 0, -0.2F), new Vector3(-1.8F, 0, -0.7F) };

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Awake ()
    {
        fieldPositions = new FieldPosition[figureCapacity];
        InitializeFieldPositions(positions);
    }
}