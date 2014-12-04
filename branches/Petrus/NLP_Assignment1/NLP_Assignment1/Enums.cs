using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// TODO: make a "setVerbosityLevels" constructor/method in LanguageModel that can set an arbitrary amount of verbosity flags
// MORE INFO:	https://www.google.com/search?q=c%23+pass+arbitrary+arguments
					http://msdn.microsoft.com/en-us/library/vstudio/ms229008(v=vs.100).aspx?cs-save-lang=1&cs-lang=csharp
					http://stackoverflow.com/questions/916616/passing-variable-number-of-arguments
					http://msdn.microsoft.com/en-us/library/w5zay9db(VS.71).aspx
internal enum Verbosity
{
	VERBOSE,
	DEBUG,
	TEST,
	EXTRA,
	NORMAL,
	SILENT
}

internal enum NGram
{
	UNIGRAM,
	BIGRAM,
	TRIGRAM
}