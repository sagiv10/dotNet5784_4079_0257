namespace BO;

[Serializable]
public class BLNotFoundException : Exception
{
    public BLNotFoundException(string type, int id) : base($"{type} with id: {id} does not exist") { }
    public BLNotFoundException(string type, int id, Exception innerException)
                : base($"{type} with id {id} does not exist", innerException) { }
}

