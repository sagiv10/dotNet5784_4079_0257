namespace DalTest;
using DalApi;
using DO;
public static class Initialization
{
    private static ITask? s_dalTask; 
    private static IEngineer? s_dalEngineer; 
    private static IDependency? s_dalDependency;
    private static readonly Random s_rand = new(DateTime.Now.Millisecond);
    private static ITask? dalTask;
    private static IEngineer? dalEngineer;
    private static IDependency? dalDependency;

    private static void createTask()
    {
        string [] AliasOfTasks=new string[]
        {
            "Theoretical analysis of the engine",
            "Theoretical analysis of aerodynamics",
            "Theoretical analysis of the structure",
            "Building a theoretical skeleton of the engine",
            "Building a general theoretical skeleton",
            "Finding suitable components",
            "Component cost calculation",
            "Ordering components",
            "Construction of a warhead",
            "Construction of an exoskeleton",
            "Engine construction",
            "Construction of a fuel tank",
            "Building the software skeleton of the missile",
            "Writing the missile software",
            "Building a communication component",
            "A test for the missile software",
            "Refactoring",
            "Final testing of the software",
            "Final assembly of the missile",
            "Dry test of the operation of the components",
            "Wet inspection of the components",
            "Dispatch"
        };
        string[] DescriptionOfTasks = new string[]
        {
            "Conducting a detailed examination of the theoretical principles underlying the design and functionality of the rocket's propulsion system to ensure optimal performance and efficiency.",
            "Evaluating the theoretical aspects of airflow and forces acting on the rocket to design a streamlined shape that minimizes air resistance and enhances stability during flight.",
            "Examining the theoretical foundations of the rocket's structural design to ensure it can withstand the stresses and forces experienced during launch and flight.",
            "Developing a preliminary conceptual framework for the rocket's propulsion system to guide the subsequent engineering and construction phases.",
            "Creating an overarching conceptual framework that outlines the fundamental elements and connections within the entire rocket design.",
            "Identifying and selecting appropriate materials and parts required for the construction of the rocket, taking into account factors such as durability, weight, and compatibility.",
            "Estimating the financial investment required for acquiring all the necessary components for the rocket, considering factors such as manufacturing, transportation, and assembly costs.",
            "Initiating the procurement process to acquire the identified components and materials, ensuring timely delivery for the construction phase.",
            "Assembling the explosive payload component of the rocket, ensuring precision and safety in handling and integration.",
            "Building the outer protective structure of the rocket, providing support and shielding for internal components during launch and flight.",
            "Physically assembling and integrating the propulsion system components to create a functional rocket engine.",
            "Building the container that holds the rocket's fuel, ensuring proper storage, stability, and controlled release during launch.",
            "Developing the initial framework for the software that controls and coordinates various functions of the rocket.",
            "Creating and programming the software that governs the rocket's navigation, guidance, and communication systems.",
            "Constructing the communication subsystem that enables the rocket to send and receive data during its mission.",
            "Conducting a preliminary evaluation to verify the functionality and reliability of the rocket's software systems.",
            "Iteratively improving and optimizing the rocket's design, software, or components based on testing and feedback.",
            "Performing comprehensive tests to ensure the rocket's software operates as intended under various conditions.",
            "Integrating all components, including the structural, propulsion, software, and payload systems, to create the complete and functional rocket.",
            "Conducting a thorough examination and simulation of the rocket's systems without using actual propellants to identify and address any potential issues.",
            "Verifying the rocket's systems and components under realistic conditions, often involving the use of actual propellants in a controlled environment.",
            "Launching the rocket into its intended trajectory or mission, marking the conclusion of the manufacturing and preparation phases."
        };
        DO.ComplexityLvls[] arrOfCmplx=new DO.ComplexityLvls[] {ComplexityLvls.Beginner, ComplexityLvls.BetterBeginner, ComplexityLvls.Advanced, ComplexityLvls.Expert, ComplexityLvls.Master };
        for (int i=0;i<AliasOfTasks.Length;i++)
        {
            DateTime scheduleTime = DateTime.Now.AddMonths(s_rand.Next());
            DateTime current = DateTime.Now;
            Task? newTask = new Task(0, DateTime.Now, false,AliasOfTasks[i], DescriptionOfTasks[i], scheduleTime, null, scheduleTime - current, scheduleTime.AddDays(14) , null, null, null,arrOfCmplx[i] ENGINERRID, true) ;
            
        }
        if()//if task isnt already exist.
    }
    private static void createEngineer()
    {
        string[] NamesOfEngineers = new string[]
        {
            "Reuven",
            "Shimon",
            "Levi",
            "Yehuda",
            "Yisachar"
        };
    }
    private static void createDependency();

    public static void Do()
    {
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
    }
}
