using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment2
{
    class TreeNode
    {
        internal string Value {get; set;}
        List<TreeNode> childNodes = new List<TreeNode>();

        internal void addChild (TreeNode child)
        {
            childNodes.Add(child);
        }

        internal TreeNode getChild (int index)
        {
            return childNodes[index];
        }

        internal bool hasChildren()
        {
            if (countChildren() > 0)
                return true;
            else
                return false;
        }

        internal int countChildren()
        {
            return childNodes.Count;
        }

        // TODO: this function may need to be altered so that it returns a List that has no references to
        //       the original List of node children
        internal List<TreeNode> getChildren()
        {
            return new List<TreeNode>(childNodes);
        }
    }
}