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
        private static ITask? s_dalTask = new TaskImplementation(); //stage 1
        private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalTask!, s_dalEngineer!, s_dalDependency!);

                int choice = 1;
                while (!(choice == 0))
                {
                    Console.WriteLine("Select an entity you want to check:\n0. exit from the menu.\n1. Engineer.\n2. Dependency.\n3. Task");
                    
                    choice = CheckIntInput(int.TryParse(Console.ReadLine(), out choice), choice);

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
                        case 0: break;
                        default: break;
                    }
                }
            }
            catch (Exception problem)
            {
                Console.WriteLine(problem.Message);
            }
        }
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

                DO.Task? oldTask = s_dalTask!.Read(idToUpdate);//prints the old details, if id not found so set to null
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

                    DateTime newScheduledDate;
                    newScheduledDate = DateTime.TryParse(Console.ReadLine(), out newScheduledDate) ? newScheduledDate : (DateTime)oldTask._scheduledDate!; //if we got a correct new info-change to what the user wrote . else dont.

                    DateTime newStartedDate;
                    newStartedDate = DateTime.TryParse(Console.ReadLine(), out newStartedDate) ? newStartedDate : (DateTime)oldTask._startDate!; //if we got a correct new info-change to what the user wrote . else dont.

                    DateTime newDeadlineTime;
                    newDeadlineTime = DateTime.TryParse(Console.ReadLine(), out newDeadlineTime) ? newDeadlineTime : (DateTime)oldTask._deadlineDate!; //if we got a correct new info-change to what the user wrote . else dont.

                    DateTime newCompletedDate;
                    newCompletedDate = DateTime.TryParse(Console.ReadLine(), out newCompletedDate) ? newCompletedDate : (DateTime)oldTask._completeDate!; //if we got a correct new info-change to what the user wrote . else dont.

                    string newDeliverables = Console.ReadLine() ?? oldTask._deliverables!;

                    string newRemarks = Console.ReadLine() ?? oldTask._remarks!;

                    int newComplexityLevel;
                    newComplexityLevel = (int.TryParse(Console.ReadLine(), out newComplexityLevel) && newComplexityLevel >= 0 && newComplexityLevel < 5) ? newComplexityLevel : (int)oldTask._complexity;  //if we got a correct new info-change to what the user wrote . else dont.

                    int newEngineerId;
                    newEngineerId = (int.TryParse(Console.ReadLine(), out newEngineerId)) ? newEngineerId : (int)oldTask._engineerId!; //if we got a correct new info-change to what the user wrote . else dont.

                    DO.Task updatedTask = new DO.Task(idToUpdate, newCreatedTime, newIsMilestone, newName, newDescription, newScheduledDate, newStartedDate, newScheduledDate - newCreatedTime, newDeadlineTime, newCompletedDate, newDeliverables, newRemarks, (DO.ComplexityLvls)newComplexityLevel, newEngineerId, true);//create a new task with the given details

                    s_dalTask!.Update(updatedTask);//update the new task
                }
                else
                {
                    throw new Exception("id is not in the system");

                }
            }
            catch (Exception problem)
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

                DO.Engineer? oldEngineer = s_dalEngineer!.Read(idToUpdate);//prints the old details, if id not found so set to null

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

                    s_dalEngineer!.Update(updatedEngineer);//update the new engineer
                }
                else
                {
                    throw new Exception("id is not in the system");
                }

            }
            catch (Exception problem)
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

                Dependency? oldDependency = s_dalDependency!.Read(idToUpdate);//prints the old details, if id not found so set to null

                if (oldDependency != null) //if we found the id in the list 
                {
                    Console.WriteLine(oldDependency);

                    Console.WriteLine("Enter the following parameters: DependentTask and DependsOnTask");

                    int dependentTask;

                    dependentTask = (int.TryParse(Console.ReadLine(), out dependentTask)) ? dependentTask : (int)oldDependency._dependentTask!;//sends to another method that gets the dependentTask from the user and  checks if the input is correct.

                    int dependsOnTask;

                    dependsOnTask = (int.TryParse(Console.ReadLine(), out dependsOnTask)) ? dependsOnTask : (int)oldDependency._dependsOnTask!;//sends to another method that gets the dependsOnTask from the user and  checks if the input is correct.

                    DO.Dependency updatedDependency = new DO.Dependency(idToUpdate, dependentTask, dependsOnTask);//create a new dependency with the given details

                    s_dalDependency!.Update(updatedDependency);//update the new dependency 
                }
                else
                {
                    throw new Exception("id is not in the system");

                }
            }
            catch (Exception problem)
            {
                Console.WriteLine(problem.Message);
            }

        }
        public static void DeleteTask()
        {
            try
            {
                Console.WriteLine("write the id of the task you want to delete:");

                int idToDelete;
                idToDelete = CheckIntInput(int.TryParse(Console.ReadLine(), out idToDelete), idToDelete);

                s_dalTask!.Delete(idToDelete);
            }
            catch (Exception problem)
            {
                Console.WriteLine(problem.Message);
            }
        }
        public static void DeleteEngineer()
        {
            try
            {
                Console.WriteLine("write the id of the Engineer you want to delete:");

                int idToDelete;
                idToDelete = CheckIntInput(int.TryParse(Console.ReadLine(), out idToDelete), idToDelete);

                s_dalEngineer!.Delete(idToDelete);
            }
            catch (Exception problem)
            {
                Console.WriteLine(problem.Message);
            }
        }
        public static void DeleteDependency()
        {
            try
            {
                Console.WriteLine("write the id of the Dependency you want to delete:");

                int idToDelete;
                idToDelete = CheckIntInput(int.TryParse(Console.ReadLine(), out idToDelete), idToDelete);

                s_dalDependency!.Delete(idToDelete);
            }
            catch (Exception problem)
            {
                Console.WriteLine(problem.Message);
            }
        }
        public static void ReadAllTask()
        {
            foreach (var item in s_dalTask!.ReadAll())
            {
                Console.WriteLine(item);
            }
        }
        public static void ReadAllEngineer()
        {
            foreach (var item in s_dalEngineer!.ReadAll())
            {
                if (item!._isActive == true)
                {
                    Console.WriteLine(item);

                }
            }
        }
        public static void ReadAllDependency()
        {
            foreach (var item in s_dalDependency!.ReadAll())
            {
                Console.WriteLine(item);
            }
        }
        public static void ReadEngineer()
        {
            try
            {
                Console.WriteLine("Write the id of the engineer you want to see:");

                int inputId;
                inputId = CheckIntInput(int.TryParse(Console.ReadLine(), out inputId), inputId);

                Engineer newEngineer = s_dalEngineer!.Read(inputId)!;

                if (newEngineer == null)
                {
                    throw new Exception("id is not in the system");

                }

                Console.WriteLine(newEngineer);
            }
            catch (Exception problem)
            {
                Console.WriteLine(problem.Message);
            }
        }
        public static void ReadTask()
        {
            try
            {
                Console.WriteLine("Write the id of the Task you want to see:");

                int inputId;
                inputId = CheckIntInput(int.TryParse(Console.ReadLine(), out inputId), inputId);

                DO.Task newTask = s_dalTask!.Read(inputId)!;

                if (newTask == null)
                {
                    throw new Exception("id is not in the system");
                }

                Console.WriteLine(newTask);
            }
            catch (Exception problem)
            {
                Console.WriteLine(problem.Message);
            }
        }
        public static void ReadDependency()
        {
            try
            {
                Console.WriteLine("Write the id of the Dependency you want to see:");

                int inputId;
                inputId = CheckIntInput(int.TryParse(Console.ReadLine(), out inputId), inputId);

                Dependency newDependency = s_dalDependency!.Read(inputId)!;

                if (newDependency == null)
                {
                    throw new Exception("id is not in the system");

                }

                Console.WriteLine(newDependency);
            }
            catch (Exception problem)
            {
                Console.WriteLine(problem.Message);
            }
        }

        //METHODS THAT HELP US TO CREATE THE CRUD METHODS: 
        public static int CheckIntInput(bool isConvertible, int input)
        {
            int newInput = input;
            while (isConvertible == false)
            {
                Console.WriteLine("wrong input, please insert another number:");

                isConvertible = int.TryParse(Console.ReadLine(), out newInput);
            }
            return newInput;
        }

        public static double CheckDoubleInput(bool isConvertible, double input)
        {
            double newInput = input;
            while (isConvertible == false)
            {
                Console.WriteLine("wrong input, please insert another number:");

                isConvertible = double.TryParse(Console.ReadLine(), out newInput);
            }
            return newInput;
        }

        public static int CheckComplexityLevelInput(bool isConvertible, int input)
        {
            int newInput = input;
            while (isConvertible == false)
            {
                Console.WriteLine("wrong input, please insert another number:");

                isConvertible = int.TryParse(Console.ReadLine(), out newInput);

                newInput = CheckIntInput(isConvertible, newInput);

                isConvertible = (newInput >= 0 && newInput < 5);
            }
            return newInput;
        }

        //this program gets a bool variable if the input is string or DateTime and the input.
        // the program returns a correct input. 
        public static DateTime CheckDateTimeInput(bool isConvertible, DateTime input)
        {
            DateTime tempInput = input;
            while (isConvertible == false)
            {
                Console.WriteLine("invalid time format");

                isConvertible = DateTime.TryParse(Console.ReadLine(), out tempInput);
            }
            return tempInput;
        }

        public static DO.Engineer GenerateEngineer(int newId)
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

        public static DO.Task GenerateTask(int newId)
        {
            Console.WriteLine("Enter the following parameters: nickname, description, if it's milestone ('Y' or 'N'), when it created, the scheduled Date to beginning, when it started, deadline date, complete date, deliverables, notes and the level of complexity, and the engineer's id:");
            
            string newName = Console.ReadLine() ?? "";
            
            string newDescription = Console.ReadLine() ?? "";
            
            bool newIsMilestone = Console.ReadLine() == "Y" ? true : false;
            
            DateTime newCreatedTime;
            
            newCreatedTime = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out newCreatedTime), newCreatedTime);
            
            DateTime newScheduledDate;
            newScheduledDate = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out newScheduledDate), newScheduledDate);
            
            DateTime newStartedDate;
            newStartedDate = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out newStartedDate), newStartedDate);
            
            DateTime newDeadlineTime;
            newDeadlineTime = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out newDeadlineTime), newDeadlineTime);
            
            DateTime newCompletedDate;
            newCompletedDate = CheckDateTimeInput(DateTime.TryParse(Console.ReadLine(), out newCompletedDate), newCompletedDate);
            
            string newDeliverables = Console.ReadLine() ?? "";
            
            string newRemarks = Console.ReadLine() ?? "";
            
            int newComplexityLevel;
            newComplexityLevel = CheckIntInput(int.TryParse(Console.ReadLine(), out newComplexityLevel), newComplexityLevel);
            newComplexityLevel = CheckComplexityLevelInput((newComplexityLevel >= 0 && newComplexityLevel < 5), newComplexityLevel);
            
            int newEngineerId;
            newEngineerId = CheckIntInput(int.TryParse(Console.ReadLine(), out newEngineerId), newEngineerId);
            
            DO.Task newTask = new DO.Task(newId, newCreatedTime, newIsMilestone, newName, newDescription, newScheduledDate, newStartedDate, newScheduledDate - newCreatedTime, newDeadlineTime, newCompletedDate, newDeliverables, newRemarks, (DO.ComplexityLvls)newComplexityLevel, newEngineerId, true);
            return newTask;
        }

        public static DO.Dependency GenerateDependencyCreate(int newId)
        {
            Console.WriteLine("Enter the following parameters: DependentTask and DependsOnTask");
            
            int dependentTask;
            dependentTask = CheckIntInput(int.TryParse(Console.ReadLine(), out dependentTask), dependentTask);
            
            int dependsOnTask;
            dependsOnTask = CheckIntInput(int.TryParse(Console.ReadLine(), out dependsOnTask), dependsOnTask);
            
            DO.Dependency newDependency = new DO.Dependency(newId,dependentTask, dependsOnTask);
            return newDependency;
        }

        public static void CreateNewEngineer()
        {
            try
            {
                Console.WriteLine("enter the new id of the engineer:");

                int newId;
                newId = CheckIntInput(int.TryParse(Console.ReadLine(), out newId), newId);

                Engineer engToAdd = GenerateEngineer(newId);

                s_dalEngineer!.Create(engToAdd);
            }
            catch (Exception problem)
            {
                Console.WriteLine(problem.Message);
            }
        }

        public static void CreateNewTask()
        {
            DO.Task taskToAdd = GenerateTask(0);

            s_dalTask!.Create(taskToAdd);
        }

        public static void CreateNewDependency()
        {
            Dependency dependencyToAdd = GenerateDependencyCreate(0);

            s_dalDependency!.Create(dependencyToAdd);

        }
    }
}


