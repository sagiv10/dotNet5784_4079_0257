namespace BlTest;
using BO;
using BlApi;
using DalTest;
using DalApi;
using DO;
using System.Linq.Expressions;

public class BlTest
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        
    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();//static readonly IDal s_dal = new DalList(); //the way we used the CRUD methods
        try
        {
            int choice = 1;
            while (!(choice == 0))
            {
                Console.WriteLine("Select an entity you want to check:\n0. exit from the menu.\n1. Engineer.\n2. Task.\n3. go on to the scheduling stage");
                choice = CheckIntInput(int.TryParse(Console.ReadLine(), out choice), choice); //gets int from the user + validation

                switch (choice)
                {
                    case 1:
                        EngineerMenu();
                        break;
                    case 2:
                        TaskMenu();
                        break;
                    case 3:
                        Console.WriteLine("please enter a date to the start of the project:");
                        DateTime startingTime;
                        startingTime = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out startingTime), startingTime);
                        s_bl.Task.StartSchedule(startingTime);
                        break;
                    case 0: break;
                    default: break;
                }
            }
        }
        catch (NullReferenceException problem)
        {
            Console.WriteLine(problem.Message);
        }
    }

    /// <summary>
    /// the secondery menu
    /// </summary>
    /// <param name="typeChoice"></param>
    public static void EngineerMenu()
    {
        Console.WriteLine("Choose method:\n1.Create.\n2.Read.\n3.ReadAll.\n4.Update.\n5.Delete.\n6.Assign task.\n7.Deassign task.\n8.finish task.\n9.Show hisTask.\n10.Show Potential tasks.");

        int choice = CheckIntInput(int.TryParse(Console.ReadLine(), out choice), choice);
                switch (choice)
                {
                    case 1: CreateNewEngineer(); break;
                    case 2: ReadEngineer(); break;
                    case 3: ReadAllEngineer(); break;
                    case 4: UpdateEngineer(); break;
                    case 5: DeleteEngineer(); break;
                    case 6: AssignEngineer(); break;
                    case 7: DeassignEngineer(); break;
                    case 8: FinishTaskEngineer(); break;
                    case 9: ShowTaskEngineer(); break;
                    case 10: ShowPotentialTaskEngineer(); break;
                }
 
    }

    public static void TaskMenu()
    {
        Console.WriteLine("Choose method:\n1.Create.\n2.Read.\n3.ReadAll.\n4.Update.\n5.Delete.\n6.Manual schedual one task.\n7.Auto schedual all tasks.\n8.Add dependency\n9.Delete dependency");

        int choice = CheckIntInput(int.TryParse(Console.ReadLine(), out choice), choice);
        switch (choice)
        {
            case 1: CreateNewTask(); break;
            case 2: ReadTask(); break;
            case 3: ReadAllTask(); break;
            case 4: UpdateTask(); break;
            case 5: DeleteTask(); break;
            case 6: ManualScheduleTask(); break;
            case 7: AutoScheduleTasks(); break;
            case 8: CreateNewDependency(); break;
            case 9: DeleteDependency(); break;

        }
    }


    /// <summary>
    /// this method gets from the user an id of task and a full details of task and updates the task with this id from the list with the new details. if not exist write a message.
    /// </summary>
    public static void UpdateTask()
    {
        try
        {
            Console.WriteLine("write the id of the task you want to update:");

            int idToUpdate;
            idToUpdate = CheckIntInput(int.TryParse(Console.ReadLine(), out idToUpdate), idToUpdate);//sends to another method that gets the id from the user and  checks if the input is correct.
            
            BO.Task oldTask = s_bl!.Task!.Read(idToUpdate);//prints the old details, if id not found so set to null
            
            Console.WriteLine(oldTask);
            
            Console.WriteLine("Enter the following parameters: alias, description, required amount of days, deliverables, remarks and the level of complexity,");
            
            Random random = new Random(DateTime.Now.Millisecond);

            string newName = Console.ReadLine() ?? oldTask.Alias; //if we got a correct new info-change to what the user wrote . else dont.
            
            string newDescription = Console.ReadLine() ?? oldTask.Description; //if we got a correct new info-change to what the user wrote . else dont.

            int newDays;
            newDays = int.TryParse(Console.ReadLine(), out newDays) == true ? newDays : -1;
            TimeSpan newRequiredEffortTime = newDays>0  ? new TimeSpan(newDays) : new TimeSpan(random.Next());
            
            BO.Status newStatus =;
            
            string? newDeliverables = Console.ReadLine() ?? oldTask.Deliverables!;
            
            string? newRemarks = Console.ReadLine() ?? oldTask.Remarks!;
            
            int newComplexityLevel;
            
            newComplexityLevel = (int.TryParse(Console.ReadLine(), out newComplexityLevel) && newComplexityLevel >= 0 && newComplexityLevel < 5) ? newComplexityLevel : (int)oldTask.Complexity;  //if we got a correct new info-change to what the user wrote . else dont.
            
            BO.Task updatedTask = new BO.Task()
            {
                Id = idToUpdate,
                Description = newDescription,
                Alias = newName,
                CreatedAtDate = DateTime.Now,
                Status = newStatus,////////////////////////////
                Dependencies = new List<BO.TaskInList?>(),
                Milestone = null,
                RequiredEffortTime = new TimeSpan(random.Next(1, 7)),
                StartDate = null,
                ScheduledDate = null,
                ForecastDate = null,
                DeadlineDate = null,
                CompleteDate = null,
                Deliverables = newDeliverables,
                Remarks = newRemarks,
                Engineer = null,
                Complexity = (BO.EngineerExperience)newComplexityLevel
            };
            
            s_bl!.Task!.Update(updatedTask);//update the new task
        }
        catch (DalNotFoundException problem)
        {
            Console.WriteLine(problem.Message);
        }

    }
    /// <summary>
    /// this method gets from the user an id of engineer and a full details of engineer and updates the engineer with this id from the list with the new details. if not exist write a message.
    /// </summary>
    public static void UpdateEngineer()
    {
        try
        {
            Console.WriteLine("write the id of the Engineer you want to update:");

            int idToUpdate;
            idToUpdate = CheckIntInput(int.TryParse(Console.ReadLine(), out idToUpdate), idToUpdate);//sends to another method that gets the id from the user and  checks if the input is correct.

            DO.Engineer? oldEngineer = s_dal!.Engineer!.Read(idToUpdate);//prints the old details, if id not found so set to null

            if (oldEngineer != null)//if we found the id in the list 
            {
                Console.WriteLine(oldEngineer);

                Console.WriteLine("Enter the following parameters: email address, salary, name and the complexity level:");

                string newEmail = Console.ReadLine() ?? oldEngineer._email!; //if we got a correct new info-change to what the user wrote . else dont.

                double newCost;
                newCost = double.TryParse(Console.ReadLine(), out newCost) ? newCost : oldEngineer._cost; //if we got a correct new info-change to what the user wrote . else dont.

                string newName = Console.ReadLine() ?? oldEngineer._name; //if we got a correct new info-change to what the user wrote . else dont.

                int newComplexityLevel = -1;
                newComplexityLevel = (int.TryParse(Console.ReadLine(), out newComplexityLevel) && newComplexityLevel >= 0 && newComplexityLevel < 5) ? newComplexityLevel : (int)oldEngineer._level!; //if we got a correct new info-change to what the user wrote . else dont.

                DO.Engineer updatedEngineer = new DO.Engineer(idToUpdate, newCost, newEmail, newName, (DO.ComplexityLvls)newComplexityLevel, true);//create a new engineer with the given details

                s_dal!.Engineer!.Update(updatedEngineer);//update the new engineer
            }
            else
            {
                throw new DalNotFoundException("id is not in the system");
            }

        }
        catch (DalNotFoundException problem)
        {
            Console.WriteLine(problem.Message);
        }

    }

    /// <summary>
    /// gets an id of task from the user and delete it from the tasks list
    /// </summary>
    public static void DeleteTask()
    {
        try
        {
            Console.WriteLine("write the id of the task you want to delete:");

            int idToDelete;
            idToDelete = CheckIntInput(int.TryParse(Console.ReadLine(), out idToDelete), idToDelete); //request an int from the user and checks if it valid

            s_dal!.Task!.Delete(idToDelete);
        }
        catch (DalNotFoundException problem)
        {
            Console.WriteLine(problem.Message);
        }
    }

    /// <summary>
    /// gets an id of engineer from the user and delete it from the engineers list
    /// </summary>
    public static void DeleteEngineer()
    {
        try
        {
            Console.WriteLine("write the id of the Engineer you want to delete:");

            int idToDelete;
            idToDelete = CheckIntInput(int.TryParse(Console.ReadLine(), out idToDelete), idToDelete); //request an int from the user and checks if it valid

            s_dal!.Engineer!.Delete(idToDelete);
        }
        catch (DalNotFoundException problem)
        {
            Console.WriteLine(problem.Message);
        }
    }

    /// <summary>
    /// gets an id of dependency from the user and delete it from the dependencys list
    /// </summary>
    public static void DeleteDependency()
    {
        try
        {
            Console.WriteLine("write the id of the Dependency you want to delete:");

            int idToDelete;
            idToDelete = CheckIntInput(int.TryParse(Console.ReadLine(), out idToDelete), idToDelete); //request an int from the user and checks if it valid

            s_dal!.Dependency!.Delete(idToDelete);
        }
        catch (DalNotFoundException problem)
        {
            Console.WriteLine(problem.Message);
        }
    }

    /// <summary>
    /// gets from the user a replica of the tasks list an then prints all their data
    /// </summary>
    public static void ReadAllTask()
    {
        foreach (var item in s_dal!.Task!.ReadAll())
        {
            Console.WriteLine(item);
        }
    }

    /// <summary>
    /// gets from the user a replica of the engineers list an then prints all their data
    /// /// </summary>
    public static void ReadAllEngineer()
    {
        foreach (var item in s_dal!.Engineer!.ReadAll())
        {
            if (item!._isActive == true)
            {
                Console.WriteLine(item);

            }
        }
    }

    /// <summary>
    /// gets an id from the user and if there is an engineer with that id it prints him, if there is not it will tell it to the user
    /// </summary>
    public static void ReadEngineer()
    {
        try
        {
            Console.WriteLine("Write the id of the engineer you want to see:");

            int inputId;
            inputId = CheckIntInput(int.TryParse(Console.ReadLine(), out inputId), inputId); //request an int from the user and checks if it valid

            Engineer newEngineer = s_dal!.Engineer!.Read(inputId)!;

            if (newEngineer == null)
            {
                throw new DalNotFoundException("id is not in the system"); //request an int from the user and checks if it valid

            }

            Console.WriteLine(newEngineer);
        }
        catch (DalNotFoundException problem)
        {
            Console.WriteLine(problem.Message);
        }
    }

    /// <summary>
    /// gets an id from the user and if there is an task with that id it prints him, if there is not it will tell it to the user
    /// </summary>
    public static void ReadTask()
    {
        try
        {
            Console.WriteLine("Write the id of the Task you want to see:");

            int inputId;
            inputId = CheckIntInput(int.TryParse(Console.ReadLine(), out inputId), inputId); //request an int from the user and checks if it valid

            DO.Task newTask = s_dal!.Task!.Read(inputId)!;

            if (newTask == null)
            {
                throw new DalNotFoundException("id is not in the system"); //request an int from the user and checks if it valid
            }

            Console.WriteLine(newTask);
        }
        catch (DalNotFoundException problem)
        {
            Console.WriteLine(problem.Message);
        }
    }

    /// <summary>
    /// create new engineer and store it in the lists
    /// </summary>
    public static void CreateNewEngineer()
    {
        try
        {
            Console.WriteLine("enter the new id of the engineer:");

            int newId;
            newId = CheckIntInput(int.TryParse(Console.ReadLine(), out newId), newId); //request an int from the user and checks if it valid

            Engineer engToAdd = GenerateEngineerCreate(newId);

            s_dal!.Engineer!.Create(engToAdd);
        }
        catch (DalAlreadyExistsException problem)
        {
            Console.WriteLine(problem.Message);
        }
    }

    /// <summary>
    /// create new task and store it in the lists
    /// </summary>
    public static void CreateNewTask()
    {
        DO.Task taskToAdd = GenerateTaskCreate();

        s_dal!.Task!.Create(taskToAdd);
    }

    /// <summary>
    /// create new dependency and store it in the lists
    /// </summary>
    public static void CreateNewDependency()
    {
        Dependency dependencyToAdd = GenerateDependencyCreate();

        s_dal!.Dependency!.Create(dependencyToAdd);

    }


    public static void AssignEngineer()
    {
        
    }


    public static void DeassignEngineer()
    {

    }


    public static void FinishTaskEngineer()
    {

    }


    public static void ShowTaskEngineer()
    {

    }


    public static void ShowPotentialTaskEngineer()
    {

    }



    public static void ManualScheduleTask()
    {

    }


    public static void AutoScheduleTasks()
    {

    }

    //METHODS THAT HELP US TO CREATE THE CRUD METHODS: 

    /// <summary>
    /// gets an int and boolean that tells if the first convertion to this int was successfull. and requests from the user new int untill the convertion will success. then it returns the changed int
    /// </summary>
    /// <param name="isConvertible">boolean that tells if the first convertion to the int was successfull</param>
    /// <param name="input">the input int we want to change if it did not converted in the first time</param>
    /// <returns></returns>
    public static int CheckIntInput(bool isConvertible, int input)
    {
        int newInput = input;
        while (isConvertible == false) //while it not converted properly - request new input from the user until he will enter proper input
        {
            Console.WriteLine("wrong input, please insert another number:");

            isConvertible = int.TryParse(Console.ReadLine(), out newInput);
        }
        return newInput;
    }

    /// <summary>
    /// gets an double and boolean that tells if the first convertion to this double was successfull. and requests from the user new double untill the convertion will success. then it returns the changed double
    /// </summary>
    /// <param name="isConvertible">boolean that tells if the first convertion to the double was successfull</param>
    /// <param name="input">the input int we want to change if it did not converted in the first time</param>
    /// <returns></returns>
    public static double CheckDoubleInput(bool isConvertible, double input)
    {
        double newInput = input;
        while (isConvertible == false)
        {
            Console.WriteLine("wrong input, please insert another number:");//while it not converted properly - request new input from the user until he will enter proper input

            isConvertible = double.TryParse(Console.ReadLine(), out newInput);
        }
        return newInput;
    }


    /// <summary>
    /// gets an ComplexityLevel and boolean that tells if the first convertion to this ComplexityLevel was successfull. and requests from the user new ComplexityLevel untill the convertion will success. then it returns the changed ComplexityLevel
    /// </summary>
    /// <param name="isConvertible">boolean that tells if the first convertion to the ComplexityLevel was successfull</param>
    /// <param name="input">the input int we want to change if it did not converted in the first time</param>
    /// <returns></returns>
    public static int CheckComplexityLevelInput(bool isConvertible, int input)
    {
        int newInput = input;
        while (isConvertible == false)//while it not converted properly - request new input from the user until he will enter proper input
        {
            Console.WriteLine("wrong input, please insert another number:");

            isConvertible = int.TryParse(Console.ReadLine(), out newInput);

            newInput = CheckIntInput(isConvertible, newInput); //check if this was valid int input

            isConvertible = (newInput >= 0 && newInput < 5);  //check if this was valid complexity level input (between 0-4)
        }
        return newInput;
    }

    /// <summary>
    /// gets an DateTime and boolean that tells if the first convertion to this DateTime was successfull. and requests from the user new DateTime untill the convertion will success. then it returns the changed DateTime
    /// </summary>
    /// <param name="isConvertible">boolean that tells if the first convertion to the DateTime was successfull</param>
    /// <param name="input">the input int we want to change if it did not converted in the first time</param>
    /// <returns></returns> 
    public static DateTime CheckDateTimeInput(bool isConvertible, DateTime input)
    {
        DateTime tempInput = input;
        while (isConvertible == false) //while it not converted properly - request new input from the user until he will enter proper input
        {
            Console.WriteLine("invalid time format");

            isConvertible = DateTime.TryParse(Console.ReadLine(), out tempInput);
        }
        return tempInput;
    }


    /// <summary>
    /// request all the pameters from the user for new engineer, create him and return it
    /// </summary>
    /// <param name="newId"> the new id for the new engineer, we won't need to get new one </param>
    /// <returns></returns>
    public static DO.Engineer GenerateEngineerCreate(int newId)
    {
        Console.WriteLine("Enter the following parameters: email address, salary, name and the complexity level:");

        string newEmail = Console.ReadLine() ?? "";

        double newCost;
        newCost = CheckDoubleInput(double.TryParse(Console.ReadLine(), out newCost), newCost);

        string newName = Console.ReadLine() ?? "";

        int newComplexityLevel;
        newComplexityLevel = CheckIntInput(int.TryParse(Console.ReadLine(), out newComplexityLevel), newComplexityLevel);
        newComplexityLevel = CheckComplexityLevelInput((newComplexityLevel >= 0 && newComplexityLevel < 5), newComplexityLevel);

        DO.Engineer newEngineer = new DO.Engineer(newId, newCost, newEmail, newName, (DO.ComplexityLvls)newComplexityLevel, true);

        return newEngineer;
    }

    /// <summary>
    /// request all the pameters from the user for new task, create him and return it
    /// </summary>
    /// <returns></returns>
    public static DO.Task GenerateTaskCreate()
    {
        Console.WriteLine("Enter the following parameters: nickname, description, if it's milestone ('1' for yes and '0' to no), when it created, the scheduled Date to beginning, when it started, deadline date, complete date, deliverables, notes and the level of complexity, and the engineer's id:");

        string newName = Console.ReadLine() ?? ""; //requst new name from the user + null check

        string newDescription = Console.ReadLine() ?? ""; //requst new description from the user + null check

        int isMilestone;
        isMilestone = CheckIntInput(int.TryParse(Console.ReadLine(), out isMilestone), isMilestone); //request an int from the user and checks if it valid
        bool newIsMilestone = isMilestone == 1 ? true : false;

        DateTime newCreatedTime;
        newCreatedTime = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out newCreatedTime), newCreatedTime); //requst new DateTime from the user + null check

        DateTime newScheduledDate;
        newScheduledDate = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out newScheduledDate), newScheduledDate); //requst new DateTime from the user + null check

        DateTime newStartedDate;
        newStartedDate = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out newStartedDate), newStartedDate); //requst new DateTime from the user + null check

        DateTime newDeadlineTime;
        newDeadlineTime = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out newDeadlineTime), newDeadlineTime); //requst new DateTime from the user + null check

        DateTime newCompletedDate;
        newCompletedDate = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out newCompletedDate), newCompletedDate); //requst new DateTime from the user + null check

        string newDeliverables = Console.ReadLine() ?? ""; //requst new name from the user + null check

        string newRemarks = Console.ReadLine() ?? ""; //requst new name from the user + null check

        int newComplexityLevel;
        newComplexityLevel = CheckIntInput(int.TryParse(Console.ReadLine(), out newComplexityLevel), newComplexityLevel);
        newComplexityLevel = CheckComplexityLevelInput((newComplexityLevel >= 0 && newComplexityLevel < 5), newComplexityLevel); //requst new complexity level from the user + validation check

        int newEngineerId;
        newEngineerId = CheckIntInput(int.TryParse(Console.ReadLine(), out newEngineerId), newEngineerId); //request an int from the user and checks if it valid

        DO.Task newTask = new DO.Task(0, newCreatedTime, newScheduledDate - newCreatedTime, newIsMilestone, newName, newDescription, newScheduledDate, newStartedDate, newDeadlineTime, newCompletedDate, newDeliverables, newRemarks, (DO.ComplexityLvls)newComplexityLevel, newEngineerId, true);
        return newTask;
    }

    /// <summary>
    /// request all the pameters from the dependency for new task, create him and return it
    /// </summary>
    /// <returns></returns>
    public static DO.Dependency GenerateDependencyCreate()
    {
        Console.WriteLine("Enter the following parameters: DependentTask and DependsOnTask");

        int dependentTask;
        dependentTask = CheckIntInput(int.TryParse(Console.ReadLine(), out dependentTask), dependentTask); //request an int from the user and checks if it valid

        int dependsOnTask;
        dependsOnTask = CheckIntInput(int.TryParse(Console.ReadLine(), out dependsOnTask), dependsOnTask); //request an int from the user and checks if it valid

        DO.Dependency newDependency = new DO.Dependency(0, dependentTask, dependsOnTask);
        return newDependency;
    }

    /// <summary>
    /// helping method that gets an DateTime from the user, if the user entered null then it return null and if the user entered wrong input then request another input.         /// </summary>
    /// <returns> new nullanle DateTime from the user </returns>
    private static DateTime? getNullableDateTimeInput()
    {
        string input = Console.ReadLine()!;
        if (input == "")
        {
            return null;
        }
        else
        {
            DateTime result;
            result = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out result), result);
            return result;
        }
    }

    /// <summary>
    /// helping method that gets an int from the user, if the user entered null then it return null and if the user entered wrong input then request another input.  
    /// </summary>
    /// <returns> new nullanle DateTime from the user </returns>
    private static int? getNullableIntInput()
    {
        string input = Console.ReadLine()!;
        if (input == "")
        {
            return null;
        }
        else
        {
            int result;
            result = CheckIntInput(int.TryParse(Console.ReadLine(), out result), result);
            return result;
        }
    }

}
