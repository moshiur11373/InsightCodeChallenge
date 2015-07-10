using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace InsightCodeC
{
    class Program
    {
        static string line;
        static string pattern = "\\s+";
        static List<string> stringArray = new List<string>();
        static string inputPath = "tweet_input/tweets.txt", outputPath1 = "tweet_output/feature1.txt", outputPath2 = "tweet_output/feature2.txt";

        public static void Main(string[] args)
        {
            //WordCount();

            UniqueWordsMedian();
        }

        public static void WordCount()
        {
            // Instantiate a dictionary to keep word and its total occurrence
            Dictionary<string, int> wordCount = new Dictionary<string, int>();

            // Read all tweets from a text file
            using (StreamReader file = new StreamReader(inputPath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    // Create a list of all words
                    stringArray.AddRange(Regex.Split(line, pattern).ToList());
                }
            }

            if (stringArray.Any())
            {
                // Iterrarte string array
                foreach (string s in stringArray)
                {
                    // If word already exists in dictionar, increment count by 1.
                    // Otherwise add 1
                    if (wordCount.ContainsKey(s))
                        wordCount[s]++;
                    else
                        wordCount[s] = 1;
                }
            }

            // Check if file exists. If does, delete it
            if (File.Exists(outputPath1))
                File.Delete(outputPath1);

            // Write each word and total occurrence by iterrating the dictionary
            using (StreamWriter writer = new StreamWriter(outputPath1, false))
            {
                if (wordCount.Any())
                {
                    foreach (var item in wordCount.OrderBy(x => x.Key))
                        writer.WriteLine(item.Key + "   " + item.Value);
                }
            }
        }

        public static void UniqueWordsMedian()
        {
            List<int> tweetUniqueWords = new List<int>();

            // Read each tweet in input file
            using (StreamReader file = new StreamReader(inputPath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    // Clear stringArray for each tweet
                    stringArray.Clear();

                    // Split words by whitespace and get distinct/remove duplicate words
                    stringArray.AddRange(Regex.Split(line, pattern).Distinct().ToList());

                    // Add the total to list
                    tweetUniqueWords.Add(stringArray.Count());
                }
            }

            // Sort the unique words count for each line
            if (tweetUniqueWords.Any())
            {
                // Find the median
                decimal median = GetMedian(tweetUniqueWords);

                // Check if file exists. If does, delete it
                if (File.Exists(outputPath2))
                    File.Delete(outputPath2);

                // Write number of unique words of each tweet and median to a text file
                using (StreamWriter writer = new StreamWriter(outputPath2, false))
                {
                    foreach (int n in tweetUniqueWords)
                        writer.WriteLine(n);

                    // Write median
                    writer.WriteLine("\nMedian is: " + median);
                }

            }
        }

        private static decimal GetMedian(IEnumerable<int> source)
        {
            // Create a copy of the input, and sort the copy
            int[] temp = source.ToArray();
            Array.Sort(temp);

            int count = temp.Length;
            if (count == 0)
            {
                throw new InvalidOperationException("Empty collection");
            }
            else if (count % 2 == 0)
            {
                // count is even, average two middle elements
                int a = temp[count / 2 - 1];
                int b = temp[count / 2];
                return (a + b) / 2m;
            }
            else
            {
                // count is odd, return the middle element
                return temp[count / 2];
            }
        }
    }
}
