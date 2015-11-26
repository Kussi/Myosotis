using UnityEngine;

public class LogicStateThrowingDice : AbstractLogicState {

    public override void ActivateDice()
    {
        Debug.Log("[" + this.GetType().Name + "] " + GameLogic.PlayerOnTurn.Color + " dice will be activated.");
    }
}