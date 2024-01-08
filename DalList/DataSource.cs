using DO;

namespace Dal;

internal static class DataSource
{
    internal static List<DO.Task?> Tasks { get; } =new();
    internal static List<DO.Engineer?> Engineers { get; } =new();
    internal static List<DO.Dependency?> Dependencies { get; } =new();

    internal static class Config
    {
        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
        
        internal static DO.Dependency? FindDependency(int wantedId)
        {
            foreach (Dependency? temp in DataSource.Dependencies)
            {
                if (temp!=null && temp.Id == wantedId)
                {
                    return temp;
                }
            }
            return null;
        }

        internal static DO.Engineer? FindEngineer(int wantedId)
        {
            foreach (Engineer? temp in DataSource.Engineers)
            {
                if (temp!=null && temp.Id == wantedId)
                {
                    return temp;
                }
            }
            return null;
        }
        internal static DO.Task? FindTask(int wantedId)
        {
            foreach (var temp in DataSource.Tasks)
            {
                if (temp != null && temp.Id == wantedId)
                {
                    return temp;
                }
            }
            return null;
        }

        internal static int FindIndexDependency(int id)
        {
            int counter = 0;
            foreach(var temp in DataSource.Dependencies)
            {
                if(temp!=null && temp.Id == id)
                {
                    return counter;
                }
            }
            return -1;
        }

        internal static int FindIndexEngineer(int id)
        {
            int counter = 0;
            foreach (var temp in DataSource.Engineers)
            {
                if (temp != null && temp.Id == id)
                {
                    return counter;
                }
            }
            return -1;
        }

        internal static int FindIndexTasks(int id)
        {
            int counter = 0;
            foreach (var temp in DataSource.Engineers)
            {
                if (temp != null && temp.Id == id)
                {
                    return counter;
                }
            }
            return -1;
        }

    }
}

