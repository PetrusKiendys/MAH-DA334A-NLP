using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace NLP_Assignment1
{
	class Processor
	{

		internal List<string> ExtractWords(string content)
		{
			List<string> res = new List<string>();
			string[] contentArray = content.Split('\n');

			for (int i = 0; i < contentArray.Length; i++)
			{
				if (contentArray[i] == "")
					continue;

				string[] tokens = contentArray[i].Split('\t');

				// LEGACY: processing of the ID field of the .conll file might not be necessary for this assignment...
				//string idval = tokens.ElementAt(0);

				string[] wordSplit = tokens.ElementAt(1).Split('_');
				string word = wordSplit.ElementAt(0);

				word = word.Trim();
				res.Add(word);

				// LEGACY: processing of the ID field of the .conll file might not be necessary for this assignment...
				//intList.Add(idval);	// NOTE: this statement was moved from DataManager
			}

			return res;
		}


		internal Dictionary<string, int> CountNGrams(List<string> wordList, NGram inNGram)
		{
			Dictionary<string, int> res = new Dictionary<string, int>();

			switch (inNGram)
			{
				// count unigrams
				case NGram.UNIGRAM:
					for (int i = 0; i < wordList.Count; i++)
					{
						string word = wordList[i];

						if (res.ContainsKey(word))
							res[word] = res[word] + 1;
						else
							res.Add(word, 1);
					}

					return res;

				// count bigrams
				case NGram.BIGRAM:
					for (int i = 0; i < wordList.Count; i++)
					{
						string word1 = wordList[i];

						if (wordList.Count > i + 1)
						{
							string word2 = wordList[i + 1];
							string bigram = word1 + " " + word2;

							if (res.ContainsKey(bigram))
								res[bigram] = res[bigram] + 1;
							else
								res.Add(bigram, 1);
						}
					}

					return res;
			}

			throw new ArgumentException("Illegal NGram enumerator was passed");
		}


		internal Dictionary<string, float> CalcProb	(Dictionary<string, int> countBigrams, Dictionary<string, int> countUnigrams)
		{
			Dictionary<string, float> res = new Dictionary<string,float>();

			// this part calculates bigram probabilities (the prob of a unigram given a different unigram)
			foreach (KeyValuePair<string, int> entry in countBigrams)
			{
				string bigram = entry.Key;
				int bigramcount = entry.Value;

				string[] split = bigram.Split(' ');

				// QUESTION: purpose of iterating through this code snippet twice (i=0 and i=1)?
				for (int i = 0; i < split.Length; i++)
				{
					string unigram = split[i];
					int unigramcount = countUnigrams[unigram];

					float prob = (float)bigramcount / (float)unigramcount;
					res[entry.Key] = prob;

					if (LanguageModel.verbosity == Verbosity.DEBUG || LanguageModel.verbosity == Verbosity.VERBOSE)
					{
						if (i == 0)
						{
							if (LanguageModel.verbosity == Verbosity.VERBOSE)
								Console.WriteLine("\"" + bigram + "\"" + " has a probability of " + prob);
							findLowProb(entry.Key, prob, 1/4000f);
						}
					}
				}
			}

			return res;
		}


		internal double calcPerplex(Dictionary<string, int> countUnigrams, Dictionary<string, float> probListBigrams)
		{
			// --variables for counters and results of processing--
			//      --results--
			int n = 0;
			double sum = 0, sum_e = 0;

			//      --counters & dev variables--
			int counter = 0;
			double d = 0, dLog = 0;


			// --STEP: calculation of N--
			foreach (KeyValuePair<string, int> entry in countUnigrams)
			{
				n += entry.Value;
			}

			// EXTRA_INFO: by calculating word tokens divided by word types we know that every word appears 5.8 times on average
			// TODO_LOW: put this into Verbosity.DEBUG later
			Console.WriteLine("value of n: " + n);

			// --STEP: summation of bigram probabilities (in log space)
			// NOTE: might want to use double datatype everywhere for probabilities (although this will require more memory/processing)
			foreach (KeyValuePair<string, float> entry in probListBigrams)
			{
				// DEV_CODE: variables for printing out factors and their values (also print the natural logarithm of the factors)
				//counter++;
				//d = entry.Value;
				//dLog = Math.Log(d);

				sum += Math.Log((double)entry.Value);

				// DEV_CODE: prints out factors and log_e factors
				//Console.WriteLine("sum is: " + sum + " after " + counter + " iterations.");
				//Console.WriteLine("d is: " + d + ", dLog is: " + dLog + "\n");
			}

			// TODO_LOW: put this into Verbosity.DEBUG later
			Console.WriteLine("total sum: " + sum);

			// --STEP: raising e to the power of sum (getting out of log space)--

			// BOOKMARK/FIX: get out of log space without underflow!



			//   Console.WriteLine("Raised sum " + sum_e);
			// UNCOMMENT: BigRational r = new BigRational(1.25);

			//  r = Math.Pow(Math.E, sum);
			//r = 1.25;
			// UNCOMMENT: Console.WriteLine("Status: " + r);
			// UNCOMMENT: string output = BigRationalExtensions.ToDecimalString(r, 3);

			// UNCOMMENT: Console.WriteLine("Status efter: " + output);


			// --STEP: inverting sum--

			// --STEP: normalizing sum by the root of N--




			// FIX: return res once we have calculated perplexity!
			//return res;
			return sum;

		}

		/// <summary>
		/// Dev function to identify low bigram probabilities.
		/// </summary>
		/// <param name="bigram">The bigram whose probability is being checked.</param>
		/// <param name="prob">The bigram probability.</param>
		/// <param name="thresh">The threshold under which the probability must lie to trigger printing of the bigram.</param>
		private void findLowProb(string bigram, float prob, float thresh)
		{
			if (thresh > prob)
			{
				Console.WriteLine("\"" + bigram + "\" has a low prob of: " + prob);
			}
		}
	}
}
