using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NLP_Assignment2
{
// TODO: at some point methods need to be generalized for both construction of grammar and lexicon files
//		 nodes need to be extracted with and without words
//		 use Enums to do this
// TODO_LOW: better to structure data management and processing in different classes, as well as having one class call which holds the main control flow
	class ParsingModelExtractor
	{
		private Dictionary<string, int> grammarRules = new Dictionary<string,int>();
		private Dictionary<string, int> lexiconRules = new Dictionary<string,int>();

		internal string LoadFile(string path)
		{
			FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string res = reader.ReadToEnd();
			reader.Close();
			file.Close();

			return res;
		}


		internal void Run()
		{
			string data = LoadFile("../../res/data/talbanken-cfg_pos-train.cfg");
			string[] sentences = data.Split('\n');
			sentences = sentences.Take(sentences.Count()-1).ToArray();  // getting rid of the last empty row in the data file

			for (int i = 0; i < sentences.Length; i++)
			//for (int i = 0; i < 1; i++)
			{
				TreeNode node = ExtractNode(sentences[i]);
				//  test code for ExtractNode(...)
				//TreeNode node = ExtractNode("(TESTTAG (...))");
				//TreeNode node = ExtractNode("(NNP (JJP (JJ Individuell))(NN beskattning)(PPP (PP av)(NNP (NN arbetsinkomster))))");
				//TreeNode node = ExtractNode("(VBP (NNP (NN Kommunalskatteavdraget))(VB slopas))");

				CountRules(node);
				//  test code for CountRules(...)
				//CountRules(node.getChild(0));
				
			}
		}





		/* PSEUDOCODE:
		 * func void CountRules (TreeNode node)
		 * {
		 *      if node has children
		 *          create rule node --> children
		 *          store the rule
		 *          call CountRules for each child
		 * }
		 */
		// TODO: implement this method
		// TODO_LOW: this method could return a "RuleGatherer" which stores the rules of one tree (a node and all of sub-nodes)
		//			 this structure could then be inserted into a "RuleManager" or counter that extracts the rules
		//			 implementing this requires additional passing of data but makes CountRules more loosely coupled from the storage of rules
		private void CountRules(TreeNode node)
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
				// sort the children strings so that permutations of the same rule is not stored separately
				children.Sort();

				for (int i = 0; i < node.countChildren(); i++)
				{
					rhs += children[i] + " ";
				}
				rhs = rhs.Trim();

				// step 1.3 - concatenate lhs and rhs to make the key for the rule
				rule = lhs + " " + rhs;

				// step 2 - store the rule
				if (grammarRules.ContainsKey(rule))
					grammarRules[rule] = grammarRules[rule] + 1;
				else
					grammarRules.Add(rule, 1);
				
				// step 3 - call CountRules for each child
				for (int i = 0; i < node.countChildren(); i++)
				{
					CountRules(node.getChild(i));
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
		// NOTE:    this method does not currently support assignment of words as nodes
		//          (it omits the terminal word from being created as a node)
		// FIX:		handle cases where the incoming string has a value of "(x)" (see line 3202 in /res/data/talbanken-cfg_pos-train.cfg)
		// TODO:	implement support for extracting only tags as node or tags & words as nodes
		private TreeNode ExtractNode(string str)
		{
			TreeNode res = new TreeNode();

			// step 1 - remove surrounding parenthesis
			// FIX: fix this logic...
			str = str.Remove(0, 1);
			str = str.Remove(str.Length - 1, 1);

			// step 2 - set the value of the node
			int delimiter_index = str.IndexOf(' ');
			if (delimiter_index != -1)
				res.Value = str.Substring(0, delimiter_index);
			else
				// FIX: fix this logic...
				//Console.WriteLine(str);

			// step 2.5 - remove the value of the node from the input string
			str = str.Substring(delimiter_index+1);

			// step 3 - make a List<string> of node children
			List<string> childNodes = GetChildrenList(str);

			for (int i = 0; i < childNodes.Count; i++)
			{
				res.addChild(ExtractNode(childNodes[i]));
			}

			return res;
		}

		private List<string> GetChildrenList(string str)
		{
			List<string> res = new List<string>();
			int count = 0;
			string tempStr = "";

			if (str != "" && !str.Contains('(') && !str.Contains(')'))
			{
				// FIX: fix this logic...
				//Console.WriteLine("cond str: " + str);
				return res;
			}
			else
			{
				for (int j = 0; j < str.Length; j++)
				{
					tempStr += str[j];
					if (str[j] == '(')
						count++;
					else if (str[j] == ')')
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
	}
}