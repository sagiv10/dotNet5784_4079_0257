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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                        case 3: ReadAllNewEngineer(); break;
                        case 4: UpdateNewEngineer(); break;
                        case 5: DeleteNewEngineer(); break;
                    }
                    break;
                case "Dependency":
                    switch (choice)
                    {
                        case 1: CreateNewDependency(); break;
                        case 2: ReadDependency(); break;
                        case 3: ReadAllNewDependency(); break;
                        case 4: UpdateNewDependency(); break;
                        case 5: DeleteNewDependency(); break;
                    }
                    break;
                case "Task":
                    switch (choice)
                    {
                        case 1: CreateNewTask(); break;
                        case 2: ReadTask(); break;
                        case 3: ReadAllNewTask(); break;
                        case 4: UpdateNewTask(); break;
                        case 5: DeleteNewTask(); break;
                    }
                    break;
            }
        }
        public static void UpdateNewTask()
        {
            try
            {
                Console.WriteLine("write the id of the task you want to update:");

                int idToUpdate;
                idToUpdate = CheckIntInput(int.TryParse(Console.ReadLine(), out idToUpdate), idToUpdate);

                DO.Task? updatedTask = GenerateTask(idToUpdate);

                s_dalTask!.Update(updatedTask);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        public static void UpdateNewEngineer()
        {
            try
            {
                Console.WriteLine("write the id of the Engineer you want to update:");

                int idToUpdate;
                idToUpdate = CheckIntInput(int.TryParse(Console.ReadLine(), out idToUpdate), idToUpdate);

                Engineer? updatedEngineer = GenerateEngineer(idToUpdate);

                s_dalEngineer!.Update(updatedEngineer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public static void UpdateNewDependency()
        {
            try
            {
                Console.WriteLine("write the id of the Dependency you want to update:");

                int idToUpdate;
                idToUpdate = CheckIntInput(int.TryParse(Console.ReadLine(), out idToUpdate), idToUpdate);
                
                Dependency? updatedDependency = GenerateDependency(idToUpdate);

                s_dalDependency!.Update(updatedDependency);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public static void DeleteNewTask()
        {
            try
            {
                Console.WriteLine("write the id of the task you want to delete:");

                int idToDelete;
                idToDelete = CheckIntInput(int.TryParse(Console.ReadLine(), out idToDelete), idToDelete);

                s_dalTask!.Delete(idToDelete);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void DeleteNewEngineer()
        {
            try
            {
                Console.WriteLine("write the id of the Engineer you want to delete:");

                int idToDelete;
                idToDelete = CheckIntInput(int.TryParse(Console.ReadLine(), out idToDelete), idToDelete);

                s_dalEngineer!.Delete(idToDelete);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void DeleteNewDependency()
        {
            try
            {
                Console.WriteLine("write the id of the Dependency you want to delete:");

                int idToDelete;
                idToDelete = CheckIntInput(int.TryParse(Console.ReadLine(), out idToDelete), idToDelete);

                s_dalDependency!.Delete(idToDelete);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void ReadAllNewTask()
        {
            foreach (var item in s_dalTask!.ReadAll())
            {
                Console.WriteLine(item);
            }
        }
        public static void ReadAllNewEngineer()
        {
            foreach (var item in s_dalEngineer!.ReadAll())
            {
                if (item!._isActive == true)
                {
                    Console.WriteLine(item);

                }
            }
        }
        public static void ReadAllNewDependency()
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
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
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
            
            bool newIsMilestone = ((char)Console.Read() == 'Y' ? true : false);
            
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

        public static DO.Dependency GenerateDependency(int newId)
        {
            Console.WriteLine("Enter the following parameters: DependentTask and DependsOnTask");
            
            int dependentTask;
            dependentTask = CheckIntInput(int.TryParse(Console.ReadLine(), out dependentTask), dependentTask);
            
            int dependsOnTask;
            dependsOnTask = CheckIntInput(int.TryParse(Console.ReadLine(), out dependsOnTask), dependsOnTask);
            
            DO.Dependency newDependency = new DO.Dependency(dependentTask, dependsOnTask);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void CreateNewTask()
        {
            DO.Task taskToAdd = GenerateTask(0);

            s_dalTask!.Create(taskToAdd);
        }

        public static void CreateNewDependency()
        {
            Dependency dependencyToAdd = GenerateDependency(0);

            s_dalDependency!.Create(dependencyToAdd);

        }
    }
}


