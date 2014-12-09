using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// TODO_EXTRA: Fully implement "verbosity" functionality
// UNIMPLEMENTED: Verbosity is not fully implemented!
internal enum Verbosity
{
	VERBOSE,
	DEBUG,
	TEST,
	NORMAL,
	SILENT
}

internal enum NGram
{
	UNIGRAM,
	BIGRAM,
	TRIGRAM
}