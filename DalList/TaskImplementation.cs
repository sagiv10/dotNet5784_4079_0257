namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task newItem = item with { _id = newId };
        DataSource.Tasks.Add(newItem);
        return newId;
    }

    public void Delete(int id)
    {
        int idxOfDeleted = DataSource.Config.FindIndexTasks(id);
        if(idxOfDeleted ==-1) {
            throw new Exception($"Task with ID={id} does Not exist");
        }
        Task? NotActiveOne = DataSource.Tasks[idxOfDeleted]! with { _isActive = false };
        DataSource.Tasks.RemoveAt(idxOfDeleted);
        DataSource.Tasks.Add(NotActiveOne);
    }
    public Task? Read(int id)
    {
        foreach (var task in DataSource.Tasks) 
        {
            if(id==task!._id && task._isActive==true)
            {
                return task;
            }
        }
        return null;
    }

    public List<Task?> ReadAll()
    {
        Task? temp;
        List<Task?> newList = new List<Task?>();
        foreach (var item in DataSource.Tasks)
        {
            temp=new Task(item!._id, item!._createdAtDate, item!._isMilestone, item!._alias, item!._description, item!._scheduledDate, item!._startDate, item!._requiredEffortTime, item!._deadlineDate,item!._completeDate, item!._deliverables,item!._remarks,item!._complexity,item!._engineerId,item!._isActive);
            newList.Add(temp);
        }
        return newList;
    }

    public void Update(Task item)
    {

        int idxOfDeleted = DataSource.Config.FindIndexTasks(item._id);
        if(idxOfDeleted == -1||DataSource.Tasks[idxOfDeleted]!._isActive==false)
        {
            throw new Exception($"Task with ID={item._id} does Not exist");
        }
        DataSource.Tasks.RemoveAt(idxOfDeleted);
        DataSource.Tasks.Add(item);
    }
}
