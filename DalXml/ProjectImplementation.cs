namespace Dal;
using DalApi;
using System.Xml.Linq;


internal class ProjectImplementation : Iproject
{
    public int getProjectStatus()
    {
        XElement configRoot = XElement.Load(@"..\xml\data-config.xml");
        return int.Parse(configRoot.Element("project-stage")!.Value);
    }

    public void setProjectStatus(int newStatus)
    {
        XElement configRoot = XElement.Load(@"..\xml\data-config.xml"); //get the previous root

        if(configRoot.Element("project-stage") == null)
        {
            configRoot.Add(new XElement("project-stage", 1));
        }
        configRoot.Element("project-stage")!.Value = String.Format("{0}", newStatus); //change the field of the projectStatus

        configRoot.Save(@"..\xml\data-config.xml"); //save it
    }

    public DateTime? getStartingDate()
    {
        XElement configRoot = XElement.Load(@"..\xml\data-config.xml");
        return configRoot.Element("project-starting-date") != null ? DateTime.Parse(configRoot.Element("project-starting-date")!.Value) : null;
    }

    public void setStartingDate(DateTime? start)
    {
        XElement configRoot = XElement.Load(@"..\xml\data-config.xml"); //get the previous root

        XElement? theTime = configRoot.Element("project-starting-date");

        if(start == null ) //then we  want to delete the starting time
        {
            if (theTime != null)
            {
                theTime.Remove();
            }
            //else - nothing
        }
        else
        {

            if (theTime != null)
            {
                configRoot.Element("project-starting-date")?.SetValue(start);
            }
            else
            {
                XElement newTime = new XElement("project-starting-date", start); //create new tag of the starting date

                configRoot.Add(newTime); //create the field of the starting date (he did not exist untill now)
            }
        }

        configRoot.Save(@"..\xml\data-config.xml"); //save it
    }

    public void ResetRunningNumbers()
    {
        XElement newNumbers = new XElement("config", new XElement("NextDependencyId", 1), new XElement("NextTaskId", 1));
        XMLTools.SaveListToXMLElement(newNumbers, "data-config");//save new running numberwhen they equal to 0 now
    }
    public void SaveProjectCurrentDateIntoXml(DateTime ProjectCurrentDate)
    {
        XElement configRoot = XElement.Load(@"..\xml\data-config.xml"); //get the previous root
        XElement? theTime = configRoot.Element("project-currently-date");
        if (ProjectCurrentDate == null) //then we  want to delete the starting time
        {
            if (theTime != null)
            {
                theTime.Remove();
            }
            //else - nothing
        }
        else
        {
            if (theTime != null)
            {
                configRoot.Element("project-currently-date")?.SetValue(ProjectCurrentDate);
            }
            else
            {
                XElement newTime = new XElement("project-currently-date", ProjectCurrentDate); //create new tag of the starting date
                configRoot.Add(newTime); //create the field of the starting date (he did not exist untill now)
            }
        }
        configRoot.Save(@"..\xml\data-config.xml"); //save 
    }
    public DateTime? getProjectCurrentDateIntoXml()
    {
        XElement configRoot = XElement.Load(@"..\xml\data-config.xml");
        return configRoot.Element("project-currently-date") != null ? DateTime.Parse(configRoot.Element("project-currently-date")!.Value) : null;
    }

}
