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
            // TODO_HIGH: add an UnknownManager (from Enum) which specifies a rule with which we generate a subset of unk# markers/words when processing the wordList
			dm.wordList = proc.ExtractWords(dm.LoadFile("../../res/talbanken-dep-train.conll"), UnknownHandler.EVERY_NTH_ROW, 10);
            //dm.wordList = proc.ExtractWords(dm.LoadFile("../../res/talbanken-dep-test.conll"), UnknownHandler.NONE, 10);

			// counting the number of unigrams, bigrams and calculating the probability of bigrams
			dm.countUnigrams = proc.CountNGrams(dm.wordList, NGram.UNIGRAM);
			dm.countBigrams = proc.CountNGrams(dm.wordList, NGram.BIGRAM);

			//dm.probListBigrams = proc.CalcProb(dm.countBigrams, dm.countUnigrams, Storage.FLOAT);
			//dm.probListBigrams_bigrat = proc.CalcProb(dm.countBigrams, dm.countUnigrams, Storage.BIGRAT, Smoothing.NONE);
            
            // NOTE: run CalcProb (...) without smoothing (we want to perform smoothing on the fly)
            dm.probListBigrams_bigrat = proc.CalcProb(dm.countBigrams, dm.countUnigrams, Storage.BIGRAT, Smoothing.NONE);
			
			// calculating perplexity of the training set
			// TODO: store perplex (in DataManager), use an appropriate datatype
            dm.perplex = proc.CalcPerplex(dm.countUnigrams, dm.probListBigrams_bigrat, Storage.BIGRAT, Smoothing.NONE);

			Console.Read();
		}
	}
}
