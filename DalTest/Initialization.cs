namespace DalTest;
using DalApi;
using DO;
using Dal;
using System.Xml.Linq;


public static class Initialization
{
    private static IDal? s_dal;
    private static readonly Random s_rand = new(DateTime.Now.Millisecond);
    private static int[] _idEngineers = new int[5];
    private static DO.ComplexityLvls[] _arrOfCmplx = new DO.ComplexityLvls[] { ComplexityLvls.Beginner, ComplexityLvls.BetterBeginner, ComplexityLvls.Advanced, ComplexityLvls.Expert, ComplexityLvls.Master };
    /// <summary>
    /// this method sets a new engineer with names we wrote and rand numbers for the id of each one and more details like email and his complexity lvl. create the new engineer and add him to the engineer list.
    /// </summary>
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
        for (int i = 0; i < NamesOfEngineers.Length; i++)
        {
            Engineer newEngineer = new Engineer(s_rand.Next(200000000, 400000001), s_rand.Next(10000, 40001) + (((double)s_rand.Next(100)) / 100), NamesOfEngineers[i].ToLower() + "@gmail.com", NamesOfEngineers[i], _arrOfCmplx[i], true);
            try
            {
                s_dal!.Engineer.Create(newEngineer);//throw if id we wrote in the create method already exist.
                _idEngineers[i] = newEngineer._id;
            }
            catch (DalAlreadyExistsException problem)
            {
                i--; //if the id already exist so ask for all the info again.
            }
        }
    }
    /// <summary>
    /// this method sets a tasks we made, create a new task, and add every each taks to a task list
    /// </summary>
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
        for (int i=0;i<AliasOfTasks.Length;i++)
        {
            Task? newTask = new Task(0, DateTime.Now, new TimeSpan(s_rand.Next(7)), false,AliasOfTasks[i], DescriptionOfTasks[i], null, null, null , null, null, null,_arrOfCmplx[i%5], _idEngineers[i%5], true) ;
            s_dal!.Task.Create(newTask);
        }
    }

    /// <summary>
    /// this method sets a dependencies we made, create a new dependency and add every each dependency to a dependency list
    /// </summary>
    private static void createDependency()
    {
        int[] arrOfDependencies = new int[]
        {
            1,4,
            1,5,
            2,5,
            3,5,
            5,6,
            1,6,
            2,6,
            3,6,
            6,7,
            7,8,
            8,9,
            2,9,
            5,9,
            8,10,
            5,10,
            8,11,
            5,11,
            4,11,
            3,13,
            2,13,
            1,13,
            8,12,
            2,12,
            5,12,
            8,15,
            13,14,
            14,15,
            14,16,
            16,17,
            17,18,
            18,19,
            15,19,
            9,19,
            12,19,
            10,19,
            11,19,
            19,20,
            19,21,
            20,22,
            21,22
        };
        for(int i=0; i<arrOfDependencies.Length;i=i+2)
        {
            Dependency? newDependency = new Dependency(0, arrOfDependencies[i], arrOfDependencies[i + 1]);
            s_dal!.Dependency.Create(newDependency);
        }
    }
    /// <summary>
    /// this program initalize the program with data we wrote and creates 3 variables that helps us to use the crud functions in program.cs.
    /// </summary>
    /// <param name="dalTask"> the implementation of the entity task that helps us reach to the interface of task </param>
    /// <param name="dalEngineer">the implementation of the entity engineer that helps us reach to the interface of engineer</param>
    /// <param name="dalDependency">the implementation of the entity dependency that helps us reach to the interface of dependency</param>
    /// <exception cref="NullReferenceException"></exception>
    public static void Do()
    {
        s_dal = Factory.Get; //choose the data source by the xml file - 'dal - config'
        XElement newNumbers = new XElement("config", new XElement("NextDependencyId", 1), new XElement("NextTaskId", 1));
        XMLTools.SaveListToXMLElement(newNumbers, "data-config");//save new running numberwhen they equal to 0 now
        s_dal.Engineer.DeleteAll(); //reset the engineer xml file
        s_dal.Task.DeleteAll(); //reset the task xml file
        s_dal.Dependency.DeleteAll(); //reset the dependency xml file
        createEngineer(); //create  the new engineers
        createTask(); //create  the new tasks
        createDependency(); //create  the new dependencies
    }   
}
