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
    /// <exception cref="BLWrongIdException"></exception>
    /// <exception cref="BLWrongStageException"></exception>
    /// <exception cref="BLWrongAliasException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public int Create(BO.Task newTask);
    /// <summary>
    /// gets id of task and returnes bo task 
    /// </summary>
    /// <param name="idOfWantedTask"></param>
    /// <returns>bo task </returns>
    /// <exception cref="BLNotFoundException"></exception>
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
    /// <exception cref="BLEmptyDatabaseException"></exception>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null); //returns the task list from dal
    /// <summary>
    /// this method gets bo task and updates an existing task using the update from do 
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="BLWrongStageException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    /// <exception cref="BLWrongAliasException"></exception>
    public void Update(BO.Task item); //Updates task
    /// <summary>
    /// this method deletes specific task from the logic section using the delete from dal section 
    /// </summary>
    /// <param name="idOfTaskToDelete"></param>
    /// <exception cref="BLWrongIdException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    /// <exception cref="BLWrongStageException"></exception>
    /// <exception cref="BLCannotDeleteHasDependencyException"></exception>
    public void Delete(int idOfTaskToDelete); //Deletes an task by its Id

    /// <summary>
    /// this method automaticly calculate valid dates for all the tasks existing 
    /// </summary>
    /// <param name="startingDate"></param>
    public void AutoScedule(DateTime startingDate);
    /// <summary>
    /// this method sets up a schedule date together with the manager of the project. provide him all the information he needs in the time he sets up times to the tasks 
    /// </summary>
    /// <param name="idOfTask"></param>
    /// <param name="wantedTime"></param>
    /// <param name="isConfirmed">if the manager still want his date, even that his date isnt the earliest. if the method is being called from one of the throws=T, if just sent from GUI=F</param>
    /// <exception cref="BLCannotSceduleOneException"></exception>
    /// <exception cref="BLCannotSceduleMoreThanOneException"></exception>
    /// <exception cref="BLToEarlySuggestOptional"></exception>
    /// <exception cref="BLSuggestOptional"></exception>
    public void ManualScedule(int idOfTask, DateTime wantedTime, bool isConfirmed);

    /// tihs method help us to add dependencies of new task that just been created 
    /// </summary>
    /// <param name="dependentTask"></param>
    /// <param name="dependsOnTask"></param>
    /// <exception cref="BLWrongStageException"></exception>
    /// <exception cref="BLCannotAddCircularDependencyException"></exception>
    public void AddDependency(int dependentTask, int DependsOnTask);

    /// <summary>
    /// this method help us to change the status of the project from Planning to Scheduling. (2 to 3)
    /// </summary>
    /// <param name="StartingDateOfProject"></param>
    /// <exception cref="BLWrongStageException"></exception>
    public void StartSchedule(DateTime StartingDateOfProject);

}
