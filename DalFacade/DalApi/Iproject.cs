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
    /// resets the running numbers for task and dependency in the database
    /// </summary>
    public void ResetRunningNumbers();
    /// <summary>
    /// Save the Project CurrentDate Into Xml file
    /// </summary>
    /// <param name="ProjectCurrentDate"></param>
    public void SaveProjectCurrentDateIntoXml(DateTime ProjectCurrentDate);
    public DateTime? getProjectCurrentDateIntoXml();


}
