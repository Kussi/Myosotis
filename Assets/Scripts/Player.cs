using UnityEngine;

public class Player {

    private const int NofGameFigures = 4;
    private const string ParentFigureObjectSuffix = "Corner";
    private const string ParentDiceObjectSuffix = "DicePosition";

    private Dice dice;
    private GameFigure[] gameFigures;
    private PlayerStateBase state;

    private readonly string color;

    private readonly int homeFieldIndex;
    private readonly int homeBenchIndex;
    private readonly int stairBenchIndex;
    private readonly int firstStairStepIndex;
    private readonly int lightAngle;

    public string Color
    {
        get { return color; }
    }

    public Dice Dice
    {
        get { return dice; }
        private set
        {
            if (dice == null) dice = value;
        }
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

    public int HomeFieldIndex
    {
        get { return homeFieldIndex; }
    }

    public int HomeBenchIndex
    {
        get { return homeBenchIndex; }
    }

    public int StairBenchIndex
    {
        get { return stairBenchIndex; }
    }

    public int FirstStairStepIndex
    {
        get { return firstStairStepIndex; }
    }

    public int LightAngle
    {
        get { return lightAngle; }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="color">color of the player</param>
    /// <param name="homeField">index of the players homefield</param>
    /// <param name="homeBench">index of the players homebench</param>
    /// <param name="stairBench">index of the the players stairbench</param>
    /// <param name="firstStairStep">index of the players first stair step</param>
    public Player(string color, int homeField, int homeBench, int stairBench, int firstStairStep, int lightAngle)
    {
        this.color = char.ToUpper(color[0]) + color.Substring(1);

        homeFieldIndex = homeField;
        homeBenchIndex = homeBench;
        stairBenchIndex = stairBench;
        firstStairStepIndex = firstStairStep;

        this.lightAngle = lightAngle;

        // initiate the first Playerstate
        this.state = new PlayerStateAllAtHome();

        // instantiate all gameFigures 
        gameFigures = new GameFigure[NofGameFigures];
        for (int i = 0; i < gameFigures.Length; ++i)
        {
            GameObject figure = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/GameFigure", typeof(GameObject)));
            gameFigures[i] = (GameFigure)figure.GetComponent<GameFigure>();
            gameFigures[i].gameObject.name = (Color + "GameFigure" + i);
            gameFigures[i].transform.parent = GameObject.Find(Color + ParentFigureObjectSuffix).transform;
            gameFigures[i].Parent = this;

            Material material = (Material)Resources.Load("Materials/" + Color + "Player", typeof(Material));
            gameFigures[i].gameObject.GetComponent<Renderer>().material = material;
        }

        // instantiate the dice
        GameObject dice = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Dice", typeof(GameObject)));
        dice.name = (Color + "Dice");
        dice.transform.parent = GameObject.Find(Color + ParentDiceObjectSuffix).transform;
        dice.transform.localPosition = new Vector3(0, 0, 0);

        Dice = (Dice)dice.GetComponent<Dice>();
        Dice.Parent = this;
	}

    /// <summary>
    /// adjusts the the players state according to his figures
    /// </summary>
    public void RefreshState()
    {
        int figuresOnGameOrStairField = GameLogic.GetFiguresOnRegularOrStairField(gameFigures).Length;
        int figuresOnGoalField = GameLogic.GetFiguresOnGoalField(gameFigures).Length;
        int figuresOnHomeField = GameLogic.GetFiguresOnHomeField(gameFigures).Length;

        if (figuresOnGoalField == 4) State = new PlayerStateStateAllInGoal();
        else if (figuresOnGameOrStairField == 0) State = new PlayerStateAllAtHome();
        else State = new PlayerStateAtLeastOneOut();
    }
}