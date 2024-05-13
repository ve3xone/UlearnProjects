using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private T TreeValue;
        private int Weight = 1;
        private BinaryTree<T> LeftNode;
        private BinaryTree<T> RightNode;
        private bool Flag = false;

        public BinaryTree() { }

        private BinaryTree(T value)
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
                    if (root.LeftNode != null)
                        index += root.LeftNode.Weight;
                    if (i == index)
                        return root.TreeValue;
                    if (i < index)
                        root = root.LeftNode;
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
            else
                InitializeTrees(this, key);
        }

        private void InitializeTrees(BinaryTree<T> node, T key)
        {
            while (true)
            {
                node.Weight++;
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

        public bool Contains(T key)
        {
            if (!Flag)
                return false;

            var parentTree = this;
            while (true)
            {
                int result = parentTree.TreeValue.CompareTo(key);
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

        public IEnumerator<T> GetEnumerator() => EnumerateNodes(this);

        private IEnumerator<T> EnumerateNodes(BinaryTree<T> root)
        {
            if (root == null || !root.Flag)
                yield break;

            if (root.LeftNode != null)
            {
                var leftEnumerator = EnumerateNodes(root.LeftNode);
                while (leftEnumerator.MoveNext())
                    yield return leftEnumerator.Current;
            }

            yield return root.TreeValue;

            if (root.RightNode != null)
            {
                var rightEnumerator = EnumerateNodes(root.RightNode);
                while (rightEnumerator.MoveNext())
                    yield return rightEnumerator.Current;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}