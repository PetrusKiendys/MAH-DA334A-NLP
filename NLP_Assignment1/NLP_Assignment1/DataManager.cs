﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Numerics;

namespace NLP_Assignment1
{
	class DataManager
	{
		internal Dictionary<string, int> countUnigrams;
		internal Dictionary<string, int> countBigrams;
		internal Dictionary<string, BigRational> probListBigrams;
		internal List<string> wordList;
		internal decimal perplex;

		// TODO_LOW: rename these "test set variables" to something better than "[var]TestSet" or "[var]2"?
		internal List<string> wordList2;
		internal Dictionary<string, int> countUnigrams2;

		internal List<string> wordDiffList;

		internal string LoadFile(string path)
		{
			FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string res = reader.ReadToEnd().ToLower();  // word and bigram counts are not case sensitive
			reader.Close();
			file.Close();

			return res;
		}

		internal void SaveFile(string path, List<string> content)
		{
			FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
			StreamWriter writer = new StreamWriter(file);

			foreach (string entry in content)
			{
				writer.WriteLine(entry);
			}

			writer.Close();
			file.Close();
		}
	
	}
}
