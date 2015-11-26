using UnityEngine;

public class Player {

    private const int nofGameFigures = 4; 

    private string color;
    private Dice dice;
    private GameFigure[] gameFigures;
    private AbstractPlayerState state;
    private int homeBank;
    private int stairBank;
    private int firstStairStep;

    public string Color
    {
        get { return color; }
    }
    public GameFigure[] GameFigures
    {
        get { return gameFigures; }
    }
    public AbstractPlayerState State
    {
        get { return state; }
        set { state = value; }
    }
    public int HomeBank
    {
        get { return homeBank; }
    }
    public int StairBank
    {
        get { return stairBank; }
    }
    public int FirstStairStep
    {
        get { return firstStairStep; }
    }

    public Player(string color, int homeBank, int stairBank, int firstStairStep)
    {
        this.color = char.ToUpper(color[0]) + color.Substring(1);
        this.homeBank = homeBank;
        this.stairBank = stairBank;
        this.firstStairStep = firstStairStep;
        this.state = new PlayerStateAllAtHome();

        // instantiate all gameFigures 
        gameFigures = new GameFigure[4];
        for (int i = 0; i < nofGameFigures; ++i)
        {
            GameObject figure = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Capsule", typeof(GameObject)));
            figure.name = (this.color + "GameFigure" + (i+1));

            Vector3 spawnPosition = GameObject.Find(this.color + "HomeField" + (i+1)).transform.position;
            figure.transform.position = spawnPosition;

            Material material = (Material)Resources.Load("Materials/" + color + "Player", typeof(Material));
            figure.GetComponent<Renderer>().material = material;

            gameFigures[i] = (GameFigure)figure.GetComponent<MonoBehaviour>();
            gameFigures[i].Index = (i + 1);
            gameFigures[i].Parent = this;
        }

        GameObject dice = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Dice", typeof(GameObject)));
        dice.name = (this.color + "Dice");
        Vector3 dicePosition = GameObject.Find(this.color + "DicePosition").transform.position;
        dice.transform.position = dicePosition;
        dice.tag = this.color;

        this.dice = (Dice)dice.GetComponent<MonoBehaviour>();
        this.dice.Parent = this;
	}
}