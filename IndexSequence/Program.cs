using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ClassLibrary;

/*
 * This class allows the user to index the provided file for implementing direct file access. 
 * The output file will contains the sequence id and corresponding offset.
 * 
 * Author: Phuong Nam Ly October 2019
 */
namespace IndexSequence
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFile = args[0];
                bool inputFileExists = ErrorHandling.CheckFileExists(inputFile);
                ErrorHandling.CheckArgIndex(args.Length);

                if (inputFileExists)
                {
                    List<string> result = new List<string>();

                    using (StreamReader sr = new StreamReader(inputFile))
                    {
                        string line;
                        int offset = 0;
                        int numLine = 1;

                        while ((line = sr.ReadLine()) != null)
                        {
                            // the loop only accesses the metadata line since only the sequence ids 
                            // and their responding offset is needed. If the metdata line contains more than
                            // one id, all those id sequences within the line will have the same offset.
                            if (numLine % 2 == 1)
                            {
                                MetaTag idSequences = new MetaTag(line);
                                List<string> AllIdSequences = idSequences.GetIdSequences();

                                for (int j = 0; j < AllIdSequences.Count; j++)
                                {
                                    string id = MetaTag.GetId(AllIdSequences[j]);
                                    result.Add(id + " " + offset.ToString());
                                }
                            }

                            // the offset is updated every loop with + 1 for the '\n' character
                            offset += line.Length + 1;
                            numLine++;
                        }

                        File.WriteAllLines(args[1], result);
                    }
                }
            }
            catch (FormatException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            // ReadLine() in the end with a message 
            // for the program to stop exiting after it finishes in Debug mode
            Console.WriteLine("Program finishes. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
