using UnityEngine;

/// <summary>
/// Represents a position on a gamefield
/// </summary>
public class FieldPosition : MonoBehaviour
{
    private Figure figure;

    public bool IsOccupied
    {
        get { return figure != null; }
    }

    public Figure Figure
    {
        get { return figure; }
        set { figure = value; }
    }
}