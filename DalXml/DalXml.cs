
namespace Dal;
using DO;
using System;
using DalApi;
sealed internal class DalXml : IDal //helping singleton entity to get access to the tasks, engineers and the dependencies that is saved in the xml file
{
    private static Lazy<IDal> instance = new Lazy<IDal>(() => new DalXml(), LazyThreadSafetyMode.ExecutionAndPublication);
    //field of the lazy singleton so that he will be safe threding - when different cores will try create different instances at once, it will create only one instance
    public static IDal Instance => instance.Value; //returns the entity by the rules of lazy singleton
    // if the 'LazyThreadSafetyMode.ExecutionAndPublication' indicate that there isn't instane exist then use the constructor, gaven to the Lazy<IDal> to create new instance and return it, else- return the existing instance
    private DalXml() { }
    public ITask Task => new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IDependency Dependency => new DependencyImplementation();
}
