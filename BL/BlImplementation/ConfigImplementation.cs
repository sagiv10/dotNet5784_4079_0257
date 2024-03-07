namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class ConfigImplementation : IConfig
{
    private readonly IBl _bl;
    internal ConfigImplementation(IBl bl) => _bl = bl;

    private DalApi.IDal _dal = DalApi.Factory.Get;//with this field we can access to the methods of task in dal. the way to use the method is being chosen inside factory.

    /// <summary>
    /// hepling method that checks if we got a circular dependency from the new dependency
    /// </summary>
    /// <param name="dependent"> the id of the dependent task </param>
    /// <param name="dependOn"> the id of the dependent On task </param>
    /// <returns></returns>
    public bool checkCircularDependency(int dependent, int dependOn)
    {
        IEnumerable<int> dependents = (from dep in _dal.Dependency.ReadAll(d => d._dependsOnTask == dependent) //get all the tasks that depends on our dependent task
                                      select (int)dep._dependentTask!).ToList();
        if (dependents.Count(id => id == dependOn) != 0) //if our dependent on task is in there - then wev'e got circular  dependency
        {
            return true;
        }
        foreach (int id in dependents) //search for the circular dependency in the dependent tasks
        {
            if (checkCircularDependency(id, dependOn) == true)
            {
                return true;
            }
        }
        return false;

    }

    public void AddDependency(int dependentTask, int dependsOnTask)
    {
        if (dependsOnTask == dependentTask)
        {
            throw new BLCannotAddTheIdentityDependency();
        }
        if (_dal.Task.Read(dependsOnTask) == null)
        {
            throw new BLNotFoundException("task", dependsOnTask);
        }
        if (_dal.Task.Read(dependentTask) == null)
        {
            throw new BLNotFoundException("task", dependentTask);

        }
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException((int)_dal.Project.getProjectStatus(), 1);
        }
        if (checkCircularDependency(dependentTask, dependsOnTask) == true)
        {
            throw new BLCannotAddCircularDependencyException(dependentTask, dependsOnTask);
        }
        if (_dal.Dependency.Read(d => d._dependsOnTask == dependsOnTask && d._dependentTask == dependentTask) != null)
        {
            throw new BLAlreadyExistException("dependency", _dal.Dependency.Read(d => d._dependsOnTask == dependsOnTask && d._dependentTask == dependentTask)!._id);
        }
        _dal.Dependency.Create(new DO.Dependency(0, dependentTask, dependsOnTask));
    }

    public void DeleteDependency(int dependentTask, int dependsOnTask)
    {
        if (_dal.Task.Read(dependsOnTask) == null)
        {
            throw new BLNotFoundException("task", dependsOnTask);
        }
        if (_dal.Task.Read(dependentTask) == null)
        {
            throw new BLNotFoundException("task", dependentTask);

        }
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)
        {
            throw new BLWrongStageException((int)_dal.Project.getProjectStatus(), 1);
        }
        DO.Dependency? dep = _dal.Dependency.Read(d => d._dependsOnTask == dependsOnTask && d._dependentTask == dependentTask);
        if (dep == null)
        {
            throw new BLNotFoundException("dependency", -1);
        }
        _dal.Dependency.Delete(dep._id);
    }

    public void StartSchedule(DateTime StartingDateOfProject)
    {
        if ((BO.ProjectStatus)_dal.Project.getProjectStatus() != BO.ProjectStatus.Planning)// this method can be acceced only in the planning stage
        {
            throw new BLWrongStageException(_dal.Project.getProjectStatus(), (int)BO.ProjectStatus.Planning);
        }
        _dal.Project.setProjectStatus((int)BO.ProjectStatus.Sceduling);
        _dal.Project.setStartingDate(StartingDateOfProject);
    }

    public int getProjectStatus()
    {
        return _dal.Project.getProjectStatus();
    }

    public DateTime? getStartingDate()
    {
        return _dal.Project.getStartingDate();
    }
    public int AddDependencies(List<int> dependendsOns, int dependentId, int endIndex)
    {
        int i = 0;
        for (; i < endIndex; i++)
        {
            try
            {
                AddDependency(dependentId, dependendsOns[i]);
            }
            catch (Exception)
            {
                return i;
            }
        }
        return i;
    }


    public int DeleteDependencies(List<int> dependendsOns, int dependentId, int endIndex)
    {
        int i = 0;
        for (; i < endIndex; i++)
        {
            try
            {
                DeleteDependency(dependentId, dependendsOns[i]);
            }
            catch (Exception)
            {
                return i;
            }
        }
        return i;
    }
    public int ParseToInt(string integer, string field)
    {
        int finalNum;
        return int.TryParse(integer, out finalNum) ? finalNum : throw new BO.BLWrongInputException("you entered wrong input when integer should be entered to " + field + "!");
    }

    public Double ParseToDouble(string number, string field)
    {
        double finalNum;
        return double.TryParse(number, out finalNum) ? finalNum : throw new BO.BLWrongInputException("you entered wrong input when number should be entered to " + field + "!");
    }

}
