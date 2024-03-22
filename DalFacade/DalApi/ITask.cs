namespace DalApi;
using DO;
public interface ITask : ICrud<Task> {
    /// <summary>
    /// return all the deleted tasks
    /// </summary>
    /// <returns> array of the deleted tasks </returns>
    public List<Task> getDeleted();
    public void GetTaskToActive(int id);
}