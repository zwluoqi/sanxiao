using System;
using System.IO;

public sealed class TableException : IOException
{

    internal TableException(string message)
        : base(message) { }


    internal static TableException ErrorReader(String fort, params object[] args)
    {
        String error = String.Format(fort, args);
        return new TableException(error);
    }
}