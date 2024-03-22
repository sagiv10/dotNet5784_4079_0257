    namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }

    public ITask Task { get; }

    public IConfig Config { get; }

    #region
    public DateTime Clock { get; }
    public void AddDay();
    public void AddWeek();
    public void AddMonth();
    public void AddYear();
    public void ResetClock();
    public void initializeClock();

    #endregion
}
