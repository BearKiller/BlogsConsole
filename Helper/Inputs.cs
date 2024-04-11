using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    internal class Inputs
    {

        public static int GetInt(string prompt) {

            bool isSuccess = false;
            int returnValue = 0;
            
            do {
                Console.Write(prompt);
                string userInput = Console.ReadLine();

                isSuccess = Int32.TryParse(userInput, out returnValue);

                if (!isSuccess) {
                    Console.WriteLine("Please enter a whole number");
                }
                
            } while (!isSuccess);
            return returnValue;

        }

        public static string GetString(string prompt) {

            while (true)
            {
                Console.Write(prompt);
                string? userInput = Console.ReadLine();

                if (!String.IsNullOrEmpty(userInput))
                {
                    return userInput;

                } else
                {

                    Console.WriteLine("Please enter some text.");

                }
            }
        }

        public static char GetChar(string prompt, char[] possibleAnswers)
        {
            while (true)
            {
                Console.Write(prompt);
                string? userInput = Console.ReadLine();

                if (String.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Please actually enter a character.");
                    continue;
                }
                else
                {
                    if (userInput.Length > 1)
                    {
                        Console.WriteLine("Please enter a single character.");
                        continue;
                    }
                }

                if (possibleAnswers.Contains(userInput[0]))
                {
                    return userInput[0];
                } else
                {
                    Console.WriteLine("Only the following characters are allowed: "
                        + String.Join(", ", possibleAnswers));
                    continue;
                }
            }
        }
    }
}
