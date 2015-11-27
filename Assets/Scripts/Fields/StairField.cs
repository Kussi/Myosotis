using UnityEngine;

public class StairField : AbstractGameField
{
    private const int figureCapacity = 4;

    // Use this for initialization
    void Start () {
        gameFigures = new GameFigure[figureCapacity];
        index = System.Int32.Parse(gameObject.name.Substring("SF".Length));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
