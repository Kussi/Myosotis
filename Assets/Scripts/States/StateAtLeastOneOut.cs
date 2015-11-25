
using UnityEngine;

public class StateAtLeastOneOut : AbstractState {

    // released figure can walk <number> steps
    public new void ThrowsRegular (int number)
    {
        Debug.Log("[" + this.GetType().Name + "] " + number + " has been thrown. A released figure can walk " + number + " steps.");
    }

    // another figure can be released or a released one can walk 5 steps
    public new void ThrowsFive ()
    {
        Debug.Log("[" + this.GetType().Name + "] 5 has been thrown. Another figure can be released or a released one can walk 5 steps.");
    }

    // released figure can walk 6 steps and the player may throw the dice another time
    public new void ThrowsSix ()
    {
        Debug.Log("[" + this.GetType().Name + "] 6 has been thrown. A released figure can walk 6 steps and the player may throw the dice another time.");
    }

}