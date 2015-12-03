using UnityEngine;

public class StairField : GameFieldBase
{
    private const int figureCapacity = 4;

    // Use this for initialization
    void Start () {
        gameFigures = new GameFigure[figureCapacity];
        Index = System.Int32.Parse(gameObject.name.Substring("StairField".Length));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
