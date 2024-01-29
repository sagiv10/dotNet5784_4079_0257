namespace BO;
/// <summary>
/// A class to full describe of a specific milestone of tasks.
/// <param name="Id">the id of the milestone</param>
/// <param name="Description">description of the milestone</param>
/// <param name="Alias">the alias of the milestone</param>
/// <param name="CreatedAtDate">the create of date of milestone</param>
/// <param name="Status">is the milestone activated and block another tasks or not?</param>
/// <param name="ForecasatDate">when we think the task will end</param>
/// <param name="DeadlineDate">the time we want the task to end</param>
/// <param name="CompleteDate">when the task actually completed</param>
/// <param name="CompletionPercentage">how much from the milestone is already done?</param>
/// <param name="Remarks">notes about the milestone</param>
/// <param name="Dependencies">describe the dependencies that this milestone represents</param>
/// </summary>
public class Milestone
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public BO.Status? Status { get; set; }
    public DateTime? ForecasatDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public double? CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public List<BO.TaskInList>? Dependencies { get; init; } = null;//init becasue we change the content of the list and not the list herself.
}


