using UnityEngine;

public class HomeField : AbstractGameField
{
    private const int figureCapacity = 1;
    private Player parent;

    public bool IsOccupied
    {
        get { return gameFigures[0] != null; }
    }

    // Use this for initialization
    void Start()
    {
        gameFigures = new GameFigure[figureCapacity];
        index = System.Int32.Parse(gameObject.name.Substring(gameObject.name.Length-1));
    }

    // Update is called once per frame
    void Update()
    {

    }
}