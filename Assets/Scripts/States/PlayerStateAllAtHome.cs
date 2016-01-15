/// <summary>
/// Representation of a playerstate, where all game figures of the player
/// are placed at home
/// </summary>
public class PlayerStateAllAtHome : IPlayerState
{
    /// <summary>
    /// player throws anything but 5
    /// </summary>
    public void ThrowsRegularOrSix()
    {
        GameCtrl.ActivateNoFigure();
    }

    /// <summary>
    /// player throws 5
    /// </summary>
    public void ThrowsFive()
    {
        GameCtrl.ActivateAllFigures();
    }
}