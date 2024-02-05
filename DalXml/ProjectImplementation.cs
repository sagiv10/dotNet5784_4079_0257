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

        configRoot.Element("project-stage")!.Value = String.Format("{0}", newStatus); //change the field of the projectStatus

        configRoot.Save(@"..\xml\data-config.xml"); //save it
    }

    public DateTime? getStartingDate()
    {
        throw new NotImplementedException();
    }

    public void setStartingDate(DateTime start)
    {
        XElement configRoot = XElement.Load(@"..\xml\data-config.xml"); //get the previous root

        XElement theTime = new XElement("project-starting-date", start); //create new tag of the starting date

        configRoot.Element("project-stage")!.Add(theTime); //create the field of the starting date (he did not exist untill now)

        configRoot.Save(@"..\xml\data-config.xml"); //save it
    }
}
