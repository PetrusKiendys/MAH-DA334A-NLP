using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NLP_Assignment2
{
	class DataManager
	{
		// TODO_LOW: global variables should be instantiated through a constructor (or in another way)?
		internal Dictionary<string, int> grammarRules = new Dictionary<string, int>();
		internal Dictionary<string[], int> lexiconRules = new Dictionary<string[], int>();

		internal string LoadFile(string path)
		{
			FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string res = reader.ReadToEnd();
			reader.Close();
			file.Close();

			return res;
		}
	}
}
