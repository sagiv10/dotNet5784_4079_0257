namespace BO;

[Serializable]
public class BLNotFoundException : Exception
{
    public BLNotFoundException(string type, int id) : base($"{type} with id: {id} does not exist") { }
    public BLNotFoundException(string type, int id, Exception innerException)
                : base($"{type} with id {id} does not exist", innerException) { }
    public BLNotFoundException(string type) : base($"{type} that is answering those parameters does not exist") { }
}
public class BLWrongInputException : Exception
{
    public BLWrongInputException(string messege) : base(messege){}
}
//---------------------------------------------------------------------
public class BLWrongIdException: BLWrongInputException
{
    public BLWrongIdException() : base($"not valid id input") {}
}
public class BLWrongAliasException : BLWrongInputException
{
    public BLWrongAliasException() : base($"not valid Alias input") { }
}
public class BLWrongEmailException : BLWrongInputException
{
    public BLWrongEmailException(string email) : base($"the email {email} is not a valid Email input") { }
}
public class BLWrongCostException : BLWrongInputException
{
    public BLWrongCostException(double cost) : base($"the cost {cost} is not a valid Cost input") { }
}
public class BLTooEarlyException : BLWrongInputException
{
    public DateTime suggest;
    public BLTooEarlyException(DateTime OptionalDate) : base($"your date is too early and not valid, here is a suggestion for a good date:{OptionalDate} do you confirm (Y) or want to quit? (N)") { suggest = OptionalDate; }
}
//---------------------------------------------------------------
public class BLExistProbException : Exception
{
    public BLExistProbException(string messege) : base(messege) { }
    public BLExistProbException(string messege, DO.DalAlreadyExistsException ex) : base(messege, ex) { }
}
public class BLHasTaskException: BLExistProbException
{
    public BLHasTaskException(int id):base($"cannot assign a task to engineer with id {id} because a task already exist") { }
}
public class BLAlreadyExistException : BLExistProbException
{
    public BLAlreadyExistException(string type, int id) : base($"{type} with id: {id} is already exist!") { }
    public BLAlreadyExistException(string type, int id, DO.DalAlreadyExistsException ex) : base($"{type} with id: {id} is already exist!", ex) { }
}
//-------------------------------------------------------------------
public class BLEmptyDatabaseException : Exception
{
    public BLEmptyDatabaseException() : base("not enough data has been assigned to do this!)") { }
}
//-------------------------------------------------------------------




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
    public BLWrongStageException(int currentStage, int WantedStage) : base($"cannot do actions of stage {WantedStage} in {currentStage} stage") { }
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
    public BLNotAvialableTaskException(int engineerId, int taskId) : base($"the task with id {taskId} is not avialable to be assigned to engineer with id {engineerId}") { }
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
    public DateTime suggest;
    public BLDateSuggestionException(DateTime optionalDate) : base($"your date is not the optimal option, here is a suggestion for a better date {optionalDate}. enter Y t confirm and N else:)") { suggest = optionalDate; }
}

public class BLCannotAddCircularDependencyException : Exception
{
    public BLCannotAddCircularDependencyException(int dep, int depOn) : base($"cannot create dependency between {dep} and {depOn} due to circular dependency creation") { }
}





