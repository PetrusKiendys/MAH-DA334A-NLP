using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLP_Assignment1;

namespace NLP_Assignment1
{
	class LanguageModel
	{
		// TODO: check if this variable is accessible from all classes and apply it for usage
		// MORE INFO: http://msdn.microsoft.com/en-us/library/79b3xss3.aspx
		public static Verbosity verbosity;

		public LanguageModel(Verbosity verbosity)
		{
			LanguageModel.verbosity = verbosity;
		}


		internal void Run()
		{
			DataManager dm = new DataManager();
			Processor proc = new Processor();

			dm.LoadFile();

			//proc.CountNGramsAndCalcProb(dm.wordList, dm.wordListUnigrams, dm.wordListBigrams, dm.probListBigrams);
			//proc.calcPerplex(dm.wordListUnigrams,dm.probListBigrams);

			Console.Read();
		}
	}
}
