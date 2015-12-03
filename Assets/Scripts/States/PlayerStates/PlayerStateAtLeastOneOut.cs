using UnityEngine;

public class PlayerStateAtLeastOneOut : PlayerStateBase {

    // released figure can walk <number> steps
    public override void ThrowsRegular (int number)
    {
        GameLogic.State = new LogicStateChoosingFigureRegular();
        Debug.Log("[" + this.GetType().Name + "] " + number + " has been thrown. A released figure can walk " + number + " steps.");
    }

    // another figure can be released or a released one can walk 5 steps
    public override void ThrowsFive ()
    {
        GameLogic.State = new LogicStateChoosingFigureFive();
        Debug.Log("[" + this.GetType().Name + "] 5 has been thrown. Another figure can be released or a released one can walk 5 steps.");
    }

    // released figure can walk 6 steps and the player may throw the dice another time
    public override void ThrowsSix()
    {
        GameLogic.State = new LogicStateChoosingFigureSix();
        Debug.Log("[" + this.GetType().Name + "] 6 has been thrown. A released figure can walk 6 steps and the player may throw the dice another time.");
    }
}