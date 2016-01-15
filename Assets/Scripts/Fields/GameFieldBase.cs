using UnityEngine;
using System.Collections;

/// <summary>
/// parental class of all game fields
/// </summary>
public abstract class GameFieldBase : MonoBehaviour
{
    protected static readonly string PositionPrefix = "Position";

    private int index;
    public bool isSingleEventTrigger = false;
    public bool isMultiEventTrigger = false;
    public FieldPosition[] fieldPositions;

    public int Index
    {
        get { return index; }
        set
        {
            if (!GameCtrl.GameIsRunning && index == 0 && GetIndexFromName() != 0)
                index = value;
        }
    }

    protected virtual bool IsOccupied
    {
        get
        {
            foreach (FieldPosition fieldPosition in fieldPositions)
                if (!fieldPosition.IsOccupied) return false;
            return true;
        }
    }

    public bool IsSingleEventTrigger
    {
        get { return isSingleEventTrigger; }
        set { isSingleEventTrigger = value; }
    }

    public bool IsMultiEventTrigger
    {
        get { return isMultiEventTrigger; }
        set { isMultiEventTrigger = value; }
    }

    /// <summary>
    /// Getting all Gamefigures, that are currently staying on this field
    /// </summary>
    /// <returns>returns the Gamefigures of this field</returns>
    protected ArrayList GetFiguresOnField()
    {
        ArrayList figures = new ArrayList();
        foreach (FieldPosition fieldPosition in fieldPositions)
            if (fieldPosition.IsOccupied) figures.Add(fieldPosition.Figure);
        return figures;
    }

    /// <summary>
    /// Removes the Figure from its actual field an places it here
    /// </summary>
    /// <param name="figure">Figure that has to be moved</param>
    public virtual void PlaceFigure(Figure figure)
    {
        if (IsOccupied) throw new InvalidGameStateException();
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

    /// <summary>
    /// Removes a Figure from this Field and changes the Field attribute of the figure to null
    /// </summary>
    /// <param name="figure">Figure that has to be removed</param>
    public virtual void RemoveFigure(Figure figure)
    {
        bool figureFound = false;
        FieldPosition position;
        for (int i = 0; i < fieldPositions.Length; ++i)
        {
            position = fieldPositions[i];
            if (position.IsOccupied && position.Figure.Equals(figure))
            {
                RemoveReferences(position, figure);
                figureFound = true;
                break;
            }

        }
        if (!figureFound) throw new InvalidGameStateException();
    }

    protected void RemoveReferences(FieldPosition position, Figure figure)
    {
        position.Figure = null;
        figure.Field = FigureCtrl.EmptyFieldEntry;
    }

    protected void AdjustReferences(FieldPosition position, Figure figure)
    {
        if (position.Figure != null || figure.Field != FigureCtrl.EmptyFieldEntry)
            throw new InvalidGameStateException();
        position.Figure = figure;
        figure.Field = index;
    }

    ///// <summary>
    ///// gets the indexnumber from the name of the gameobject
    ///// </summary>
    ///// <returns>the index of the gameObject</returns>
    protected int GetIndexFromName()
    {
        int indexLength = 3; // Gamefieldnames have always the form <Prefix><3-digit-index>
        return int.Parse(gameObject.name.Substring(gameObject.name.Length - indexLength));
    }

    protected void InitializeFieldPositions(Vector3[] positions)
    {
        for (int i = 0; i < fieldPositions.Length; ++i)
        {
            GameObject positionObject = new GameObject();
            positionObject.name = PositionPrefix + i;
            positionObject.transform.parent = gameObject.transform;
            positionObject.transform.localPosition = positions[i];
            positionObject.AddComponent<FieldPosition>();
            fieldPositions[i] = positionObject.GetComponent<FieldPosition>();
        }
    }

    protected virtual void MoveFigureObject(FieldPosition position, Figure figure)
    {
        figure.StartWalking(position.transform);
    }
}