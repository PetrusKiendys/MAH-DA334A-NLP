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
			ParsingModelExtractor pme = new ParsingModelExtractor();
			pme.Run();

			// keep the console window open/active at the end of runtime
			Console.Read();
		}
	}
}
