using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO;

/// <summary>
/// 
/// <param name="Id"> the id of the task  </param>
/// <param name="Description"> the description of the task  </param>
/// <param name="Alias"> the nickname of the task </param>
/// <param name="CreatedAtDate"> the creation date of the task </param>
/// <param name="Status"> the status of the task </param>
/// <param name="Dependencies"> the tasks that depends of the task </param>
/// <param name="Milestone"> the milestone that the task depend on it </param>
/// <param name="RequiredEffortTime"> the Required effort time of the task </param>
/// <param name="StartDate"> the starting date of the task </param>
/// <param name="ScheduledDate"> the time Scheduled for the task </param>
/// <param name="ForecastDate"> the time that forcasted for the task </param>
/// <param name="DeadlineDate"> the deadline time for the task </param>
/// <param name="CompleteDate"> the time that the task has been completed </param>
/// <param name="Deliverables"> the Deliverables of the task </param>
/// <param name="Remarks"> the Deliverables of the task </param>
/// <param name="Engineer"> the id of the engineer that working on the task </param>
/// <param name="Complexity"> the level of complexity of the task </param>
/// </summary>
public class Task
{
    public int Id { get; init; }

    public string Description { get; set; }

    public string Alias { get; set; }

    public DateTime CreatedAtDate { get; init; }

    public BO.Status? Status { get; set; }

    public List<BO.TaskInList>? Dependencies { get; init; } = null; //the content will change, but not the list itself

    public BO.MilestoneInTask? Milestone { get; set; }

    public TimeSpan? RequiredEffortTime { get; set; }

    public DateTime? StartDate {  get; set; }

    public DateTime? ScheduledDate { get; set; }

    public DateTime? ForecastDate { get; set;}

    public DateTime? DeadlineDate { get; set;}

    public DateTime? CompleteDate { get; set;}

    public string? Deliverables { get; set; }

    public string? Remarks { get; set; }

    public BO.EngineerInTask? Engineer { get; set; }

    public BO.EngineerExperience? Complexity {  get; set; }
}
