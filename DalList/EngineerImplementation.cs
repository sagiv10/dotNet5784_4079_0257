namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        Engineer? ifExists = DataSource.Config.FindEngineer(item._id);
        if (ifExists!= null)
        {
            if (ifExists._isActive == false)
            {
                Engineer updatedEngineer = item with { _isActive = true };
                int theIndex = DataSource.Config.FindIndexEngineer(ifExists._id);
                DataSource.Engineers.RemoveAt(theIndex);
                DataSource.Engineers.Add(updatedEngineer);
                return item._id;
            }
            if(ifExists._isActive ==true ) 
            {
                throw new Exception($"Engineer with ID={ifExists._id} already exists");
            }

        }
        Engineer newEngineer = item;
        DataSource.Engineers.Add(newEngineer);
        return item._id;
    }

    public void Delete(int id)
    {
        Engineer? oldEngineer = DataSource.Config.FindEngineer(id);
        if (oldEngineer != null && oldEngineer._isActive==true)
        {
            Engineer updatedEngineer = oldEngineer! with { _isActive = false};
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
        if(returnedValue != null && returnedValue._isActive == true)
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
            temp=new Engineer(item!._id, item!._cost, item!._email, item!._name, item!._level,item!._isActive);
            newList.Add(temp);
        }
        return newList;
    }

    public void Update(Engineer item)
    {
        Engineer? oldValue = DataSource.Config.FindEngineer(item._id);
        if(oldValue==null || oldValue._isActive==false)
        {
            throw new Exception($"Engineer with ID={item._id} does Not exist");
        }
        int theIndex = DataSource.Config.FindIndexEngineer(item._id);
        DataSource.Engineers[theIndex] = item;
    }
}

