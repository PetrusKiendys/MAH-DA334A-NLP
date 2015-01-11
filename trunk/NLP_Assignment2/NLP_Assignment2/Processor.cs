using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment2
{
	// TODO_LOW: may have to rename some methods, "Count..." and "Extract..." are not describing the functionality of the methods in a clear fashion.
	class Processor
	{

		/* PSEUDOCODE:
		 * func void CountRules (TreeNode node, ...)
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
		// TODO_LOW: see if there's a way to increase readability of this implementation
		private void CountRules(TreeNode node, Dictionary<string, int> ruleset, ExtractMode extractmode)
		{
			if (node.hasChildren())
			{
				string rule, lhs, rhs = "";
				List<string> children = new List<string>();

				switch (extractmode)
				{
					case ExtractMode.GRAMMAR:
						// step 1 - create rule for node --> children
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
						break;

					case ExtractMode.LEXICON:
						// step 1 - create rule for node --> children
						// step 1.1 - getting the left hand side of the rule
						lhs = node.Value;

						for (int i = 0; i < node.countChildren(); i++)
						{
							// step 1.2 - getting the right hand side of the rule
							rhs = node.getChild(i).Value;

							// step 1.3 - concatenate lhs and rhs to make the key for the rule
							rule = lhs + " " + rhs;

							// step 2 - store the rule
							if (ruleset.ContainsKey(rule))
								ruleset[rule] = ruleset[rule] + 1;
							else
								ruleset.Add(rule, 1);

							// step 3 - call CountRules for each child
							CountRules(node.getChild(i), ruleset, extractmode);
						}
						break;

					default:
						throw new ArgumentException("Illegal ExtractMode enumerator was passed");
				}

			}
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

		// given a str which holds a bracketing formatted phrase structure tree, this method extracts all siblings (node children of the currently processed node) of str
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

		internal void ExtractRules(string[] sentences, Dictionary<string, int> ruleset, ExtractMode extractmode)
		{
			for (int i = 0; i < sentences.Length; i++)
			{
				TreeNode node = ExtractNode(sentences[i], extractmode);
				CountRules(node, ruleset, extractmode);
			}
		}

		// formats the rules of the ruleset into a specific string format (which will later be written to file)
		internal List<string> FormatRules(Dictionary<string, int> ruleset, out int ruleCounter, ExtractMode extractmode, Separator separator)
		{
			ruleCounter = 0;							// counts the number of times a unique rule occurs
			List<string> res = new List<string>();
			List<string> rhslist = new List<string>();	// list that holds unique "right hand side" keys, used when formatting the lexicon

			string sep = "";
			if (separator.Equals(Separator.WHITESPACE))
			{
				sep = " ";
			}
			else if (separator.Equals(Separator.TAB))
			{
				sep = "\t";
			}
			else
			{
				throw new ArgumentException("Illegal Separator enumerator was passed");
			} 


			switch (extractmode)
			{
				case ExtractMode.GRAMMAR:
					foreach (KeyValuePair<string, int> entry in ruleset)
					{
						string rule = entry.Key;
						rule = rule.Replace(' ', '\t');
						string count = entry.Value.ToString();

						res.Add(count + sep + rule);
						ruleCounter++;
					}
					return res;

				case ExtractMode.LEXICON:
					foreach (KeyValuePair<string, int> entry in ruleset)
					{
						string[] rule = entry.Key.Split(' ');
						string lhs = rule[0];
						string rhs = rule[1];
						string count = entry.Value.ToString();

						// if the right hand side rule has not been added to res
						if (!rhslist.Contains(rhs))
						{
							res.Add(rhs + sep + lhs + sep + count);
							rhslist.Add(rhs);
							ruleCounter++;
						}

						// if the right hand side rule has previously been added to res
						else
						{
							string searchStr = rhs + sep;
							string resStr = res.FirstOrDefault(elem => elem.StartsWith(searchStr));
							int index = res.IndexOf(resStr);

							res[index] = res[index] + sep + lhs + sep + count;
							ruleCounter++;
						}
					}
					return res;

				default:
					throw new ArgumentException("Illegal ExtractMode enumerator was passed");
			}
		}
	}
}
