using System;
using System.IO;
using System.Text;

using Blender.Cryptography;

namespace Blender
{
    public class BlenderConfig
    {
        public BlenderHashMode BlenderHashMode { get; set; }
        public string DataString { get; set; }
        public string FilePath { get; set; }

        public int HashLength { get; set; }
        public int Passes { get; set; }

        public BlenderConfig()
        {
            BlenderHashMode = BlenderHashMode.File;
            HashLength = 32;
            Passes = 3;
        }

        public void Execute()
        {
            switch (BlenderHashMode)
            {
                case BlenderHashMode.File:
                    Console.WriteLine(new BlenderHash().Hash(new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read), HashLength, Passes));
                    break;
                case BlenderHashMode.Repl:
                    BlenderHash hash = new BlenderHash();
                    while (true)
                    {
                        Console.Write("> ");
                        Console.WriteLine(hash.Hash(ASCIIEncoding.ASCII.GetBytes(Console.ReadLine()), HashLength, Passes));
                    }
                case BlenderHashMode.String:
                    Console.WriteLine(new BlenderHash().Hash(ASCIIEncoding.ASCII.GetBytes(DataString), HashLength, Passes));
                    break;
            }
        }
    }

    public enum BlenderHashMode
    {
        File,
        Repl,
        String
    }
}