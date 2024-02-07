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
    public bool checkCircularDependency(int dependent, int dependOn)
    {
        IEnumerable<int> dependents = from dep in _dal.Dependency.ReadAll(d => d._dependsOnTask == dependent) //get all the tasks that depends on our dependent task
                                      select (int)dep._dependentTask!;
        if(dependents.Count(id => id==dependOn) != 0) //if our dependent on task is in there - then wev'e got circular  dependency
        {
            return true;
        }
        foreach (int id in dependents) //search for the circular dependency in the dependent tasks
        {
            if (checkCircularDependency(id, dependOn) == true)
            {
                return true;
            }
        }
        return false;

    }

    public int Create(BO.Task newTask)//Check all input, add dependencies to ,cast to DO,then use do.create
    {
        if(newTask.Id<=0)
            throw new BLWrongIdException();

        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException((int)_dal.Project.getProjectStatus(), 1);
        }

        if (newTask.Alias == "")
        {
            throw new BLWrongAliasException();
        }
        DO.Task? doTaskToCheck = _dal.Task.Read(newTask.Id);//get task to check if exist
        if(doTaskToCheck != null)
            throw new NotImplementedException();
        foreach (var dep in newTask.Dependencies){ AddDependency(newTask.Id, dep!.Id); }
        DO.Task? doTaskToCreate = BOToDOTask(newTask);
        _dal.Task.Create(doTaskToCreate!);
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
            Engineer= CheckIfEngineerFromTaskIsExist(doTask._engineerId),
            Complexity=(BO.EngineerExperience)doTask._complexity
        };
    }

    /// <summary>
    /// helping method to calculate the forecast field 
    /// </summary>
    /// <param name="scheduledDate"></param>
    /// <param name="startDate"></param>
    /// <param name="RequiredEffortTime"></param>
    /// <returns>forecast field </returns>
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
    /// check if engineerInTask actually exist. if it does so return it, else return null.
    /// </summary>
    /// <param name="idOfTask"></param>
    /// <returns>engineer or null</returns>
    private BO.EngineerInTask? CheckIfEngineerFromTaskIsExist(int? idOfEngineer)
    {
        if (idOfEngineer == null)
        {
            return null; 
        }
        DO.Engineer? doEngineer = _dal.Engineer.Read((int)idOfEngineer!);//use read func from dal to get details of specific task
        if(doEngineer == null)
        {
            throw new BLNotFoundException("task", (int)idOfEngineer!);
        }
        BO.EngineerInTask eng = new BO.EngineerInTask() { Id = (int)doEngineer!._id, Name = doEngineer._name };
        return eng;
    }

    /// <summary>
    /// return the whole dependency list from the dal section
    /// </summary>
    /// <param name="idOfWantedTask"></param>
    /// <returns>dependency list</returns>
    private List<TaskInList> GetDependenciesFromDal(int idOfWantedTask)
    {
        return (from dep in _dal.Dependency.ReadAll(d => d._dependentTask == idOfWantedTask)
               let theTask = _dal.Task.Read((int)dep._dependsOnTask!)
               select new TaskInList(theTask._id, theTask._description, theTask._alias, (BO.Status)WhatStatus(theTask._scheduledDate, theTask._startDate, theTask._completeDate))).ToList();

    }

    /// <summary>
    /// helping method to calcualte the status of specific task
    /// </summary>
    /// <param name="scheduledDate"></param>
    /// <param name="startDate"></param>
    /// <param name="completeDate"></param>
    /// <returns>int</returns>
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
    /// <summary>
    /// this method calculates the best date for task to happan, returns it.
    /// </summary>
    /// <param name="TaskId"></param>
    /// <returns>date time </returns>
    private DateTime findOptionalDate(int TaskId)
    {
        IEnumerable<DateTime> formerDates = from dep in _dal.Dependency.ReadAll(d => d._dependentTask == TaskId)
                                     let hisTask = _dal.Task.Read((int)dep._dependsOnTask!)
                                     select (DateTime)hisTask._scheduledDate! +  hisTask._requiredEffortTime;
        if(formerDates.Count() == 0)
        {
            return (DateTime)_dal.Project.getStartingDate()!;
        }
        return formerDates.Max();
    }
    public void Delete(int idOfTaskToDelete)
    {
        IEnumerable<DO.Task> AllDOTasks = _dal.Task.ReadAll();//use read func from dal to get details of all tasks
        DO.Task? check = AllDOTasks.FirstOrDefault(task => idOfTaskToDelete == task._id);
        if (idOfTaskToDelete <= 0)
        {
            throw new BLWrongIdException();
        }
        if (check == null)
        {
            throw new BLNotFoundException("Task", idOfTaskToDelete);
        }
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException(_dal.Project.getProjectStatus(),(int)BO.ProjectStatus.Planning);
        }
        IEnumerable<DO.Dependency> AllDODependency = _dal.Dependency.ReadAll();//use read func from dal to get details of all tasks
        DO.Dependency? check2 = AllDODependency.FirstOrDefault(dep => dep._dependsOnTask == idOfTaskToDelete);
        if (check2 != null)
        {
            throw new BLCannotDeleteHasDependencyException(idOfTaskToDelete);
        }
        _dal.Task.Delete(idOfTaskToDelete);
        IEnumerable<DO.Dependency> filteredIEN = _dal.Dependency.ReadAll(cond => cond._dependentTask==idOfTaskToDelete);
        foreach (var dep in filteredIEN)
            _dal.Dependency.Delete(dep._id);
    }
    public BO.Task Read(int idOfWantedTask)
    {
        DO.Task? doTask = _dal.Task.Read(idOfWantedTask);//use read func from dal to get details of specific task
        if (doTask == null)
            throw new BLNotFoundException("Task",idOfWantedTask);

        return MakeBOFromDoTASK(doTask);
    }
    public BO.Task? Read(Func<BO.Task?, bool> filter)
    {
        IEnumerable<DO.Task?> AllDOTasks = _dal.Task.ReadAll();//use read func from dal to get details of all tasks
        if (AllDOTasks == null)
            throw new BLEmptyDatabaseException();
        IEnumerable<BO.Task?> AllBOTasks = AllDOTasks.Select(DOTtaskInList => MakeBOFromDoTASK(DOTtaskInList!));
        BO.Task? chosen= AllBOTasks.FirstOrDefault(filter);//FILTER
        return chosen;
    }
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task?, bool>? filter = null)
    {
        IEnumerable<DO.Task> AllDOTasks = _dal.Task.ReadAll();//use read func from dal to get details of all tasks
        if (AllDOTasks == null)
            throw new BLEmptyDatabaseException();
        IEnumerable<BO.Task?> AllBOTasks = AllDOTasks.Select(DOTtaskInList => MakeBOFromDoTASK(DOTtaskInList));
        if (filter != null)
        {
            AllBOTasks = AllBOTasks.Where(TaskEx => filter(TaskEx));//FILTER
        }
        IEnumerable<TaskInList> TasksInList = AllBOTasks.Select((BOtaskInList => new TaskInList(BOtaskInList!.Id, BOtaskInList.Description, BOtaskInList.Alias, BOtaskInList.Status)));//make to task in list to return properly
        return TasksInList;
    }
    public void Update(BO.Task item)
    {
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException(_dal.Project.getProjectStatus(), (int)BO.ProjectStatus.Planning);
        }
        IEnumerable<DO.Task> AllDOTasks = _dal.Task.ReadAll();
        DO.Task? check = AllDOTasks.FirstOrDefault(task => item.Id == task._id);
        if (check == null)
        {
            throw new BLNotFoundException("Task",item.Id);
        }
        if(item.Alias=="")
        {
            throw new BLWrongAliasException();
        }
        DO.Task? doTask = BOToDOTask(item);
        _dal.Task.Update(doTask!);
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
    public void AddDependency(int dependentTask, int dependsOnTask)
    {
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException((int)_dal.Project.getProjectStatus(), 1);
        }
        if(checkCircularDependency(dependentTask, dependsOnTask) == true)
        {
            throw new BLCannotAddCircularDependencyException(dependentTask, dependsOnTask);
        }
        _dal.Dependency.Create(new DO.Dependency(dependentTask, dependsOnTask));
    }
    public void ManualScedule(int idOfTask, DateTime wantedTime, bool isConfirmed)
    {
        if (isConfirmed == false)
        {
            if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Sceduling)//if we are in the wrong stage
                throw new BLWrongStageException(_dal.Project.getProjectStatus(), (int)BO.ProjectStatus.Sceduling);
            IEnumerable<int> nullDates = from dep in _dal.Dependency.ReadAll(d => d._dependentTask == idOfTask)
                                         let hisTask = _dal.Task.Read((int)dep._dependsOnTask!)
                                         where hisTask._scheduledDate == null
                                         select hisTask._id;
            if (nullDates.Count() == 1)
            {
                throw new BLCannotScheduleOneFormerUnscheduledException(nullDates.FirstOrDefault());
            }
            if (nullDates.Count() > 1)
            {
                throw new BLCannotScheduleMoreTanOneFormerUnscheduledException(nullDates.Count());
            }
            DateTime optionalDate = findOptionalDate(idOfTask);
            if (optionalDate > wantedTime) //the time the manager want to start is too soon
            {
                throw new BLTooEarlyException(optionalDate);
            }
            if (optionalDate < wantedTime)
            {
                throw new BLDateSuggestionException(optionalDate);
            }
        }
        DO.Task updatedTask = _dal.Task.Read(idOfTask)! with { _scheduledDate = wantedTime };
        _dal.Task.Update(updatedTask);
        if(_dal.Task.ReadAll(t=>t._scheduledDate == null).Count() == 0)
        {
            _dal.Project.setProjectStatus((int)BO.ProjectStatus.Execution);
        }
    }
    public void StartSchedule(DateTime StartingDateOfProject)
    {
        if((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)// this method can be acceced only in the planning stage
        {
            throw new BLWrongStageException(_dal.Project.getProjectStatus(), (int)BO.ProjectStatus.Planning);
        }
        _dal.Project.setProjectStatus((int)BO.ProjectStatus.Sceduling);
        _dal.Project.setStartingDate(StartingDateOfProject);
    }
}


