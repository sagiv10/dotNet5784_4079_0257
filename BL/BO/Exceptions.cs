namespace BO;

[Serializable]
public class BLNotFoundException : Exception
{
    public BLNotFoundException(string type, int id) : base($"{type} with id: {id} does not exist") { }
    public BLNotFoundException(string type, int id, Exception innerException)
                : base($"{type} with id {id} does not exist", innerException) { }
}

public class BLCannotDeleteException : Exception
{
    public BLCannotDeleteException(string messege) : base(messege) { }
}

public class BLCannotDeleteHasDependencyException : BLCannotDeleteException
{
    public BLCannotDeleteHasDependencyException(int id) : base($"cannot delete the task with id: {id} because he has a dependency") { }
}

public class BLCannotDeleteHasTasksException : BLCannotDeleteException
{
    public BLCannotDeleteHasTasksException(int id) : base($"cannot delete the engineer with id: {id} because he has worked or working on a task") { }
}

public class BLWrongStageException : Exception
{
    public BLWrongStageException(int currentStage, int WantedStage) : base($"cannot do stage {WantedStage} in {currentStage} stage") { }
}

public class BLCannotLowerLevelException : Exception
{
    public BLCannotLowerLevelException(int newLevel) : base($"cannot lower this engineer's level to {newLevel}") { }
}

public class BLDoesNotHasTaskException : Exception
{
    public BLDoesNotHasTaskException(int engineerId) : base($"the engineer with id {engineerId} does not has a task assigned") { }
}

public class BLNotAvialableTaskException : Exception
{
    public BLNotAvialableTaskException(int taskId) : base($"the task with id {taskId} is not avialable to be assigned") { }
}

public class BLCannotScheduleOneFormerUnscheduledException : BLCannotScheduleException
{
    public BLCannotScheduleOneFormerUnscheduledException(int taskId) : base($"cannot schedule the task because the task with id: {taskId} need to be scheduled before") { }
}

public class BLCannotScheduleMoreTanOneFormerUnscheduledException : BLCannotScheduleException
{
    public BLCannotScheduleMoreTanOneFormerUnscheduledException(int numOfTasks) : base($"cannot schedule the task because there is {numOfTasks} tasks that need to be scheduled before") { }
}

public class BLCannotScheduleException : Exception
{
    public BLCannotScheduleException(string messege) : base(messege) { }
}

public class BLDateSuggestionException : Exception
{
    public BLDateSuggestionException(DateTime optionalDate) : base("your date is not the optimal option, here is a suggestion for a better date:)") { }
}







