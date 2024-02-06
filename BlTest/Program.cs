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

        //-------------------------------------------------------------
        //TASK:
        BO.Task task = s_bl.Task.Read(1); 
        Console.WriteLine(task);

        //List<BO.TaskInList> tasks = (List<BO.TaskInList>)s_bl.Task.ReadAll();     X
        //Console.WriteLine(tasks);

        //BO.Task task = new BO.Task();
        //s_bl.Task.Create(task);
        //Console.WriteLine(task);

        //s_bl.Task.Delete(task.Id);
        //Console.WriteLine(task);

        //BO.Task task= new BO.Task();
        //s_bl.Task.Update(task);
        //Console.WriteLine(task);

        //DateTime startDate= DateTime.Now;
        //s_bl.Task.AutoScedule(startDate);
        //s_bl.Task.StartSchedule(startDate);
        //----------------------------------------------------------
        //ENGINEER:

        //BO.Engineer eng = s_bl.Engineer.ReadEngineer(1);

        //List<BO.Engineer> Engs = s_bl.Engineer.ReadAllEngineers();

        //BO.Engineer eng= new BO.Engineer();
        //s_bl.Engineer.CreateEngineer(eng);

        //BO.Engineer eng = new BO.Engineer();
        //s_bl.Engineer.UpdateEngineer(eng);

        //s_bl.Engineer.DeleteEngineer(eng.Id);

        //BO.Task task= new BO.Task();
        //BO.Engineer eng= new BO.Engineer();
        //s_bl.Engineer.AssignTask(eng.Id,task.Id);

        //BO.Engineer eng = new BO.Engineer();
        //s_bl.Engineer.DeAssignTask(eng.Id);

        //BO.Engineer eng = new BO.Engineer();
        //s_bl.Engineer.FinishTask(eng.Id);

        //BO.Engineer eng = new BO.Engineer();
        //s_bl.Engineer.GetPotentialTasks(eng.Id);
        //--------------------------------------------------------
    }
}