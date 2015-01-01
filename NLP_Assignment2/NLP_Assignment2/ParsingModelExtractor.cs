using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment2
{
// TODO: at some point methods need to be generalized for both construction of grammar and lexicon files
//		 nodes need to be extracted with and without words
//		 use Enums to do this
	class ParsingModelExtractor
	{
		internal void Run()
		{
			DataManager dm = new DataManager();
			Processor proc = new Processor();

			string data = dm.LoadFile("../../res/data/talbanken-cfg_pos-train.cfg");
			string[] sentences = data.Split('\n');
			sentences = sentences.Take(sentences.Count()-1).ToArray();  // getting rid of the last empty row in the data file
			proc.ExtractRules(sentences, ExtractMode.GRAMMAR, dm.grammarRules);
			proc.ExtractRules(sentences, ExtractMode.LEXICON, dm.lexiconRules);


		}
	}
}