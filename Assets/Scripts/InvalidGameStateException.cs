using System;

/// <summary>
/// Describes all misbehaviour in the game
/// </summary>
public class InvalidGameStateException : Exception
{

    public InvalidGameStateException() : base() { }

    public InvalidGameStateException(string message) : base(message) { }

}