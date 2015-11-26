using System.Collections;

public static class GameLogic {

    private static Player[] player;
    private static ArrayList gameFields;
    private static AbstractLogicState logicState;

    private static int playerOnTurn = 0;

    public static Player PlayerOnTurn
    {
        get { return player[playerOnTurn]; }
    }
    public static AbstractLogicState State
    {
        get { return logicState; }
        set { logicState = value; }
    }


    public static void Initialize(Player[] player, ArrayList gameFields)
    {
        GameLogic.player = player;
        GameLogic.gameFields = gameFields;
        GameLogic.logicState = new LogicStateThrowingDice();
    }

    public static void ExecuteTurn(int number)
    {
        if (number == 5)
        {
            PlayerOnTurn.State.ThrowsFive();
        }
        else if (number == 6)
        {
            PlayerOnTurn.State.ThrowsSix();
        }
        else
        {
            PlayerOnTurn.State.ThrowsRegular(number);
        }
    }

    public static GameFigure[] GetFiguresOnGameFields(GameFigure[] figures)
    {
        ArrayList figuresOnGameFields = new ArrayList();
        foreach(GameFigure figure in figures)
        {
            if(figure.Field.GetType() == typeof(GameField))
            {
                figuresOnGameFields.Add(figure);
            }
        }
        return (GameFigure[])figuresOnGameFields.ToArray();
    }

    public static GameFigure[] GetFiguresOnGameOrHomeFields(GameFigure[] figures)
    {
        ArrayList figuresOnGameFields = new ArrayList();
        foreach (GameFigure figure in figures)
        {
            if (figure.Field.GetType() == typeof(GameField) || figure.Field.GetType() == typeof(HomeField))
            {
                figuresOnGameFields.Add(figure);
            }
        }
        return (GameFigure[])figuresOnGameFields.ToArray();
    }
}