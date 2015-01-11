using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment1
{
	// REMINDER_LOW: make sure that POLP and proper encapsulation/accessibility of variables and methods is enforced
	class Program
	{
		static void Main(string[] args)
		{
			LanguageModel lm = new LanguageModel(Verbosity.NORMAL);
			lm.Run();

			Console.WriteLine("Press Enter to close this window...");
			Console.Read();		// keep the console window open/active at the end of runtime
		}
	}
}
