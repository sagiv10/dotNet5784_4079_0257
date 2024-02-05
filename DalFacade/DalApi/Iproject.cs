namespace DalApi;

public interface Iproject
{
    /// <summary>
    /// helping method that gets from the dal-config xml file the current progect status
    /// </summary>
    /// <returns>the current status of the project</returns>
    int getProjectStatus();
    //{
    //    XElement configRoot = XElement.Load(@"..\xml\data-config.xml");
    //    return (BO.ProjectStatus)int.Parse(configRoot.Element("project-stage")!.Value);
    //}

    /// <summary>
    /// helping method that change the dal-config xml file into the current progect status
    /// </summary>
    void setProjectStatus(int newStatus);
    //{
    //    XElement configRoot = XElement.Load(@"..\xml\data-config.xml"); //get the previous root

    //    configRoot.Element("project-stage")!.Value = String.Format("{0}", newStatus); //change the field of the projectStatus

    //    configRoot.Save(@"..\xml\data-config.xml"); //save it
    //}

    /// <summary>
    /// helping method that change the dal-config xml file into the current progect starting time
    /// </summary>
    void setStartingDate(DateTime start);
    //{
    //    XElement configRoot = XElement.Load(@"..\xml\data-config.xml"); //get the previous root

    //    XElement theTime = new XElement("project-starting-date", start); //create new tag of the starting date

    //    configRoot.Element("project-stage")!.Add(theTime); //create the field of the starting date (he did not exist untill now)

    //    configRoot.Save(@"..\xml\data-config.xml"); //save it
    //}

    /// <summary>
    /// helping method that change the dal-config xml file into the current progect starting time
    /// </summary>
    void getStartingDate(DateTime start);
}
