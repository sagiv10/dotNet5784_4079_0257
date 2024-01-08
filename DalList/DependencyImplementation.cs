﻿namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int newId = DataSource.Config.NextDependencyId;
        Dependency newDependency=item with { Id = newId };
        DataSource.Dependencies.Add(newDependency);
        return newId;
    }

    public void Delete(int id)
    {
        int theIndex = DataSource.Config.FindIndexDependency(id);
        if (theIndex != -1)
        {
            DataSource.Dependencies.RemoveAt(theIndex);
            return;
        }
        throw new Exception($"Dependency with ID={id} does Not exist");
    }

    public Dependency? Read(int id)
    {
        return DataSource.Config.FindDependency(id);
    }

    public List<Dependency?> ReadAll()
    {
        Dependency? temp;
        List<Dependency?> newList = new List<Dependency?>();
        foreach (var item in DataSource.Dependencies)
        {
            temp = new Dependency(item!.Id, item!.DependentTask,item!.DependsOnTask)??null;
            newList.Add((Dependency?)item);
        }
        return newList;
    }

    public void Update(Dependency item)
    {
        int theIndex = DataSource.Config.FindIndexDependency(item.Id);
        if (theIndex == -1)
        {
            throw new Exception($"Dependency with ID={item.Id} does Not exist");
        }
        DataSource.Dependencies[theIndex] = item;
    }
}
