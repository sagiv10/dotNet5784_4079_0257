namespace BO;
/// <summary>
/// A class to use the milestone inside a specific task.
/// <param name="Id">the id of the milestone</param>
/// <param name="Description">description of the milestone</param>
/// <param name="Alias">the alias of the milestone</param>
/// <param name="Status">is the milestone activated and block another tasks or not?</param>
/// <param name="CompletionPercentage">how much from the milestone is already done?</param>
/// </summary>
public class MilestoneInList
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public BO.Status Status { get; set; }
    public double CompletionPercentage { get; set; }

}
