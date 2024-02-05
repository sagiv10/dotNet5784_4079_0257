namespace BlApi;

/// <summary>
/// the engineers access interface for the buissness layer
/// </summary>
public interface IEngineer
{
    /// <summary>
    /// get all the engineers that follow the requirements
    /// </summary>
    /// <param name="filter"> the requirements </param>
    /// <returns> list of the engineers that follows the requirements </returns>
    public List<BO.Engineer> ReadAllEngineers(Func<BO.Engineer, bool>? filter=null);

    /// <summary>
    /// get the engineer with the required id
    /// </summary>
    /// <param name="id"> the id of the engineer we want </param>
    /// <returns> the engineer with the id we want </returns>
    /// <exception cref="BLNotFoundException"></exception>
    /// <exception cref="BLWrongIdInputException"></exception>
    public BO.Engineer ReadEngineer(int id);

    /// <summary>
    /// get the engineer that answer the requirments
    /// </summary>
    /// <param name="filter"> the requirment </param>
    /// <returns> the engineer with the id we want </returns>
    /// <exception cref="BLNotFoundException"></exception>
    public BO.Engineer ReadEngineer(Func<DO.Engineer, bool>? filter);

    /// <summary>
    /// adds new engineers to the system
    /// very important: we will always add new engineer with no task assigned no matter what will be at the field 'Task'.
    /// </summary>
    /// <param name="newEngineer"> the engineer we want to add </param>
    /// <exception cref="BLWrongIdInputException"></exception>
    /// <exception cref="BLWrongEmailInputException"></exception>
    /// <exception cref="BLWrongDoubleInputException"></exception>
    /// <exception cref="BLAlredyExistsException"></exception>
    public void CreateEngineer(BO.Engineer newEngineer);

    /// <summary>
    /// delete an engineer from the system
    /// </summary>
    /// <param name="engineerId"> the engineer's id that we want to delete </param>
    /// <exception cref="BLWrongIdInputException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    /// <exception cref="BLHasTaskException"></exception>
    public void DeleteEngineer(int  engineerId);

    /// <summary>
    /// updates an engineer from the system
    /// the update method will not change the task status of the engineer - only in assign\deassign\finish method, no matter what will be at the field 'Task'.
    /// </summary>
    /// <param name="newEngineer"> the engineer we want to update </param>
    /// <exception cref="BLWrongIdInputException"></exception>
    /// <exception cref="BLWrongEmailInputException"></exception>
    /// <exception cref="BLWrongCostInputException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    /// <exception cref="BLCannotLowerLevelException"></exception>
    public void UpdateEngineer(BO.Engineer newEngineer);

    /// <summary>
    /// get the list of the avialable tasks to the requested engineer
    /// </summary>
    /// <returns> the list of the avialable tasks to the requested engineer </returns>
    /// <param name="id"> the id of the engineer  </param>
    /// <exception cref="BLWrongIdInputException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    public List<BO.TaskInList> GetPotentialTasks(int id);

    /// <summary>
    /// assign engineer to task
    /// </summary>
    /// <param name="engineerId"> the id of the engineer  </param>
    /// <param name="taskId"> the id of the task  </param>
    /// <exception cref="BLNotFoundException"></exception>
    /// <exception cref="BLWrongIdException"></exception>
    /// <exception cref="BLHasAlredyTaskException"></exception>
    /// <exception cref="BLUnavailableTaskException"></exception>
    /// <exception cref="BLWrongStageException"></exception>

    public void AssignTask(int engineerId, int taskId);

    /// <summary>
    /// remove engineer from a task
    /// </summary>
    /// <param name="engineerId"> the id of the engineer  </param>
    /// <exception cref="BLWrongIdInputException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    /// <exception cref="BLNoTaskAssignedException"></exception>
    /// <exception cref="BLWrongStageException"></exception>
    public void DeAssignTask(int engineerId);

    /// <summary>
    /// return the task assigned to the engineer
    /// </summary>
    /// <param name="engineerId"> the id of the engineer  </param>
    /// <returns> the task assigned to the engineer </returns>
    /// <exception cref="BLWrongIdInputException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    /// <exception cref="BLNoTaskAssignedException"></exception>
    /// <exception cref="BLWrongStageException"></exception>
    public BO.TaskInEngineer ShowTask(int engineerId);

    /// <summary>
    /// finish the task of engineer
    /// </summary>
    /// <param name="engineerId"> the id of the engineer  </param>
    /// <exception cref="BLWrongIdInputException"></exception>
    /// <exception cref="BLNoTaskAssignedException"></exception>
    /// <exception cref="BLNotFoundException"></exception>
    /// <exception cref="BLWrongStageException"></exception>

    public void FinishTask(int engineerId);

}
