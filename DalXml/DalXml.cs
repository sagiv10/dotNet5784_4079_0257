
namespace Dal;
using DO;
using System;
using DalApi;
sealed public class DalXml : IDal //helping entity to get access to the tasks, engineers and the dependencies that is saved in the xml file
{
    public ITask Task => new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IDependency Dependency => new DependencyImplementation();
}
