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
		// TODO_LOW: check if "verbosity" is accessible from all classes and apply it for usage
		// MORE_INFO: http://msdn.microsoft.com/en-us/library/79b3xss3.aspx
		internal static Verbosity verbosity;
		internal static Mode mode;

		public LanguageModel(Verbosity verbosity, Mode mode)
		{
			LanguageModel.verbosity = verbosity;
			LanguageModel.mode = mode;
		}


		public void Run()
		{
			DataManager dm = new DataManager();
			Processor proc = new Processor();

			dm.wordList = dm.ExtractWords(dm.LoadFile());

			dm.wordListUnigrams = proc.CountNGrams(dm.wordList, NGram.UNIGRAM);
			dm.wordListBigrams = proc.CountNGrams(dm.wordList, NGram.BIGRAM);
			dm.probListBigrams = proc.CalcProb(dm.wordListBigrams, dm.wordListUnigrams);
			// TODO: store perplex somewhere (in DataManager?)
			var perplex = proc.calcPerplex(dm.wordListUnigrams, dm.probListBigrams);
			
			//proc.calcPerplex(dm.wordListUnigrams,dm.probListBigrams);

			Console.Read();
		}
	}
}
