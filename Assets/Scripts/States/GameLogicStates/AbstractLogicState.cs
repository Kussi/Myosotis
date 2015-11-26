using UnityEngine;

public abstract class AbstractLogicState {

    public virtual void ActivateDice()
    {
        Debug.Log("[" + this.GetType().Name + "] no dice will be activated.");
    }

    public virtual void ActivateFigures()
    {
        Debug.Log("[" + this.GetType().Name + "] no figure will be activated.");
    }

    public virtual void ShiftFigure()
    {
        Debug.Log("[" + this.GetType().Name + "] no figure will be shifted.");
    }
}