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

    public IEnumerable<Task> ReadAll(Func<BO.Task, bool>? filter = null); //returns the task list from dal
    public void Update(BO.Task item); //Updates task
    public void Delete(int idOfTaskToDelete); //Deletes an task by its Id
    //public void UpdateOrAdd(int idOfTask,DateTime dateToCheck); //Updates or adding a schedule start of task with given date 

}
