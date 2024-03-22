namespace Dal;
using DalApi;
using System;

internal class ProjectImplementation : Iproject
{

    public DateTime? getStartingDate()
    {
        return DataSource.Config.startingDate;
    }

    public void ResetRunningNumbers()
    {
        DataSource.Config.NextDependencyId = 1;
        DataSource.Config.NextTaskId = 1;
    }

    public void setStartingDate(DateTime? start)
    {
        DataSource.Config.startingDate = start;
    }

    int Iproject.getProjectStatus()
    {
        return DataSource.Config.projectStatus;
    }

    void Iproject.setProjectStatus(int newStatus)
    {
        DataSource.Config.projectStatus = newStatus;
    }
    public void SaveProjectCurrentDateIntoXml(DateTime ProjectCurrentDate)
    {
        DataSource.Config.currentDate = ProjectCurrentDate;

    }
    public DateTime? getProjectCurrentDateIntoXml()
    {
        return DataSource.Config.currentDate;
    }


}
