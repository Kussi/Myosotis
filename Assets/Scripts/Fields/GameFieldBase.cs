using UnityEngine;
using System.Collections;

public abstract class GameFieldBase : MonoBehaviour
{

    private int index;
    protected GameFigure[] gameFigures;

    public int Index
    {
        get { return index; }
        protected set { index = value; }
    }

    public GameFigure[] GameFigures
    {
        get { return gameFigures; }
    }

    /// <summary>
    /// Places a GameFigure on this Field and changes the Field attribute of the figure to this
    /// </summary>
    /// <param name="figure">Figure that has to be placed</param>
    public void PlaceGameFigure(GameFigure figure)
    {
        bool isFullyOccupied = true;
        for(int i = 0; i < gameFigures.Length; ++i)
        {
            if(gameFigures[i] == null)
            {
                gameFigures[i] = figure;
                figure.Field = this;
                isFullyOccupied = false;
                break;
            }
        }
        if(isFullyOccupied) throw new InvalidGameStateException("Figure cannot be placed on this Field (" + gameObject.name + ")");
    }

    /// <summary>
    /// Removes a GameFigure from this Field and changes the Field attribute of the figure to null
    /// </summary>
    /// <param name="figure">Figure that has to be removed</param>
    public void RemoveGameFigure(GameFigure figure)
    {
        bool figureFound = false;
        for(int i = 0; i < gameFigures.Length; ++i)
        {
            if(gameFigures[i] != null && gameFigures[i].Equals(figure))
            {
                gameFigures[i] = null;
                figure.Field = null;
                figureFound = true;
                break;
            }
        }
        if(!figureFound) throw new InvalidGameStateException("Figure cannot be removed from this Field (" + gameObject.name + ")");
    }
}
