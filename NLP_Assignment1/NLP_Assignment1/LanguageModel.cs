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
		internal static Verbosity verbosity;


		public LanguageModel(Verbosity verbosity)
		{
			LanguageModel.verbosity = verbosity;
		}


		public void Run()
		{	
			DataManager dm = new DataManager();
			Processor proc = new Processor();

			// extracting a List of words from the training set
			dm.wordList = proc.ExtractWords(dm.LoadFile("../../res/talbanken-dep-train.conll"));

			// counting the number of unigrams, bigrams and calculating the probability of bigrams
			dm.countUnigrams = proc.CountNGrams(dm.wordList, NGram.UNIGRAM);
			dm.countBigrams = proc.CountNGrams(dm.wordList, NGram.BIGRAM);
			//dm.probListBigrams = proc.CalcProb(dm.countBigrams, dm.countUnigrams, Storage.FLOAT);
			dm.probListBigrams_bigrat = proc.CalcProb(dm.countBigrams, dm.countUnigrams, Storage.BIGRAT);
			
			// calculating perplexity of the training set
			// TODO: store perplex (in DataManager), use an appropriate datatype
			var perplex = proc.CalcPerplex(dm.countUnigrams, dm.probListBigrams_bigrat, Storage.BIGRAT);

			Console.Read();
		}
	}
}
