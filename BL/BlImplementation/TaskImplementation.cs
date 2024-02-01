namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;//with this field we can access to the methods of task in dal. the way to use the method is being chosen inside factory.
    public int Create(BO.Task? newTask)
    {
        throw new NotImplementedException();
    }

    public void Delete(int idOfTaskToDelete)
    {
        throw new NotImplementedException();
    }

    public BO.Task? Read(int idOfWantedTask)
    {
        BO.EngineerInTask? eng = null;
        DO.Task? doTask = _dal.Task.Read(idOfWantedTask);//use read func from dal to get details of specific task
        if (doTask._engineerId != null)//some engineer works on this task
        {
            DO.Engineer? doEngineer = _dal.Engineer.Read((int)doTask._engineerId);//use read func from dal to get details of specific task
            eng = new BO.EngineerInTask() { Id = (int)doTask._engineerId, Name = doEngineer._name };
        }

        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={idOfWantedTask} does Not exist");

        return new BO.Task()//return a BO.task using the info from the do.task
        {
            Id = idOfWantedTask,
            Description = doTask._description,
            Alias = doTask._alias,
            CreatedAtDate = doTask._createdAtDate,
            Status = (Status?)WhatStatus(doTask._scheduledDate, doTask._startDate, doTask._completeDate),
            Dependencies = CheckDependenciesFromDal(idOfWantedTask),///////
            RequiredEffortTime = ((doTask._deadlineDate) - (doTask._startDate)),
            StartDate = doTask._startDate,
            ScheduledDate = doTask._scheduledDate,
            ForecastDate = 0,///////////
            DeadlineDate = doTask._deadlineDate,
            CompleteDate = doTask._completeDate,
            Deliverables = doTask._alias,
            Remarks = doTask._remarks,
            Engineer = eng,
            Complexity = (BO.EngineerExperience)doTask._complexity
        };
    }

    List<TaskInList?> CheckDependenciesFromDal(int idOfWantedTask)
    {
        //change like that: all dependencies => only the dependent dependencies => only their id's => their correct tasks => their correct TaskInList
        IEnumerable <DO.Dependency?> listDependencies = _dal.Dependency.ReadAll();//use read func from dal to get details of specific task
        listDependencies = listDependencies.Where(dependency => dependency._dependentTask == idOfWantedTask);
        IEnumerable<int?> listID = listDependencies.Select(dependency => dependency?._id);
        IEnumerable<DO.Task?> listTask = listID.Select(dependencyID => _dal.Task.Read((int)dependencyID));
        IEnumerable<TaskInList?> listTaskInList = listTask.Select(TaskEx => new TaskInList() 
        {
            Id= TaskEx._id,
            Description=TaskEx._description,
            Alias=TaskEx._alias,
            Status= (Status?)WhatStatus(TaskEx._scheduledDate, TaskEx._startDate, TaskEx._completeDate)
        } );
        return (List<TaskInList?>)listTaskInList;
    }

    public BO.Task? Read(Func<BO.Task?, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task?, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task? item)
    {
        throw new NotImplementedException();
    }
    int WhatStatus(DateTime? scheduledDate, DateTime? startDate, DateTime? completeDate)//////////////////
    {
        if (scheduledDate == null)//Unscheduled
        { return 0; }
        if (DateTime.Now < scheduledDate)//Scheduled
        { return 1; }
        if (DateTime.Now < completeDate)//OnTrack
        { return 2; }
        else
        { return 3; }
        //IF WE WANT TAKE CARE OF JEOPARDY 
    }
}
