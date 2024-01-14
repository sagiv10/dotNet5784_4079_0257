namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// this method gets a dependency and gets him into the dependency list with a new id from the staic property. in the end returnes the new id.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        int newId = DataSource.Config.NextDependencyId;
        Dependency newDependency=item with { _id = newId };
        DataSource.Dependencies.Add(newDependency);
        return newId;
    }

    /// <summary>
    /// this method removes a dependency from the list by a given id. if we didnt find the id in the list so throw 
    /// </summary>
    /// <param name="id">the id of the old dependency we want to remove </param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Delete(int id)
    {
        int theIndex = DataSource.Config.FindIndexDependency(id);
        if (theIndex != -1)
        {
            DataSource.Dependencies.RemoveAt(theIndex);
            return;
        }
        throw new DalNotFoundException($"Dependency with ID={id} does Not exist");
    }

    /// <summary>
    /// this method returns the dependency with the id it got.
    /// </summary>
    /// <param name="id">the id of the dependency we want to find </param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        return DataSource.Config.FindDependency(id);
    }

    /// <summary>
    /// this method returns the dependency with the id it got.
    /// </summary>
    /// <param name="filter"> predicat that determine wich dependency we want to print</param>
    /// <returns></returns>
    /// 
    public DO.Dependency? Read(Func<DO.Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(filter!);
    }

    /// <summary>
    /// this method creats a new dependency list and returns him. 
    /// </summary>
    /// <param name="filter"> predicat that determine wich dependencies we want to return</param>
    /// <returns>the new list </returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter)
    {
        if (filter== null) 
        {
            IEnumerable<Dependency?> newList = DataSource.Dependencies.Select(item => item);
            return newList;
        }
        else
        {
            IEnumerable<Dependency?> newList = DataSource.Dependencies.Where(item => filter(item!));
            return newList;
        }
    }

    /// <summary>
    /// this method gets a new dependency and makes the list from now and on the point on the new dependency and not on the old dependency that already was in the list with the same id as the new we got.
    /// </summary>
    /// <param name="item">the new dependency</param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Update(Dependency item)
    {
        int theIndex = DataSource.Config.FindIndexDependency(item._id);
        if (theIndex == -1)
        {
            throw new DalNotFoundException($"Dependency with ID={item._id} does Not exist");
        }
        DataSource.Dependencies[theIndex] = item;
    }
}
