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
        //no implementation here because if we in list implementation then we cannot save the clock from running to running

    }
    public DateTime? getProjectCurrentDateIntoXml()
    {
        DataSource.Config.currentDate = DateTime.Now; //initialize the clock to be now date 
        return DataSource.Config.currentDate;
    }


}
