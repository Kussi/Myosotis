using UnityEngine;

public class RegularField : GameFieldBase
{

    private const int figureCapacity = 2;
    private const string parentBenchObject = "BenchFields";

    private bool isBench = false;

    public bool IsBench
    {
        get { return isBench; }
    }
    public bool IsBarrier
    {
        get { return gameFigures[0] != null && gameFigures[1] != null; }
    }

    // Use this for initialization
    void Start()
    {
        gameFigures = new GameFigure[figureCapacity];
        Index = System.Int32.Parse(gameObject.name.Substring("Field".Length));
        if(gameObject.transform.parent.name.Equals(parentBenchObject))
        {
            isBench = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}