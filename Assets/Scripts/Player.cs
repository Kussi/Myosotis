using UnityEngine;

public class Player {

    private const int nofGameFigures = 4; 

    private string color;
    private Dice dice;
    private GameFigure[] gameFigures;
    private PlayerStateBase state;
    private HomeField[] homeFields;
    private int homeBench;
    private int stairBench;
    private int firstStairStep;

    public string Color
    {
        get { return color; }
    }
    public Dice Dice
    {
        get { return dice; }
    }
    public GameFigure[] GameFigures
    {
        get { return gameFigures; }
    }
    public PlayerStateBase State
    {
        get { return state; }
        set { state = value; }
    }
    public HomeField[] HomeFields
    {
        get { return homeFields; }
    }
    public int HomeBench
    {
        get { return homeBench; }
    }
    public int StairBench
    {
        get { return stairBench; }
    }
    public int FirstStairStep
    {
        get { return firstStairStep; }
    }

    public Player(string color, int homeBank, int stairBank, int firstStairStep)
    {
        this.color = char.ToUpper(color[0]) + color.Substring(1);
        this.homeBench = homeBank;
        this.stairBench = stairBank;
        this.firstStairStep = firstStairStep;
        this.state = new PlayerStateAllAtHome();

        // instantiate all homeFields
        homeFields = new HomeField[nofGameFigures];
        for (int i = 0; i < homeFields.Length; ++i)
        {
            GameObject homeField = GameObject.Find(this.color + "HomeField" + i);
            homeFields[i] = (HomeField)homeField.gameObject.GetComponent<MonoBehaviour>();
        }


        // instantiate all gameFigures 
        gameFigures = new GameFigure[nofGameFigures];
        for (int i = 0; i < gameFigures.Length; ++i)
        {
            GameObject figure = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/GameFigure", typeof(GameObject)));
            gameFigures[i] = (GameFigure)figure.GetComponent<MonoBehaviour>();

            gameFigures[i].Index = i;
            gameFigures[i].Parent = this;
            gameFigures[i].Field = homeFields[i];
            gameFigures[i].Field.PlaceGameFigure(gameFigures[i]);

            Material material = (Material)Resources.Load("Materials/" + color + "Player", typeof(Material));
            gameFigures[i].gameObject.GetComponent<Renderer>().material = material;
            gameFigures[i].gameObject.name = (this.color + "GameFigure" + i);
            gameFigures[i].gameObject.transform.position = homeFields[i].transform.position;
        }

        // instantiate the dice
        GameObject dice = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Dice", typeof(GameObject)));
        dice.name = (this.color + "Dice");
        Vector3 dicePosition = GameObject.Find(this.color + "DicePosition").transform.position;
        dice.transform.position = dicePosition;

        this.dice = (Dice)dice.GetComponent<MonoBehaviour>();
        this.dice.Parent = this;
	}

    public void RefreshState()
    {
        int figuresOnGameField = GameLogic.GetFiguresOnRegularField(gameFigures).Length;
        int figuresOnGoalField = GameLogic.GetFiguresOnGoalField(gameFigures).Length;
        int figuresOnHomeField = GameLogic.GetFiguresOnHomeField(gameFigures).Length;

        if (figuresOnGoalField == 4) State = new PlayerStateStateAllInGoal();
        else if (figuresOnGameField == 0) State = new PlayerStateAllAtHome();
        else State = new PlayerStateAtLeastOneOut();
    }
}