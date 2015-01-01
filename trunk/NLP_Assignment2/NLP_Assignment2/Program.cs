using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment2
{
	class Program
	{
		static void Main(string[] args)
		{
			ParsingModelExtractor pme1 = new ParsingModelExtractor();
			pme1.Run("../../res/data/talbanken-cfg_pos-train.cfg");
			ParsingModelExtractor pme2 = new ParsingModelExtractor();	// NOTE_LOW: not sure if another instance of PME needs to be created but it is done just in case, fix this if needed
			pme2.Run("../../res/data/talbanken-cfg_dep-train.cfg");

			//		NOTE: test code for FormatRules with ExtractMode.LEXICON
			//		TODO: commit this for reference purposes and later remove
			//List<string> list = new List<string>();
			//list.Add("XXX YYY ZZZ");
			//list.Add("AAA BBB CCC");
			//list.Add("AAA XXX CCC");
			//Console.WriteLine(list.Foo("XXX", ' '));	// should return 0 (index of element) or element of list
			//Console.WriteLine(list.Foo("YYY", ' '));	// should return -1 or null
			//Console.WriteLine(list.Foo("AAA", ' '));	// should return 1 (index of element) or element of list
			//Console.WriteLine(list.Foo("DDD", ' '));	// should return -1 or null

			//string searchstr = "AAA ";
			//string str = list.FirstOrDefault(x => x.StartsWith(searchstr));
			//int i = list.IndexOf(str);

			//Console.WriteLine("index of "+"\""+searchstr+"\": "+i);

			Console.WriteLine("Press Enter to close this window...");
			Console.Read();		// keep the console window open/active at the end of runtime
		}
	}
}
