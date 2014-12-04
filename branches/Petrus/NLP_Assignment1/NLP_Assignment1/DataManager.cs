using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NLP_Assignment1
{
	class DataManager
	{
		// SEM: rename "wordListNgram" variables to "countListNgram(?)"
		internal Dictionary<string, int> wordListUnigrams;
		internal Dictionary<string, int> wordListBigrams;
		internal Dictionary<string, float> probListBigrams;
		// SCOPE: processing of the ID field of the .conll file might not be necessary for this assignment...
		//internal List<string> intList = new List<string>();
		internal List<string> wordList;


		internal string LoadFile()
		{
			FileStream file = new FileStream("../../res/talbanken-dep-train.conll", FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string res = reader.ReadToEnd().ToLower();  // Word and bigram counts are not case sensitive
			file.Close();

			return res;
		}

		internal List<string> ExtractWords(string content)
		{
			List<string> res = new List<string>();
			string[] contentArray = content.Split('\n');

			for (int i = 0; i < contentArray.Length; i++)
			{
				if (contentArray[i] == "")
					continue;

				string[] tokens = contentArray[i].Split('\t');

				// SCOPE: processing of the ID field of the .conll file might not be necessary for this assignment...
				//string idval = tokens.ElementAt(0);

				string[] wordSplit = tokens.ElementAt(1).Split('_');
				string word = wordSplit.ElementAt(0);

				word = word.Trim();

				// BOOKMARK: check if res is properly instantiated
				res.Add(word);
				// SCOPE: processing of the ID field of the .conll file might not be necessary for this assignment...
				//intList.Add(idval);
			}
			
			return res;
		
		}
	
	}
}
