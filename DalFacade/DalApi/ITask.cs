namespace DalApi;
using DO;
public interface ITask
{
    int Create(Task item); //Creates new entity of Task in DAL
    Task? Read(int id); //Reads entity of Task by its ID 
    List<Task> ReadAll(); //stage 1 only, Reads all entity Tasks
    void Update(Task item); //Updates entity object
    void Delete(int id); //Deletes an Task by its Id

}
