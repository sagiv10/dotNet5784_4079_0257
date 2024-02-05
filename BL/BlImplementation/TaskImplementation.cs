namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;//with this field we can access to the methods of task in dal. the way to use the method is being chosen inside factory.

    /// <summary>
    /// reqursive helping method for the AutoScedule method
    /// </summary>
    /// <param name="taskId"> the task we want to scedule </param>
    /// <param name="formerForcastDate"> the forcast date from the task that was before him in the reqursive method </param>
    private void autoSceduleTask(int taskId, DateTime formerForcastDate)
    {
        DO.Task theTask = _dal.Task.Read(taskId)!;
        DO.Task sceduledTask;
        if (theTask._scheduledDate == null || formerForcastDate > theTask._scheduledDate) 
        {
            sceduledTask = theTask with { _scheduledDate = formerForcastDate}; //when the new suggested date is later the previous one or when there is no previous one
        }
        else
        {
            sceduledTask = theTask; //in case there is alredy leter date so keep him
        }
        _dal.Task.Update(sceduledTask);
        foreach (var dep in _dal.Dependency.ReadAll(d => d._dependsOnTask == taskId)) //change all the task that depends on him
        {
            autoSceduleTask((int)dep!._dependentTask!, (DateTime)(sceduledTask._scheduledDate + sceduledTask._requiredEffortTime!));
        }
    }

    /// <summary>
    /// hepling method that checks if we got a circular dependency from the new dependency
    /// </summary>
    /// <param name="dependent"> the id of the dependent task </param>
    /// <param name="dependOn"> the id of the dependent On task </param>
    /// <returns></returns>
    //public bool checkCircularDependency(int dependent, int dependOn)
    //{

    //}

    public int Create(BO.Task? newTask)//Check all input, add dependencies to ,cast to DO,then use do.create
    {
        if(newTask.Id<=0)
            throw new lkmklmlk();

        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException();
        }

        if (newTask.Alias=="")
        {
            throw new drgd();
        }
        DO.Task? doTaskToCheck = _dal.Task.Read(newTask.Id);//get task to check if exist
        if(doTaskToCheck != null)
            throw new NotImplementedException();
        foreach (var dep in newTask.Dependencies){ AddDependency(newTask.Id, dep.Id); }
        DO.Task? doTaskToCreate = BOToDOTask(newTask);
        _dal.Task.Create(doTaskToCreate);
        return newTask.Id;
    }

    /// <summary>
    /// helping method that converts BO.Task to DO.Task
    /// </summary>
    /// <param name="newTask"> BO.Task </param>
    /// <returns> DO.Task </returns>
    private DO.Task? BOToDOTask(BO.Task newTask)
    {
        return new DO.Task()
        {
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
            _complexity = (ComplexityLvls)newTask.Complexity!,
            _engineerId = newTask.Engineer.Id,
            _isActive = true
        };
    }

    /// <summary>
    /// helping method that converts DO.Task to BO.Task
    /// </summary>
    /// <param name="doTask"> DO.Task </param>
    /// <returns> BO.Task </returns>
    private BO.Task MakeBOFromDoTASK(DO.Task doTask)
    {
        return new BO.Task()
        {
            Id = doTask._id,
            Description=doTask._description,
            Alias=doTask._alias,
            CreatedAtDate=doTask._createdAtDate,
            Status=(Status?)WhatStatus(doTask._scheduledDate,doTask._startDate,doTask._completeDate), //inducate his status
            Dependencies=GetDependenciesFromDal(doTask._id), //all his dependencies
            Milestone=null,
            RequiredEffortTime=doTask._requiredEffortTime,
            StartDate=doTask._startDate,
            ScheduledDate=doTask._scheduledDate,
            ForecastDate=ForecastCalc(doTask._scheduledDate, doTask._startDate, doTask._requiredEffortTime),
            DeadlineDate=doTask._deadlineDate,
            CompleteDate=doTask._completeDate,
            Deliverables=doTask._deliverables,
            Remarks=doTask._remarks,
            Engineer= CheckIfEngineerFromTaskIsExist((int)doTask._engineerId),
            Complexity=(BO.EngineerExperience)doTask._complexity
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scheduledDate"></param>
    /// <param name="startDate"></param>
    /// <param name="RequiredEffortTime"></param>
    /// <returns></returns>
    private DateTime? ForecastCalc(DateTime? scheduledDate, DateTime? startDate, TimeSpan RequiredEffortTime)
    {
        if (scheduledDate == null && startDate == null) //if n one of those 2 times has been started
        {
            return null;
        }
        if (startDate == null || scheduledDate < startDate) //if the scheduledDate is before the start or if the task hasn't started yet
            return scheduledDate + RequiredEffortTime;
        else //then the start is before the scheduledDate
        {
            return startDate + RequiredEffortTime;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idOfTask"></param>
    /// <returns></returns>
    private BO.EngineerInTask? CheckIfEngineerFromTaskIsExist(int idOfTask)
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idOfWantedTask"></param>
    /// <returns></returns>
    private List<TaskInList?> GetDependenciesFromDal(int idOfWantedTask)
    {
        //change like that: all dependencies => only the dependent dependencies => only their id's => their correct tasks => their correct TaskInList
        IEnumerable<DO.Dependency?> listDependencies = _dal.Dependency.ReadAll();//use read func from dal to get details of specific task
        listDependencies = listDependencies.Where(dependency => dependency._dependentTask == idOfWantedTask);
        IEnumerable<int?> listID = listDependencies.Select(dependency => dependency?._id);
        IEnumerable<DO.Task?> listTask = listID.Select(dependencyID => _dal.Task.Read((int)dependencyID));
        IEnumerable<TaskInList?> listTaskInList = listTask.Select(TaskEx => new TaskInList(TaskEx._id, TaskEx._description, TaskEx._alias, (Status)WhatStatus(TaskEx._scheduledDate, TaskEx._startDate, TaskEx._completeDate)));
        return (List<TaskInList?>)listTaskInList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scheduledDate"></param>
    /// <param name="startDate"></param>
    /// <param name="completeDate"></param>
    /// <returns></returns>
    private int WhatStatus(DateTime? scheduledDate, DateTime? startDate, DateTime? completeDate)//////////////////
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

    private DateTime findOptionalDate(int TaskId)
    {
        IEnumerable<DateTime> nullDates = from dep in _dal.Dependency.ReadAll(d => d._dependentTask == TaskId)
                                     let hisTask = _dal.Task.Read((int)dep._dependsOnTask!)
                                     select (DateTime)hisTask._scheduledDate! +  hisTask._requiredEffortTime;
        if(nullDates.Count() == 0)
        {
            return (DateTime)_dal.Project.getStartingDate()!;
        }
        return nullDates.Max();
    }

    public void Delete(int idOfTaskToDelete)
    {
        IEnumerable<DO.Task?> AllDOTasks = _dal.Task.ReadAll();//use read func from dal to get details of all tasks
        DO.Task? check = AllDOTasks.FirstOrDefault(task => idOfTaskToDelete == task._id);
        if (idOfTaskToDelete <= 0)
        {
            throw new BLWrongIdInputException();
        }
        if (check != null)
        {
            throw BLIdNotExist();
        }
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException();
        }
        IEnumerable<DO.Dependency?> AllDODependency = _dal.Dependency.ReadAll();//use read func from dal to get details of all tasks
        DO.Dependency? check2 = AllDODependency.FirstOrDefault(dep => dep._dependsOnTask == idOfTaskToDelete);
        if (check2 != null)
        {
            throw BLcantDeleteBczDependency();
        }
        _dal.Task.Delete(idOfTaskToDelete);
    }

    public BO.Task? Read(int idOfWantedTask)
    {
        DO.Task? doTask = _dal.Task.Read(idOfWantedTask);//use read func from dal to get details of specific task
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={idOfWantedTask} does Not exist");

        return MakeBOFromDoTASK(doTask);
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
        IEnumerable<TaskInList?> TasksInList = AllBOTasks.Select((BOtaskInList => new TaskInList(BOtaskInList.Id, BOtaskInList.Description, BOtaskInList.Alias, BOtaskInList.Status)));//make to task in list to return properly
        return TasksInList;
    }

    public void Update(BO.Task? item)
    {
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException();
        }
        IEnumerable<DO.Task?> AllDOTasks = _dal.Task.ReadAll();
        DO.Task? check = AllDOTasks.FirstOrDefault(task => item.Id == task._id);
        if (check != null)
        {
            throw BLItemNotExist();
        }
        if(item.Alias=="")
        {
            throw new drgd();
        }
        DO.Task? doTask = BOToDOTask(item);
        _dal.Task.Update(doTask);
    }

    public void AutoScedule(DateTime startingDate)
    {
        _dal.Project.setStartingDate(startingDate); //saving the new starting date
        _dal.Project.setProjectStatus((int)BO.ProjectStatus.Sceduling); //we are now at the Sceduling stage

        IEnumerable<int> basicTasks = from task in _dal.Task.ReadAll()     //all the task
                                      where !((_dal.Dependency.ReadAll(d => d._dependentTask == task._id)).Any()) //all the task that nothing depends on them
                                      select task._id; //all their ids

        foreach (int taskId in basicTasks)
        {
            autoSceduleTask(taskId, startingDate);
        }

        _dal.Project.setProjectStatus((int)BO.ProjectStatus.Execution); //we are now at the Execution stage
    }

    public void AddDependency(int dependentTask, int DependsOnTask)
    {
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException();
        }
        _dal.Dependency.Create(new DO.Dependency(dependentTask, DependsOnTask));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idOfTask"></param>
    /// <param name="wantedTime"></param>
    /// <param name="isConfirmed">if the manager still want his date, even that his date isnt the earliest. if the method is being called from one of the throws=T, if just sent from GUI=F</param>
    /// <exception cref="BLCannotSceduleOneException"></exception>
    /// <exception cref="BLCannotSceduleMoreThanOneException"></exception>
    /// <exception cref="BLToEarlySuggestOptional"></exception>
    /// <exception cref="BLSuggestOptional"></exception>
    public void ManualScedule(int idOfTask,DateTime wantedTime,bool isConfirmed)
    {
        if (isConfirmed == false)
        {
            if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Sceduling)//if thats the first time we using this so get project status from "PLANNING" to "SCEDULING"
                _dal.Project.setProjectStatus(2);
            IEnumerable<int> nullDates = from dep in _dal.Dependency.ReadAll(d => d._dependentTask == idOfTask)
                                         let hisTask = _dal.Task.Read((int)dep._dependsOnTask!)
                                         where hisTask._scheduledDate == null
                                         select hisTask._id;
            if (nullDates.Count() == 1)
            {
                throw new BLCannotSceduleOneException(nullDates.FirstOrDefault());
            }
            if (nullDates.Count() > 1)
            {
                throw new BLCannotSceduleMoreThanOneException(nullDates.Count());
            }
            DateTime optionalDate = findOptionalDate(idOfTask);
            if (optionalDate > wantedTime) //the time the manager want to start is too soon
            {
                throw new BLToEarlySuggestOptional(optionalDate);
            }
            if (optionalDate < wantedTime)
            {
                throw new BLSuggestOptional(optionalDate);
            }
        }
        DO.Task updatedTask = _dal.Task.Read(idOfTask) with { _scheduledDate = wantedTime };
        _dal.Task.Update(updatedTask);
    }
}


