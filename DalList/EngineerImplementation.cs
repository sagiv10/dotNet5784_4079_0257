namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        Engineer? ifExists = DataSource.Config.FindEngineer(item.Id);
        if (ifExists!= null)
        {
            if (ifExists.IsActive == false)
            {
                Engineer updatedEngineer = item with { IsActive = true };
                int theIndex = DataSource.Config.FindIndexEngineer(ifExists.Id);
                DataSource.Engineers.RemoveAt(theIndex);
                DataSource.Engineers.Add(updatedEngineer);
                return item.Id;
            }
            if(ifExists.IsActive ==true ) 
            {
                throw new Exception($"Engineer with ID={ifExists.Id} already exists");
            }

        }
        Engineer newEngineer = item;
        DataSource.Engineers.Add(newEngineer);
        return item.Id;
    }

    public void Delete(int id)
    {
        Engineer? oldEngineer = DataSource.Config.FindEngineer(id);
        if (oldEngineer != null && oldEngineer.IsActive==true)
        {
            Engineer updatedEngineer = oldEngineer! with { IsActive = false};
            int theIndex=DataSource.Config.FindIndexEngineer(id);
            DataSource.Engineers.RemoveAt(theIndex);
            DataSource.Engineers.Add(updatedEngineer);
            return;
        }
        throw new Exception($"Engineer with ID={id} does Not exist");
    }

    public Engineer? Read(int id)
    {
        Engineer? returnedValue = DataSource.Config.FindEngineer(id);
        if(returnedValue != null && returnedValue.IsActive == true)
        {
            return returnedValue;
        }
        else
        {
            return null;
        }

    }

    public List<Engineer?> ReadAll()
    {
        Engineer? temp;
        List<Engineer?> newList = new List<Engineer?>();
        foreach (var item in DataSource.Engineers)
        {
            temp=new Engineer(item!.Id, item!.Cost, item!.Email, item!.Name, item!.Level,true);
            newList.Add(temp);
        }
        return newList;
    }

    public void Update(Engineer item)
    {
        Engineer? oldValue = DataSource.Config.FindEngineer(item.Id);
        if(oldValue==null || oldValue.IsActive==false)
        {
            throw new Exception($"Engineer with ID={item.Id} does Not exist");
        }
        int theIndex = DataSource.Config.FindIndexEngineer(item.Id);
        DataSource.Engineers[theIndex] = item;
    }
}

