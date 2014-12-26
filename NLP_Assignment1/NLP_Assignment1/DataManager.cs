using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Numerics;

namespace NLP_Assignment1
{
	class DataManager
	{
		internal Dictionary<string, int> countUnigrams;
		internal Dictionary<string, int> countBigrams;
		internal Dictionary<string, float> probListBigrams;
		internal Dictionary<string, BigRational> probListBigrams_bigrat;
		// LEGACY: processing of the ID field of the .conll file might not be necessary for this assignment...
		//internal List<string> intList = new List<string>();
		internal List<string> wordList;
        internal decimal perplex;

		internal string LoadFile(string path)
		{
			FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string res = reader.ReadToEnd().ToLower();  // Word and bigram counts are not case sensitive
			file.Close();

			return res;
		}
	
	}
}
