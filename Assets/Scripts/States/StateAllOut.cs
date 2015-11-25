
using UnityEngine;

public class StateAllOut : StateAtLeastOneOut {

    // released figure can walk 5 steps
    public new void ThrowsFive()
    {
        Debug.Log("[" + this.GetType().Name + "] 5 has been thrown. A released figure can walk 5 steps.");
        ThrowsRegular(5);
    }
}
