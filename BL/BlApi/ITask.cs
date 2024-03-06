namespace BlApi;

/// <summary>
/// the tasks access interface for the buissness layer
/// </summary>
public interface ITask
{

    /// <summary>
    /// this method creats new Task in the logic section by using a task from the do section
    /// </summary>
    /// <param name="newTask"></param>
    /// <returns>id of the new bo task</returns>
    /// <exception cref="BO.BLWrongIdException"></exception>
    /// <exception cref="BO.BLWrongAliasException"></exception>
    /// <exception cref="BO.BLWrongStageException"></exception>
    /// <exception cref="BO.BLEmptyDatabaseException"></exception>
    public int Create(BO.Task newTask);
    /// <summary>
    /// gets id of task and returnes bo task 
    /// </summary>
    /// <param name="idOfWantedTask"></param>
    /// <returns>bo task </returns>
    /// <exception cref="BO.BLNotFoundException"></exception>
    public BO.Task Read(int idOfWantedTask); //getting the details of specific task 
    /// <summary>
    /// get filter of task and returnes the first task that anwsers this filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>bo task</returns>
    /// <exception cref="BLEmptyDatabaseException"></exception>
    public BO.Task Read(Func<BO.Task, bool> filter); //getting the details of task that the filter accept
    /// <summary>
    /// this method returnes a TaskInList list of all the tasks that anwsers specifc filter.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>list of TaskInList</returns>
    /// <exception cref="BO.BLEmptyDatabaseException"></exception>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null); //returns the task list from dal
    /// <summary>
    /// this method gets bo task and updates an existing task using the update from do 
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="BO.BLWrongStageException"></exception>
    /// <exception cref="BO.BLNotFoundException"></exception>
    /// <exception cref="BO.BLWrongAliasException"></exception>
    public void Update(BO.Task item); //Updates task
    /// <summary>
    /// this method deletes specific task from the logic section using the delete from dal section 
    /// </summary>
    /// <param name="idOfTaskToDelete"></param>
    /// <exception cref="BO.BLWrongIdException"></exception>
    /// <exception cref="BO.BLNotFoundException"></exception>
    /// <exception cref="BO.BLCannotDeleteHasDependencyException"></exception>
    public void Delete(int idOfTaskToDelete); //Deletes an task by its Id

    /// <summary>
    /// this method automaticly calculate valid dates for all the tasks existing 
    /// </summary>
    /// <param name="startingDate"></param>
    /// <exception cref="BO.BLWrongStageException"></exception>
    public void AutoScedule();
    /// <summary>
    /// this method sets up a schedule date together with the manager of the project. provide him all the information he needs in the time he sets up times to the tasks 
    /// </summary>
    /// <param name="idOfTask"></param>
    /// <param name="wantedTime"></param>
    /// <exception cref="BO.BLTooEarlyException"></exception>
    /// <exception cref="BO.BLWrongStageException"></exception>
    public void ManualScedule(int idOfTask, DateTime wantedTime);

    /// <summary>
    /// help method for the pl stage that returnes all the id's
    /// </summary>
    /// <returns>list that contatin all id's</returns>
    public List<int> GetAllTasks();

    /// <summary>
    /// this method calculates the best date for task to happen and returns it.
    /// </summary>
    /// <param name="TaskId"></param>
    /// <returns>date time </returns>
    public DateTime findOptionalDate(int TaskId);


    /// <summary>
    /// helping method that checks if all the previous tasks was alredy scheduled. if not then it will throw the right exception
    /// </summary>
    /// <param name="taskId"> the task</param>
    /// <returns> untill where the method added </returns>
    public void CheckPreviousTasks(int taskId);

    /// <summary>
    /// for the gunt - get the precentage of the time has past since the task has started - if the task's scheduled task hasn't happened yet the method will return 0
    /// </summary>
    /// <param name="TaskId"> the task's id </param>
    /// <returns>the precentage of the time has past</returns>
    public double GetPrecentage(int TaskId);

    /// <summary>
    /// read all the tasks by the dependencies order  - for the gunt
    /// </summary>
    /// <returns> the 'sorted' array</returns>
    public List<BO.TaskInList> ReadAllByDependencies();

  
}
