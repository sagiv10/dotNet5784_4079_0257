namespace BlApi;
public interface ITask
{
    public int Create(Task newTask); //Creates new task to dal

    public Task Read(int idOfWantedTask); //getting the details of specific task 

    public Task Read(Func<Task, bool> filter); //getting the details of task that the filter accept

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null); //returns the task list from dal
    public void Update(Task item); //Updates task
    public void Delete(int idOfTaskToDelete); //Deletes an task by its Id
    //public void UpdateOrAdd(int idOfTask,DateTime dateToCheck); //Updates or adding a schedule start of task with given date 

}
