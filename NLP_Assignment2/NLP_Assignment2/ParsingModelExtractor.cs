using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NLP_Assignment2
{
	class ParsingModelExtractor
	{
        /*
        static Array parenthesisFinder(string row)
        {

            string pattern = @"\(\d+\)";
            string[] lines = Regex.Split(row, pattern);

            for (int i = 0; i < lines.Length; i++)
            {
                string fline = lines[i];

                if (fline.Contains('('))
                {
                    string[] fln = Regex.Split(row, pattern);

                    return parenthesisFinder(fln);
                }

            }

            return output;
        }
        */
		internal void Run()
		{
			// TODO: read in dep-cfg & pos-cfg
			//		 from these files extract a grammar file & lexicon file

            string filepath = "../../res/talbanken-cfg_pos-train.cfg";

            FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            string output = reader.ReadToEnd();
            file.Close();

            // VBP HPP VB VBP PMP MADP
            var row = "(SENT (VBP (HPP (HP Vem))(VB kan)(VBP (VB få)(PMP (PM ATP)))(MADP (MAD ?))))";
            row = row.Substring(6);

		}
	}
}
