using UnityEngine;

public class LogicStateThrowingDice : AbstractLogicState {

    public LogicStateThrowingDice()
    {
        if(GameLogic.LastDiceValue != 6 && GameLogic.State != null && GameLogic.State.GetType() != typeof(LogicStateChoosingFigureSix))
        {
            GameLogic.NextPlayer();
        }
        GameLogic.PlayerOnTurn.Dice.SetActive(true);
    }
}