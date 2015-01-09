using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment2
{
	// TODO_LOW: maybe rename ParsingModelExtractor to something more clear..
	//			 (which makes it clear that it outputs rule sets in the form of grammar and lexicon files)
	//		f.e. - RulesExtractor (maybe not, too many functions in Processor.cs use this naming style?)
	//			 - Phrase Structure Tree Rules Extractor (messy..)
	//			 - Bracketing Format Tree Rules Extractor (still messy but getting somewhere..)
	class ParsingModelExtractor
	{
		internal void Run(string file)
		{
			DataManager dm = new DataManager();
			Processor proc = new Processor();
			
			string[] filename_array = file.Split('/');
			string filename = filename_array[filename_array.Length-1];

			// load the phrase structure trees
			Console.WriteLine("Loading the phrase structure tree from "+filename);
			string data = dm.LoadFile(file);
			string[] sentences = data.Split('\n');
			sentences = sentences.Take(sentences.Count()-1).ToArray();	// getting rid of the last empty row in the data file

			// extract rules from the structure trees
			Console.WriteLine("Extracting rules from phrase structure trees...");
			proc.ExtractRules(sentences, dm.grammarRules, ExtractMode.GRAMMAR);
			proc.ExtractRules(sentences, dm.lexiconRules, ExtractMode.LEXICON);

			// format grammar and lexicon
			Console.WriteLine("Formatting grammar and lexicon lists...");
			dm.formattedGrammarRules = proc.FormatRules(dm.grammarRules, out dm.grammarRulesCounter, ExtractMode.GRAMMAR, Separator.TAB);
			dm.formattedLexiconRules = proc.FormatRules(dm.lexiconRules, out dm.lexiconRulesCounter, ExtractMode.LEXICON, Separator.TAB);

			// save the grammar and lexicon rules to text files
			if (file.Contains("pos"))
			{
				dm.SaveFile("../../out/grammar-pos.txt", dm.formattedGrammarRules, ExtractMode.GRAMMAR);
				dm.SaveFile("../../out/lexicon-pos.txt", dm.formattedLexiconRules, ExtractMode.LEXICON);
			}
			else if (file.Contains("dep"))
			{
				dm.SaveFile("../../out/grammar-dep.txt", dm.formattedGrammarRules, ExtractMode.GRAMMAR);
				dm.SaveFile("../../out/lexicon-dep.txt", dm.formattedLexiconRules, ExtractMode.LEXICON);
			}
			Console.WriteLine("The grammar and lexicon files have been successfully created for "+filename+"\n");
			
		}
	}
}