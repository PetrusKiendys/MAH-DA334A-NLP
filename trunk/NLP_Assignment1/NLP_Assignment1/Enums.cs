using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// TODO_EXTRA: Fully implement "verbosity" functionality wherever it is appropriate
internal enum Verbosity
{
	VERBOSE,
	DEBUG,
	TEST_CALCPROB_PRINT_NGRAMCOUNTS_BIGRAT,
	TEST_CALCPERPLEX_PRINT_SUM_AND_TERMS_IN_LOGSPACE,
	TEST_CALCPERPLEX_DO_APPROX_OF_SUM_OUTSIDE_LOGSPACE,
	TEST,
	NORMAL,
	SILENT
}
internal enum Storage
{
	FLOAT,
	BIGRAT
}

internal enum NGram
{
	UNIGRAM,
	BIGRAM,
	TRIGRAM
}