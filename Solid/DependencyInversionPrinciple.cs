using System;

namespace DesignPatterns.Solid;

public enum Relationship
{
    Parent,
    Child,
    Sibling
}

public class Person
{
    public string Name;
    // public DateTime DateOfBirth;
}

public interface IRelationshipBrowser
{
    IEnumerable<Person> FindAllChildrenOf(string name);
}

// low-level
public class Relationships : IRelationshipBrowser
{
    private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();

    public void AddParentAndChild(Person parent, Person child)
    {
        relations.Add((parent, Relationship.Parent, child));
        relations.Add((child, Relationship.Child, parent));
    }

    //* Correct approach
    public IEnumerable<Person> FindAllChildrenOf(string name)
    {
        foreach (var r in relations.Where(x => x.Item1.Name == name && x.Item2 == Relationship.Parent))
        {
            yield return r.Item3;
        }
    }

    //! Incorrect approach
    // public List<(Person, Relationship, Person)> Relations => relations;
}

// high-level
public class Research
{
    //! Incorrect approach
    // public Research(Relationships relationships)
    // {
    //     var relations = relationships.Relations;

    // foreach (var r in relations.Where(x => x.Item1.Name == "John" && x.Item2 == Relationship.Parent))
    // {
    //     Console.WriteLine($"John has a child called {r.Item3.Name}");
    // }
    // }

    //* Correct approach
    public Research(IRelationshipBrowser browser)
    {
        foreach (var r in browser.FindAllChildrenOf("John"))
        {
            Console.WriteLine($"John has a child called {r.Name}");
        }
    }
}

public class DependencyInversionPrinciple
{
    public static void RunDemo()
    {
        var parent = new Person { Name = "John" };
        var child1 = new Person { Name = "Chris" };
        var child2 = new Person { Name = "Matt" };

        var relationships = new Relationships();
        relationships.AddParentAndChild(parent, child1);
        relationships.AddParentAndChild(parent, child2);

        new Research(relationships);
    }
}
