namespace BO;
/// <summary>
/// A class to use the task in the list with a mini version of the real task. 
/// <param name="Id">the id of the task</param>
/// <param name="Description">description of the task</param>
/// <param name="Alias">the alias of the task</param>
/// <param name="Status">is the task already taken or not</param>
/// </summary>
public class TaskInList
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public BO.Status Status { get; set; }
}
