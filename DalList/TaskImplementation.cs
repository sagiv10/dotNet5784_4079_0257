namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task newItem = item with { Id = newId };
        DataSource.Tasks.Add(newItem);
        return newId;
    }

    public void Delete(int id)
    {
        int idxOfDeleted = DataSource.Config.FindIndexTasks(id);
        if(idxOfDeleted ==-1) {
            throw new Exception($"Task with ID={id} does Not exist");
        }
        Task? NotActiveOne = DataSource.Tasks[idxOfDeleted] with { isActive = false };
        DataSource.Tasks.RemoveAt(idxOfDeleted);
        DataSource.Tasks.Add(NotActiveOne);
    }
    public Task? Read(int id)
    {
        foreach (var task in DataSource.Tasks) 
        {
            if(id==task!.Id && task.isActive==true)
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
            temp=new Task(item!.Id, item!.CreatedAtDate, item!.IsMilestone, item!.Alias, item!.Description, item!.ScheduledDate, item!.StartDate, item!.RequiredEffortTime, item!.DeadlineDate,item!.CompleteDate, item!.Deliverables,item!.Remarks,item!.Complexity,item!.EngineerId,item!.isActive);
            newList.Add(temp);
        }
        return newList;
    }

    public void Update(Task item)
    {

        int idxOfDeleted = DataSource.Config.FindIndexTasks(item.Id);
        if(idxOfDeleted == -1||DataSource.Tasks[idxOfDeleted]!.isActive==false)
        {
            throw new Exception($"Task with ID={item.Id} does Not exist");
        }
        DataSource.Tasks.RemoveAt(idxOfDeleted);
        DataSource.Tasks.Add(item);
    }
}
