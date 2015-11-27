using UnityEngine;

public class Player {

    private const int nofGameFigures = 4; 

    private string color;
    private Dice dice;
    private GameFigure[] gameFigures;
    private AbstractPlayerState state;
    private HomeField[] homeFields;
    private int homeBank;
    private int stairBank;
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
    public AbstractPlayerState State
    {
        get { return state; }
        set { state = value; }
    }
    public HomeField[] HomeFields
    {
        get { return homeFields; }
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

        // instantiate all homeFields
        homeFields = new HomeField[nofGameFigures];
        for (int i = 0; i < homeFields.Length; ++i)
        {
            GameObject homeField = GameObject.Find(this.color + "HomeField" + i);
            homeFields[i] = (HomeField)homeField.gameObject.GetComponent<MonoBehaviour>();
        }


        // instantiate all gameFigures 
        gameFigures = new GameFigure[nofGameFigures];
        Debug.Log("gameFigures: " + gameFigures.Length);
        for (int i = 0; i < gameFigures.Length; ++i)
        {
            GameObject figure = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Capsule", typeof(GameObject)));
            gameFigures[i] = (GameFigure)figure.GetComponent<MonoBehaviour>();

            Debug.Log(this.color + "hier ist 1");

            gameFigures[i].Index = i;
            gameFigures[i].Parent = this;
            gameFigures[i].Field = homeFields[i];
            Debug.Log(this.color + "hier ist 2");
            gameFigures[i].Field.PlaceGameFigure(gameFigures[i]);

            Material material = (Material)Resources.Load("Materials/" + color + "Player", typeof(Material));
            gameFigures[i].gameObject.GetComponent<Renderer>().material = material;
            gameFigures[i].gameObject.name = (this.color + "GameFigure" + i);
            gameFigures[i].gameObject.transform.position = homeFields[i].transform.position;
            Debug.Log(this.color + "hier ist 3");
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
        int figuresOnGameField = GameLogic.GetFiguresOnGameField(gameFigures).Length;
        int figuresOnGoalField = GameLogic.GetFiguresOnGoalField(gameFigures).Length;
        int figuresOnHomeField = GameLogic.GetFiguresOnHomeField(gameFigures).Length;

        if (figuresOnGoalField == 4) State = new PlayerStateStateAllInGoal();
        else if (figuresOnGameField == 0) State = new PlayerStateAllAtHome();
        else if (figuresOnHomeField == 0) State = new PlayerStateAllOut();
        else State = new PlayerStateAtLeastOneOut();
    }
}