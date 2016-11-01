using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Blender.Cryptography;

namespace Blender
{
    public class BlenderTest
    {
        public static void CollisionCheck(string letters, int maxLength, int hashLength, int passes)
        {
            BlenderHash hashGenerator = new BlenderHash();
            Dictionary<string, string> hashes = new Dictionary<string, string>();
            
            char firstLetter = letters.First();
            char lastLetter = letters.Last();

            for (int length = 1; length < maxLength; length++)
            {
                StringBuilder accum = new StringBuilder(new String(firstLetter, length));
                while (true)
                {
                    if (accum.ToString().All(val => val == lastLetter))
                        break;
                    for (int i = length - 1; i >= 0; --i)
                        if (accum[i] != lastLetter)
                    {
                        accum[i] = letters[letters.IndexOf(accum[i]) + 1];
                        break;
                    }
                    else
                        accum[i] = firstLetter;
                    hashGenerator.ComputeHash(ASCIIEncoding.ASCII.GetBytes(accum.ToString()), hashLength, passes);
                    string hash = hashGenerator.Hash;
                    if (hashes.ContainsKey(hash))
                        Console.WriteLine("{0} {1} = {2}", hashes[hash], accum, hash);
                    else
                        hashes.Add(hash, accum.ToString());
                }
            }
        }
    }
}

