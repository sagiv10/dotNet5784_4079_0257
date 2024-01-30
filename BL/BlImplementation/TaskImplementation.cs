namespace BlImplementation;
using BlApi;
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal= DalApi.Factory.Get;//with this field we can access to the methods of task in dal. the way to use the method is being chosen inside factory.
    public int Create(Task newTask)
    {
        throw new NotImplementedException();
    }

    public void Delete(int idOfTaskToDelete)
    {
        throw new NotImplementedException();
    }

    public Task Read(int idOfWantedTask)
    {
        throw new NotImplementedException();
    }

    public Task Read(Func<Task, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Task item)
    {
        throw new NotImplementedException();
    }
}
