namespace BlApi;
public interface ITask
{
    void Create(Task newTask); //Creates new task to dal

    Task Read(int idOfWantedTask); //getting the details of specific task 

    Task Read(Func<Task, bool> filter); //getting the details of task that the filter accept

    IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null); //returns the task list from dal
    void Update(Task item); //Updates task
    void Delete(int idOfTaskToDelete); //Deletes an task by its Id
    //void UpdateOrAdd(int idOfTask,DateTime dateToCheck); //Updates or adding a schedule start of task with given date 

}
