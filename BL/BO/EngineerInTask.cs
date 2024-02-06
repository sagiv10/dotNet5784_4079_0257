namespace BO;
/// <summary>
/// A class to describe the engineer that working on a specific task. a mini version of the engineer.
/// <param name="Id">the id of the engineer</param>
/// <param name="Name">the name of the engineer</param>
/// </summary>
public class EngineerInTask
{
    public int Id { get; init; }
    public string Name { get; set; }
    public override string ToString() => this.ToStringProperty();

}
