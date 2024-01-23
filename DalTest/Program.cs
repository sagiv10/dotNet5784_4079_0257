namespace DalTest
{
    using Dal;
    using DalApi;
    using DO;
    using System;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    internal class Program
    {
        //static readonly IDal s_dal = new DalList(); //the way we used the CRUD methods
        static readonly IDal s_dal=new DalXml(); //the way we used the CRUD methods through XML files.

        /// <summary>
        /// the main functions
        /// </summary>
        /// <param name="typeChoice"></param>
        static void Main(string[] args)
        {
            try
            {
                //Initialization.Do(s_dal!); //init the list with starting data //stage2 
                int choice = 1;
                while (!(choice == 0))
                {
                    Console.WriteLine("Select an entity you want to check:\n0. exit from the menu.\n1. Engineer.\n2. Dependency.\n3. Task.\n4. initalize the whole data.");
                    choice = CheckIntInput(int.TryParse(Console.ReadLine(), out choice), choice); //gets int from the user + validation

                    switch (choice)
                    {
                        case 1:
                            EntityMenu("Engineer");
                            break;
                        case 2:
                            EntityMenu("Dependency");
                            break;
                        case 3:
                            EntityMenu("Task");
                            break;
                        case 4:
                            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                            if (ans == "Y") //stage 3
                                Initialization.Do(s_dal); //stage 2
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
        public static void EntityMenu(string typeChoice)
        {
            Console.WriteLine("Choose method:\n1.Create.\n2.Read.\n3.ReadAll.\n4.Update.\n5.Delete.");

            int choice = CheckIntInput(int.TryParse(Console.ReadLine(), out choice), choice);

            switch (typeChoice)
            {
                case "Engineer":
                    switch (choice)
                    {
                        case 1: CreateNewEngineer(); break;
                        case 2: ReadEngineer(); break;
                        case 3: ReadAllEngineer(); break;
                        case 4: UpdateEngineer(); break;
                        case 5: DeleteEngineer(); break;
                    }
                    break;
                case "Dependency":
                    switch (choice)
                    {
                        case 1: CreateNewDependency(); break;
                        case 2: ReadDependency(); break;
                        case 3: ReadAllDependency(); break;
                        case 4: UpdateDependency(); break;
                        case 5: DeleteDependency(); break;
                    }
                    break;
                case "Task":
                    switch (choice) 
                    {
                        case 1: CreateNewTask(); break;
                        case 2: ReadTask(); break;
                        case 3: ReadAllTask(); break;
                        case 4: UpdateTask(); break;
                        case 5: DeleteTask(); break;
                    }
                    break;
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

                DO.Task? oldTask = s_dal!.Task!.Read(idToUpdate);//prints the old details, if id not found so set to null
                if (oldTask != null)//if we found the id in the list 
                {
                    Console.WriteLine(oldTask);

                    Console.WriteLine("Enter the following parameters: nickname, description, if it's milestone ('1' for yes and '0' to no), when it created, the scheduled Date to beginning, when it started, deadline date, complete date, deliverables, notes and the level of complexity, and the engineer's id:");

                    string newName = Console.ReadLine() ?? oldTask._alias; //if we got a correct new info-change to what the user wrote . else dont.

                    string newDescription = Console.ReadLine() ?? oldTask._description; //if we got a correct new info-change to what the user wrote . else dont.

                    int intIsMilestone;
                    intIsMilestone = (int.TryParse(Console.ReadLine(), out intIsMilestone)) ? intIsMilestone : ((oldTask._isMilestone!) ? 1 : 0); //if we got a correct new info-change to what the user wrote . else dont.
                    bool newIsMilestone = intIsMilestone == 1 ? true : false;

                    DateTime newCreatedTime;
                    newCreatedTime = DateTime.TryParse(Console.ReadLine(), out newCreatedTime) ? newCreatedTime : (DateTime)oldTask._createdAtDate!; //if we got a correct new info-change to what the user wrote . else dont.

                    DateTime? newScheduledDate = getNullableDateTimeInput();
                    newScheduledDate = (newScheduledDate != null) ? newScheduledDate : (DateTime?)oldTask._scheduledDate; //if we got a correct new info-change to what the user wrote . else dont.

                    DateTime? newStartedDate = getNullableDateTimeInput();
                    newStartedDate = (newStartedDate != null) ? newStartedDate : (DateTime?)oldTask._startDate; //if we got a correct new info-change to what the user wrote . else dont.

                    DateTime? newDeadlineTime = getNullableDateTimeInput();
                    newDeadlineTime = (newDeadlineTime != null) ? newDeadlineTime : (DateTime?)oldTask._deadlineDate; //if we got a correct new info-change to what the user wrote . else dont.

                    DateTime? newCompletedDate = getNullableDateTimeInput();
                    newCompletedDate = (newCompletedDate != null) ? newCompletedDate : (DateTime?)oldTask._completeDate; //if we got a correct new info-change to what the user wrote . else dont.

                    string? newDeliverables = Console.ReadLine() ?? oldTask._deliverables!;

                    string? newRemarks = Console.ReadLine() ?? oldTask._remarks!;

                    int newComplexityLevel;
                    newComplexityLevel = (int.TryParse(Console.ReadLine(), out newComplexityLevel) && newComplexityLevel >= 0 && newComplexityLevel < 5) ? newComplexityLevel : (int)oldTask._complexity;  //if we got a correct new info-change to what the user wrote . else dont.

                    int? newEngineerId = getNullableIntInput();
                    newEngineerId = (newEngineerId != null) ? newEngineerId : (int)oldTask._engineerId!; //if we got a correct new info-change to what the user wrote . else dont.

                    DO.Task updatedTask = new DO.Task(idToUpdate, newCreatedTime, newIsMilestone, newName, newDescription, newScheduledDate, newStartedDate, (newScheduledDate!=null) ? newScheduledDate - newCreatedTime : null, newDeadlineTime, newCompletedDate, newDeliverables, newRemarks, (DO.ComplexityLvls)newComplexityLevel, newEngineerId, true);//create a new task with the given details

                    s_dal!.Task!.Update(updatedTask);//update the new task
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

                    int newComplexityLevel=-1;
                    newComplexityLevel = (int.TryParse(Console.ReadLine(), out newComplexityLevel) && newComplexityLevel>=0 && newComplexityLevel<5) ? newComplexityLevel : (int)oldEngineer._level!; //if we got a correct new info-change to what the user wrote . else dont.

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
        /// this method gets from the user an id of dependecy and a full details of dependecy and updates the dependency with this id from the list with the new details. if not exist write a message.
        /// </summary>
        public static void UpdateDependency()
        {
            try
            {
                Console.WriteLine("write the id of the Dependency you want to update:");

                int idToUpdate;
                idToUpdate = CheckIntInput(int.TryParse(Console.ReadLine(), out idToUpdate), idToUpdate);//sends to another method that gets the id from the user and  checks if the input is correct.

                Dependency? oldDependency = s_dal!.Dependency!.Read(idToUpdate);//prints the old details, if id not found so set to null

                if (oldDependency != null) //if we found the id in the list 
                {
                    Console.WriteLine(oldDependency);

                    Console.WriteLine("Enter the following parameters: DependentTask and DependsOnTask");

                    int? dependentTask = getNullableIntInput();

                    dependentTask = (dependentTask != null) ? dependentTask : (int)oldDependency._dependentTask!;//sends to another method that gets the dependentTask from the user and  checks if the input is correct.

                    int? dependsOnTask = getNullableIntInput();

                    dependsOnTask = (dependsOnTask != null) ? dependsOnTask : (int)oldDependency._dependsOnTask!;//sends to another method that gets the dependsOnTask from the user and  checks if the input is correct.

                    DO.Dependency updatedDependency = new DO.Dependency(idToUpdate, dependentTask, dependsOnTask);//create a new dependency with the given details

                    s_dal!.Dependency!.Update(updatedDependency);//update the new dependency 
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
        /// gets from the user a replica of the dependencies list an then prints all their data
        /// /// </summary>
        public static void ReadAllDependency()
        {
            foreach (var item in s_dal!.Dependency!.ReadAll())
            {
                Console.WriteLine(item);
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
        /// gets an id from the user and if there is an dependency with that id it prints him, if there is not it will tell it to the user
        /// </summary>
        public static void ReadDependency()
        {
            try
            {
                Console.WriteLine("Write the id of the Dependency you want to see:");
                
                int inputId;
                inputId = CheckIntInput(int.TryParse(Console.ReadLine(), out inputId), inputId); //request an int from the user and checks if it valid

                Dependency newDependency = s_dal!.Dependency!.Read(inputId)!;

                if (newDependency == null)
                {
                    throw new DalNotFoundException("id is not in the system"); //request an int from the user and checks if it valid

                }

                Console.WriteLine(newDependency);
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

            DO.Task newTask = new DO.Task(0, newCreatedTime, newIsMilestone, newName, newDescription, newScheduledDate, newStartedDate, newScheduledDate - newCreatedTime, newDeadlineTime, newCompletedDate, newDeliverables, newRemarks, (DO.ComplexityLvls)newComplexityLevel, newEngineerId, true);
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

            DO.Dependency newDependency = new DO.Dependency(0,dependentTask, dependsOnTask);
            return newDependency;
        }

        /// <summary>
        /// helping method that gets an DateTime from the user, if the user entered null then it return null and if the user entered wrong input then request another input.         /// </summary>
        /// <returns> new nullanle DateTime from the user </returns>
        private static DateTime? getNullableDateTimeInput()
        {
            string input = Console.ReadLine()!;
            if(input == "")
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
}


