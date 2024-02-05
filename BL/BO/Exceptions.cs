namespace BO;

[Serializable]
public class BLNotFoundException : Exception
{
    public BLNotFoundException(string type, int id) : base($"{type} with id: {id} does not exist") { }
    public BLNotFoundException(string type, int id, Exception innerException)
                : base($"{type} with id {id} does not exist", innerException) { }
}
public class BLWrongInputException : Exception
{
    public BLWrongInputException(string messege) : base(messege){}
}
//---------------------------------------------------------------------
public class BLWrongIdException: BLWrongInputException
{
    public BLWrongIdException(int id) : base($"not valid id input") {}
}
public class BLWrongAliasException : BLWrongInputException
{
    public BLWrongAliasException(string alias) : base($"not valid Alias input") { }
}
public class BLWrongEmailException : BLWrongInputException
{
    public BLWrongEmailException(string email) : base($"not valid Email input") { }
}
public class BLWrongCostException : BLWrongInputException
{
    public BLWrongCostException(double cost) : base($"not valid Cost input") { }
}
public class BLTooEarlyException : BLWrongInputException
{
    public BLTooEarlyException(DateTime OptionalDate) : base($"your date is too early and not valid, here is a suggestion for a good date:{OptionalDate} ") { }
}
//---------------------------------------------------------------
public class BLAlreadyExistException : Exception
{
    public BLAlreadyExistException(string messege) : base(messege) { }
}
public class BLHasTaskException: BLAlreadyExistException
{
    public BLHasTaskException():base($"cannot assign a task because a task already exist") { }
}
public class BLExistProbException : BLAlreadyExistException
{
    public BLExistProbException(string type) : base($"{type} is already exist!") { }
}
//-------------------------------------------------------------------
public class BLEmptyDatabaseException : Exception
{
    public BLEmptyDatabaseException() : base("not enough data has been assigned to do this!)") { }
}
//-------------------------------------------------------------------




