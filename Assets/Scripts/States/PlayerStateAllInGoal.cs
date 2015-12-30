
using System;
using UnityEngine;

public class PlayerStateStateAllInGoal : IPlayerState
{
    public void ThrowsRegularOrSix()
    {
        throw new InvalidGameStateException();
    }

    public void ThrowsFive()
    {
        throw new InvalidGameStateException();
    }
}