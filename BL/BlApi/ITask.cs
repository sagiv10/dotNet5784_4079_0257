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
    /// <param name="isConfirmed">if the manager still want his date, even that his date isnt the earliest. if the method is being called from one of the throws=T, if just sent from GUI=F</param>
    /// <exception cref="BO.BLCannotScheduleException"></exception>
    /// <exception cref="BO.BLTooEarlyException"></exception>
    /// <exception cref="BO.BLWrongStageException"></exception>
    /// <exception cref="BO.BLDateSuggestionException"></exception>
    public void ManualScedule(int idOfTask, DateTime wantedTime, bool isConfirmed);

    /// <summary>
    /// tihs method help us to add dependencies of new task that just been created 
    /// </summary>
    /// <param name="dependentTask"></param>
    /// <param name="dependsOnTask"></param>
    /// <exception cref="BO.BLWrongStageException"></exception>
    /// <exception cref="BO.BLCannotAddCircularDependencyException"></exception>
    /// <exception cref="BO.BLNotFoundException"></exception>
    /// <exception cref="BO.BLAlreadyExistException"></exception>
    public void AddDependency(int dependentTask, int DependsOnTask);

    /// <summary>
    /// tihs method help us to delete dependencies 
    /// </summary>
    /// <param name="dependentTask"></param>
    /// <param name="dependsOnTask"></param>
    /// <exception cref="BO.BLWrongStageException"></exception>
    /// <exception cref="BO.BLNotFoundException"></exception>
    public void DeleteDependency(int dependentTask, int DependsOnTask);


    /// <summary>
    /// this method help us to change the status of the project from Planning to Scheduling. (2 to 3)
    /// </summary>
    /// <param name="StartingDateOfProject"></param>
    /// <exception cref="BLWrongStageException"></exception>
    public void StartSchedule(DateTime StartingDateOfProject);

    /// <summary>
    /// helping method that gets the current progect status
    /// </summary>
    /// <returns>the current status of the project</returns>
    public int getProjectStatus();

    /// <summary>
    /// helping method that get the current progect starting time
    /// </summary>
    public DateTime? getStartingDate();
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
    /// helping method that delete the list of dependenciecies to the marked value. if an error accures then stop the deleting
    /// </summary>
    /// <param name="dependendsOns"> list of depends on ids</param>
    /// <param name="dependentId"> the dependent task</param>
    /// <returns> untill where the method deleted </returns>
    public int DeleteDependencies(List<int> dependendsOns, int dependentId, int endIndex);

    /// <summary>
    /// helping method that adds the list of dependenciecies to the marked value. if an error accures then stop the adding
    /// </summary>
    /// <param name="dependendsOns"> list of depends on ids</param>
    /// <param name="dependentId"> the dependent task</param>
    /// <returns> untill where the method added </returns>
    public int AddDependencies(List<int> dependendsOns, int dependentId, int endIndex);
}
