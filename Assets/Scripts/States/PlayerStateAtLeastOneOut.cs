/// <summary>
/// Representation of a playerstate, where at least one of the players
/// gamefigures is placed on a regular or stairfield (not home and not
/// goal)
/// </summary>
public class PlayerStateAtLeastOneOut : IPlayerState
{
    /// <summary>
    /// player throws anything but 5
    /// </summary>
    public void ThrowsRegularOrSix()
    {
        GameCtrl.ActivateReleasedFigures();
    }

    /// <summary>
    /// player throws 5
    /// </summary>
    public void ThrowsFive()
    {
        GameCtrl.ActivateAllFigures();
    }
}