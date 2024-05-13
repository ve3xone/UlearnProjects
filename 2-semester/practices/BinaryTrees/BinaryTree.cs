using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable
{
    T TreeValue;
    int Weight = 1;
    BinaryTree<T> LeftNode;
    BinaryTree<T> RightNode;
    bool Flag = false;
    public BinaryTree() { }
    BinaryTree(T value)
    {
        TreeValue = value;
        Flag = true;
    }

    public T this[int i]
    {
        get
        {
            BinaryTree<T> root = this;
            int parentNodeWeight = 0;
            while (true)
            {
                int index = parentNodeWeight;
                if (root.LeftNode != null) index += root.LeftNode.Weight;
                if (i == index) return root.TreeValue;
                if (i < index) root = root.LeftNode;
                else
                {
                    root = root.RightNode;
                    parentNodeWeight = index + 1;
                }
            }
        }
    }

    public void Add(T key)
    {
        if (!Flag)
        {
            TreeValue = key;
            Flag = true;
        }
        else InitializeTrees(this, key);
    }

    public void InitializeTrees(BinaryTree<T> node, T key)
    {
        var cycleFlag = true;
        while (cycleFlag)
        {
            node.Weight++;
            if (node.TreeValue.CompareTo(key) > 0)
            {
                if (node.LeftNode != null)
                {
                    node = node.LeftNode;
                }
                else
                {
                    node.LeftNode = new BinaryTree<T>(key);
                    cycleFlag = false;
                }
            }
            else
            {
                if (node.RightNode != null)
                {
                    node = node.RightNode;
                }
                else
                {
                    node.RightNode = new BinaryTree<T>(key);
                    cycleFlag = false;
                }

            }
        }
    }

    public bool Contains(T key)
    {
        if (!Flag) return false;
        var parentTree = this;
        int result;
        while (true)
        {
            result = parentTree.TreeValue.CompareTo(key);
            if (result == 0) return true;
            if (result < 0)
            {
                if (parentTree.RightNode != null) parentTree = parentTree.RightNode;
                else return false;
            }
            else
            {
                if (parentTree.LeftNode != null) parentTree = parentTree.LeftNode;
                else return false;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => EnumeratorForNode(this);

    IEnumerator<T> EnumeratorForNode(BinaryTree<T> root)
    {
        if (root == null || !root.Flag) yield break;
        var enumeratorNode = EnumeratorForNode(root.LeftNode);
        while (enumeratorNode.MoveNext())
            yield return enumeratorNode.Current;
        yield return root.TreeValue;
        enumeratorNode = EnumeratorForNode(root.RightNode);
        while (enumeratorNode.MoveNext())
            yield return enumeratorNode.Current;
    }
}