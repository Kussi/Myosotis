
using UnityEngine;

public abstract class AbstractState {

    public void ThrowsRegular (int number)
    {
        Debug.Log("[" + this.GetType().Name + "] " + number + " has been thrown. Nothing happens.");
    }

    public void ThrowsFive ()
    {
        Debug.Log("[" + this.GetType().Name + "] 5 has been thrown. Nothing happens.");
    }

    public void ThrowsSix ()
    {
        Debug.Log("[" + this.GetType().Name + "] 6 has been thrown. Nothing happens.");
    }
}