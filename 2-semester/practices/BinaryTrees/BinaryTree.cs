using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable
{
    private T TreeValue;
    private int Size = 1;
    private BinaryTree<T> LeftNode;
    private BinaryTree<T> RightNode;
    private bool IsInitialized = false;

    public BinaryTree() { }

    private BinaryTree(T value)
    {
        TreeValue = value;
        IsInitialized = true;
    }

    public T this[int i] => GetElementByIndex(i);

    public void Add(T key)
    {
        if (!IsInitialized)
        {
            TreeValue = key;
            IsInitialized = true;
        }
        else
            AddElement(this, key);
    }

    public bool Contains(T key)
    {
        if (!IsInitialized)
            return false;

        var currentNode = this;
        while (true)
        {
            int result = currentNode.TreeValue.CompareTo(key);
            if (result == 0) return true;
            currentNode = result < 0 ? currentNode.RightNode : currentNode.LeftNode;
            if (currentNode == null) return false;
        }
    }

    public IEnumerator<T> GetEnumerator() => EnumerateNodes(this);

    private IEnumerator<T> EnumerateNodes(BinaryTree<T> root)
    {
        if (root == null || !root.IsInitialized)
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
        while (true)
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
    }

    private void AddElement(BinaryTree<T> node, T key)
    {
        while (true)
        {
            node.Size++;
            if (node.TreeValue.CompareTo(key) > 0)
            {
                if (node.LeftNode != null) node = node.LeftNode;
                else { node.LeftNode = new BinaryTree<T>(key); break; }
            }
            else
            {
                if (node.RightNode != null) node = node.RightNode;
                else { node.RightNode = new BinaryTree<T>(key); break; }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}