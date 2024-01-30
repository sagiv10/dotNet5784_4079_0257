namespace BlApi;


/// <summary>
/// the engineers access interface for the buissness layer
/// </summary>
internal interface IEngineer
{
    /// <summary>
    /// get all the engineers that follow the requirements
    /// </summary>
    /// <param name="filter"> the requirements </param>
    /// <returns> list of the engineers that follows the requirements </returns>
    public List<BO.Engineer> GetWantedEngineers(Func<DO.Engineer, bool>? filter=null);

    /// <summary>
    /// get the engineer with the required id
    /// </summary>
    /// <param name="id"> the id of the engineer we want </param>
    /// <returns> the engineer with the id we want </returns>
    /// <exception cref="BLNotFoundException"></exception>
    /// <exception cref="BLWrongIntInputException"></exception>
    /// <exception cref="BLWrongStringInputException"></exception>
    /// <exception cref="BLWrongDoubleInputException"></exception>
    public BO.Engineer GetEngineer(int id);

    /// <summary>
    /// adds new engineers to the system
    /// </summary>
    /// <param name="newEngineer"> the engineer we want to add </param>
    /// <exception cref="BLWrongIntInputException"></exception>
    public void AddEngineer(BO.Engineer newEngineer);

    /// <summary>
    /// delete an engineer from the system
    /// </summary>
    /// <param name="engineerId"> the engineer's id that we want to delete </param>
    /// <exception cref="BLWrongIntInputException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    public void DeleteEngineer(int  engineerId);

    /// <summary>
    /// updates an engineer from the system
    /// </summary>
    /// <param name="newEngineer"> the engineer we want to update </param>
    /// <exception cref="BLWrongIntInputException"></exception>
    /// <exception cref="BLWrongStringInputException"></exception>
    /// <exception cref="BLWrongDoubleInputException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    public void updateEngineer(BO.Engineer newEngineer);

    /// <summary>
    /// get the list of the avialable tasks to the requested engineer
    /// </summary>
    /// <returns> the list of the avialable tasks to the requested engineer </returns>
    /// <param name="id"> the id of the engineer  </param>
    /// <exception cref="BLWrongIntInputException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    public List<BO.Task> GetPotentialTasks(int id);
}
