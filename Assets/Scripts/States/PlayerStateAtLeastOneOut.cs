using UnityEngine;

public class PlayerStateAtLeastOneOut : IPlayerState
{

    public void ThrowsRegularOrSix()
    {
        GameCtrl.ActivateReleasedFigures();
    }

    public void ThrowsFive()
    {
        GameCtrl.ActivateAllFigures();
    }
}