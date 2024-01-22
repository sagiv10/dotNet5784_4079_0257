
namespace Dal;
using DO;
using System;
using DalApi;
sealed public class DalXml : IDal
{
    public ITask Task => new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IDependency Dependency => new DependencyImplementation();
}
