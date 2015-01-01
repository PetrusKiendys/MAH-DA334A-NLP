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

			Console.WriteLine("Press Enter to close this window...");
			Console.Read();		// keep the console window open/active at the end of runtime
		}
	}
}
