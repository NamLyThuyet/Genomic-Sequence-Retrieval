using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /*
     * This class is a constructor class for the metadata lines. 
     * It helps extract the neccessary data within a metadata line. 
     * 
     * Author: Phuong Nam Ly October 2019
     */
    public class MetaTag
    {
        private List<string> sequenceList = new List<string>();

        // The constructor splits the metadataline into a list of individual sequences.
        public MetaTag(string line)
        {          
            List<string> AllIdSequences = DataManipulation.OptimizedSplit(line);

            for (int i = 0; i < AllIdSequences.Count; ++i)
            {
                sequenceList.Add(AllIdSequences[i]);
            }
        }

        /* GetIdSequence() searches for the specific id Sequence in case the metadata line contains 
         * more than one sequence.  
         * 
         * Parameters: the metadata line, the idSequence. 
         * 
         * Return the specific id Sequence from the metadata line.
         */
        public string GetIdSequence(string idSequence)
        {
            string idSplit = "";

            // the for loop searches for the specific id Sequence out of all the id Sequences
            for (int i = 0; i < sequenceList.Count; ++i)
            {
                if (sequenceList[i].Contains(idSequence))
                {
                    idSplit = sequenceList[i];
                }
            }

            return idSplit;
        }

        /* GetIdSequences() returns all id sequences within the metadata line. 
         * 
         * Parameters: the metadata line, the idSequence. 
         * 
         * Return the specific id Sequence from the metadata line.
         */
        public List<string> GetIdSequences()
        {
            return sequenceList;
        }

        /* GetId() only returns the id of the provided id sequence by using Substring().
         * 
         * Parameters: the provided id sequence. 
         * 
         * Return returns the id of the provided id sequence.
         */
        public static string GetId(string idSequence)
        {
            int sequenceIdIndex = 11;

            return idSequence.Substring(0, sequenceIdIndex);
        }
    }
}
