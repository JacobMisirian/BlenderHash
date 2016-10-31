using System;
using System.Text;

using Blender.Cryptography;

namespace Blender
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            BlenderHash generator = new BlenderHash();

            while (true)
            {
                Console.Write("> ");
                Console.WriteLine(generator.Hash(ASCIIEncoding.ASCII.GetBytes(Console.ReadLine()), 64, 3));
            }
        }
    }
}