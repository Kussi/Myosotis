using UnityEngine;

public class GameField : MonoBehaviour {

    private GameFigure[] gameFigures;
    private int index;

    public int Index
    {
        get { return index; }
    }
    public bool IsBarrier
    {
        get { return gameFigures[0] != null && gameFigures[1] != null; }
    }

    // Use this for initialization
    void Start()
    {
        gameFigures = new GameFigure[2];
        index = System.Int32.Parse(gameObject.name.Substring("Field".Length));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceGameFigure(GameFigure figure)
    {
        if (gameFigures[0] == null) gameFigures[0] = figure;
        else if (gameFigures[1] == null) gameFigures[1] = figure;
        else throw new UnityException();
    }

    public void RemoveGameFigure(GameFigure figure)
    {
        if (gameFigures[0].Equals(figure)) { gameFigures[0] = null; }
        else if (gameFigures[1].Equals(figure)) { gameFigures[1] = null; }
        else throw new UnityException();
    }

    public GameFigure[] GetGameFigures()
    {
        return gameFigures;
    }
}