using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable
{
    private T TreeValue;
    private int Size;
    private BinaryTree<T> LeftNode;
    private BinaryTree<T> RightNode;

    public BinaryTree() { Size = 0; }

    private BinaryTree(T value)
    {
        TreeValue = value;
        Size = 1;
    }

    public T this[int i] => GetElementByIndex(i);

    public void Add(T key)
    {
        if (Size == 0)
        {
            TreeValue = key;
            Size = 1;
        }
        else
            AddElement(this, key);
    }

    public bool Contains(T key)
    {
        if (Size == 0)
            return false;

        var currentNode = this;
        while (currentNode != null)
        {
            int result = currentNode.TreeValue.CompareTo(key);
            if (result == 0)
                return true;
            currentNode = result < 0 ? currentNode.RightNode : currentNode.LeftNode;
        }
        return false;
    }

    public IEnumerator<T> GetEnumerator() => EnumerateNodes(this);

    private IEnumerator<T> EnumerateNodes(BinaryTree<T> root)
    {
        if (root == null || root.Size == 0)
            yield break;

        if (root.LeftNode != null)
        {
            foreach (var item in root.LeftNode)
                yield return item;
        }

        yield return root.TreeValue;

        if (root.RightNode != null)
        {
            foreach (var item in root.RightNode)
                yield return item;
        }
    }

    private T GetElementByIndex(int i)
    {
        var current = this;
        while (current != null)
        {
            int leftSize = current.LeftNode != null ? current.LeftNode.Size : 0;
            if (i == leftSize)
                return current.TreeValue;
            if (i < leftSize)
                current = current.LeftNode;
            else
            {
                current = current.RightNode;
                i -= leftSize + 1;
            }
        }
        throw new IndexOutOfRangeException();
    }

    private void AddElement(BinaryTree<T> node, T key)
    {
        while (node != null)
        {
            node.Size++;
            if (node.TreeValue.CompareTo(key) > 0)
            {
                if (node.LeftNode != null) node = node.LeftNode;
                else
                {
                    node.LeftNode = new BinaryTree<T>(key) { Size = 1 };
                    return;
                }
            }
            else
            {
                if (node.RightNode != null) node = node.RightNode;
                else
                {
                    node.RightNode = new BinaryTree<T>(key) { Size = 1 };
                    return;
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}