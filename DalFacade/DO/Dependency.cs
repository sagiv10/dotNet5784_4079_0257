namespace DO;
/// <summary>
/// record of dependency with the following parameters:
/// </summary>
/// <param name="_id">the id of the dependency</param>
/// <param name="_dependentTask">id of the mission that dependet</param>
/// <param name="_dependsOnTask">id of the mission that dependes on</param>
public record Dependency
(
    int _id, 
    int? _dependentTask = null, 
    int? _dependsOnTask = null
)
{ 
    public Dependency() : this(0) { }
}
