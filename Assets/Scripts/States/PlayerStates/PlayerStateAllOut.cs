
using UnityEngine;

public class PlayerStateAllOut : PlayerStateAtLeastOneOut {

    // released figure can walk 5 steps
    public override void ThrowsFive()
    {
        Debug.Log("[" + this.GetType().Name + "] 5 has been thrown. A released figure can walk 5 steps.");
        ThrowsRegular(5);
    }
}
