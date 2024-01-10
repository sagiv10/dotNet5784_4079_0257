namespace DO;
/// <summary>
/// Task entity represents a task with the following properties:
/// </summary>
/// <param name="_id"> unique number </param>
/// <param name="_alias"> nickname </param>
/// <param name="_description"> description of the task in words </param>
/// <param name="_isMilestone"> is the task a milestone or not </param>
/// <param name="_createdAtDate"> when the task has been created </param>
/// <param name="_scheduledDate"> the time the task was supposed to start </param>
/// <param name="_startDate"> the time the task actually started </param>
/// <param name="_requiredEffortTime"> the time it will probably take </param>
/// <param name="_deadlineDate"> the time we alow the task to end </param>
/// <param name="_completeDate"> the time the task actually ended </param>
/// <param name="_deliverables"> the products </param>
/// <param name="_remarks"> notes on the task </param>
/// <param name="_engineerId"> linked to the engineer who took the task </param>
/// <param name="_complexity"> the difficulty lvl of the task </param>
/// <param name="_isActive"> instead if deleting the Engineer thius boolean determine if this object is active or not </param>
public record Task
(
   int _id,
   DateTime _createdAtDate,
   bool _isMilestone = false,
   string _alias = "",
   string _description ="", 
   DateTime? _scheduledDate = null,
   DateTime? _startDate = null, 
   TimeSpan? _requiredEffortTime = null, 
   DateTime? _deadlineDate = null, 
   DateTime? _completeDate = null,
   string? _deliverables = null, 
   string? _remarks = null, 
   DO.ComplexityLvls _complexity = DO.ComplexityLvls.Beginner,
   int? _engineerId=null,
   bool _isActive=false
)
{
    public Task() : this(0,DateTime.Now,false) { }
}
