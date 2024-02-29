namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class TaskImplementation : BlApi.ITask
{
    private readonly IBl _bl;
    internal TaskImplementation(IBl bl) => _bl = bl;

    private DalApi.IDal _dal = DalApi.Factory.Get;//with this field we can access to the methods of task in dal. the way to use the method is being chosen inside factory.
    /// <summary>
    /// helping method to create new taskInList
    /// </summary>
    /// <param name="task"> DO task </param>
    /// <returns> the task as TaskInList </returns>
    private BO.TaskInList MakeTaskInList(DO.Task task)
    {
        return new TaskInList(task._id, task._description, task._alias, (BO.Status)WhatStatus(task._scheduledDate, task._startDate, task._completeDate));
    }

    /// <summary>
    /// reqursive helping method for the AutoScedule method
    /// </summary>
    /// <param name="taskId"> the task we want to scedule </param>
    /// <param name="formerForcastDate"> the forcast date from the task that was before him in the reqursive method </param>
    private void autoSceduleTask(int taskId, DateTime formerForcastDate)
    {
        DO.Task theTask = _dal.Task.Read(taskId)!;
        if (theTask._scheduledDate == null || formerForcastDate > theTask._scheduledDate) 
        {
            DO.Task sceduledTask;
            sceduledTask = theTask with { _scheduledDate = formerForcastDate}; //when the new suggested date is later the previous one or when there is no previous one
            _dal.Task.Update(sceduledTask);
            foreach (var dep in _dal.Dependency.ReadAll(d => d._dependsOnTask == taskId)) //change all the task that depends on him
            {
                autoSceduleTask((int)dep!._dependentTask!, (DateTime)(sceduledTask._scheduledDate + sceduledTask._requiredEffortTime!));
            }
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
        DO.Task? doTaskToCreate = BOToDOTask(newTask);
        int newId = _dal.Task.Create(doTaskToCreate!);
        try
        { //try to insert all the dependencies
            foreach (var dep in newTask.Dependencies!) { AddDependency(newId, dep!.Id); }
        }
        catch(Exception ex) //we couldn't add the dependency - so now we will erase all the dependencied we added and delete the new task
        {
            foreach (var dep in _dal.Dependency.ReadAll(d => d._dependsOnTask == newId)!) //delete all the dependencied we added
            {
                _dal.Dependency.Delete(dep._id);
            }
            _dal.Dependency.Delete(newId); //delete the new Task
            throw ex;
        }
        return newId;
    }

    /// <summary>
    /// helping method that converts BO.Task to DO.Task
    /// </summary>
    /// <param name="newTask"> BO.Task </param>
    /// <returns> DO.Task </returns>
    private DO.Task BOToDOTask(BO.Task newTask)
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
            _engineerId = (newTask.Engineer != null) ? newTask.Engineer.Id : null,
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
        return new BO.Task(
        
            doTask._id,
            doTask._description,
            doTask._alias,
            doTask._createdAtDate,
            (Status?)WhatStatus(doTask._scheduledDate,doTask._startDate,doTask._completeDate), //inducate his status
            GetDependenciesFromDal(doTask._id), //all his dependencies
            null,
            doTask._requiredEffortTime,
            doTask._startDate,
            doTask._scheduledDate,
            ForecastCalc(doTask._scheduledDate, doTask._startDate, doTask._requiredEffortTime),
            doTask._deadlineDate,
            doTask._completeDate,
            doTask._deliverables,
            doTask._remarks,
            CheckIfEngineerFromTaskIsExist(doTask._engineerId),
            (BO.EngineerExperience)doTask._complexity
        );
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
               select MakeTaskInList(theTask)).ToList();

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
        if (startDate == null)//Scheduled
        { return 1; }
        if (completeDate == null)//OnTrack
        { return 2; }
        else
        { return 4; }//done
        //IF WE WANT TAKE CARE OF JEOPARDY 
    }


    public DateTime findOptionalDate(int TaskId)
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
        DO.Task? check = _dal.Task.Read(idOfTaskToDelete);
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
        try
        {
            _dal.Task.Delete(idOfTaskToDelete);
        }
        catch(BLNotFoundException ex)
        {
            throw new BLNotFoundException("Task", idOfTaskToDelete);
        }
        IEnumerable<DO.Dependency> filteredIEN = _dal.Dependency.ReadAll(cond => cond._dependentTask==idOfTaskToDelete);
        foreach (var dep in filteredIEN)
            _dal.Dependency.Delete(dep._id); //delete all his dependencies
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
        return TasksInList.OrderBy(t=>t.Id);
    }
    public void Update(BO.Task item)
    {
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
    public void AutoScedule()
    {
        if((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Sceduling)
        {
            throw new BO.BLWrongStageException(_dal.Project.getProjectStatus(), (int)BO.ProjectStatus.Sceduling);
        }
        DateTime startingDate = (DateTime)_dal.Project.getStartingDate()!;
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
        if(dependsOnTask == dependentTask)
        {
            throw new BLCannotAddTheIdentityDependency();
        }
        if(_dal.Task.Read(dependsOnTask) == null)
        {
            throw new BLNotFoundException("task", dependsOnTask);
        }
        if (_dal.Task.Read(dependentTask) == null)
        {
            throw new BLNotFoundException("task", dependentTask);

        }
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException((int)_dal.Project.getProjectStatus(), 1);
        }
        if(checkCircularDependency(dependentTask, dependsOnTask) == true)
        {
            throw new BLCannotAddCircularDependencyException(dependentTask, dependsOnTask);
        }
        if(_dal.Dependency.Read(d=> d._dependsOnTask == dependsOnTask && d._dependentTask == dependentTask) != null)
        {
            throw new BLAlreadyExistException("dependency", _dal.Dependency.Read(d => d._dependsOnTask == dependsOnTask && d._dependentTask == dependentTask)!._id);
        }
        _dal.Dependency.Create(new DO.Dependency(0,dependentTask, dependsOnTask));
    }

    public void DeleteDependency(int dependentTask, int dependsOnTask)
    {
        if (_dal.Task.Read(dependsOnTask) == null)
        {
            throw new BLNotFoundException("task", dependsOnTask);
        }
        if (_dal.Task.Read(dependentTask) == null)
        {
            throw new BLNotFoundException("task", dependentTask);

        }
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException((int)_dal.Project.getProjectStatus(), 1);
        }
        DO.Dependency? dep = _dal.Dependency.Read(d => d._dependsOnTask == dependsOnTask && d._dependentTask == dependentTask);
        if (dep == null)
        {
            throw new BLNotFoundException("dependency", -1);
        }
        _dal.Dependency.Delete(dep._id);
    }


    public void ManualScedule(int idOfTask, DateTime wantedTime)
    {

        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Sceduling)//if we are in the wrong stage
            throw new BLWrongStageException(_dal.Project.getProjectStatus(), (int)BO.ProjectStatus.Sceduling);

       
        DateTime optionalDate = findOptionalDate(idOfTask); //just to see if somehow we entered wrong time
        if (optionalDate > wantedTime) //the time the manager want to start is too soon
        {
            throw new BLTooEarlyException(optionalDate);
        }
        DO.Task updatedTask = _dal.Task.Read(idOfTask)! with { _scheduledDate = wantedTime };
        _dal.Task.Update(updatedTask);
        if(_dal.Task.ReadAll(t=>t._scheduledDate == null).Count() == 0) //if all the tasks has been updated:
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

    public int getProjectStatus()
    {
        return _dal.Project.getProjectStatus();
    }

    public DateTime? getStartingDate()
    {
        return _dal.Project.getStartingDate();
    }
    /// <summary>
    /// this method helps us to handle with dependencies in the pl stage.
    /// </summary>
    /// <returns>all the id's of the tasks</returns>
    public List<int> GetAllTasks()
    {
        List<int> idList = new List<int>();
        List<DO.Task> TasksList = new List<DO.Task>();
        TasksList = _dal.Task.ReadAll().ToList();
        foreach (var _task in TasksList)
        {
            idList.Add(_task._id);
        }
        return idList;
    }


    public int AddDependencies(List<int> dependendsOns, int dependentId, int endIndex)
    {
        int i = 0;
        for (; i < endIndex; i++)
        {
            try
            {
                AddDependency(dependentId, dependendsOns[i]);
            }
            catch (Exception)
            {
                return i;
            }
        }
        return i;
    }


    public int DeleteDependencies(List<int> dependendsOns, int dependentId, int endIndex)
    {
        int i = 0;
        for (; i < endIndex; i++)
        {
            try
            {
                DeleteDependency(dependentId, dependendsOns[i]);
            }
            catch (Exception)
            {
                return i;
            }
        }
        return i;
    }

    public void CheckPreviousTasks(int taskId)
    {
        IEnumerable<int> nullDates = from dep in _dal.Dependency.ReadAll(d => d._dependentTask == taskId)
                                     let hisTask = _dal.Task.Read((int)dep._dependsOnTask!)
                                     where hisTask._scheduledDate == null
                                     select hisTask._id;
        if (nullDates.Count() >= 1) //if the other task didn't even scheduled:
        {
            string ids = "";
            foreach (int i in nullDates) { ids += i.ToString() + ", "; }  //all the ids of the dependent tasks
            ids = ids.Substring(0, ids.Length - 2); //delete the two last letters
            ids += ".";
            throw new BLCannotScheduleMoreOrOneFormerUnscheduledException(ids);
        }
    }

    public double GetPrecentage(int TaskId)
    { 
        DateTime currentTime = _bl.Clock!; //the time is not null right now because this function will be called only in the execution stage
        DO.Task theTask = _dal.Task.Read(TaskId)!;
        if (theTask._completeDate != null) //then it finished
        {
            return 2;
        }
        DateTime scheduledTime = (DateTime)theTask._scheduledDate!;//the scheduled time is not null right now because this function will be called only in the execution stage
        if (currentTime < scheduledTime) //if the task was nott supposed to start
        {
            return -1;
        }
        DateTime forcastTime = scheduledTime + theTask._requiredEffortTime;
        if (currentTime > forcastTime) //the task was supposed to end
        {
            return 1;
        }
        double requiredDays = (theTask._requiredEffortTime).Days, passedDays = (currentTime - scheduledTime).Days;
        return passedDays / requiredDays;  //days that has past / days that the task has
    }

    public List<BO.TaskInList> ReadAllByDependencies()
    {
        List<BO.TaskInList> sortedList = new List<BO.TaskInList>();
        List<BO.TaskInList> tempTasks = (from task in _dal.Task.ReadAll()     //all the task
                                         where !((_dal.Dependency.ReadAll(d => d._dependentTask == task._id)).Any()) //all the task that nothing depends on them
                                         select MakeTaskInList(task)).ToList(); //get all the 'basic' tasks
        BO.TaskInList temp;
        while(tempTasks.Count > 0)
        {
            temp = tempTasks[0];
            tempTasks.RemoveAt(0);
            if (!sortedList.Any(t => t.Id == temp.Id)) //if we haven't handlled this task before 
            {
                bool flag = true;
                foreach(var dependsOnTask in GetDependenciesFromDal(temp.Id)) { //check if all the previous tasks has alredy entered to the list
                    if (!sortedList.Any(t=>t.Id == dependsOnTask.Id)) //if we haven't handeled this task alredy - then it should be handled before
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag == true)
                {
                    //add all the tasks that depends on him to the 'handle' array
                    tempTasks.AddRange((from DO.Dependency dep in _dal.Dependency.ReadAll(d => d._dependsOnTask == temp.Id)
                                        select MakeTaskInList(_dal.Task.Read((int)dep._dependentTask!)!)).ToList());
                    sortedList.Add(temp);
                }
            }
        }
        return sortedList;
    }
}


