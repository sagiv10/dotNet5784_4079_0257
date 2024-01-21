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

    private static XElement? findXElement(IEnumerable<XElement> elements, int requestedId) //helping method that gets an id and find in the XElement array the index of the id
    {
        foreach(var xel in elements)
        {
            XElement? idSon = xel.Element("_id"); //if the id field exists then idSon has it, if not it will be null
            if (idSon != null && int.Parse(idSon.Value) == requestedId)
            {//the otherc parts of the program made the input that it will be converticle no int so we wont do try parse
                return xel;
            }
        }
        return null;
    }

    private static int? getSomeId(XElement element, string typeOfId) //helping method that gets an XElement of Depandency and returns his id if he exists or null else.
    {
        XElement? returnedId = element.Element(typeOfId); //if the id field exists then returnedId has it, if not it will be null
        int isConvertible;
        if (returnedId != null && int.TryParse(returnedId.Value,out isConvertible)==true)
        {
            return int.Parse(returnedId.Value);
        }
        return null;
    }

    public int Create(Dependency item)
    {
        XElement root = XElement.Load(s_dependencies_xml);
        XElement? ifExists = findXElement(root.Elements(), item._id);

    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement root = XElement.Load(s_dependencies_xml);
        return from XElement item in root.Elements()
               select new Dependency(
                   (int)getSomeId(item, "_id")!,
                   getSomeId(item, "_dependentTask"),
                   getSomeId(item, "_dependsOnTask")
                   );
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
