using UnityEngine;

public abstract class PlayerStateBase {

    public virtual void ThrowsRegular(int number)
    {
        GameLogic.State = new LogicStateThrowingDice();
        Debug.Log("[" + this.GetType().Name + "] Player has thrown a " + number + " - Nothing happens.");
    }

    public virtual void ThrowsFive()
    {
        GameLogic.State = new LogicStateThrowingDice();
        Debug.Log("[" + this.GetType().Name + "] Player has thrown a 5 - Nothing happens.");
    }

    public virtual void ThrowsSix()
    {
        GameLogic.State = new LogicStateThrowingDice();
        Debug.Log("[" + this.GetType().Name + "] Player has thrown a 6 - Nothing happens.");
    }
}