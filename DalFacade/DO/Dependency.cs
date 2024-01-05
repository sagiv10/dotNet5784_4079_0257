namespace DO;
/// <summary>
/// record of dependency with the following parameters:
/// </summary>
/// <param name="Id">the id of the dependency</param>
/// //A cannot happan until B is done.
/// <param name="DependentTask">id of A</param>
/// <param name="DependsOnTask">id of B</param>
public record Dependency
(
    int Id, 
    int? DependentTask = null, 
    int? DependsOnTask = null
)
{ 
    public Dependency() : this(0) { }
}
