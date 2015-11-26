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
}