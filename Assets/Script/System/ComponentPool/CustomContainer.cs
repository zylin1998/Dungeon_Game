using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public interface IContainerGroup 
{
    public string GroupName { get; }
}

public static class CustomContainer
{
    public class Container
    {
        public string Group { get; private set; }
        private List<object> Contents { get; set; }

        public int Count => Contents.Count;

        public Container(string group) 
        {
            this.Group = group;
            Contents = new List<object>();
        }

        public bool CheckGroup(string group) 
        {
            return group.Equals(this.Group);
        }

        public void AddContent(object content) 
        {
            Contents.Add(content);
        }

        public void RemoveContent(object content) 
        {
            Contents.Remove(content);
        }

        public T GetContent<T>()
        {
            return (T)Contents.Find(match => match.GetType().Equals(typeof(T)));
        }

        public List<T> GetContents<T>()
        {
            return Contents.FindAll(match => match.GetType().Equals(typeof(T))).ConvertAll(c => (T)c);
        }
    }

    public static List<Container> Containers { get; private set; }

    public static void AddContent(object content, string group) 
    {
        var container = GetContainer(group);

        container.AddContent(content);
    }

    public static void RemoveContent(object content, string group)
    {
        var container = GetContainer(group);

        container.RemoveContent(content);
    }

    public static TContent GetContent<TContent>(string group) 
    {
        return GetContainer(group).GetContent<TContent>();
    }

    public static List<TContent> GetContents<TContent>(string group) 
    {
        return GetContainer(group).GetContents<TContent>();
    }

    private static Container GetContainer(string group) 
    {
        if (Containers == null) { Containers = new List<Container>(); }

        return Containers.Find(match => match.CheckGroup(group)) ?? AddContainer(group);
    }

    private static Container AddContainer(string group) 
    {
        var container = new Container(group);

        Containers.Add(container);

        return container;
    }

    public static void Reset() 
    {
        Containers.Clear();
    }
}
