namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;//with this field we can access to the methods of task in dal. the way to use the method is being chosen inside factory.
    public int Create(BO.Task? newTask)//Check all input, add dependencies to ,cast to DO,then use do.create
    {
        if(newTask.Id<=0)
            throw new NotImplementedException();
        if(newTask.Alias=="")
            throw new NotImplementedException();
        DO.Task? doTaskToCheck = _dal.Task.Read(newTask.Id);//get task to check if exist
        if(doTaskToCheck != null)
            throw new NotImplementedException();
        newTask.Dependencies.Select(dependency=>createTloot(newTask.Id, dependency));
        DO.Task? doTaskToCreate = BOToDOTask(newTask);
        _dal.Task.Create(doTaskToCreate);
        return newTask.Id;
    }
    private DO.Task? BOToDOTask(BO.Task newTask)
    {
        return new DO.Task() {
            _id = newTask.Id,
            _createdAtDate = newTask.CreatedAtDate,
            _isMilestone = false,
            _alias = newTask.Alias,
            _description = newTask.Description,
            _scheduledDate = newTask.ScheduledDate,
            _startDate = newTask.StartDate,
            _requiredEffortTime = newTask.RequiredEffortTime,
            _deadlineDate = newTask.DeadlineDate,
            _completeDate = newTask.CompleteDate,
            _deliverables = newTask.Deliverables,
            _remarks = newTask.Remarks,
            _complexity = (ComplexityLvls)newTask.Complexity,
            _engineerId = newTask.Engineer.Id,
            _isActive = true
        };
    }

    public void Delete(int idOfTaskToDelete)
    {
        throw new NotImplementedException();
    }

    public BO.Task? Read(int idOfWantedTask)
    {
        DO.Task? doTask = _dal.Task.Read(idOfWantedTask);//use read func from dal to get details of specific task
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={idOfWantedTask} does Not exist");

        return MakeBOFromDoTASK(doTask);
    }

    private DateTime? ForecastCalc(DateTime? scheduledDate, DateTime? startDate, TimeSpan? RequiredEffortTime)
    {
        if(scheduledDate > startDate)
            return scheduledDate + RequiredEffortTime;
        else
            return startDate + RequiredEffortTime;
    }

    private EngineerInTask CheckIfEngineerFromTaskIsExist(int idOfTask)
    {
        BO.EngineerInTask? eng = null;
        DO.Task? doTask = _dal.Task.Read(idOfTask);//use read func from dal to get details of specific task
        if (doTask._engineerId != null)//some engineer works on this task
        {
            DO.Engineer? doEngineer = _dal.Engineer.Read((int)doTask._engineerId);//use read func from dal to get details of specific task
            eng = new BO.EngineerInTask() { Id = (int)doTask._engineerId, Name = doEngineer._name };
        }
        return eng;
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
        IEnumerable<DO.Task?> AllDOTasks = _dal.Task.ReadAll();//use read func from dal to get details of all tasks
        if (AllDOTasks == null)
            throw new BO.BlDoesNotExistException($"Task does Not exist");
        IEnumerable<BO.Task?> AllBOTasks = AllDOTasks.Select(DOTtaskInList => MakeBOFromDoTASK(DOTtaskInList));
        BO.Task? chosen= AllBOTasks.FirstOrDefault(filter);//FILTER
        return chosen;
    }

    public IEnumerable<BO.TaskInList?> ReadAll(Func<BO.Task?, bool>? filter = null)
    {
        IEnumerable<DO.Task?> AllDOTasks = _dal.Task.ReadAll();//use read func from dal to get details of all tasks
        if (AllDOTasks == null)
            throw new BO.BlDoesNotExistException($"Tasks list does Not exist");
        IEnumerable<BO.Task?> AllBOTasks = AllDOTasks.Select(DOTtaskInList => MakeBOFromDoTASK(DOTtaskInList));
        AllBOTasks = AllBOTasks.Where(TaskEx=>filter(TaskEx));//FILTER
        IEnumerable<TaskInList?> TasksInList = AllBOTasks.Select(BOtaskInList => new TaskInList()//make to task in list to return properly
        {
            Id = BOtaskInList.Id,
            Description = BOtaskInList.Description,
            Alias = BOtaskInList.Alias,
            Status = BOtaskInList.Status 
        }) ;
        return TasksInList;
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
        { return 3; }//done
        //IF WE WANT TAKE CARE OF JEOPARDY 
    }
    BO.Task? MakeBOFromDoTASK (DO.Task? doTask)
    {
        return new BO.Task()
        {
            Id = doTask._id,
            Description = doTask._description,
            Alias = doTask._alias,
            CreatedAtDate = doTask._createdAtDate,
            Status = (Status?)WhatStatus(doTask._scheduledDate, doTask._startDate, doTask._completeDate),
            Dependencies = CheckDependenciesFromDal(doTask._id) ?? null,
            RequiredEffortTime = doTask._requiredEffortTime,
            StartDate = doTask._startDate,
            ScheduledDate = doTask._scheduledDate,
            ForecastDate = ForecastCalc(doTask._scheduledDate, doTask._startDate, doTask._requiredEffortTime),
            DeadlineDate = doTask._deadlineDate,
            CompleteDate = doTask._completeDate,
            Deliverables = doTask._alias,
            Remarks = doTask._remarks,
            Engineer = CheckIfEngineerFromTaskIsExist(doTask._id),
            Complexity = (BO.EngineerExperience)doTask._complexity
        };
    }
}

