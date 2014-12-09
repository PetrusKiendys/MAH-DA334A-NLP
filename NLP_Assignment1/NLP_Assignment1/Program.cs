using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP_Assignment1
{
	// REMINDER_LOW: make sure that POLP and proper encapsulation/accessibility of variables and methods is enforced
	class Program
	{
		static void Main(string[] args)
		{
			LanguageModel lm = new LanguageModel(Verbosity.NORMAL);
			lm.Run();
		}
	}
}
