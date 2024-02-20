using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;

internal class StatusCollection : IEnumerable //for the filter in tasks window
{
    static readonly IEnumerable<BO.Status> status =
            (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

    public IEnumerator GetEnumerator() => status.GetEnumerator();

}
internal class TasksIdsCollection : IEnumerable //for adding/removing dependency
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
   // static IEnumerable<int> ids = s_bl.Task.GetAllTasks();

    public IEnumerator GetEnumerator() => ids.GetEnumerator();

}
internal class ComplexityLevelCollection :IEnumerable //for the levels of complexity in engineer for the big listview
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums =
            (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();

}
internal class RealComplexityLevelCollection : IEnumerable//for the choosing of lvl (without none and all)
{
    static readonly IEnumerable<BO.EngineerExperience> s_real_enums =
            (Enum.GetValues(typeof(ComplexityCollection)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_real_enums.GetEnumerator();

}
public enum ComplexityCollection
{
    Beginner,
    Amature,
    Advanced,
    Expert,
    Master
}

