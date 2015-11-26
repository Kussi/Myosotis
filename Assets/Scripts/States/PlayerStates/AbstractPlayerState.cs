using UnityEngine;

public abstract class AbstractPlayerState {

    public virtual void ThrowsRegular (int number)
    {
        GameLogic.State = new LogicStateThrowingDice();
        Debug.Log("[" + this.GetType().Name + "] " + number + " has been thrown. Nothing happens.");
    }

    public virtual void ThrowsFive ()
    {
        GameLogic.State = new LogicStateThrowingDice();
        Debug.Log("[" + this.GetType().Name + "] 5 has been thrown. Nothing happens.");
    }

    public virtual void ThrowsSix ()
    {
        GameLogic.State = new LogicStateThrowingDice();
        Debug.Log("[" + this.GetType().Name + "] 6 has been thrown. Nothing happens.");
    }
}