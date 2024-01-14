using System;
namespace DO;
[Serializable]
public class DalNotFoundException : Exception
{
    DalNotFoundException(string? message) : base(message) { }
}
public class DalAlreadyExistsException : Exception
{
    DalAlreadyExistsException(string? message) : base(message) { }
}