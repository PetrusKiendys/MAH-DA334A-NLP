using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NLP_Assignment2
{
	class ParsingModelExtractor
	{
        public void Run()
        {
            string filepath = "../../res/talbanken-cfg_pos-train.cfg";

            FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            string output = reader.ReadToEnd();
            file.Close();
            
            string[] rows = output.Split('\n');
            
            for (int i = 0; i < rows.Length; i++)
            {
                string row = rows[i];
                Console.WriteLine(row);
                this.Start(row);
            }

            Console.ReadLine();
        }
        public void Start(string input, string start=null)
        {
            if (input == null)
                return;
            if (input[0] != '(' || input[input.Length - 1] != ')')
            {
                Console.WriteLine(start);
                return;
            }
            int count = 0;
            List<string> subStrs = new List<string>();

            input = input.Remove(0, 1);
            input = input.Remove(input.Length - 1, 1);
            int i = input.IndexOf(' ');

            string nextInput = i > 0 ? input.Substring(0, i) : input;

            if (start != null)
                start = start + " -> " + nextInput;
            else
                start = nextInput;

            input = input.Remove(0, i + 1);

            string tempStr = "";
            for (int j = 0; j < input.Length; j++)
            {
                tempStr += input[j];
                if (input[j] == '(')
                    count++;
                else if (input[j] == ')')
                {
                    count--;
                    if (count == 0)
                    {
                        subStrs.Add(tempStr);
                        tempStr = "";
                    }
                }
            }
            if (subStrs.Count == 0)
                subStrs.Add(tempStr);

            subStrs.ForEach(delegate(string it)
            {
                this.Start(it, start);
            });
        }

    }
}
