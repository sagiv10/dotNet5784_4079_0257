namespace BO;
/// <summary>
/// A class to fully describe an engineer.
/// <param name="Id">the id of the engineer</param>
/// <param name="Name">the name of the engineer</param>
/// <param name="Email">the email of the engineer </param>
/// <param name="Level">the complexity level of the engineer </param>
/// <param name="Cost">the cost to make this engineer work</param>
/// <param name="Task">the task the engineer is currently do</param>
/// </summary>
public class Engineer
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public EngineerExperience Level { get; set; }
    public double Cost { get; set; }
    public BO.TaskInEngineer? Task { get; set; }
    public Engineer(int id, string name, string email, EngineerExperience level, double cost, TaskInEngineer? task)
    {
        Id = id;
        Name = name;
        Email = email;
        Level = level;
        Cost = cost;
        Task = task;
    }

    public override string ToString() => this.ToStringProperty();

}
