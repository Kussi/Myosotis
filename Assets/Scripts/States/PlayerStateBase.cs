using UnityEngine;

public abstract class PlayerStateBase {

    public virtual void ThrowsRegular(int number)
    {
        GameCtrl.State = new GameStateThrowingDice();
        Debug.Log("[" + this.GetType().Name + "] Player has thrown a " + number + " - Nothing happens.");
    }

    public virtual void ThrowsFive()
    {
        GameCtrl.State = new GameStateThrowingDice();
        Debug.Log("[" + this.GetType().Name + "] Player has thrown a 5 - Nothing happens.");
    }

    public virtual void ThrowsSix()
    {
        GameCtrl.State = new GameStateThrowingDice();
        Debug.Log("[" + this.GetType().Name + "] Player has thrown a 6 - Nothing happens.");
    }
}