using Dal;
using DalApi;
using System;

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










            }
            catch (Exception ex)
            {
                Console.WriteLine(  );
            }
        }

        public static int IntInputCheck(bool isConvertible, int input)
        {
            int newInput = input;
            while (isConvertible == false)
            {
                Console.WriteLine("wrong input, please insert another number:");
                isConvertible = int.TryParse(Console.ReadLine(), out newInput);
            }
            return newInput;
        }

        public static double DoubleInputCheck(bool isConvertible, double input)
        {
            double newInput = input;
            while (isConvertible == false)
            {
                Console.WriteLine("wrong input, please insert another number:");
                isConvertible = double.TryParse(Console.ReadLine(), out newInput);
            }
            return newInput;
        }

        public static int ComplexityLevelInputCheck(bool isConvertible, int input)
        {
            int newInput = input;
            while (isConvertible == false)
            {
                Console.WriteLine("wrong input, please insert another number:");
                isConvertible = int.TryParse(Console.ReadLine(), out newInput);
                newInput=IntInputCheck(isConvertible, newInput);
                isConvertible = (newInput >= 0 && newInput < 5);
            }
            return newInput;
        }

        public static DO.Engineer GenerateEngineer()
        {
            Console.WriteLine("Enter the following parameters: id, email address, salary, name and the complexity level:");
            int newId;
            newId = IntInputCheck(int.TryParse(Console.ReadLine(), out newId), newId);
            string newEmail = Console.ReadLine() ?? "";
            double newCost;
            newCost = DoubleInputCheck(double.TryParse(Console.ReadLine(), out newCost), newCost);
            string newName = Console.ReadLine() ?? "";
            int newComplexityLevel;
            newComplexityLevel = IntInputCheck(int.TryParse(Console.ReadLine(), out newComplexityLevel), newComplexityLevel);
            newId = ComplexityLevelInputCheck((newComplexityLevel >= 0 && newComplexityLevel < 5), newComplexityLevel);
            DO.Engineer newEngineer=new DO.Engineer(newId,newCost, newEmail,newName, (DO.ComplexityLvls)newComplexityLevel, true);
            return newEngineer;
        }

        public static DO.Task GenerateTask(int newId)
        {
            Console.WriteLine("Enter the following parameters: nickname, description, if it's milestone ('Y' or 'N'), when it created, the scheduled Date to beginning, when it started, the required effort of time, deadline date, complete date, deliveables, notes and the level of complexity, and the engineer's id:");
            string newName = Console.ReadLine() ?? "";
            string newDescription = Console.ReadLine() ?? "";
            bool newIsMilestone = ((char)Console.Read()=='Y' ? true : false);
            DateTime newCreatedAtDate=


        }

        public static DO.Dependency GenerateDependency()
        {

        }
    }
}
