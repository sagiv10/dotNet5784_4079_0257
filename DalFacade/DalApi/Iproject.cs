namespace DalApi;

public interface Iproject
{
    /// <summary>
    /// helping method that gets from the dal-config xml file the current progect status
    /// </summary>
    /// <returns>the current status of the project</returns>
    public int getProjectStatus();

    /// <summary>
    /// helping method that change the dal-config xml file into the current progect status
    /// </summary>
    public void setProjectStatus(int newStatus);

    /// <summary>
    /// helping method that change the dal-config xml file into the current progect starting time
    /// </summary>
    public void setStartingDate(DateTime? start);

    /// <summary>
    /// helping method that gets the dal-config xml file into the current progect starting time
    /// </summary>
    public DateTime? getStartingDate();

    /// <summary>
    /// helping method that change the dal-config xml file into the current time
    /// </summary>
    public void SetCurrentDate(DateTime? currentDate);

    /// <summary>
    /// helping method that gets the dal-config xml file into the current time
    /// </summary>
    public DateTime? GetCurrentDate();
}
