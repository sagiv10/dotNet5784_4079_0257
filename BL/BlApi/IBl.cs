using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }

    public ITask Task { get; }

    public BO.ProjectStatus Status {  get; }

    public static void AutoScedule(DateTime startingDate) { }

    public static void ManualScedule(DateTime startingDate) { }
}
