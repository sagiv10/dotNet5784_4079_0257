namespace DalTest
{
    using Dal;
    using DalApi;
    using DO;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;
    using System.Runtime.CompilerServices;

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
                int choice=1;
                while(!(choice==0))
                {
                    Console.WriteLine("Select an entity you want to check:\n 0. exit from the menu.\n1. Engineer.\n2. Dependency.\n3.Task");
                    choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:EntityMenu("Engineer");
                            break;
                        case 2:EntityMenu("Dependency");
                            break;
                        case 3:EntityMenu("Task");
                            break;
                        case 0: break;
                        default: break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(  );
            }
        }
        public static void EntityMenu(string typeChoice)
        {
            Console.WriteLine("Choose method:/n1.Create./n2.Read./n3.ReadAll./n4.Update./n5.Delete.");
            int charMethod=int.Parse(Console.ReadLine());
            switch (typeChoice)
            {
                case "Engineer":
                    switch(charMethod)
                    {
                        case 1: CreateNewEngineer(); break;
                        case 2: ReadEngineer(); break;
                        case 3: ReadAllNewEngineer(); break;
                        case 4: UpdateNewEngineer(); break;
                        case 5: DeleteNewEngineer(); break;
                    }
                    break;
                case "Dependency":
                    switch (charMethod)
                    {
                        case 1: CreateNewDependency(); break;
                        case 2: ReadDependency(); break;
                        case 3: ReadAllNewDependency(); break;
                        case 4: UpdateNewDependency(); break;
                        case 5: DeleteNewDependency(); break;
                    }
                    break;
                case "Task":
                    switch (charMethod)
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
                int idToUpdate = int.Parse(Console.ReadLine());
                Task? updatedTask = generatedTask;
                s_dalTask.Update(updatedTask);
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
                int idToUpdate = int.Parse(Console.ReadLine());
                Engineer? updatedEngineer = generatedEngineer;
                s_dalEngineer.Update(updatedEngineer);
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
                int idToUpdate = int.Parse(Console.ReadLine());
                Dependency? updatedDependency = generatedDependency;
                s_dalDependency.Update(updatedDependency);
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
                int idToDelete = int.Parse(Console.ReadLine());
                s_dalTask.Delete(idToDelete);
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
                int idToDelete = int.Parse(Console.ReadLine());
                s_dalEngineer.Delete(idToDelete);
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
                int idToDelete = int.Parse(Console.ReadLine());
                s_dalDependency.Delete(idToDelete);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void ReadAllNewTask() 
        {
            foreach (var item in s_dalTask.ReadAll())
            {
                Console.WriteLine(item);
            }
        }
        public static void ReadAllNewEngineer()
        {
            foreach (var item in s_dalEngineer.ReadAll())
            {
                Console.WriteLine(item);
            }
        }
        public static void ReadAllNewDependency()
        {
            foreach (var item in s_dalDependency.ReadAll())
            {
                Console.WriteLine(item);
            }
        }
        public static void ReadEngineer()
        {
            try
            {
                Console.WriteLine("Write the id of the engineer you want to see:");
                int inputId = int.Parse(Console.ReadLine());
                Engineer newEngineer = s_dalEngineer.Read(inputId);
                if (newEngineer == null)
                    throw new Exception ("id is not in the system");
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
                int inputId = int.Parse(Console.ReadLine());
                Task newTask = s_dalTask.Read(inputId);
                if (newTask == null)
                    throw new Exception("id is not in the system");
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
                int inputId = int.Parse(Console.ReadLine());
                Dependency newDependency = s_dalDependency.Read(inputId);
                if (newDependency == null)
                    throw new Exception("id is not in the system");
                Console.WriteLine(newDependency);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        //this program gets a bool variable if the input is string or DateTime and the input.
        // the program returns a correct input. 
        public static DateTime inputCheck(bool isConvertible,DateTime input)
        {
            DateTime tempInput = input;
            while (isConvertible == false)
            {
                Console.WriteLine("invalid time format");
                isConvertible = DateTime.TryParse(Console.ReadLine(), out tempInput);
            }
            return tempInput;
        }
    }
}
