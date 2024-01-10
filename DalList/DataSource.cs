using DO;

namespace Dal;

internal static class DataSource
{
    internal static List<DO.Task?> Tasks { get; } = new();
    internal static List<DO.Engineer?> Engineers { get; } = new();
    internal static List<DO.Dependency?> Dependencies { get; } = new();

    internal static class Config
    {
        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
        /// <summary>
        /// this method returns a dependency by the id we gave it. if not exists return null.
        /// </summary>
        /// <param name="wantedId">the id of the dependency we want to return</param>
        /// <returns>the wanted dependency</returns>
        internal static DO.Dependency? FindDependency(int wantedId)
        {
            foreach (Dependency? temp in DataSource.Dependencies)//go all over the dependency list 
            {
                if (temp != null && temp._id == wantedId)//if we found the dependency in the list with the correct id 
                {
                    return temp;
                }
            }
            return null;
        }
        /// <summary>
        /// this method returns a engineer by the id we gave it. if not exists return null.
        /// </summary>
        /// <param name="wantedId">the id of the engineer we want to return</param>
        /// <returns>the wanted engineer</returns>
        internal static DO.Engineer? FindEngineer(int wantedId)
        {
            foreach (Engineer? temp in DataSource.Engineers)
            {
                if (temp != null && temp._id == wantedId)//if we found the engineer in the list with the correct id
                {
                    return temp;
                }
            }
            return null;
        }

        /// <summary>
        /// this method returns a task by the id we gave it. if not exists return null.
        /// </summary>
        /// <param name="wantedId">the id of the task we want to return</param>
        /// <returns>the wanted engineer</returns>
        internal static DO.Task? FindTask(int wantedId)
        {
            foreach (var temp in DataSource.Tasks)//go all over the list of task
            {
                if (temp != null && temp._id == wantedId)//if we found the task in the list with the correct id
                {
                    return temp;
                }
            }
            return null;
        }
        /// <summary>
        /// this method returns the index of a dependency from the list with the id we got, if not exist in the list so returns -1.
        /// </summary>
        /// <param name="id">the id of the dependency we want to return his index </param>
        /// <returns>the index</returns>
        internal static int FindIndexDependency(int id)
        {
            int counter = 0;
            foreach (var temp in DataSource.Dependencies)//go all over the list of dependency
            {
                if (temp != null && temp._id == id)//if we didnt find yet the dependency we want with the same id we got 
                {
                    return counter;
                }
                counter++; //count how many failed checks we had 
            }
            return -1; // if didnt find
        }

        /// <summary>
        /// this method returns the index of a engineer from the list with the id we got, if not exist in the list so returns -1.
        /// </summary>
        /// <param name="id">the id of the engineer we want to return his index </param>
        /// <returns>the index</returns>
        internal static int FindIndexEngineer(int id)
        {
            int counter = 0;
            foreach (var temp in DataSource.Engineers)//go all over the list of engineer
            {
                if (temp != null && temp._id == id)//if we didnt find yet the engineer we want with the same id we got 
                {
                    return counter;
                }
                counter++;//count how many failed checks we had 
            }
            return -1;// if didnt find
        }

        /// <summary>
        /// this method returns the index of a task from the list with the id we got, if not exist in the list so returns -1.
        /// </summary>
        /// <param name="id">the id of the task we want to return his index </param>
        /// <returns>the index</returns>
        internal static int FindIndexTasks(int id)
        {
            int counter = 0;
            foreach (var temp in DataSource.Tasks)//go all over the list of task
            {
                if (temp != null && temp._id == id)//if we didnt find yet the task we want with the same id we got 
                {
                    return counter;
                }
                counter++;//count how many failed checks we had 
            }
            return -1;// if didnt find
        }
    }
}

