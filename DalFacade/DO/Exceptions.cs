using System;
namespace DO;


/// <summary>
/// class of new exception for case that we  put id that was not found in the lists
/// </summary>
[Serializable]
public class DalNotFoundException : Exception
{
    public DalNotFoundException(string? message) : base(message) { }
}

/// <summary>
/// class of new exception for case that we tried tob create new item with id that alredy exists in the lists
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}
/// <summary>
/// class of new exception for case that we failed to load the xml file to our program
/// </summary>
[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}

