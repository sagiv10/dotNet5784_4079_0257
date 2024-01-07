namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (DataSource.Config.FindEngineer(item.Id) == null)
        {
            throw new NotImplementedException("אובייקט מסוג T עם ID כזה כבר קיים");
        }
        Engineer newEngineer = item;
        DataSource.Engineers.Add(newEngineer);
        return item.Id;
    }

    public void Delete(int id)
    {
        int theIndex = DataSource.Config.FindIndexEngineer(id);
        if (theIndex != -1)
        {
            Engineer oldEngineer = Read(id)!;
            Engineer updatedEngineer = oldEngineer! with { IsActive = false};
            DataSource.Engineers.RemoveAt(theIndex);
            DataSource.Engineers.Add(updatedEngineer);
            return;
        }
        throw new NotImplementedException("אובייקט מסוג T עם ID כזה לא קיים");
    }

    public Engineer? Read(int id)
    {
        return DataSource.Config.FindEngineer(id);
    }

    public List<Engineer> ReadAll()
    {
        Engineer temp;
        List<Engineer> newList = new List<Engineer>();
        foreach (var item in DataSource.Engineers)
        {
            temp=new Engineer(item!.Id, item!.Cost, item!.Email, item!.Name, item!.Level);
            newList.Add((Engineer)item);
        }
        return newList;
    }

    public void Update(Engineer item)
    {
        int theIndex=DataSource.Config.FindIndexEngineer(item.Id);
        if(theIndex==-1)
        {
            throw new NotImplementedException("אובייקט מסוג T עם ID כזה לא קיים");
        }
        DataSource.Engineers[theIndex] = item;
    }
}

