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
                // special case: assign end-marker #e at the end of the set, break out of the loop after doing this (no need to process trailing empty lines)
                if (i == contentArray.Length-2)
                {
                    res.Add("#e");
                    break;
                }

                // when the application encounters an empty line
				else if (contentArray[i] == "")
                {
                    res.Add("#e");
                    res.Add("#s");
                }

                // when the application encounters a line containing a tagged word
                else
                {
                    // special case: assign start-marker #s at the beginning of the set
                    if (i == 0)
                    {
                        res.Add("#s");
                    }

                    string[] tokens = contentArray[i].Split('\t');

                    string id_field = tokens.ElementAt(0);
                    string word_field = tokens.ElementAt(1);

                    //word = word.Trim();
                    //res.Add(word);

                    // NOTE: just in case..
                    word_field = word_field.Trim();
                    res.Add(word_field);

                    // LEGACY: processing of the ID field of the .conll file might not be necessary for this assignment...
                    //intList.Add(idval);	// NOTE: this statement was moved from DataManager
                }
				
				
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
        // TODO_LOW: fix support for Laplace add-one smoothing when using float type?
		internal Dictionary<string, BigRational> CalcProb	(Dictionary<string, int> countBigrams, Dictionary<string, int> countUnigrams, Enum storage, Enum smoothing)
		{

			Dictionary<string, BigRational> res_bigrat = new Dictionary<string, BigRational>();
			Dictionary<string, float> res = new Dictionary<string, float>();
            BigRational prob_bigrat = new BigRational(1,1);
			float prob;
			   

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

					// NOTE:	the reason why the operator == does not work in this case is because Enums are strongly typed
					//			and as such, the operator == compares the reference of the Enums and not their values
					if (storage.Equals(Storage.FLOAT))
					{
						prob = (float)bigramcount / (float)unigramcount;
						res[entry.Key] = prob;
					}
						
					else if (storage.Equals(Storage.BIGRAT))
					{
                        if (smoothing.Equals(Smoothing.ADDONE))
                        {
                            int numerator = bigramcount + 1;
                            int denominator = unigramcount + countUnigrams.Count;

                            prob_bigrat = new BigRational(numerator, denominator);
                            res_bigrat[entry.Key] = prob_bigrat;
                        }

                        if (smoothing.Equals(Smoothing.NONE))
                        {
                            prob_bigrat = new BigRational(bigramcount, unigramcount);
                            res_bigrat[entry.Key] = prob_bigrat;
                        }
						

						if (LanguageModel.verbosity == Verbosity.TEST_CALCPROB_PRINT_NGRAMCOUNTS_BIGRAT)
						{
							// only print for when bigramcount has the value 2 or more (this is more interesting to look at)
							if (bigramcount > 1)
							{
								Console.WriteLine("\"" + entry.Key + "\": bigramcount=" + bigramcount + ", unigramcount=" + unigramcount);
								Console.WriteLine("prob_bigrat                    =" + prob_bigrat);
							}
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

			if (storage.Equals(Storage.FLOAT))
				throw new NotImplementedException("fix return types of this function!");
			// TODO_LOW: fix this so that you can return a float, this error occurs because this method only returns one datatype (BigRational for now...)
			// return res;

			else if (storage.Equals(Storage.BIGRAT))
				return res_bigrat;

			return null;
		}

		// TODO_LOW:    fix this so that you can send both Dictionary<string, float> and Dictionary<string, BigRational>, using "optional"
		//              so that we're able to send either "probListBigrams_bigrat" or "probListBigrams"
		internal decimal CalcPerplex(Dictionary<string, int> countUnigrams, Dictionary<string, BigRational> probListBigrams, Enum storage)
		{
			// --variables for counters and results of processing--
			//      --results--
            int n = 0;
            // CLEANUP: sum_e is no longer in use, can be safely deleted?
			double sum = 0, sum_e = 0;
            decimal sum_e_hardcoded = 0;

			//      --counters & dev variables--
			int counter = 0;
			double d = 0, dLog = 0;
			BigRational bigrat_smallest = new BigRational(1,1), bigrat_try_smaller;
			decimal deci_smallest = 1.0m, deci_try_smaller;
			bool zero_reached_deci = false, zero_reached_bigrat = false;


			// --STEP: calculation of N--
			foreach (KeyValuePair<string, int> entry in countUnigrams)
			{
				n += entry.Value;
			}

			// EXTRA_INFO: by calculating word tokens divided by word types we know that every word appears 5.8 times on average
			if (LanguageModel.verbosity == Verbosity.DEBUG)
				Console.WriteLine("value of n: " + n);

			// --STEP: summation of bigram probabilities (in log space)
			// NOTE: might want to use double datatype everywhere for probabilities (although this will require more memory/processing)
			
			// TODO_LOW: make a conditional that runs either:
			//      foreach (KeyValuePair<string, float> entry in probListBigrams)		or
			//      foreach (KeyValuePair<string, BigRational> entry in probListBigrams)
			foreach (KeyValuePair<string, BigRational> entry in probListBigrams)
			{
				counter++;
				sum += Math.Log((double)entry.Value);

				// DEBUG_CODE: prints out factors and log_e factors
				if (LanguageModel.verbosity == Verbosity.TEST_CALCPERPLEX_PRINT_SUM_AND_TERMS_IN_LOGSPACE)
				{
					d = (double)entry.Value;
					dLog = Math.Log(d);

					Console.WriteLine("sum is: " + sum + " after " + counter + " iterations.");
					Console.WriteLine("d is: " + d + ", dLog is: " + dLog + "\n");
				}

				// TEST_CODE: approximation of sum outside of logspace, holds the smallest value that sum can be in order to be stored in a variable outside of logspace
				if (LanguageModel.verbosity == Verbosity.TEST_CALCPERPLEX_DO_APPROX_OF_SUM_OUTSIDE_LOGSPACE)
				{
					deci_try_smaller = ((decimal)Math.Pow(Math.E, sum));
					if (deci_try_smaller == 0.0m && zero_reached_deci == false)
					{
						//Console.WriteLine("deci_try_smaller is 0.0 after " + counter + " iterations!");
						zero_reached_deci = true;
					}

					bigrat_try_smaller = ((decimal)Math.Pow(Math.E, sum));
					if (bigrat_try_smaller.Equals(new BigRational(0,1)) && zero_reached_bigrat == false)
					{
						//Console.WriteLine("bigrat_try_smaller is 0.0 after " + counter + " iterations!");
						zero_reached_bigrat = true;
					}

					if (counter < 25)
					{
						Console.WriteLine("deci_try_smaller=" + deci_try_smaller + " after " + counter + " iterations");
						Console.WriteLine("bigrat_try_smaller=" + bigrat_try_smaller + " after " + counter + " iterations\n");
					}

				}
			}

			//if (LanguageModel.verbosity == Verbosity.DEBUG)
			Console.WriteLine("sum printed as double: " + sum);
			Console.WriteLine("total number of added terms (for-loop iterations): " + counter);




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
			
			//double res_dbl = Math.Pow(Math.E, sum);
			//decimal res_deci = (decimal)res_dbl;
			//BigRational res_bigrat = new BigRational(res_deci);

			BigRational bigrat = new BigRational((decimal)Math.Pow(Math.E, sum));
			Console.WriteLine("BigRational: " + bigrat);

			// NOTE: computationally viable to set the precision between 10k and 40k
			string output = BigRationalExtensions.ToDecimalString(bigrat, 1000);
			Console.WriteLine("BigRational (toDecimalString): " + output);

            // FIX:     because we calculated perplex after implementing start & end-markers, we need to recalculate this with the new values
            // NOTE:    the description below describes how to calculate perplexity without start & end-markers and without implementing management of unknown words
			// TODO:	because C# has difficulty handling very small numbers, we will calculate "sum_e"
			//			by inputting the "sum" into a computational engine (such as Wolfram Alpha) and extracting
			//			"sum_e" which can be used in the subsequent steps to calculate the perplexity.
			// LINK:	http://www.wolframalpha.com/input/?i=e%5E%28-174535.446785493%29
            // EXTERNAL:    this number was calculated through wolfram alpha:
            //              1.6541105885843545100551470696874531291108711955629919916855659990642064458619384300080
            //              284241984176186094758979992376887731735906925163945239440931530517915804023855480363215
            //              7957730260930179126966852901223646001084199656991457820174231... × 10^-75800
            // NOTE:        C# has difficulty handling this number, we will invert it to continue with the perplexity calculation

			// --STEP: inverting sum--
            // LINK:    http://www.wolframalpha.com/input/?i=1%2Fe%5E%28-174535.446785493%29
            // NOTE:    C# has difficulty with handling the number 1/e^(-174535.446785493)
            //          We have calculated this number with wolfram alpha:
            //          6.04554500104998935711733071395160523237170900189086752690959951862974246839745464388983577972127413041233
            //          7843819177174613557637995137097258865503608392846520743003129861716961802231406502134171634994161639926069
            //          103416553130558789805554... × 10^75799
			
			// --STEP: normalizing sum by the root of N--
            // LINK:    http://www.wolframalpha.com/input/?i=%281%2Fe%5E%28-174535.446785493%29%29%5E%281%2F75970%29
            // NOTE:    C# has a difficulty with calculating the root when the argument provided consists of very large numbers
            //          We have calculated this number with wolfram alpha:
            //          9.94854108007602812177072860401898554115372243591722588104955931582742967481942169505813521900927379428535
            //          5594929572236758991481328872819494773771651499871083331169747341427166337549459064678348218047152739124260
            //          7546365242329060695883293875644...
            //          Rounding down we will store this number with a precision of 6 decimals.
            decimal perplex = 9.948541m;

			return perplex;

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
