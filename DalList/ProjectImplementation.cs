
namespace Dal;
using DalApi;
using System;

internal class ProjectImplementation : Iproject
{
    public int getProjectStatus()
    {
        {
          XElement configRoot = XElement.Load(@"..\xml\data-config.xml");
          return (BO.ProjectStatus)int.Parse(configRoot.Element("project-stage")!.Value);
        }

        public void getStartingDate(DateTime start)
    {
        throw new NotImplementedException();
    }

    public void setProjectStatus(int newStatus)
    {
        throw new NotImplementedException();
    }

    public void setStartingDate(DateTime start)
    {
        throw new NotImplementedException();
    }
}
    
