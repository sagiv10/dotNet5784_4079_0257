namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

internal class TaskImplementation : ITask
{
    /// <summary>
    /// this method takes the info of the task the user wrote us as a given parameter and changes the id by the static property and adds it to the tasks list.
    /// </summary>
    /// <param name="item"> new task info from the user</param>
    /// <returns> the new id</returns>
    public int Create(DO.Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        DO.Task newItem = item with { _id = newId };
        DataSource.Tasks.Add(newItem);
        return newId;
    }
    /// <summary>
    /// this program deletes the task that has the id we got. if not found-throw. if found so set isActive to false (by using new one and add him and remove the old one)
    /// </summary>
    /// <param name="id"> id to delete</param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Delete(int id)
    {
        int idxOfDeleted = DataSource.Config.FindIndexTasks(id);
        if(idxOfDeleted ==-1) {
            throw new DalNotFoundException($"Task with ID={id} does Not exist");
        }
        DO.Task? NotActiveOne = DataSource.Tasks[idxOfDeleted]! with { _isActive = false };
        DataSource.Tasks.RemoveAt(idxOfDeleted);
        DataSource.Tasks.Add(NotActiveOne);
    }

    /// <summary>
    /// this method helps us to print the info of a specific task that we recognize from given id by returning the task. if not found return null.
    /// </summary>
    /// <param name="id"> what task we want to print</param>
    /// <returns> returns the task we want to print</returns>
    public DO.Task? Read(int requestedId)
    {
        foreach (DO.Task? task in DataSource.Tasks)
        {
            if (task!._id == requestedId &&task!._isActive==true)
            {
                return task;
            }
        }
        return null;
    }

    /// <summary>
    /// this method helps us to print the info of a specific task that we recognize from given id by returning the task. if not found return null.
    /// </summary>
    /// <param name="filter"> predicat that determine wich task we want to print</param>
    /// <returns> returns the task we want to print</returns
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        ////////////////////////////////////////////////////////////////////////////
        return DataSource.Tasks.FirstOrDefault(filter!);
    }
    /// <summary>
    /// this method creates a new task list, copy the whole old list to the new one and returns the new.
    /// </summary>
    /// <param name="filter"> predicat that determine wich tasks we want to return</param>
    /// <returns>the new list </returns>
    public IEnumerable<DO.Task> ReadAll(Func<DO.Task, bool>? filter)
    {
        if (filter == null)
        {
            IEnumerable<DO.Task> newList = DataSource.Tasks.Select(item => item);
            return newList;
        }
        else
        {
            //////////////////////////////////////////////////////////
            IEnumerable<DO.Task?> newList = DataSource.Tasks.Where(item => filter(item!));
            return newList;
        }
    }
    /// <summary>
    /// this method gets a task, finds other task in the list with the same id to remove and sets the new task with the same id as the new task given.
    /// </summary>
    /// <param name="item">the new task</param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Update(DO.Task item)
    {

        int idxOfDeleted = DataSource.Config.FindIndexTasks(item._id);
        if(idxOfDeleted == -1||DataSource.Tasks[idxOfDeleted]!._isActive==false)//if the old task with the same id is not active or if we didnt found a task with the same id so throw.
        {
            throw new DalNotFoundException($"Task with ID={item._id} does Not exist");
        }
        DataSource.Tasks.RemoveAt(idxOfDeleted);
        DataSource.Tasks.Add(item);
    }
    public void DeleteAll() { DataSource.Tasks.Clear(); }

}
