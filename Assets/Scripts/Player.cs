using UnityEngine;

public class Player
{

    private const string ParentDiceObjectSuffix = "DicePosition";

    private IPlayerState state;

    private readonly string color;
    private readonly int playerAngle;

    public string Color
    {
        get { return color; }
    }

    public int PlayerAngle
    {
        get { return playerAngle; }
    }

    public IPlayerState State
    {
        get { return state; }
        set { state = value; }
    }

    public int LightAngle
    {
        get { return (playerAngle + 180 - 45) % 360; }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="color">color of the player</param>
    public Player(string color, int angle)
    {
        this.color = char.ToUpper(color[0]) + color.Substring(1);
        this.playerAngle = angle;
        this.state = new PlayerStateAllAtHome();
    }

    /// <summary>
    /// adjusts the the players state according to his figures
    /// </summary>
    public void RefreshState()
    {
        int figuresOnGameOrStairField = FigureCtrl.GetFiguresOnRegularOrStairField(this).Count;
        int figuresOnGoalField = FigureCtrl.GetFiguresOnGoalField(this).Count;
        int figuresOnHomeField = FigureCtrl.GetFiguresOnHomeField(this).Count;

        if (figuresOnGoalField == FigureCtrl.NofFigures) State = new PlayerStateStateAllInGoal();
        else if (figuresOnGameOrStairField == 0) State = new PlayerStateAllAtHome();
        else State = new PlayerStateAtLeastOneOut();
    }
}