
using UnityEngine;

public class PlayerStateAllAtHome : PlayerStateBase {

    public override void ThrowsFive()
    {
        GameLogic.State = new LogicStateChoosingFigureFive();
        Debug.Log("[" + this.GetType().Name + "] 5 has been thrown. A figure can be released.");
    }
}