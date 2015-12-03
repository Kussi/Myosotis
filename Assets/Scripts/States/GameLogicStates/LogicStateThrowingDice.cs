using UnityEngine;

public class LogicStateThrowingDice : ILogicState {

    public LogicStateThrowingDice()
    {
        if (GameLogic.PlayerOnTurn.Dice.Value != 6 
            && GameLogic.State != null 
            && GameLogic.State.GetType() != typeof(LogicStateChoosingFigureSix))
        {
            GameLogic.NextPlayer();
        }

        GameLogic.ActivateDice();
    }
}