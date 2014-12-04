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
		internal Dictionary<string, int> wordListUnigrams = new Dictionary<string, int>();
		internal Dictionary<string, int> wordListBigrams = new Dictionary<string, int>();
		internal Dictionary<string, float> probListBigrams = new Dictionary<string, float>();
		//internal List<string> intList = new List<string>();
		internal List<string> wordList = new List<string>();


		public void LoadFile()
		{
			FileStream file = new FileStream("../../res/talbanken-dep-train.conll", FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string content = reader.ReadToEnd().ToLower();  // Word and bigram counts are not case sensitive
            file.Close();
			string[] contentArray = content.Split('\n');

			for (int i = 0; i < contentArray.Length; i++)
			{
				if (contentArray[i] == "")
					continue;

				string[] tokens = contentArray[i].Split('\t');

                //string idval = tokens.ElementAt(0);

				string[] wordSplit = tokens.ElementAt(1).Split('_');
                string word = wordSplit.ElementAt(0);

				word = word.Trim();

				wordList.Add(word);
                //intList.Add(idval);
			}
		}

		

	}
}
