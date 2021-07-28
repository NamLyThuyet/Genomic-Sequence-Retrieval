using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /*
     * This class is a constructor class for the inputs in level 1. 
     * It provides appropriate warnings if the inputs are incorrect. 
     * 
     * Author: Phuong Nam Ly October 2019
     */
    public class L1input
    {
        private int startLine;
        private int numSequence;
        private int endLine;
        private int maxLine;

        // The constructor checks starting line and number of sequence whether they are numbers 
        // and then proceeds to check the inputs for level 1
        public L1input(string startLineAsString, string numSequenceAsString, int lineCount)
        {
            bool successStartLine = Int32.TryParse(startLineAsString, out startLine);
            bool successNumSequence = Int32.TryParse(numSequenceAsString, out numSequence);

            if (successStartLine && successNumSequence)
            {
                maxLine = lineCount;
                endLine = endLine = (startLine + numSequence * 2) - 1;

                CheckL1Inputs(startLine, endLine, maxLine);
            }

            if (!successStartLine)
            {
                throw new System.FormatException($"The starting line ({startLineAsString}) must be a number.");
            }
            if (!successNumSequence)
            {
                throw new System.FormatException($"The number of sequence ({numSequenceAsString}) must be a number.");
            }
        }

        /* WarningL1() checks if the inputs from Level1() are correct.
         * 
         * Parameters: the starting line, the ending line, the maximum line from the input file
         * 
         * Return the bool value, which determines the success of the check. 
         * If unsuccessful, throw the exception that gives the appropriate warning.
         */
        public void CheckL1Inputs(int startLine, int endLine, int maxLine)
        {
            // the loop checks all the possible errors specifcally 
            // and output the corresponding warning to the console.
            if (startLine <= 0)
            {
                throw new System.FormatException($"Starting line ({startLine}) must be larger than 0.");
            }
            else if (endLine <= startLine)
            {
                throw new System.FormatException($"The number of sequence ({numSequence}) must be larger than 0.");
            }
            else if ((startLine % 2) != 1)
            {
                throw new System.FormatException($"Starting line ({startLine}) must be an odd number.");
            }
            else if (endLine > maxLine)
            {
                throw new System.FormatException($"The total number of lines ({endLine}) must not exceed maximum line ({maxLine}).");
            }
        }


        /* The following functions are get() functions for the starting line,
         * number of sequence and the ending line.
         */
        public int GetStartLine()
        {
            return startLine;
        }

        public int GetNumSequence()
        {
            return numSequence;
        }

        public int GetEndLine()
        {
            return endLine;
        }
    }
}
