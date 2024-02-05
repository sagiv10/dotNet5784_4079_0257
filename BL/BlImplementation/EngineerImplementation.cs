﻿namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Xml.Linq;

internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// helping method that gets an DO engineer and returns BO engineer according to the DO engineer. we assume that the function will be called only to engineers with isActive=true
    /// </summary>
    /// <param name="oldEngineer"> the DO engineer </param>
    /// <returns> BO engineer </returns>
    private BO.Engineer DoToBoEngineer(DO.Engineer oldEngineer)
    {
        DO.Task? workingOn = _dal.Task.Read(task => task._engineerId == oldEngineer._id && task._completeDate == null);
        return new BO.Engineer(
            oldEngineer._id,
            oldEngineer._name,
            oldEngineer._email,
            (BO.EngineerExperience)(oldEngineer._level!),
            oldEngineer._cost,
            workingOn != null ? new BO.TaskInEngineer(workingOn._id, workingOn._alias) : null
            );
    }

    /// <summary>
    /// helping method that gets an BO engineer and returns DO engineer according to the BO engineer.
    /// </summary>
    /// <param name="oldEngineer"> the BO engineer </param>
    /// <returns> DO engineer </returns>
    private DO.Engineer BoToDoEngineer(BO.Engineer oldEngineer)
    {
        return new DO.Engineer(
            oldEngineer.Id,
            oldEngineer.Cost,
            oldEngineer.Email,
            oldEngineer.Name,
            (DO.ComplexityLvls)(oldEngineer.Level!),
            true //of course he is active we've just made him...
            );
    }

    /// <summary>
    /// helping method that gets an BO engineer and throws exceptions according to it's wrong input
    /// </summary>
    /// <param name="engineer"> the BO engineer </param>
    /// <exception cref="BLWrongIdInputException"></exception>
    /// <exception cref="BLWrongEmailInputException"></exception>
    /// <exception cref="BLWrongCostInputException"></exception>
    private void checkEngineerInput(BO.Engineer engineer)
    {
        if (engineer.Id < 0)
        {
            throw new BLWrongIdInputException();
        }
        if(engineer.Cost < 0)
        {
            throw new BLWrongCostInputException();
        }
        try
        {
            MailAddress mailAddress = new MailAddress(engineer.Email); //try to make an mailAdress entity, if the email adress id invalid then a format exception will be thrown.
        }
        catch(FormatException ex)
        {
            throw new BLWrongEmailInputException();
        }

    }

    /// <summary>
    /// helping method that checks if all the previous task to one task has been done or not *only in the execution stage!*
    /// input is alredy valid
    /// </summary>
    /// <param name="task"> the task </param>
    /// <returns> inducation if the task is available </returns>
    private bool isEnabeled(DO.Task task)
    {
        return !((from dep in _dal.Dependency.ReadAll(d => d._dependentTask == task._id)
               where _dal.Task.Read((int)dep._dependsOnTask!)!._completeDate != null
               select true).Any());
    }

    public void CreateEngineer(BO.Engineer newEngineer)
    {
        try
        {
            checkEngineerInput(newEngineer);
        }
        catch (BLWrongInputException ex) //can be one of 3: id, cost or email exception
        {
            throw ex;
        }
        DO.Engineer newDOEngineer = BoToDoEngineer(newEngineer);  //convert the engineer to do entity
        try
        {
            _dal.Engineer.Create(newDOEngineer); //try to create it
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BLAlredyExistsException(newEngineer.Id, ex); //if he alredy exists
        }
    }

    public void DeleteEngineer(int id)
    {
        if (id < 0)//check if the id is valid
        {
            throw new BLWrongIdInputException();
        }
        if(_dal.Engineer.Read(id) == null) //check if he exists in the DAL
        {
            throw new BLNotFoundException(id);
        }
        if(_dal.Task.ReadAll(t => t._engineerId == id).Count() != 0)
        {
            throw new BLHasTasksException(ifExists.Id); //if this did not thrown yet  it means that he has assigned task
        }
        _dal.Engineer.Delete(id);
    }

    public List<BO.Engineer> ReadAllEngineers(Func<BO.Engineer, bool>? filter = null)
    {
        return (from engineer in _dal.Engineer.ReadAll((filter != null) ? en => filter(DoToBoEngineer(en)) : null)
               select DoToBoEngineer(engineer)).ToList();
    }

    public BO.Engineer ReadEngineer(int id)
    {
        if(id< 0) //check if the id is valid
        {
            throw new BLWrongIdInputException();
        }
        DO.Engineer? engineer = _dal.Engineer.Read(id);
        return engineer == null ? throw new BLNotFoundException(id) : DoToBoEngineer(engineer);
    }

    public BO.Engineer ReadEngineer(Func<DO.Engineer, bool>? filter)
    {
        DO.Engineer? engineer = _dal.Engineer.Read(filter!);
        return engineer == null ? throw new BLNotFoundException() : DoToBoEngineer(engineer);
    }

    public void UpdateEngineer(BO.Engineer newEngineer)
    {
        try
        {
            checkEngineerInput(newEngineer);
        }
        catch (BLWrongInputException ex) //can be one of 3: id, cost or email exception
        {
            throw ex;
        }
        DO.Engineer? originalEngineer = _dal.Engineer.Read(newEngineer.Id);
        if(originalEngineer == null)
        {
            throw new BLNotFoundException(newEngineer.Id); //if he not exists
        }
        if((int)originalEngineer!._level! > (int)newEngineer.Level) //cannot lower level of engineer
        {
            throw new BLCannotLowerLevelException();
        }
        DO.Engineer newDOEngineer = BoToDoEngineer(newEngineer);  //convert the engineer to do entity
        _dal.Engineer.Update(newDOEngineer); //update the engineer
    }

    public void DeAssignTask(int id)
    {
        if (id <= 0)
        {
            throw new BLWrongIdInputException(id);
        }

        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Execution)
        {
            throw new BLWrongStageException();
        }

        if (_dal.Engineer.Read(id) == null)
        {
            throw new BLNotFoundException(id);
        }
        DO.Task? hisTask = _dal.Task.Read(t => t._engineerId == id);
        if (hisTask == null)
        {
            throw new BLNoTaskAssignedException(id);
        }
        DO.Task unassignedTask = hisTask with { _engineerId = null };
        _dal.Task.Update(unassignedTask);
    }

    public void AssignTask(int engineerId, int taskId)
    {
        if (engineerId <= 0)
        {
            throw new BLWrongIdInputException(engineerId);
        }

        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Execution)
        {
            throw new BLWrongStageException();
        }

        if (taskId <= 0)
        {
            throw new BLWrongIdInputException(taskId);
        }

        DO.Task? theTask = _dal.Task.Read(taskId);
        DO.Engineer? theEngineer = _dal.Engineer.Read(engineerId);
        if (theEngineer == null)
        {
            throw new BLNotFoundException(engineerId);
        }
        if (theTask == null)
        {
            throw new BLNotFoundException(taskId);
        }
        try
        {
            ShowTask(engineerId); //try to see if the engineer alredy has a task assigned
            throw new BLHasAlredyTaskException(engineerId); //if we got herethen he has a task assigned
        }
        catch(BLNoTaskAssignedException ex) //if we got here then the engineer doesnt have a task assigned
        {
            //everything here its okay!:)
        }
        catch (BLHasAlredyTaskException ex)
        {
            throw ex;
        }

        if(!isEnabeled(theTask) || theTask._completeDate != null || theTask._complexity > theEngineer._level || theTask._engineerId != null) //if the task is not enabled, or completed, or not in his level, or has anothe engineer then we must throw an exception
        {
            throw new BLUnavailableTaskException(engineerId, taskId);
        }
        DO.Task assignedTask = theTask with
        {
            _engineerId = engineerId,
            _startDate = theTask._startDate ?? DateTime.Now
        };
        _dal.Task.Update(assignedTask);
    }

    public List<BO.TaskInList> GetPotentialTasks(int id)
    {
        if(id <= 0)
        {
            throw new BLWrongIdInputException(id);
        }

        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Execution)
        {
            throw new BLWrongStageException();
        }

        try
        {
            DO.Engineer engineer = _dal.Engineer.Read(id)!;
            return (from task in _dal.Task.ReadAll(t=> isEnabeled(t) && t._completeDate == null && t._complexity<=engineer._level && t._engineerId == null ) //all the missions that all there privious one has been done, in the right level, still undone and doesn't has alredy engineer
                   select new BO.TaskInList(task._id, task._description, task._alias, BO.Status.Scheduled )).ToList();
        }
        catch(DalNotFoundException ex)
        {
            throw new BLNotFoundException(ex);
        }
    }

    public BO.TaskInEngineer ShowTask(int id)
    {
        if (id <= 0)
        {
            throw new BLWrongIdInputException(id);
        }

        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Execution)
        {
            throw new BLWrongStageException();
        }

        if (_dal.Engineer.Read(id) == null)
        {
            throw new BLNotFoundException(id);
        }
        DO.Task? hisTask = _dal.Task.Read(t => t._engineerId == id);
        if (hisTask == null)
        {
            throw new BLNoTaskAssignedException(id);
        }
        return new BO.TaskInEngineer(hisTask._id, hisTask._alias);
    }

    public void FinishTask(int id)
    {
        if (id <= 0)
        {
            throw new BLWrongIdInputException(id);
        }

        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Execution)
        {
            throw new BLWrongStageException();
        }

        if (_dal.Engineer.Read(id) == null)
        {
            throw new BLNotFoundException(id);
        }
        DO.Task? hisTask = _dal.Task.Read(t => t._engineerId == id);
        if (hisTask == null)
        {
            throw new BLNoTaskAssignedException(id);
        }
        DO.Task doneTask = hisTask with { _completeDate = DateTime.Now};
        _dal.Task.Update(doneTask);
    }
}
