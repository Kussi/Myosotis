
using System;
using UnityEngine;

public class PlayerStateAllAtHome : IPlayerState {

    public void ThrowsRegularOrSix()
    {
        GameCtrl.ActivateNoFigure();
    }

    public void ThrowsFive()
    {
        GameCtrl.ActivateAllFigures();
    }
}