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

		// FIX: this method needs to be altered so that it returns a new List that has no references to
		//       the original List of node children as manipulation of the return of this method also changes the properties
		//		 of the original nodes (the original nodes and their properties are accessible, this should not be the case!)
		internal List<TreeNode> getChildren()
		{
			return new List<TreeNode>(childNodes);
		}
	}
}