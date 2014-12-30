﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NLP_Assignment2
{
	class TreePathingUtility
	{
		public void Run()
		{
			string filepath = "../../res/data/talbanken-cfg_pos-train.cfg";
			FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string output = reader.ReadToEnd();
			reader.Close();
			file.Close();
			
			string[] rows = output.Split('\n');
			
			for (int i = 0; i < rows.Length; i++)
			{
				string row = rows[i];
				Console.WriteLine(row);
				Start(row);

				break;
			}

			Console.ReadLine();
		}


		public void Start(string input, string start=null)
		{
			// invoked when input is not surrounded by parenthesis
			if (input[0] != '(' || input[input.Length - 1] != ')')
			{
				Console.WriteLine(start);
				return;
			}

			// count is used to determine the extent of a "node"
			int count = 0;
			// list that contains whole POS tags
			List<string> subStrs = new List<string>();

			// removes the parenthesis
			input = input.Remove(0, 1);
			input = input.Remove(input.Length - 1, 1);
			

			int i = input.IndexOf(' ');

			// removes the parsing of word from reading in words and POS tags
			string nextInput = i > 0 ? input.Substring(0, i) : input;

			if (start != null)
				start = start + " -> " + nextInput;
			else
				start = nextInput;

			input = input.Remove(0, i + 1);

			// parsing of a whole POS tag
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
