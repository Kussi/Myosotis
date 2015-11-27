using UnityEngine;
using System.Collections;

public abstract class AbstractGameField : MonoBehaviour
{

    protected int index;
    protected GameFigure[] gameFigures;

    public int Index
    {
        get { return index; }
    }
    public GameFigure[] GameFigures
    {
        get { return gameFigures; }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
        if(isFullyOccupied) throw new UnityException();
    }

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
        if(!figureFound) throw new UnityException();
    }
}
