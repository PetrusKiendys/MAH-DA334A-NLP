using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment1
{
	// TODO_HIGH: cleanup and improve the console output of this application when LM is running with verbosity level "Verbosity.NORMAL"
	// TODO: remove support for passing "float" or decimal datatypes, only BigRational should be used
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
			// TODO_HIGH: add an UnknownManager (from Enum) which specifies a rule with which we generate a subset of unk# markers/words when processing the wordList(???)
			dm.wordList = proc.ExtractWords(dm.LoadFile("../../res/data/talbanken-dep-train.conll"), UnknownHandler.RANDOMIZE, 50);

			// counting the number of unigrams, bigrams and calculating the probability of bigrams
			dm.countUnigrams = proc.CountNGrams(dm.wordList, NGram.UNIGRAM);
			dm.countBigrams = proc.CountNGrams(dm.wordList, NGram.BIGRAM);
			
			// NOTE: run CalcProb (...) without smoothing (we want to perform smoothing on the fly)
			dm.probListBigrams = proc.CalcProb(dm.countBigrams, dm.countUnigrams, Storage.BIGRAT, Smoothing.NONE);
			
			// calculating perplexity of the training set
			// TODO: store perplex (in DataManager), use an appropriate datatype
			dm.perplex = proc.CalcPerplex(dm.countUnigrams, dm.probListBigrams, Storage.BIGRAT, Smoothing.ADDONE);

			dm.wordList2 = proc.ExtractWords(dm.LoadFile("../../res/data/talbanken-dep-test.conll"), UnknownHandler.NONE, 0);
			dm.countUnigrams2 = proc.CountNGrams(dm.wordList2, NGram.UNIGRAM);

			dm.wordDiffList = proc.ExtractDiffWords(dm.countUnigrams, dm.countUnigrams2);
			dm.SaveFile("../../out/output.txt", dm.wordDiffList);
		}
	}
}
