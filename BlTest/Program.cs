namespace BlTest;
using BO;
using BlApi;
using DalTest;

public class BlTest
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public static void Main()
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();
        List<BO.Engineer> engineers = s_bl.Engineer.ReadAllEngineers();
        foreach(BO.Engineer engineer in engineers)
        {
            Console.WriteLine(engineer);
        }
        IEnumerable<BO.TaskInList> tasks = s_bl.Task.ReadAll();
        foreach (BO.TaskInList task in tasks)
        {
            Console.WriteLine(task);
        }
    }
}