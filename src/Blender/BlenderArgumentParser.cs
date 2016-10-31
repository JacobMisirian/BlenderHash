using System;

namespace Blender
{
    public class BlenderArgumentParser
    {
        private string[] args;
        private int position;

        public BlenderConfig Parse(string[] args)
        {
            if (args.Length == 0)
                displayHelp();

            this.args = args;
            BlenderConfig config = new BlenderConfig();

            for (position = 0; position < args.Length; position++)
            {
                switch (args[position])
                {
                    case "-f":
                    case "--file":
                        config.FilePath = expectData("file");
                        break;
                    case "-h":
                    case "--help":
                        displayHelp();
                        break;
                    case "-l":
                    case "--length":
                        config.HashLength = Convert.ToInt32(expectData("length"));
                        break;
                    case "-p":
                    case "--passes":
                        config.Passes = Convert.ToInt32(expectData("passes"));
                        break;
                    case "-r":
                    case "--repl":
                        config.BlenderHashMode = BlenderHashMode.Repl;
                        break;
                    case "-s":
                    case "--string":
                        config.DataString = expectData("string");
                        config.BlenderHashMode = BlenderHashMode.String;
                        break;
                    default:
                        die(string.Format("Unexpected flag or data {0}! Run Blender --help for help."));
                        break;
                }
            }

            return config;
        }

        private void displayHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("Blender [MODE] [OPTIONS]");

            Console.WriteLine("\n-h --help                 Displays this help and exits.");

            Console.WriteLine("\nMODE:");
            Console.WriteLine("-f --file [PATH]            Calculates the hash of the specified file.");
            Console.WriteLine("-r --repl                   Starts an instace of the Blender repl.");
            Console.WriteLine("-s --string [STRING]        Calculates the hash of a single string.");

            Console.WriteLine("\nOPTIONS:");
            Console.WriteLine("-l --length [LENGTH]        Specifies the length of the result hash. Default 32.");
            Console.WriteLine("-p --passes [PASSES]        Specifies the amount of passes to rehash. Default 3.");
            die(string.Empty);
        }

        private string expectData(string type)
        {
            if (args[++position].StartsWith("-"))
                die(string.Format("Unexpected flag {0}, was expecting {1}!", args[position], type));
            return args[position];
        }

        private void die(string msg)
        {
            Console.WriteLine(msg);
            Environment.Exit(0);
        }
    }
}

