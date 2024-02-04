namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }

    public ITask Task { get; }

    public void AutoScedule(DateTime startingDate);

    public void ManualScedule(DateTime startingDate);

}
