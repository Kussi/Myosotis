using UnityEngine;

public class HomeField : MonoBehaviour
{

    private GameFigure gameFigure;
    private int index;
    private Player parent;

    public int Index
    {
        get { return index; }
    }
    public bool IsOccupied
    {
        get { return gameFigure != null; }
    }

    // Use this for initialization
    void Start()
    {
        index = System.Int32.Parse(gameObject.name.Substring(gameObject.name.Length-1));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceGameFigure(GameFigure figure)
    {
        if (gameFigure == null) gameFigure = figure;
        else throw new UnityException();
    }

    public void RemoveGameFigure(GameFigure figure)
    {
        if (gameFigure.Equals(figure)) { gameFigure = null; }
        else throw new UnityException();
    }

    public GameFigure GetGameFigure()
    {
        return gameFigure;
    }
}