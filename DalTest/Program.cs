using Dal;
using DalApi;
using System.Reflection.Metadata;

namespace DalTest
{
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

        }

    }
}
