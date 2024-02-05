
namespace Dal;
using DalApi;
using System;

internal class ProjectImplementation : Iproject
{
    public DateTime? getStartingDate()
    {
        return DataSource.Config.startingDate;
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
}
