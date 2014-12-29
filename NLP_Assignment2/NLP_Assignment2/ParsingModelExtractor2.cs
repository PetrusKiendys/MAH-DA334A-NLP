using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NLP_Assignment2
{
	class ParsingModelExtractor2
    {


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

            //for (int i = 0; i < sentences.Length; i++)
            for (int i = 0; i < 1; i++)
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

        // TODO_LOW: commit this code for legacy/reference reasons to the repo and then delete it
        private void CountRulesDraft(TreeNode node)
        {
            if (node.hasChildren())
            {
                List<TreeNode> copy = node.getChildren();

                Console.WriteLine(node.getChild(0).Value);
                //node.getChild(0).Value = "XXX";
                //Console.WriteLine(node.getChild(0).Value);

                //Console.WriteLine(copy[0].Value);
                copy[0].Value = "AAA";
                Console.WriteLine(node.getChild(0).Value);

                //Console.WriteLine("node has " + node.countChildren() + " children!");
                string rule = node.Value+" ";
                string[] rule_append;
                
                // step 1 - create rule for node --> children
                for (int i = 0; i < node.countChildren(); i++)
                {
                    
                }

                //Array.Sort(rule_append);

                // step 2 - call CountRules for each child
            }
        }


        /* PSEUDOCODE:
         * func void CountRules (TreeNode node)
         * {
         *      if node has children
         *          create rule node --> children
         *          call CountRules for each child
         * }
         */
        // TODO: implement this method
        // NOTE: make sure that before a rule is stored, the order of right hand side items are sorted
        //       ie. the rule NP --> VB JJ is equivalent to NP --> JJ VB, make sure they don't register separately!
        private void CountRules(TreeNode node)
        {
            if (node.hasChildren())
            {
                // step 0 - get a List of all the children of this node
                

                // step 1 - create rule for node --> children

                // iterate over all of the children of this node
                for (int i = 0; i < node.countChildren(); i++)
                {
                    // create and store the rule by extracting the value of this node with the values of child nodes
                    // f.e. if this node is NNP and it has 2 child nodes with the values DTP and ROP then store
                    // this as "NNP DTP ROP"
                    // NOTE: as explained above the function head, make sure the right hand side is normalized
                    // ie. "NNP DTP ROP" and "NNP ROP DTP" should be registered as the same rule!

                    // if this rule has already occured, increase the counter

                    // the rules can be stored in a structure like:
                    //      internal Dictionary<string, int> rules;
                    // store it as a global variable for this class ParsingModelExtractor2
                }

                // step 2 - call CountRules for each child
                // once the rule has been applied for this node, call CountRules for all child nodes
            }
        }

        // NOTE:    this method does not currently support assignment of words as nodes
        //          (it omits the terminal word from being created as a node)
        private TreeNode ExtractNode(string str)
        {
            TreeNode res = new TreeNode();

            // step 0 - print out the incoming string
            //Console.WriteLine("Step 0 - input str:\n" + str + "\n");

            // step 1 - remove surrounding parenthesis
            str = str.Remove(0, 1);
            str = str.Remove(str.Length - 1, 1);
            //Console.WriteLine("Step 1 - rem braces:\n" + str + "\n");

            // step 2 - set the value of the node
            int delimiter_index = str.IndexOf(' ');
            //Console.WriteLine("Step 2 - delim index: " + delimiter_index);
            res.Value = str.Substring(0, delimiter_index);
            //Console.WriteLine("Step 2 - node val:\n" + res.Value + "\n");

            // step 2.5 - remove the value of the node from the input string
            str = str.Substring(delimiter_index+1);
            //Console.WriteLine("Step 2.5 - rem node val:\n" + str + "\n");

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