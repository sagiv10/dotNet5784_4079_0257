namespace DO;

public record Engineer
(
    int Id,// id of the engineer
    string? Email = null,//Email of the engineer
    double? Cost = null,//salary for hour of the engineer 
    string? Name = null,//name of the engineer 
    DO.EngineerExperience? Level = null//skill level of the engineer 
)
{
    public Engineer() : this(0) { }
}
