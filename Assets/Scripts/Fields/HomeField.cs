using UnityEngine;

public class HomeField : GameFieldBase
{
    private const int figureCapacity = 1;
    private Player parent = null;

    public bool IsOccupied
    {
        get { return gameFigures[0] != null; }
    }
    public Player Parent
    {
        get { return parent; }
    }

    // Use this for initialization
    void OnEnable()
    {
        gameFigures = new GameFigure[figureCapacity];
        Index = System.Int32.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
    }

    // Update is called once per frame
    void Update()
    {

    }
}