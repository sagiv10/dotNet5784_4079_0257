namespace DalTest;
using DalApi;
using DO;
public static class Initialization
{
    private static ITask? s_dalTask; 
    private static IEngineer? s_dalEngineer; 
    private static IDependency? s_dalDependency;
    private static readonly Random s_rand = new();
    private static void createTask();
    private static void createEngineer();
    private static void createDependency();
}
