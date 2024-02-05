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
    /// helping method that gets from the dal-config xml file the current progect status
    /// </summary>
    /// <returns>the current status of the project</returns>
    private BO.ProjectStatus getProjectStatus()
    {
        XElement configRoot = XElement.Load(@"..\xml\data-config.xml");
        return (BO.ProjectStatus)int.Parse(configRoot.Element("project-stage")!.Value);
    }

    /// <summary>
    /// helping method that change the dal-config xml file into the current progect status
    /// </summary>
    private void setProjectStatus(int newStatus)
    {
        XElement configRoot = XElement.Load(@"..\xml\data-config.xml"); //get the previous root
        
        configRoot.Element("project-stage")!.Value = String.Format("{0}", newStatus); //change the field of the projectStatus
        
        configRoot.Save(@"..\xml\data-config.xml"); //save it
    }

    /// <summary>
    /// helping method that change the dal-config xml file into the current progect starting time
    /// </summary>
    private void saveStartingDate(DateTime start)
    {
        XElement configRoot = XElement.Load(@"..\xml\data-config.xml"); //get the previous root
        
        XElement theTime = new XElement("project-starting-date", start); //create new tag of the starting date
        
        configRoot.Element("project-stage")!.Add(theTime); //create the field of the starting date (he did not exist untill now)
        
        configRoot.Save(@"..\xml\data-config.xml"); //save it
    }

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

    public int Create(BO.Task? newTask)//Check all input, add dependencies to ,cast to DO,then use do.create
    {
        if(newTask.Id<=0)
            throw new NotImplementedException();

        if ((int)getProjectStatus() != 1)
        {
            throw new BLWrongStageException();
        }

        if (newTask.Alias=="")
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
            _complexity = (ComplexityLvls)newTask.Complexity,
            _engineerId = newTask.Engineer.Id,
            _isActive = true
        };
    }

    private BO.Task? MakeBOFromDoTASK(DO.Task? doTask)
    {
        IEnumerable<DO.Task?> AllDOTasks = _dal.Task.ReadAll();//use read func from dal to get details of all tasks
        int? check = AllDOTasks.FirstOrDefault(task => idOfTaskToDelete == task._id);
        if (check != null)
        {
            throw BLIdNotExist();
        }
        if ((int)getProjectStatus() != 1)
        {
            throw new BLWrongStageException();
        }
        IEnumerable<DO.Dependency?> AllDODependency = _dal.Dependency.ReadAll();//use read func from dal to get details of all tasks
        int? check2 = AllDODependency.FirstOrDefault(dep => dep._dependsOnTask/**/== idOfTaskToDelete);
        if(check2!=null)
        {
            throw BLcantDeleteBczDependency();
        }
        _dal.Task.Delete(idOfTaskToDelete);
    }

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

    private List<TaskInList?> CheckDependenciesFromDal(int idOfWantedTask)
    {
        //change like that: all dependencies => only the dependent dependencies => only their id's => their correct tasks => their correct TaskInList
        IEnumerable<DO.Dependency?> listDependencies = _dal.Dependency.ReadAll();//use read func from dal to get details of specific task
        listDependencies = listDependencies.Where(dependency => dependency._dependentTask == idOfWantedTask);
        IEnumerable<int?> listID = listDependencies.Select(dependency => dependency?._id);
        IEnumerable<DO.Task?> listTask = listID.Select(dependencyID => _dal.Task.Read((int)dependencyID));
        IEnumerable<TaskInList?> listTaskInList = listTask.Select(TaskEx => new TaskInList(TaskEx._id, TaskEx._description, TaskEx._alias, (Status)WhatStatus(TaskEx._scheduledDate, TaskEx._startDate, TaskEx._completeDate)));
        return (List<TaskInList?>)listTaskInList;
    }

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

    public void Delete(int idOfTaskToDelete)
    {
        IEnumerable<DO.Task?> AllDOTasks = _dal.Task.ReadAll();//use read func from dal to get details of all tasks
        DO.Task? check = AllDOTasks.FirstOrDefault(task => idOfTaskToDelete == task._id);
        if (check != null)
        {
            throw BLIdNotExist();
        }
        if ((int)getProjectStatus() != 1)
        {
            throw new BLWrongStageException();
        }
        IEnumerable<DO.Dependency?> AllDODependency = _dal.Dependency.ReadAll();//use read func from dal to get details of all tasks
        DO.Dependency? check2 = AllDODependency.FirstOrDefault(dep => dep._dependsOnTask/**/== idOfTaskToDelete);
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
        if ((int)getProjectStatus() != 1)
        {
            throw new BLWrongStageException();
        }
    }

    public void AutoScedule(DateTime startingDate)
    {
        saveStartingDate(startingDate); //saving the new starting date
        setProjectStatus((int)BO.ProjectStatus.Sceduling); //we are now at the Sceduling stage

        IEnumerable<int> basicTasks = from task in _dal.Task.ReadAll()     //all the task
                                      where !((_dal.Dependency.ReadAll(d => d._dependentTask == task._id)).Any()) //all the task that nothing depends on them
                                      select task._id; //all their ids

        foreach (int taskId in basicTasks)
        {
            autoSceduleTask(taskId, startingDate);
        }

        setProjectStatus((int)BO.ProjectStatus.Execution); //we are now at the Execution stage
    }

    public void AddDependency(int dependentTask, int DependsOnTask)
    {
        throw new NotImplementedException();
    }
}

