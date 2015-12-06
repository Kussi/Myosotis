using UnityEngine;
using System.Collections;

public abstract class GameFieldBase : MonoBehaviour
{
    private int index;
    private FieldPosition[] fieldPositions;

    public int Index
    {
        get { return index; }
        private set { index = value; }
    }

    public FieldPosition[] FieldPositions
    {
        get { return fieldPositions; }
    }

    protected virtual bool IsOccupied
    {
        get {
            foreach (FieldPosition fieldPosition in FieldPositions)
            {
                if(!fieldPosition.IsOccupied) return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Awake()
    {
        Index = getIndexFromName();
    }

    /// <summary>
    /// Getting all Gamefigures, that are currently staying on this field
    /// </summary>
    /// <returns>returns the Gamefigures of this field</returns>
    public GameFigure[] GetGameFigures()
    {
        ArrayList gameFigures = new ArrayList();
        foreach(FieldPosition fieldPosition in fieldPositions)
        {
            if (fieldPosition.IsOccupied) gameFigures.Add(fieldPosition.GameFigure);
        }
        return (GameFigure[])gameFigures.ToArray(typeof(GameFigure));
    }

    /// <summary>
    /// Sets the Fieldpositions of this field
    /// </summary>
    /// <param name="fieldPositions">possible positions a GameFigure can stand on this field</param>
    protected void SetFieldPositions(FieldPosition[] fieldPositions)
    {
        if (FieldPositions == null) this.fieldPositions = fieldPositions;
    }

    /// <summary>
    /// Removes the GameFigure from its actual field an places it here
    /// </summary>
    /// <param name="figure">Figure that has to be moved</param>
    public virtual void PlaceGameFigure(GameFigure figure)
    {
        if (IsOccupied) throw new InvalidGameStateException("Figure cannot be placed on this Field (" + gameObject.name + ")");

        foreach (FieldPosition fieldPosition in FieldPositions)
        {
            if (!fieldPosition.IsOccupied)
            {
                fieldPosition.GameFigure = figure;
                RefreshPositionsAfterPlacement();
                break;
            }
        }
    }

    /// <summary>
    /// Removes a GameFigure from this Field and changes the Field attribute of the figure to null
    /// </summary>
    /// <param name="figure">Figure that has to be removed</param>
    public virtual void RemoveGameFigure(GameFigure figure)
    {
        bool figureFound = false;

        for (int i = 0; i < FieldPositions.Length; ++i)
        {
            if (FieldPositions[i].IsOccupied && FieldPositions[i].GameFigure.Equals(figure))
            {
                FieldPositions[i].GameFigure = null;
                figureFound = true;
                break;
            }
            
        }
        if (!figureFound) throw new InvalidGameStateException("Figure cannot be removed from this Field (" + gameObject.name + ")");

        RefreshPositionsAfterRemoval();
    }

    /// <summary>
    /// moves the figure objects to their position
    /// </summary>
    protected void RefreshPositionsAfterPlacement()
    {
        foreach(FieldPosition fieldPosition in fieldPositions)
        {
            if (fieldPosition.IsOccupied && fieldPosition.GameFigure.transform.position != fieldPosition.transform.position)
            {
                fieldPosition.GameFigure.Field = this;
                fieldPosition.GameFigure.transform.position = fieldPosition.transform.position;
            } 
        }
    }

    /// <summary>
    /// adjusts the positions of the figures
    /// </summary>
    protected abstract void RefreshPositionsAfterRemoval();

    /// <summary>
    /// gets the indexnumber from the name of the gameobject
    /// </summary>
    /// <returns>the index of the gameObject</returns>
    protected int getIndexFromName()
    {
        int indexLength = 3; // Gamefieldnames have always the form <Prefix><3-digit-index>
        return int.Parse(gameObject.name.Substring(gameObject.name.Length - indexLength));
    }
}