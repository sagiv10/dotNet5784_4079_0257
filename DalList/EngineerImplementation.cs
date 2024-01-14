namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// this method gets a new engineer and if he doesnt exist in the engineer list- add him. else throw.
    /// </summary>
    /// <param name="item"> the new engineer we want to add</param>
    /// <returns>the id of the new engineer</returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Engineer item)
    {
        Engineer? ifExists = DataSource.Config.FindEngineer(item._id);
        if (ifExists!= null)
        {
            if (ifExists._isActive == false)//if we didnt found him and its ok to add him 
            {
                Engineer updatedEngineer = item with { _isActive = true };
                int theIndex = DataSource.Config.FindIndexEngineer(ifExists._id);
                DataSource.Engineers.RemoveAt(theIndex);
                DataSource.Engineers.Add(updatedEngineer);
                return item._id;
            }
            if(ifExists._isActive ==true ) // if we found him and we cant add him
            {
                throw new DalAlreadyExistsException($"Engineer with ID={ifExists._id} already exists");
            }

        }
        Engineer newEngineer = item;
        DataSource.Engineers.Add(newEngineer);
        return item._id;
    }
    /// <summary>
    /// this method set an engineer to not active if he exists(we can know his existence by given id), else throw.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Delete(int id)
    {
        Engineer? oldEngineer = DataSource.Config.FindEngineer(id);
        if (oldEngineer != null && oldEngineer._isActive==true) //if the given id is in the list and he active 
        {
            Engineer updatedEngineer = oldEngineer! with { _isActive = false};//create new one with the same details but not active.
            int theIndex=DataSource.Config.FindIndexEngineer(id);
            DataSource.Engineers.RemoveAt(theIndex);
            DataSource.Engineers.Add(updatedEngineer);
            return;
        }
        throw new DalNotFoundException($"Engineer with ID={id} does Not exist");
    }

    /// <summary>
    /// this method gets id and returns the engineer in the list that has this id, if not exists so return null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        Engineer? returnedValue = DataSource.Config.FindEngineer(id);
        if (returnedValue != null && returnedValue._isActive == true)//if the id is exist in the list and the founded engineer is active.
        {
            return returnedValue;
        }
        else
        {
            return null;
        }

    }

    /// <summary>
    /// this method gets id and returns the engineer in the list that has this id, if not exists so return null.
    /// </summary>
    /// <param name="filter"> predicat that determine wich engineer we want to print</param>
    /// <returns></returns>
    public DO.Engineer? Read(Func<DO.Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter!);
    }

    /// <summary>
    /// this method creats a new list of engineers with the same details of the old list.
    /// </summary>
    /// <param name="filter"> predicat that determine wich engineers we want to return</param>
    /// <returns>the new list</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter)
    {
        if (filter == null)
        {
            IEnumerable<Engineer?> newList = DataSource.Engineers.Select(item => item);
            return newList;
        }
        else
        {
            IEnumerable<Engineer?> newList = DataSource.Engineers.Where(item => filter(item!));
            return newList;
        }
    }
    /// <summary>
    /// this program gets a new engineer and removes the old engineer with the same id that in the list and gets the new one in. 
    /// </summary>
    /// <param name="item">the new engineer </param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Update(Engineer item)
    {
        Engineer? oldValue = DataSource.Config.FindEngineer(item._id);
        if(oldValue==null || oldValue._isActive==false) //if the engineer from the list isn't active or not found 
        {
            throw new DalNotFoundException($"Engineer with ID={item._id} does Not exist");
        }
        int theIndex = DataSource.Config.FindIndexEngineer(item._id);
        DataSource.Engineers[theIndex] = item;
    }
}

