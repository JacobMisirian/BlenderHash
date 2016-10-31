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
            BlenderHash hash = new BlenderHash();
            switch (BlenderHashMode)
            {
                case BlenderHashMode.File:
                    hash.ComputeHash(new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read), HashLength, Passes);
                    Console.WriteLine(hash.Hash);
                    break;
                case BlenderHashMode.Repl:
                    while (true)
                    {
                        Console.Write("> ");
                        hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Console.ReadLine()), HashLength, Passes);
                        Console.WriteLine(hash.Hash);
                    }
                case BlenderHashMode.String:
                    hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(DataString), HashLength, Passes);
                    Console.WriteLine(hash.Hash);
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