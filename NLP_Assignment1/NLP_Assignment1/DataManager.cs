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

        // TODO_LOW: rename these "test set variables" to something more generic
        internal List<string> wordListTestSet;
        internal Dictionary<string, int> countUnigramsTestSet;

        internal List<string> wordDiffList;

		internal string LoadFile(string path)
		{
			FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string res = reader.ReadToEnd().ToLower();  // Word and bigram counts are not case sensitive
            reader.Close();
            file.Close();

			return res;
		}

        internal void SaveFile(string path, List<string> content)
        {
            FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);

            // TODO: write content to file via: writer.WriteLine
            foreach (string entry in content)
            {
                writer.WriteLine(entry);
            }

            writer.Close();
            file.Close();
        }
	
	}
}
