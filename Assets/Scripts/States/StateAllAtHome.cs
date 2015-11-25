
using UnityEngine;

public class StateAllAtHome : AbstractState {

    // Figure can be released
    public new void ThrowsFive()
    {
        Debug.Log("[" + this.GetType().Name + "] 5 has been thrown. A figure can be released.");
    }
}