using UnityEngine;

public class Player {

    private const int NofGameFigures = 4;
    
    private const string ParentDiceObjectSuffix = "DicePosition";

    private Dice dice;
    private GameFigure[] gameFigures;
    private PlayerStateBase state;

    private readonly string color;

    private readonly int homeFieldIndex;
    private readonly int homeBenchIndex;
    private readonly int stairBenchIndex;
    private readonly int firstStairStepIndex;
    private readonly int playerAngle;

    public string Color
    {
        get { return color; }
    }

    public string Name
    {
        get { return Color; }
    }

    public PlayerStateBase State
    {
        get { return state; }
        set { state = value; }
    }

    

    //public int LightAngle
    //{
    //    get { return (playerAngle + 180 - 45) % 360; }
    //}

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="color">color of the player</param>
    public Player(string color)
    {
        this.color = char.ToUpper(color[0]) + color.Substring(1);

        // initiate the first Playerstate
        this.state = new PlayerStateAllAtHome();       
	}

    /// <summary>
    /// adjusts the the players state according to his figures
    /// </summary>
    public void RefreshState()
    {
        int figuresOnGameOrStairField = GameCtrl.GetFiguresOnRegularOrStairField(gameFigures).Length;
        int figuresOnGoalField = GameCtrl.GetFiguresOnGoalField(gameFigures).Length;
        int figuresOnHomeField = GameCtrl.GetFiguresOnHomeField(gameFigures).Length;

        if (figuresOnGoalField == 4) State = new PlayerStateStateAllInGoal();
        else if (figuresOnGameOrStairField == 0) State = new PlayerStateAllAtHome();
        else State = new PlayerStateAtLeastOneOut();
    }
}