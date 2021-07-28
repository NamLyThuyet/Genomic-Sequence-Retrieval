using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /*
     * This class is for extracting the neccessary data within a pair of sequences. 
     * 
     * Author: Phuong Nam Ly October 2019
     */
    public class SequencePair
    {
        /* GetSequencePair() returns the specific sequence pair in case the metadata line contains 
         * more than one sequence. 
         * 
         * Parameters: the metadata line, the idSequence. 
         * 
         * Return the specific id Sequence from the metadata line.
         */
        public static List<string> GetSequencePair(string sequences, string idSequence)
        {
            List<string> result = new List<string>();
            List<string> AllIdSequences = DataManipulation.OptimizedSplit(sequences);

            for (int i = 0; i < AllIdSequences.Count; ++i)
            {
                if (AllIdSequences[i].Contains(idSequence))
                {
                    String Metadataline = AllIdSequences[i];
                    String Sequence = AllIdSequences[AllIdSequences.Count - 1];

                    result.Add(Metadataline);
                    result.Add(Sequence);
                }
            }

            return result;
        }
    }
}
