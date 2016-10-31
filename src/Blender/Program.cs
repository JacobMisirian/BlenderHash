using System;

namespace Blender
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new BlenderArgumentParser().Parse(args).Execute();
        }
    }
}