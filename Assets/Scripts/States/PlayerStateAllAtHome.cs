
using UnityEngine;

public class PlayerStateAllAtHome : PlayerStateBase {

    public override void ThrowsFive()
    {
        GameCtrl.State = new GameStateChoosingFigureFive();
        Debug.Log("[" + this.GetType().Name + "] 5 has been thrown. A figure can be released.");
    }
}