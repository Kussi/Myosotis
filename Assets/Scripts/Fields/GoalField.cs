using UnityEngine;

public class GoalField : GameFieldBase
{

    private const int figureCapacity = 16;

    // Use this for initialization
    void Start()
    {
        gameFigures = new GameFigure[figureCapacity];
        Index = 1000;
    }

    // Update is called once per frame
    void Update()
    {

    }
}