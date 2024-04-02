namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    /// <summary>
    /// find an XElement with some id in XElement collection and returns it by refference
    /// </summary>
    /// <param name="elements"> the collection of XElements </param>
    /// <param name="requestedId"> the id to look for </param>
    /// <returns> the XElement with this id by reference </returns>
    private static XElement? findXElement(IEnumerable<XElement> elements, int requestedId) //helping method that gets an id and find in the XElement array the index of the id
    {
        foreach(var xel in elements)
        {
            XElement? idSon = xel.Element("_id"); //if the id field exists then idSon has it, if not it will be null
            if (idSon != null && int.Parse(idSon.Value) == requestedId)
            {//the other parts of the program made the input that it will be converticle no int so we wont do try parse
                return xel; 
            }
        }
        return null;
    }

/// <summary>
/// helping method that gets an Dependency and make him as an XElement and when the field is null then is don'd create him
/// </summary>
/// <param name="item"> the origin Dependency </param>
/// <returns> the new created XElement</returns>
    private static XElement createNullableXElement(Dependency item)
    {
        if (item._dependentTask == null)
        {
            if (item._dependsOnTask == null)
            {
                return new XElement("Dependency", new XElement("_id", item._id));
            }
            else
            {
                return new XElement("Dependency", new XElement("_id", item._id), new XElement("_dependsOnTask", item._dependsOnTask));
            }
        }
        else
        {
            if (item._dependsOnTask == null)
            {
                return new XElement("Dependency", new XElement("_id", item._id), new XElement("_dependentTask", item._dependentTask));
            }
            else
            {
                return new XElement("Dependency", new XElement("_id", item._id), new XElement("_dependentTask", item._dependentTask), new XElement("_dependsOnTask", item._dependsOnTask));

            }
        }
    }

    /// <summary>
    /// helping method that creates new Dependency from an XElement
    /// </summary>
    /// <param name="Xele"> the XElement </param>
    /// <returns> new Dependency </returns>
    private static DO.Dependency CreateDependecyFromXElement(XElement Xele)
    {
        return new Dependency(
                   (int)XMLTools.ToIntNullable(Xele, "_id")!,
                   XMLTools.ToIntNullable(Xele, "_dependentTask"),
                   XMLTools.ToIntNullable(Xele, "_dependsOnTask"));
    }

    /// <summary>
    /// this method gets a dependency and gets him into the dependency xml file with a new id from the staic property. in the end returnes the new id.
    /// </summary>
    /// <param name="item"> the new Dependency</param>
    /// <returns> the id of the new Dependency</returns>
    public int Create(Dependency item)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        int newId = Config.NextDependencyId;
        DO.Dependency newItem= item with { _id = newId };
        //get the new item into the xml file:
        root.Add(createNullableXElement(newItem));
        XMLTools.SaveListToXMLElement(root, s_dependencies_xml);
        return newId;
    }

    /// <summary>
    /// this method removes a dependency from the XML file by a given id. if we didnt find the id in the file so throw exception
    /// </summary>
    /// <param name="id">the id of the old dependency we want to remove </param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Delete(int id)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement deletedDependency = findXElement(root.Elements(), id) ?? throw new DalNotFoundException($"Dependency with ID={id} does Not exist"); //find the XElement that need to be updated
        deletedDependency.Remove();
        XMLTools.SaveListToXMLElement(root, s_dependencies_xml);
    }

    /// <summary>
    /// this method returns the dependency with the id it got.
    /// </summary>
    /// <param name="id">the id of the dependency we want to find </param>
    /// <returns>the requested id or null if there is not any Dependency with that id</returns>
    public Dependency? Read(int id)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);//root is "ArrayOfDependencies"
        XElement? ifExists = findXElement(root.Elements(), id);
        if(ifExists==null)
            return null;
        //make this XElement (ifExists) into Dependency type:
        Dependency depToReturn = CreateDependecyFromXElement(ifExists);
        return depToReturn;
    }

    /// <summary>
    /// this method returns the dependency that answering the filter requirments.
    /// </summary>
    /// <param name="filter"> p
    ///icat that determine wich dependency we want to return</param>
    /// <returns> the requested id or null if there is not any Dependency with that id</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);//root is "ArrayOfDependencies
        foreach (var xel in root.Elements())
        {
            if (filter == null || filter(CreateDependecyFromXElement(xel)))
            {
                return CreateDependecyFromXElement(xel);
            }
        }
        return null;
    }

    /// <summary>
    /// this method creats a new dependency collection and returns him. 
    /// </summary>
    /// <param name="filter"> predicat that determine wich dependencies we want to return</param>
    /// <returns>the new collection </returns>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        return from XElement item in root.Elements() //convert the elements collection into a INumerable of Dependencies.
               where filter==null || filter(CreateDependecyFromXElement(item)) //if filter is null then we dont need to do anything, if he is not then build an dependency and activate the filter on it
               select (CreateDependecyFromXElement(item) //create the Dependency
                   );
    }

    /// <summary>
    /// this method gets a new dependency and update the xml file with the updated value of one dependency.
    /// </summary>
    /// <param name="item">the new dependency to update</param>
    /// <exception cref="DalNotFoundException"></exception>
    public void Update(Dependency item)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement oldDependency = findXElement(root.Elements(), item._id) ?? throw new DalNotFoundException($"Dependency with ID={item._id} does Not exist"); //find the XElement that need to be updated
        XElement newDependency = createNullableXElement(item); //create the new XElement properly
        oldDependency.Remove();
        root.Add(newDependency);
        XMLTools.SaveListToXMLElement(root, s_dependencies_xml);
    }
    /// <summary>
    /// Delete all existing data from xml file to let us do a new initalize.
    /// </summary>
    public void DeleteAll()
    {
        XElement emptyRoot = new XElement("ArrayOfDependency");//get the list from the xml file to work with it
        XMLTools.SaveListToXMLElement(emptyRoot, s_dependencies_xml);//save the changes we did in the list we got in the start
    }
}
