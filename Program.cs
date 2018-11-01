using System;
using System.Linq;
using System.IO;
using hashcrack.Modules;

namespace hashcrack
// Hashcracker is an open-source tool that cracks MD5, SHA256 and SHA512 hashses without using the internet.
{
    class Program
    {
        // List of supported has functions
        public static string[] hashFuncs = { "MD5", "SHA256", "SHA512"};

        // List of supported parameters with their descriptions/roles
        public static string[] paramsInfo = { "-wlist - wordlist file path", "-thash - target hash", "-hfunc - Encryption algorithm used to encrypt hash", "-ofile - Output file (Optional)", "-pcount - Progress counter displays how many lines have been hashed in a file, this slows performance, default on (true/false) (optional) )" };

        // Reset console to color it was
        public static ConsoleColor c = Console.ForegroundColor;

        static void Main(string[] args)
        {
            // Check if there are no arguments passed
            if (args is null || args.Length < 1 || (!(args.Contains<string>("-wlist")) && args.Contains<string>("-thash") && args.Contains<string>("-tfunc")))
            {
                logError("Syntax error! Here's our help message!");
                // Display help message upon syntax error
                displayHelpMessage();
                
                return;
            }

            // Wordlist file path, declared with -wl
            string wlFilePath = String.Empty;

            // Hash that is to be cracked, declared with -h
            string targetHash = String.Empty;

            // Hash function used to encrypt the target hash, declared with -hfunc
            string hashFunction = String.Empty;

            // Output file, declared with -ofile
            string outputFile = String.Empty;
            
            // Progress counter displays how many lines have been hashed in a file, this slows performance
            bool progressCounter = true;

            // List all support hash functions
            if (args[0] == "-help") { displayHelpMessage(); }
            else if (args[0] == "-hFuncs") { writeArray(hashFuncs); }
            else if (args[0] == "-params") { writeArray(paramsInfo); }

            // Iterate through passed arguments
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                // List of parameters
                {
                    // WordList File
                    case "-wlist":
                        wlFilePath = args[i + 1];
                        break;

                    //  Target Hash
                    case "-thash":
                        targetHash = args[i + 1].ToLower();
                        break;

                    // Hash Function
                    case "-hfunc":
                        hashFunction = args[i + 1];
                        break;

                    // Output File
                    case "-ofile":
                        outputFile = args[i + 1];
                        break;

                    // Progress Counter
                    case "-pcount":
                        if (args[i + 1] == "false")
                        {
                            progressCounter = false;
                        }
                        else
                            progressCounter = true;
                        break;
                }
            }

            // Check if wordlist file path is valid
            if (!(File.Exists(wlFilePath)))
            {
                logError("Wordlist file does not exist!");
                return;
            }

            // Check if the entered hash function is supported
            if (!(hashFuncs.Contains<string>(hashFunction)))
            {
                logError("Hash function is not supported, to view supported has function use -hFuncs");
                logError(hashFunction);
                return;
            }

            try
            {
                // Iterate through every line of the wordlist file
                using (StreamReader sr = new StreamReader(wlFilePath))
                {
                    string line = String.Empty;

                    // Declares the progress counter
                    int x = 1;

                    // Declares the line count
                    int lineCount = 1;

                    // If the progress counter is enabled them count the number of lines in the file
                    if (progressCounter)
                    {
                       lineCount = totalLines(wlFilePath);
                    }

                    // Declares the decrypted result string
                    string decryptedHash = String.Empty;

                    switch (hashFunction)
                    {
                        case "MD5":
                            while ((line = sr.ReadLine()) != null && decryptedHash == String.Empty)
                            {
                                // Only works properly if progress counter is enabled, this writes how many lines have been hashed
                                Console.WriteLine($"Line Number {x}/{lineCount}");

                                // Increment the counter
                                x++;

                                // Hash each line and check if the hash matches the target hash
                                if (hashFunctions.MD5Hash(line) == targetHash)
                                {
                                    // Output the hash
                                    decryptedHash = line;
                                }
                            }
                            break;
                        case "SHA256":
                            while ((line = sr.ReadLine()) != null && decryptedHash == String.Empty)
                            {
                                // Only works properly if progress counter is enabled, this writes how many lines have been hashed
                                Console.WriteLine($"Line Number {x}/{lineCount}");

                                // Increment the counter
                                x++;

                                // Hash each line and check if the hash matches the target hash
                                if (hashFunctions.SHA256Hash(line) == targetHash)
                                {
                                    // Output the hash
                                    decryptedHash = line;
                                }
                            }
                            break;
                        case "SHA512":
                            while ((line = sr.ReadLine()) != null && decryptedHash == String.Empty)
                            {
                                // Only works properly if progress counter is enabled, this writes how many lines have been hashed
                                Console.WriteLine($"Line Number {x}/{lineCount}");

                                // Increment the counter
                                x++;

                                // Hash each line and check if the hash matches the target hash
                                if (hashFunctions.SHA512Hash(line) == targetHash)
                                {
                                    // Output the hash
                                    decryptedHash = line;
                                }
                            }
                            break;
                        default:
                            return;
                    }

                    if (!(decryptedHash == String.Empty))
                    // Hash Found
                    {
                        logString(decryptedHash, targetHash);
                        if (!(outputFile == String.Empty))
                        {
                            logString(decryptedHash, targetHash);
                            File.WriteAllText(outputFile, $"{decryptedHash} = {targetHash}");
                        }
                    } else
                    // Hash not found
                    {
                       logError("Hash not found!");
                    }
                }
            }
            catch (Exception ex)
            {
                // Write error message
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private static int totalLines(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                int i = 0;
                while (r.ReadLine() != null) { i++; }
                return i;
            }
        }

        private static void writeArray(string[] array)
        {
            // Write to console all items in array
            foreach (string str in array)
            {
                Console.WriteLine(str);
            }
        }

        private static void displayHelpMessage()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-- Welcome to hashcrack - by Jack Barham (https://github.com/JackBarns) --\n");
            Console.ForegroundColor = c;

            Console.WriteLine("Commands: \n-hFuncs (Lists all supported hash functions)\n-params (Lists all parameters)\n");
            Console.WriteLine("Supported Hash Functions:");
            writeArray(hashFuncs);
            Console.WriteLine("\nParameters:");
            writeArray(paramsInfo);
            Console.WriteLine("\nUsage:\nhashcrack -wlist [wordlist file path] -thash [target hash] -hfunc [MD5/SHA256/SHA512] -ofile [output file path (optional)] -pcount [true/false (optional)]");
            Console.WriteLine("\nExample Usage:\nhashcrack -wlist wordlist.txt -thash 06d80eb0c50b49a509b49f2424e8c805 -hfunc MD5");
            Console.ForegroundColor = c;
        }

        private static void logError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = c;
        }

        private static void logString(string decryptedHash, string targetHash)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"\nString Found!\n{decryptedHash} = {targetHash}");
            Console.ForegroundColor = c;
        }
    }
}
