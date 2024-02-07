namespace BlApi;

/// <summary>
/// the tasks access interface for the buissness layer
/// </summary>
public interface ITask
{

    /// <summary>
    /// Creates new task to dal
    /// </summary>
    /// <param name="newEngineer"> the task we want to add </param>
    /// <exception cref="BLWrongIntInputException"></exception>
    /// <exception cref="BLWrongAlredyExistsException"></exception>
    public int Create(BO.Task newTask);

    public BO.Task Read(int idOfWantedTask); //getting the details of specific task 

    public BO.Task Read(Func<BO.Task, bool> filter); //getting the details of task that the filter accept

    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null); //returns the task list from dal

    public void Update(BO.Task item); //Updates task

    public void Delete(int idOfTaskToDelete); //Deletes an task by its Id

    /// <summary>
    /// helping method to the general automatic scedule method
    /// </summary>
    public void AutoScedule(DateTime startingDate);

    public void ManualScedule(int idOfTask, DateTime wantedTime, bool isConfirmed);

    /// <summary>
    /// adds new dependency
    /// </summary>
    /// <param name="dependentTask"> the dependent task </param>
    /// <param name="DependsOnTask"> the depends on task </param>
    /// <exception cref="BLCannotAddCircularDependencyException"></exception>
    public void AddDependency(int dependentTask, int DependsOnTask);

    public void AddDependency(int dependentTask, int DependsOnTask);

    /// <summary>
    /// starts the schedualing stage
    /// </summary>
    /// <param name="StartingDateOfProject"> the starting time of the project </param>
    public void StartSchedule(DateTime StartingDateOfProject);

}
