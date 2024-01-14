using System;
namespace DO;
[Serializable]
public class DalNotFoundException : Exception
{
    public DalNotFoundException(string? message) : base(message) { }
}
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}