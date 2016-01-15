/// <summary>
/// Representation of a playerstate, where all of the players
/// gamefigures are played in goal. The player has therefore
/// finished the game
/// </summary>
public class PlayerStateStateAllInGoal : IPlayerState
{
    /// <summary>
    /// player throws anything but 5
    /// </summary>
    public void ThrowsRegularOrSix()
    {
        throw new InvalidGameStateException();
    }

    /// <summary>
    /// player throws 5
    /// </summary>
    public void ThrowsFive()
    {
        throw new InvalidGameStateException();
    }
}