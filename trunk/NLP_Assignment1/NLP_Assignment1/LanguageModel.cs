using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLP_Assignment1;

namespace NLP_Assignment1
{
    class LanguageModel
    {
        internal void Run()
        {
            DataManager dm = new DataManager();
            Processor proc = new Processor();

            dm.LoadFile();

            proc.CountNGramsAndCalcProb(dm.wordList, dm.wordListUnigrams, dm.wordListBigrams, dm.probListBigrams);
            proc.calcPerplex(dm.wordListUnigrams,dm.probListBigrams);

            Console.Read();
        }
    }
}
