using DalApi;
namespace Dal;
using DO;
using System.Data.Common;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";
    /// <summary>
    /// this method takes the info of the task the user wrote us as a given parameter and changes the id by the static property and adds it to the xml file.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(DO.Task item)
    {
        int newId = Config.NextTaskId;
        DO.Task newItem = item with { _id = newId };
        List<Task> ListToWorkWith = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//get the list from the xml file to work with it
        ListToWorkWith.Add(newItem);
        XMLTools.SaveListToXMLSerializer<Task>(ListToWorkWith, s_tasks_xml);//save the changes we did in the list we got in the start 
        return newId;
    }
    /// <summary>
    /// this program deletes the task that has the id we got. if not found-throw. if found so set isActive to false (by using new one and add him and remove the old one)
    /// </summary>
    /// <param name="id">id to delete</param>
    /// <exception cref="DalXMLFileLoadCreateException"></exception>
    public void Delete(int id)
    {
        List<Task> ListToWorkWith = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //get the list from the xml file to work with it
        int indexToUpdate = ListToWorkWith.FindIndex(item => item._id == id && item._isActive == true);//if the task with the correct id exists-return his idx, else return -1.
        if (indexToUpdate == -1)
            throw new DalNotFoundException($"Task with ID={id} does Not exist");
        DO.Task? NotActiveOne = ListToWorkWith[indexToUpdate]! with { _isActive = false };
        ListToWorkWith.RemoveAt(indexToUpdate);
        ListToWorkWith.Add(NotActiveOne);
        XMLTools.SaveListToXMLSerializer<Task>(ListToWorkWith, s_tasks_xml);//save the changes we did in the list we got in the start
    }
    /// <summary>
    /// this method helps us to print the info of a specific task that we recognize from given id by returning the task. if not found return null.
    /// </summary>
    /// <param name="id"> what task we want to print</param>
    /// <returns> returns the task we want to print</returns>
    public DO.Task? Read(int id)
    {
        List<Task> ListToWorkWith = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//get the list from the xml file to work with it
        foreach (DO.Task? task in ListToWorkWith)
        {
            if (task!._id == id && task!._isActive == true)
            {
                //XMLTools.SaveListToXMLSerializer<Task>(ListToWorkWith, s_tasks_xml); //save the changes we did in the list we got in the start
                return task;
            }
        }
        //XMLTools.SaveListToXMLSerializer<Task>(ListToWorkWith, s_tasks_xml); //save the changes we did in the list we got in the start
        return null;
    }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<Task> ListToWorkWith = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//get the list from the xml file to work with it
        //XMLTools.SaveListToXMLSerializer<Task>(ListToWorkWith, s_tasks_xml); //save the changes we did in the list we got in the start
        return ListToWorkWith.FirstOrDefault(item => filter!(item) && item._isActive == true);
    }
    /// <summary>
    /// this method creates a new task list, copy the whole old list to the new one and returns the new.
    /// </summary>
    /// <param name="filter"> predicat that determine wich tasks we want to return</param>
    /// <returns>the new list </returns>
    public IEnumerable<DO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<Task> ListToWorkWith = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//get the list from the xml file to work with it
        if (filter == null)
        {
            IEnumerable<DO.Task> newList = ListToWorkWith.Select(item => item);
            //XMLTools.SaveListToXMLSerializer<Task>(ListToWorkWith, s_tasks_xml);//save the changes we did in the list we got in the start
            return newList;
        }
        else
        {
            IEnumerable<DO.Task> newList = ListToWorkWith.Where(item => filter(item!));
            //XMLTools.SaveListToXMLSerializer<Task>(ListToWorkWith, s_tasks_xml);//save the changes we did in the list we got in the start
            return newList;
        }
    }
    /// <summary>
    /// this method gets a task, finds other task in the list with the same id to remove and sets the new task with the same id as the new task given.
    /// </summary>
    /// <param name="item">the new task</param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Update(DO.Task itemUpdated)
    {
        List<Task> ListToWorkWith = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//get the list from the xml file to work with it
        int indexToUpdate = ListToWorkWith.FindIndex(itemOfTheLoop => itemOfTheLoop._id == itemUpdated._id && itemOfTheLoop._isActive == true);//if the task with the correct id exists-return his idx, else return -1.
        if (indexToUpdate == -1)
            throw new DalXMLFileLoadCreateException($"Task with ID={itemUpdated._id} does Not exist");
        ListToWorkWith.RemoveAt(indexToUpdate);
        
        ListToWorkWith.Add(itemUpdated);
        XMLTools.SaveListToXMLSerializer<Task>(ListToWorkWith, s_tasks_xml);//save the changes we did in the list we got in the start
    }
    /// <summary>
    /// Delete all existing data from xml file to let us do a new initalize.
    /// </summary>
    public void DeleteAll()
    {
        XElement emptyRoot = new XElement("ArrayOfTask");//get the list from the xml file to work with it
        XMLTools.SaveListToXMLElement(emptyRoot, s_tasks_xml);//save the changes we did in the list we got in the start
    }
}
