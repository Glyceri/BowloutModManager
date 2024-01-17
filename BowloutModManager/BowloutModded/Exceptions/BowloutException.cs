using System;

public class BowloutException : Exception
{
    public BowloutException() { }
    public BowloutException(string message) : base(message) { }
}
