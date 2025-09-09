using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FindMyWord
{
    internal class Program
    {
        //this class to store the value of the dictionary
        class ValueInfo
        {
            public List<HashSet<string>> fils;
            public int count = 0;
            // to initialize the list with the number of files
            public ValueInfo(int fileCount)
            {
                fils = new List<HashSet<string>>(fileCount);
                for (int i = 0; i < fileCount; i++)
                {
                    fils.Add(new HashSet<string>());
                }
            }
        }

        // dictionary to store preprocessed data
        static Dictionary<string, ValueInfo> dic = new Dictionary<string, ValueInfo>();




        //reading folder and return list of fils
        static List<string> Read(string path)
        {
            List<string> files = Directory.GetFiles(path).ToList();
            if (files == null)
                throw new Exception("File is empty");
            return files;
        }
       
        //print the result
        static void print(string word)
        {
            //check if the word exist in the dictionary
            if (!dic.ContainsKey(word))
            {
                Console.WriteLine("the word is not found");
                return;
            }

            var target = dic[word];
            int fileCount = 0;
            foreach (var f in target.fils)
            {
                if (f.Count > 0)
                    fileCount++;
            }
            Console.WriteLine($"the word is found {target.count} times in {fileCount} files");
            for (int i = 0; i < target.fils.Count; i++)
            {
                if (target.fils[i].Count == 0)  // skip empty files
                    continue;
                Console.WriteLine($"in file {i + 1}:");
                foreach (string x in target.fils[i])
                {
                    Console.WriteLine(x);
                }
                Console.WriteLine("===================================================");
            }
        }
        // wrap the word with ** in the line
        static string WrapedWord(string line, string word)
        {
            string[] words = line.Split(' ');
            // use StringBuilder for better memory usage and performance
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < words.Length; i++)
            {
                // rebuild the line with wrapped word 
                //compare words and dont care about the case sensitivity
                if (string.Equals(words[i], word, StringComparison.OrdinalIgnoreCase))
                {
                     sb.Append("**").Append(words[i]).Append("**");// wrap with **
                }
                else
                {
                    sb.Append(words[i]);
                }
                if (i < words.Length - 1)
                    sb.Append(" "); 

            }
            
            return sb.ToString();
        }
       
        static void PreProcess(List<string> files)
        {

            Console.WriteLine("Searching files... please wait");

            //search in each file
            for (int i = 0; i < files.Count; i++)
            {

                List<string> lins = File.ReadAllLines(files[i]).ToList();
                //search in each line
                for (int j = 0; j < lins.Count; j++)
                {

                    List<string> words = lins[j].Split(' ').ToList();
                    //search in each word
                    foreach (string word in words)
                    {
                        // skip empty words
                        if (string.IsNullOrEmpty(word))  
                            continue;
                        string ProcessedWord = WrapedWord(lins[j], word);
                        //preprocess 
                        if (!dic.ContainsKey(word))
                        {
                            dic.Add(word, new ValueInfo(files.Count));
                            dic[word].fils[i].Add(ProcessedWord);
                        }
                        else
                        {
                            dic[word].fils[i].Add(ProcessedWord);
                        } 
                        dic[word].count++;
                    }

                }

            }
        }

    
            static void Main(string[] args)
            {
            Console.WriteLine("enter the path of the folder without \" \"");
            string path= Console.ReadLine();
            Console.Clear();
            PreProcess(Read(path));
            Console.Clear();
            bool PrintOneTime = true;
            while (true)
            {
                if (PrintOneTime) 
                {
                    Console.WriteLine("enter a word");
                    PrintOneTime = false;
                }
                else {
                    Console.WriteLine("to exit enter e to continue enter the word");
                }
                string input = Console.ReadLine();
                if( input.ToLower() == "e")
                    return;
                print(input);
            }
           
            }

    }
}
