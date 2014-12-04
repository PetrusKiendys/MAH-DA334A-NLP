using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// REMINDER_LOW: make sure that POLP and proper encapsulation/accessibility of variables and methods is enforced
// MORE_INFO:	http://en.wikipedia.org/wiki/Principle_of_least_privilege
//				http://en.wikipedia.org/wiki/Encapsulation_(object-oriented_programming)
//				http://msdn.microsoft.com/en-us/library/ba0a1yw2.aspx
//				http://msdn.microsoft.com/en-us/library/ms173121.aspx
//				http://msdn.microsoft.com/en-us/library/7c5ka91b.aspx

namespace NLP_Assignment1
{
	class Program
	{
		static void Main(string[] args)
		{
            // TODO_EXTRA: fixa verbosity och mode om det finns tid
			LanguageModel lm = new LanguageModel(Verbosity.NORMAL, Mode.NORMAL);
			lm.Run();
		}
	}
}
