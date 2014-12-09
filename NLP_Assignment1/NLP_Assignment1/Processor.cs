using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Numerics;

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

        // TODO_LOW:    fix this method so that it can return both Dictionary<string, float>
        //              and Dictionary<string, BigRational> through the use of "out" and "ref"
		internal Dictionary<string, BigRational> CalcProb	(Dictionary<string, int> countBigrams, Dictionary<string, int> countUnigrams, Enum storage)
		{
            //if (storage == (Enum)Storage.BIGRAT)
            //{
            //    Dictionary<string, BigRational> res_bigrat = new Dictionary<string, BigRational>();
            //}
            //if (storage == (Enum)Storage.FLOAT)
            //{
            //      ...
            //}

            Dictionary<string, BigRational> res_bigrat = new Dictionary<string, BigRational>();
            Dictionary<string, float> res = new Dictionary<string, float>();
            BigRational prob_bigrat;
            float prob;
            Enum storage_casted = (Enum)storage;
               

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

                    if (storage_casted == (Enum)Storage.FLOAT)
                    {
                        prob = (float)bigramcount / (float)unigramcount;
                        res[entry.Key] = prob;
                    }
                        
                    else if (storage_casted == (Enum)Storage.BIGRAT)
                    {
                        prob_bigrat = new BigRational(bigramcount, unigramcount);
                        res_bigrat[entry.Key] = prob_bigrat;

                        if (LanguageModel.verbosity == Verbosity.DEBUG)
                        {
                            Console.WriteLine("for " + entry.Key + ": bigramcount="+bigramcount+", unigramcount="+unigramcount);
                            Console.WriteLine("prob_bigrat="+prob_bigrat);
                        }
                    }
                    

					if (LanguageModel.verbosity == Verbosity.DEBUG || LanguageModel.verbosity == Verbosity.VERBOSE)
					{
						if (i == 0)
						{
                            // TODO_LOW: assign prob properly
                            //if (LanguageModel.verbosity == Verbosity.VERBOSE)
                            //    Console.WriteLine("\"" + bigram + "\"" + " has a probability of " + prob);
                            //findLowProb(entry.Key, prob, 1/4000f);
						}
					}
				}
			}

            if (storage == (Enum)Storage.FLOAT)
                throw new NotImplementedException("fix return types of this functioN!");
            // TODO_LOW: fix this so that you can return a float, this error occurs because this method only returns one datatype (BigRational for now...)
            // return res;
            else if (storage == (Enum)Storage.BIGRAT)
                return res_bigrat;

            return null;
		}

        // TODO_LOW:    fix this so that you can send both Dictionary<string, float> and Dictionary<string, BigRational>, using "optional"
        //              so that we're able to send either "probListBigrams_bigrat" or "probListBigrams"
		internal double calcPerplex(Dictionary<string, int> countUnigrams, Dictionary<string, BigRational> probListBigrams, Enum storage)
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
            
            // TODO_LOW: make a conditional that runs either:
            //      foreach (KeyValuePair<string, float> entry in probListBigrams)  or
            //      foreach (KeyValuePair<string, BigRational> entry in probListBigrams)
			foreach (KeyValuePair<string, BigRational> entry in probListBigrams)
			{
				// DEV_CODE: variables for printing out factors and their values (also print the natural logarithm of the factors)
				//counter++;
				//d = entry.Value;
				//dLog = Math.Log(d);

				sum += Math.Log((double)entry.Value);

				// DEV_CODE: prints out factors and log_e factors
				if (LanguageModel.verbosity == Verbosity.TEST)
				{
					Console.WriteLine("sum is: " + sum + " after " + counter + " iterations.");
					Console.WriteLine("d is: " + d + ", dLog is: " + dLog + "\n");
				}
				
			}

			// TODO_LOW: put this into Verbosity.DEBUG later
			Console.WriteLine("sum printed as double: " + sum);

			// --STEP: raising e to the power of sum (getting out of log space)--


			//   Console.WriteLine("Raised sum " + sum_e);

			//	NOTE: 18446744073709551615 is the max value that BigInteger can hold (this equals to (2^64)-1)
			//BigRational r = new BigRational(new BigInteger(1), new BigInteger(18446744073709551615));
			//BigRational s = new BigRational(new BigInteger(1), new BigInteger(100000000000000000));
			//BigRational t = r+s;

			//float flt = 0.5f;
			//double dbl = 0.5;
			//decimal deci = 0.5m;
			//BigRational u = new BigRational(deci);

			//decimal sum_deci = (decimal)sum;
			//BigRational u = new BigRational(sum_deci);
			
            double res_dbl = Math.Pow(Math.E, sum);
			decimal res_deci = (decimal)res_dbl;
			BigRational res_bigrat = new BigRational(res_deci);

            BigRational bigrat = new BigRational((decimal)Math.Pow(Math.E, sum));

			//  r = Math.Pow(Math.E, sum);
			//r = 1.25;
			Console.WriteLine("BigRational: " + bigrat);
			// NOTE: computationally viable to set the precision between 10k and 40k
			string output = BigRationalExtensions.ToDecimalString(bigrat, 1000);

			Console.WriteLine("BigRational (toDecimalString): " + output);


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
