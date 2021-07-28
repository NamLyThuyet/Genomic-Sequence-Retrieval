using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /*
     * This program assissts Program.cs in error handling by throwing exceptions.
     * 
     * Author: Phuong Nam Ly October 2019
     */
    public class ErrorHandling
    {
        /* CheckArg() checks if the provided level and the number of arguments for that level are appropriate.
         * 
         * Parameters: the level, the length of the argument corresponding to that level.
         * 
         * Return the bool value, which determines whether the inputs are correct. 
         * If unsuccessful, throw the exception that gives the appropriate warning.
         */
        public static bool CheckArg(string level, int LengthArg)
        {
            bool success = false;

            // the loop checks firstly, if the provided level is one of the expected ones; 
            // secondly, if the number of arguments match the expected arguments.
            if ((level != "-level1")
                    && (level != "-level2")
                    && (level != "-level3")
                    && (level != "-level4")
                    && (level != "-level5")
                    && (level != "-level6")
                    && (level != "-level7"))
            {
                throw new System.FormatException($"({level}) is an incorrect level or it has not been implemented");
            }
            else if ((level == "-level1" && LengthArg == 4)
                || (level == "-level2" && LengthArg == 3)
                || (level == "-level3" && LengthArg == 4)
                || (level == "-level4" && LengthArg == 5)
                || (level == "-level5" && LengthArg == 3)
                || (level == "-level6" && LengthArg == 3)
                || (level == "-level7" && LengthArg == 3))
            {
                success = true;
            }
            else
            {
                throw new System.FormatException($"The number of arguments ({LengthArg}) for ({level}) is incorrect");
            }

            return success;
        }

        /* CheckArgIndex(int LengthArg) checks whether the number of arguments for 
         * the index program is appropriate.
         * 
         * Parameters: the length of the arguments.
         * 
         * If the length is correct, throw the exception that gives the appropriate warning.
         */
        public static void CheckArgIndex(int LengthArg)
        {
            if (LengthArg != 2)
            {
                throw new System.FormatException($"The number of arguments ({LengthArg}) is incorrect");
            }
        }

        /* CheckFileExists() checks if the provided file exists.
         * 
         * Parameters: the file
         * 
         * Return the bool value, which determines the success of the check. 
         * If unsuccessful, throw the exception that gives the appropriate warning.
         */
        public static bool CheckFileExists(string inputFile)
        {
            bool success = false;

            if (File.Exists(inputFile))
            {
                success = true;
            }
            else
            {
                throw new System.IO.FileNotFoundException($"The input file {inputFile} does not exist or it is incorrectly formatted");
            }

            return success;
        }

        /* WarningIdSequence() outputs the warning to the console, when the information is not found.
         * 
         * Parameters: the information, the type of information.
         * 
         * If unsuccessful, throw the exception that gives the appropriate warning.
         */
        public static void WarningInformation(string information, string type)
        {
            Console.WriteLine($"Error, {type} { information} not found");
        }
    }
}
