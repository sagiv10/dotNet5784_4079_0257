namespace BlApi;
public interface ITask
{
    public int Create(BO.Task? newTask); //Creates new task to dal
    public BO.Task? Read(int idOfWantedTask); //getting the details of specific task 
    public BO.Task? Read(Func<BO.Task?, bool> filter); //getting the details of task that the filter accept
    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task?, bool>? filter = null); //returns the task list from dal
    public void Update(BO.Task? item); //Updates task
    public void Delete(int idOfTaskToDelete); //Deletes an task by its Id
    //public void UpdateOrAdd(int idOfTask,DateTime dateToCheck); //Updates or adding a schedule start of task with given date 

}
