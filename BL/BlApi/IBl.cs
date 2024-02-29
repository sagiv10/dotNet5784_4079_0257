    namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }

    public ITask Task { get; }

    #region
    public DateTime Clock { get; }
    public void AddDays(int days);
    public void AddWeeks(int days);
    public void AddYear(int days);
    public void ResetClock();
    #endregion
}
