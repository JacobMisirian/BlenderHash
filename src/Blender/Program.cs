using System;

using Blender.Cryptography;

namespace Blender
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //BlenderTest.CollisionCheck("!@#$%^&*()1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 10, 32, 3);
            new BlenderArgumentParser().Parse(args).Execute();
        }
    }
}