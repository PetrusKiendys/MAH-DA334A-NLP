using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment2
{
	// TODO_LOW: may have to rename some methods, "Count..." and "Extract..." are not describing the functionality of the methods in a clear fashion.
	// TODO_LOW: some methods in this class are overloaded, consider implementing a solution which does not require overloading (rather, the functionality should be handled by the same method)
	//			 http://stackoverflow.com/questions/1996426/pass-multiple-optional-parameters-to-a-c-sharp-function (relevant?)

	class Processor
	{
		/* PSEUDOCODE:
		 * func void CountRules (TreeNode node)
		 * {
		 *      if node has children
		 *          create rule node --> children
		 *          store the rule
		 *          call CountRules for each child
		 * }
		 */
		// TODO_LOW: this method could return a "RuleGatherer" which stores the rules of one tree (a node and all of sub-nodes)
		//			 this structure could then be inserted into a "RuleManager" or counter that extracts the rules
		//			 implementing this requires additional passing of data but makes CountRules more loosely coupled from the storage of rules
		private void CountRules(TreeNode node, Dictionary<string, int> ruleset, ExtractMode extractmode)
		{
			if (node.hasChildren())
			{
				// step 1 - create rule for node --> children

				string rule, lhs, rhs = "";
				List<string> children = new List<string>();

				// step 1.1 - getting the left hand side of the rule
				lhs = node.Value;

				// step 1.2 - getting the right hand side of the rule
				for (int i = 0; i < node.countChildren(); i++)
				{
					children.Add(node.getChild(i).Value);
				}
				children.Sort();	// sort the children strings so that permutations of the same rule is not stored separately

				for (int i = 0; i < node.countChildren(); i++)
				{
					rhs += children[i] + " ";
				}
				rhs = rhs.Trim();

				// step 1.3 - concatenate lhs and rhs to make the key for the rule
				rule = lhs + " " + rhs;

				// step 2 - store the rule
				if (ruleset.ContainsKey(rule))
					ruleset[rule] = ruleset[rule] + 1;
				else
					ruleset.Add(rule, 1);

				// step 3 - call CountRules for each child
				for (int i = 0; i < node.countChildren(); i++)
				{
					CountRules(node.getChild(i), ruleset, extractmode);
				}
			}
		}

		// NOTE: this is an overloaded method of CountRules(...), see the "original" method for a description of usage
		private void CountRules(TreeNode node, Dictionary<string[], int> ruleset, ExtractMode extractmode)
		{
			// BOOKMARK
			// TODO: implement an algorithm that stores rules in "ruleset" on the following format:
			//			ruleset key[0] = lhs
			//			ruleset key[1] = rhs
			//			ruleset value = count of lhs --> rhs rule
		}


		/* PSEUDOCODE:
		 * func TreeNode ExtractNode(string str)
		 * {
		 *  create a node
		 *	remove parenthesis surrounding the input string
		 *	assign the left-most tag/word as the value of the node
		 *	retrieve a string list of the children (based on input string)
		 *	add each child to the node by calling this function (recursively)
		 * }
		 */
		// TODO:	 implement support for extracting only tags as node or tags & words as nodes
		// TODO_LOW: to improve clarity of this implementation two control flow branches could be constructed (one for non-terminals and another for terminals)
		private TreeNode ExtractNode(string str, ExtractMode extractmode)
		{
			TreeNode res = new TreeNode();
			bool isTerminal = false;

			// step 0 - check if the incoming string is a terminal
			if (!str.Contains(' '))
				isTerminal = true;

			// step 1 - remove surrounding parenthesis (for non-terminals)
			if (str[0] == '(' && !isTerminal)
				str = str.Remove(0, 1);
			if (str[str.Length - 1] == ')' && !isTerminal)
				str = str.Remove(str.Length - 1, 1);

			// step 2 - set the value of the node
			if (!isTerminal)
				res.Value = str.Substring(0, str.IndexOf(' '));
			else
				res.Value = str;

			// step 2.5 - remove the value of the node from the input string
			if (!isTerminal)
				str = str.Substring(str.IndexOf(' ') + 1);
			else
				str = "";

			// step 3 - make a List<string> of node children
			List<string> childNodes = GetChildrenList(str);

			for (int i = 0; i < childNodes.Count; i++)
			{
				if (!childNodes[i].Contains(' ') && extractmode.Equals(ExtractMode.GRAMMAR))	// if the childnode is a terminal and the extraction mode is set to "grammar"
					break;
				else
					res.addChild(ExtractNode(childNodes[i], extractmode));
			}

			return res;
		}

		private List<string> GetChildrenList(string str)
		{
			List<string> res = new List<string>();
			int count = 0;
			string tempStr = "";

			if (str.Equals(""))
				return res;
			else if (!str.Contains(' '))
			{
				res.Add(str);
				return res;
			}
			else
			{
				for (int i = 0; i < str.Length; i++)
				{
					tempStr += str[i];
					if (str[i] == '(')
						count++;
					else if (str[i] == ')')
					{
						count--;
						if (count == 0)
						{
							res.Add(tempStr);
							tempStr = "";
						}
					}
				}

				return res;
			}
		}

		internal void ExtractRules(string[] sentences, ExtractMode extractmode, Dictionary<string, int> ruleset)
		{
			switch (extractmode)
			{
				case ExtractMode.GRAMMAR:
					for (int i = 0; i < sentences.Length; i++)
					{
						TreeNode node = ExtractNode(sentences[i], extractmode);
						CountRules(node, ruleset, extractmode);
					}
					break;
			}
		}

		// NOTE: this is an overloaded method of ExtractRules(...), see the "original" method for a description of usage
		internal void ExtractRules(string[] sentences, ExtractMode extractmode, Dictionary<string[], int> ruleset)
		{
			switch (extractmode)
			{
				case ExtractMode.LEXICON:
					for (int i = 0; i < sentences.Length; i++)
					{
						TreeNode node = ExtractNode(sentences[i], extractmode);
						CountRules(node, ruleset, extractmode);
					}
					break;
			}
		}

	}
}
