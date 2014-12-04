using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// TODO_EXTRA:	make a "setVerbosityLevels" constructor/method in LanguageModel that can set an arbitrary amount of verbosity flags
//				or just make sure that all verbosity scenarios are represented by single flags
// MORE_INFO:	https://www.google.com/search?q=c%23+pass+arbitrary+arguments
//					http://msdn.microsoft.com/en-us/library/vstudio/ms229008(v=vs.100).aspx?cs-save-lang=1&cs-lang=csharp
//					http://stackoverflow.com/questions/916616/passing-variable-number-of-arguments
//					http://msdn.microsoft.com/en-us/library/w5zay9db(VS.71).aspx
internal enum Verbosity
{
	VERBOSE,
	DEBUG,
	TEST,
	EXTRA,
	NORMAL,
	SILENT
}

// TODO: clean up code and apply conditional statements for test runs and normal runs
internal enum Mode
{
	TEST,
	NORMAL
}

internal enum NGram
{
	UNIGRAM,
	BIGRAM,
	TRIGRAM
}