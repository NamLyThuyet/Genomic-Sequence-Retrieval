using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace Search
{
    /*
     * This class allows the user to access specific information from the input file
     * in a sequential order except level 4. The search level implemented in this class is 1-7. 
     * 
     * This class contains Main() and methods which represent search levels from 1-7.
     * 
     * Author: Phuong Nam Ly October 2019
     */
    class Program
    {
        /* Main() takes arguments from the command line, check whether they are correct 
         * and give the corresponding results. If there is a problem with the arguments, 
         * the program will give appropriate warnings to user.
         * 
         * Parameters: a string of arguments from the command lines.
         * 
         * After giving the result, the program will wait for the user to hit enter to 
         * terminate itself through ReadLine().
         */
        public static void Main(string[] args)
        {
            try
            {
                string inputFile = args[1];
                bool inputFileExists = ErrorHandling.CheckFileExists(inputFile);

                if (inputFileExists)
                {
                    string level = args[0];
                    bool ArgCorrect = ErrorHandling.CheckArg(level, args.Length);

                    if (ArgCorrect)
                    {
                        if (level == "-level1")
                        {
                            string startLineAsString = args[2];
                            string numSequenceAsString = args[3];
                            int lineCount = File.ReadLines(inputFile).Count();

                            L1input l1Input = new L1input(startLineAsString, numSequenceAsString, lineCount);

                            int startLine = l1Input.GetStartLine();
                            int numSequence = l1Input.GetNumSequence();
                            int endLine = l1Input.GetEndLine();
                            Level1(inputFile, startLine, endLine);
                        }

                        else if (level == "-level2")
                        {
                            string idSequence = args[2];
                            Level2(inputFile, idSequence);
                        }
                        else if (level == "-level3")
                        {
                            bool L3FileExists = ErrorHandling.CheckFileExists(args[2]);

                            if (L3FileExists)
                            {
                                string[] queryLines = File.ReadAllLines(args[2]);
                                List<string> outputLines = Level3(inputFile, queryLines);
                                File.WriteAllLines(args[3], outputLines);
                            }
                        }
                        else if (level == "-level4")
                        {
                            bool L4IndexedFileExists = ErrorHandling.CheckFileExists(args[2]);
                            bool L4FileExists = ErrorHandling.CheckFileExists(args[3]);

                            if (L4IndexedFileExists && L4FileExists)
                            {
                                string[] indexFile = File.ReadAllLines(args[2]);
                                string[] queryLines = File.ReadAllLines(args[3]);
                                List<string> outputLines = Level4(inputFile, indexFile, queryLines);
                                File.WriteAllLines(args[4], outputLines);
                            }
                        }
                        else if (level == "-level5")
                        {
                            string DNA_String = args[2];
                            Level5(inputFile, DNA_String);
                        }
                        else if (level == "-level6")
                        {
                            string keyword = args[2];
                            Level6(inputFile, keyword);
                        }
                        else if (level == "-level7")
                        {
                            string wildCardSequence = args[2];
                            Level7(inputFile, wildCardSequence);
                        }
                    }
                }
            }
            catch(FileNotFoundException exception)
            {
                Console.WriteLine(exception.Message);
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
            Console.WriteLine();
            Console.WriteLine("Program finishes. Press Enter to exit.");
            Console.ReadLine();
        }

        /* Level1() ouputs a number of sequences to the console based on the provided arguments
         * or give warnings if the provided arguments are incorrect. 
         * 
         * Parameters: the name of the file, starting line, ending line. 
         * The name of the file is for StreamReader access, starting line and ending line is 
         * for the number of desired sequences
         * 
         * Does not return any value, but output the desired number of sequences or error warnings.
         */
        public static void Level1(string file, int startLine, int endLine)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                int lineNumber = 1;
                bool reading = false;

                while ((line = sr.ReadLine()) != null)
                {
                    // The following line helps the program know when to start and when to end
                    // writing to the console
                    if ((lineNumber == startLine) || (lineNumber == endLine + 1))
                    {
                        reading = !reading;
                    }
                    if (reading)
                    {
                        Console.WriteLine(line);
                    }
                    ++lineNumber;
                }             
            }
        }

        /* Level2() sequentially searches and ouputs the provided id Sequence with its corresponding DNA line
         * to the console, or gives an error warning if the id Sequence does not exist.
         * 
         * Parameters: the name of the file, the provided id Sequence.
         * The name of the file is for StreamReader access.
         * 
         * Does not return any value, but ouput the specifically desired sequence,
         * even if the metadata line contains the desired sequence and more sequences.
         */
        public static void Level2(string file, string idSequence)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    // If the metadata line contains the idSequence, the program will continue to search 
                    // the specific idSequence in case the metadata line contains more than one sequence
                    if (line.Contains(idSequence))
                    {
                        MetaTag idSequences = new MetaTag(line);
                        string specificIdSequence = idSequences.GetIdSequence(idSequence);
                        string DnaLine = sr.ReadLine();

                        // The metadatline is splitted at '>', thus '>' was removed from the id Sequence 
                        // so '>' is added here
                        Console.WriteLine('>' + specificIdSequence);
                        Console.WriteLine(DnaLine);
                        return; // stop the function when the id Sequence is found
                    }
                }

                ErrorHandling.WarningInformation(idSequence, "sequence");
            }
        }

        /* Level3() sequentially searches and ouputs a number of provied sequences to a List<string> file.
         * This file will be sent to a text file through Main(). The program will output any 
         * error warning to the console. List<string> is used because the number of the sequences 
         * that will be found is not known at the start. 
         * 
         * Parameters: the name of the file, the array of the provided id Sequence(s)
         * The name of the file is for StreamReader access.           
         *             
         * Return a List<string> variable that contains a list of found sequences
         * so that Main() can receive and convert it into the text file. Any warning
         * error is sent to the console output.
         */
        public static List<string> Level3(string file, string[] queryLines)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                List<string> result = new List<string>();
                List<string> optimizedQuery = DataManipulation.OptimizedArray(queryLines);
                int totalQueryLines = optimizedQuery.Count;
                // Each id sequence will be assigned a specific bool variable
                // to let the program know which id sequence is found
                bool[] success = new bool[totalQueryLines];
                DataManipulation.Fill(success, false);

                while ((line = sr.ReadLine()) != null)
                {
                    // This line must not be put inside the next for loop, 
                    // due to the fact that the loop checks many id Sequences in a line,
                    // ReadLine() might be called more than necessary and DnaLine will be incorrect
                    string DnaLine = sr.ReadLine(); 

                    // The nested loop checks each line in the input file against every provided id Sequence.
                    // If the metadata line contains more than one sequence, the loop will continue to search 
                    // for the specific id sequence.
                    for (int i = 0; i < totalQueryLines; ++i)
                    {
                        if (line.Contains(optimizedQuery[i]))
                        {
                            MetaTag idSequences = new MetaTag(line);
                            string specificIdSequence = idSequences.GetIdSequence(optimizedQuery[i]);

                            // The metadatline is splitted at '>', thus '>' was removed from the id Sequence 
                            // so '>' is added here
                            result.Add('>' + specificIdSequence);
                            result.Add(DnaLine);
                            // let the program which search is succesful    
                            success[i] = true;                        
                        }
                    }
                }

                // The loop check for any unsuccessful search and give appropriate warnings
                for (int i = 0; i < totalQueryLines; ++i)
                {                   
                    if (!success[i]) ErrorHandling.WarningInformation(optimizedQuery[i], "Id sequence");
                }

                return result;
            }
        }

        /* Level4() searches using direct file access and ouputs a number of provied sequences to a List<string> file.
         * This file will be sent to a text file through Main(). The program will output any 
         * error warning to the console. List<string> is used because the number of the sequences 
         * that will be found is not known at the start. 
         * 
         * Parameters: the name of the file, the indexed file, the array of the provided id Sequence(s)
         * The name of the file is for StreamReader and Filestream access.           
         *             
         * Return a List<string> variable that contains a list of found sequences
         * so that Main() can receive and convert it into the text file. Any warning
         * error is sent to the console output.
         */
        public static List<string> Level4(string file, string[] indexFile, string[] queryLines)
        {
            List<string> result = new List<string>();
            List<string> indexQueryLines = new List<string>();
            // Remove any empty line within queryLines to avoid possible bugs with using stream reader
            List<string> optimizedQuery = DataManipulation.OptimizedArray(queryLines);
            int totalQueryLines = optimizedQuery.Count;
            // Each id sequence will be assigned a specific bool variable
            // to let the program know which id sequence is found
            bool[] success = new bool[totalQueryLines];
            DataManipulation.Fill(success, false);

            // The nested loop check every line within the indexed file against every provided id sequence.
            // If the metadata line contains more than one sequence, the loop will continue to search for 
            // the specific id sequence. When there is a match, add the corresponding offset value next to 
            // the id sequence. 
            for (int i = 0; i < indexFile.Length; i++)
            {
                for (int j = 0; j < totalQueryLines; j++)
                {
                    if (indexFile[i].Contains(optimizedQuery[j]))
                    {
                        string[] lineSplitted = indexFile[i].Split(' ');
                        // currentOffset and nextOffset are included to calculate the size of the line
                        long currentOffset = Convert.ToInt64(lineSplitted[1]);
                        long nextOffset;
                        bool successOffset = false;
                        int counter = 0;

                        // The loop searches for the next offset value. However, since id sequences on the 
                        // same metadata line contain the same offset, the loop continues until a larger 
                        // offset is obtainted.
                        do
                        {
                            // The next offset is the offset of the next line, unless when the program 
                            // reaches the end of the indexed file. In that case, the offset is the 
                            // size of the original DNA file.
                            if (i + 1 + counter < indexFile.Length)
                            {
                                string[] nextLineSplitted = indexFile[i + 1 + counter].Split(' ');
                                nextOffset = Convert.ToInt64(nextLineSplitted[1]);
                            }
                            else
                            {
                                nextOffset = new System.IO.FileInfo(file).Length;
                            }

                            if (nextOffset != currentOffset)
                            {
                                successOffset = true;
                            }
                            else
                            {
                                counter++;
                            }
                        }
                        while (!successOffset);                        

                        string size = Convert.ToString(nextOffset - currentOffset);
                        indexQueryLines.Add(indexFile[i] + ' ' + size);
                        // let the program which search is succesful 
                        success[j] = true; 
                    }
                }
            }

            // The loop check for any unsuccessful search and give appropriate warnings
            for (int i = 0; i < totalQueryLines; ++i)
            {
                if (!success[i]) ErrorHandling.WarningInformation(optimizedQuery[i], "Id sequence");
            }

            // use FileStream for direct file access.
            // For using Seek(), the offset has been provided. 
            // Seek() is for directly going to an id sequence.
            // For using Read(), the size of the line is also calculated.
            // Read() is for getting the content of the sequence pair.
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                for (int i = 0; i < indexQueryLines.Count; i++)
                {
                    string[] indexQueryLinesSplitted = indexQueryLines[i].Split(' ');
                    long pos = Convert.ToInt64(indexQueryLinesSplitted[1]);
                    int size = Convert.ToInt32(indexQueryLinesSplitted[2]);
                    byte[] bytes = new byte[size];
                    fs.Seek(pos, SeekOrigin.Begin);
                    fs.Read(bytes, 0, size);

                    // Once the sequences are got, the program searches for the specific id sequence
                    // and its corresponding DNA sequence.
                    string sequences = Encoding.Default.GetString(bytes);
                    List<string> specificSequencePair = SequencePair.GetSequencePair(sequences, indexQueryLinesSplitted[0]);

                    for (int j = 0; j < specificSequencePair.Count; j++)
                    {
                        result.Add(specificSequencePair[j]);
                    }
                }
            }

            return result;
        }

        /* Level5() sequentially searches and any id Sequence that contains the provided DNA string
         * to the console, or gives an error warning if no such Sequence id exist. If a matched DNA 
         * line has more than one sequence id, all the sequences id within the corresponding metadata
         * line is sent to the console.
         * 
         * Parameters: the name of the file, the provided DNA string.
         * The name of the file is for StreamReader access.
         * 
         * Does not return any value, but ouput the a set of id sequences that contain the provided
         * DNA string, or ouput appropriate warning.
         */
        public static void Level5(string file, string DNA_String)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                string metadataLine = "";
                bool success = false;

                while ((line = sr.ReadLine()) != null)
                {
                    // The loop searches for the DNA line that contains the DNA string. When there is
                    // a match, the program outputs all the corresponding id sequences to the console.
                    // The metadata line is stored as the previous line. 
                    if (line.Contains(DNA_String))
                    {
                        MetaTag idSequences = new MetaTag(metadataLine);
                        List<string> AllIdSequences = idSequences.GetIdSequences();

                        for (int i = 0; i < AllIdSequences.Count; ++i)
                        {
                            string id = MetaTag.GetId(AllIdSequences[i]);
                            Console.WriteLine(id);
                        }

                        success = true;
                    }

                    metadataLine = line;
                }

                if (!success) ErrorHandling.WarningInformation(DNA_String, "DNA sequence");
            }
        }

        /* Level6() sequentially searches and any id Sequence that contains the provided keyword
         * to the console, or gives an error warning if no such Sequence id exist.
         * 
         * Parameters: the name of the file, the provided keyword.
         * The name of the file is for StreamReader access.
         * 
         * Does not return any value, but ouput the a set of id sequences that contain the provided
         * keyword, or ouput appropriate warning.
         */
        public static void Level6(string file, string keyword)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                bool success = false;

                while ((line = sr.ReadLine()) != null)
                {
                    // The loop searches for the metadata line that contains the keyword. If the 
                    // metadata line contains more than one sequence, the loop will continue to 
                    // search for the specific id sequence.
                    if (line.Contains(keyword))
                    {
                        MetaTag idSequences = new MetaTag(line);
                        List<string> AllIdSequences = idSequences.GetIdSequences();

                        for (int i = 0; i < AllIdSequences.Count; ++i)
                        {
                            if (AllIdSequences[i].Contains(keyword))
                            {
                                string id = MetaTag.GetId(AllIdSequences[i]);
                                Console.WriteLine(id);

                                success = true;
                            }
                        }
                    }
                }

                if (!success) ErrorHandling.WarningInformation(keyword, "keyword");
            }
        }

        /* Level7() sequentially searches and any id Sequence that contains the provided wild card sequence
         * to the console, or gives an error warning if no such Sequence id exist.
         * 
         * A line contains the wild card sequence if that line contains all of the sub-sequences within that
         * sequence in a sequential order.
         * 
         * Parameters: the name of the file, the provided wild card sequence.
         * The name of the file is for StreamReader access.
         * 
         * Does not return any value, but ouput the a set of id sequences that contain the provided
         * wild card sequence, or ouput appropriate warning.
         */
        public static void Level7(string file, string wildCardSequence)
        {
            string[] sequences = wildCardSequence.Split('*');

            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                string metadataLine = "";
                bool success = false;

                // IndexOf() is the method used to verify whether a line contains the wild card sequence
                // sequence. IndexOf() provides two information: the index of the sub-sequence when successful
                // and -1 when unsuccessful. 
                while ((line = sr.ReadLine()) != null)
                {
                    int index = 0;

                    // The loop sequentially searches for the sub-sequences. When a sub-sequence is found,
                    // the loop will continue to search the rest of the line for the next sub-sequence.
                    // The metadata line is stored as the previous line. 
                    for (int i = 0; i < sequences.Length; i++)
                    {
                        index = line.IndexOf(sequences[i], index);

                        // when the search is unsuccesful, break out of the loop to go to the next line
                        if (index == -1)
                        {
                            break;
                        }

                        // if the last substring is successfully found, the line contains all the sub-string
                        // in a sequential order and thus, contains the wild string.
                        if (i == sequences.Length - 1)
                        {
                            MetaTag idSequences = new MetaTag(metadataLine);
                            List<string> AllIdSequences = idSequences.GetIdSequences();

                            for (int j = 0; j < AllIdSequences.Count; ++j)
                            {
                                string id = MetaTag.GetId(AllIdSequences[j]);
                                Console.WriteLine(id);
                            }

                            success = true;
                        }

                        // index is incremented the length of the substring so that the next IndexOf()
                        // will be applied to the rest of the line.
                        index += sequences[i].Length;
                    }

                    metadataLine = line;
                }

                if (!success) ErrorHandling.WarningInformation(wildCardSequence, "Wild card sequence");
            }
        }
    }
}