using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree;

public class DiskTreeTask
{
    public class Node
    {
        public HashSet<Node> Children = new HashSet<Node>();
        public string Name;

        public Node(string name)
        {
            Name = name;
        }

        public List<string> GetDirectoryStructure(int level, List<string> list)
        {
            if (level >= 0)
                list.Add(new string(' ', level) + Name);
            level++;
            foreach (var child in Children.OrderBy(c => c.Name, StringComparer.Ordinal))
                child.GetDirectoryStructure(level, list);
            return list;
        }

        public Node GetOrAddChild(string name)
        {
            foreach (var child in Children)
            {
                if (child.Name == name)
                    return child;
            }
            var newNode = new Node(name);
            Children.Add(newNode);
            return newNode;
        }
    }

    public static List<string> Solve(List<string> input)
    {
        var root = new Node("");
        foreach (var path in input)
        {
            string[] pathArray = path.Split('\\');
            Node currentNode = root;
            foreach (var directoryName in pathArray)
                currentNode = currentNode.GetOrAddChild(directoryName);
        }

        return root.GetDirectoryStructure(-1, new List<string>());
    }
}