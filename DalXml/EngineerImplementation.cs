namespace Dal;
using DalApi;
using DO;



internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";
    /// <summary>
    /// this method gets a new engineer and if he doesnt exist in the engineer xml file - add him. else throw an exception.
    /// </summary>
    /// <param name="item"> the new engineer we want to add</param>
    /// <returns>the id of the new engineer</returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Engineer item)
    {
        List<Engineer>? engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml); //get the list from the xml file
        int isExists = engineers.FindIndex(temp=>temp._id==item._id);
        if (isExists!=-1 && engineers[isExists]._isActive==true)
        {
            throw new DalAlreadyExistsException($"Engineer with ID={item._id} already exists");
        }
        if(isExists != -1 && engineers[isExists]._isActive == false)
        {
            engineers.RemoveAt(isExists);
        }
        Engineer newEngineer = item;
        engineers.Add(newEngineer);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml); //save the new list in the xml file
        return item._id;
    }

    /// <summary>
    /// this method set an engineer to not active if he exists(we can know his existence by given id), else throw an exeption.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Delete(int id)
    {
        List<Engineer>? engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml); //get the list from the xml file
        int isExists = engineers.FindIndex(temp => temp._id == id && temp._isActive==true);
        if (isExists ==-1)
        {
            throw new DalNotFoundException($"Engineer with ID={id} does Not exist");
        }
        Engineer updatedEngineer = engineers[isExists] with { _isActive = false };
        engineers.RemoveAt(isExists);
        engineers.Add(updatedEngineer);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml); //save the new list in the xml file
    }

    /// <summary>
    /// this method gets id and returns the engineer in the xml file that has this id, if not exists so return null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        List<Engineer>? engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml); //get the list from the xml file
        int isExists = engineers.FindIndex(temp => temp._id == id);
        if (isExists != -1 && engineers[isExists]._isActive == true)
        {
            //XMLTools.SaveListToXMLSerializer(engineers,   s_engineers_xml); //save the new list in the xml file
            return null;
        }
        //XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml); //save the new list in the xml file
        return engineers[isExists];
    }

    /// <summary>
    /// this method gets id and returns the engineer in the xml file that has this id, if not exists so return null.
    /// </summary>
    /// <param name="filter"> predicat that determine wich engineer we want to print</param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer>? engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml); //get the list from the xml file
        int isExists = engineers.FindIndex(temp=> filter(temp)&&temp._isActive==true);
        if (isExists == -1)
        {
            //XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml); //save the new list in the xml file
            return null;
        }
        //XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml); //save the new list in the xml file
        return engineers[isExists];
    }


    /// <summary>
    /// this method creats a new list of engineers with the same details at the xml file.
    /// </summary>
    /// <param name="filter"> predicat that determine wich engineers we want to return</param>
    /// <returns>the new list</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer>? engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml); //get the list from the xml file
        if (filter == null)
        {
            IEnumerable<Engineer?> newList = engineers.Select(item => item);
            //XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml); //save the new list in the xml file
            return newList;
        }
        else
        {
            IEnumerable<Engineer?> newList = engineers.Where(item => filter(item!));
            //XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml); //save the new list in the xml file
            return newList;
        }
    }

    /// <summary>
    /// this program gets a new engineer and removes the old engineer with the same id that in the xml file and gets the new one in. 
    /// </summary>
    /// <param name="item">the new engineer </param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Update(Engineer item)
    {
        List<Engineer>? engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml); //get the list from the xml file
        int isExists = engineers.FindIndex(temp => temp._id == item._id && temp._isActive == true);
        if (isExists == -1)
        {
            throw new DalNotFoundException($"Engineer with ID={item._id} does Not exist");
        }
        engineers[isExists] = item;
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml); //save the new list in the xml file
    }
    /// <summary>
    /// Delete all existing data from xml file to let us do a new initalize.
    /// </summary>
    public void DeleteAll()
    {
        List<Task> ListToWorkWith = XMLTools.LoadListFromXMLSerializer<Task>(s_engineers_xml);//get the list from the xml file to work with it
        ListToWorkWith.Clear();
        XMLTools.SaveListToXMLSerializer<Task>(ListToWorkWith, s_engineers_xml);//save the changes we did in the list we got in the start
    }
}
