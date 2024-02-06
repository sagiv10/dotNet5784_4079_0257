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
        BO.Task task = s_bl.Task.Read(1);
        Console.WriteLine(task);
    }
}