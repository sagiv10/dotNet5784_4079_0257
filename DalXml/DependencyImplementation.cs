namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    private static int findIdx(IEnumerable<XElement> elements, int requestedId)
    {
        int theIndex = 0;
        foreach(var xel in elements)
        {
            XElement? idSon = xel.Element("_id"); //if the id field exists then idSon has it, if not it will be null
            if (idSon != null && int.Parse(idSon.Value) == requestedId)
            {//the otherc parts of the program made the input that it will be converticle no int so we wont do try parse
                return theIndex;
            }
            theIndex++;
        }
        return -1;
    }

    public int Create(Dependency item)
    {
        XElement root = XElement.Load(s_dependencies_xml);
        int theIndex = root.Elements().
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
        throw new NotImplementedException();
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
