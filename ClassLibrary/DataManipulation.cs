using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /*
     * This program assissts Program.cs in manipulating data for the data search.
     * 
     * Author: Phuong Nam Ly October 2019
     */
    public class DataManipulation
    {
        /* OptimizedArray() removes any empty element within the string array 
         * and returns a List<string> that only contains relevant elements.
         * 
         * Parameters: the provided string array. 
         * 
         * return a List<string> that contains non-empty string.
         */
        public static List<string> OptimizedArray(string[] Array)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < Array.Length; ++i)
            {
                if (Array[i].Length > 0)
                {
                    result.Add(Array[i]);
                }
            }

            return result;
        }

        /* OptimizedSplit() splits the given string, removes any empty sub-string 
         * and returns a List<string> that only contains relevant sub-string.
         * 
         * Parameters: the provided string. 
         * 
         * return a List<string> that contains non-empty string.
         */
        public static List<string> OptimizedSplit(string line)
        {
            List<string> result = new List<string>();
            string[] AllIdSequences = line.Split('>', '\n');

            for (int i = 0; i < AllIdSequences.Length; ++i)
            {
                if (AllIdSequences[i].Length > 0)
                {
                    result.Add(AllIdSequences[i]);
                }
            }

            return result;
        }

        /* Fill<T> fills the provided array with the provided value with <T> being a generic type parameter.  
         * 
         * Parameters: the array, the desired value. 
         * With the use of <T>, the array and the desired value can be arbitrary type T,
         * leading to the versatile use of filling any type of array with any type of value (within reasons).
         * 
         * Does not return any value, but every element in the provided array is assined to the provided value.
         */
        public static void Fill<T>(T[] originalArray, T value)
        {
            for (int i = 0; i < originalArray.Length; ++i)
            {
                originalArray[i] = value;
            }
        }
    }
}