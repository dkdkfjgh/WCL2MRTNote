using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WCL2MRTNoteGUI
{
    public static class Skills
    {
        public static int[] CDs = new int[0];
        public static void loadCDs()
        {
            CDs = LoadNumbersFromTextFile("CDs.txt");
        }
        public static int[] LoadNumbersFromTextFile(string filePath)
        {
            // Read the entire text file into a string
            string fileContent = File.ReadAllText(filePath);

            // Use a regular expression to match lines that contain a number followed by a comma and space
            string pattern = @"\d+,";
            Regex regex = new Regex(pattern);

            // Find all the matches in the file content
            MatchCollection matches = regex.Matches(fileContent);

            // Create a list to store the numbers
            List<int> numbers = new List<int>();

            // Loop through all the matches
            foreach (Match match in matches)
            {
                // Convert the number from a string to an integer
                int number = int.Parse(match.Value.Replace(",",String.Empty));

                // Add the number to the list
                numbers.Add(number);
            }

            return numbers.ToArray();
        }

    }
}
