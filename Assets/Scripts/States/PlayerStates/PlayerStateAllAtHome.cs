
using UnityEngine;

public class PlayerStateAllAtHome : AbstractPlayerState {

    // Figure can be released
    public override void ThrowsFive()
    {
        GameLogic.State = new LogicStateChoosingFigureFive();
        Debug.Log("[" + this.GetType().Name + "] 5 has been thrown. A figure can be released.");
    }
}