using System;

public class InvalidGameStateException : Exception
{

    public InvalidGameStateException() : base() { }

    public InvalidGameStateException(string message) : base(message) { }

}