namespace DO;
/// <summary>
/// Task entity represents a task with the following properties:
/// </summary>
/// <param name="Id"> unique number </param>
/// <param name="Alias"> nickname </param>
/// <param name="Description"> description of the task in words </param>
/// <param name="IsMilestone"> is the task a milestone or not </param>
/// <param name="CreatedAtDate"> when the task has been created </param>
/// <param name="ScheduledDate"> the time the task was supposed to start </param>
/// <param name="StartDate"> the time the task actually started </param>
/// <param name="RequiredEffortTime"> the time it will probably take </param>
/// <param name="DeadlineDate"> the time we alow the task to end </param>
/// <param name="CompleteDate"> the time the task actually ended </param>
/// <param name="Deliverables"> the products </param>
/// <param name="Remarks"> notes on the task </param>
/// <param name="EngineerId"> linked to the engineer who took the task </param>
/// <param name="Complexity"> the difficulty lvl of the task </param>
public record Task
(
   int Id,
   int EngineerId,
   bool IsMilestone = false,
   string? Alias = null,
   string? Description = null, 
   DateTime? CreatedAtDate = null,
   DateTime? ScheduledDate = null,
   DateTime? StartDate = null, 
   DateTime? RequiredEffortTime = null, 
   DateTime? DeadlineDate = null, 
   DateTime? CompleteDate = null,
   string? Deliverables = null, 
   string? Remarks = null, 
   DO.EngineerExperience? Complexity = null 
)
{
    public Task() : this (0,0) { }
}
