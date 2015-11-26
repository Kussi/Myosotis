using UnityEngine;

public class LogicStateThrowingDice : AbstractLogicState {

    public LogicStateThrowingDice()
    {
        GameLogic.NextPlayer();
        GameLogic.PlayerOnTurn.Dice.SetActive(true);
    }
}