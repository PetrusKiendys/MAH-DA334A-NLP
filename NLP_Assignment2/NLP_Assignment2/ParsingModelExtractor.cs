using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment2
{
	class ParsingModelExtractor
	{
		internal void Run()
		{
			DataManager dm = new DataManager();
			Processor proc = new Processor();

			string data = dm.LoadFile("../../res/data/talbanken-cfg_pos-train.cfg");
			string[] sentences = data.Split('\n');
			sentences = sentences.Take(sentences.Count()-1).ToArray();	// getting rid of the last empty row in the data file

			proc.ExtractRules(sentences, dm.grammarRules, ExtractMode.GRAMMAR);
			//string[] tempSentences = {"(SENT (NNP (JJP (JJ Individuell))(NN beskattning)(PPP (PP av)(NNP (NN arbetsinkomster)))))"};
			//proc.ExtractRules(tempSentences, ExtractMode.LEXICON, dm.lexiconRules);
			proc.ExtractRules(sentences, dm.lexiconRules, ExtractMode.LEXICON);

			// format grammar & lexicon
			dm.formattedGrammarRules = proc.FormatRules(dm.grammarRules, ExtractMode.GRAMMAR);
			dm.formattedLexiconRules = proc.FormatRules(dm.lexiconRules, ExtractMode.LEXICON);
		}
	}
}