using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Blender.Cryptography
{
    public class BlenderHash : HashAlgorithm
    {
        public new string Hash { get { return getHexString(result); } }
        private byte[] result;

        private Prng prng;

        protected override void HashCore(byte[] data, int hashLength, int passes)
        {
            this.result = new byte[hashLength];

            for (int i = 0; i < passes * 2; i++)
                for (int j = 0; j < data.Length; j++)
                    result[j % hashLength] += (byte)(prng.NextByte(data[j]) ^ (prng.NextByte((byte)j) * data[j]));
        }

        protected override byte[] HashFinal()
        {
            return result;
        }

        public override void Initialize()
        {

        }

        public byte[] ComputeHash(Stream data, int hashLength = 32, int passes = 3)
        {
            if (data.Length < hashLength)
            {
                byte[] bytes = new byte[hashLength];
                data.Read(bytes, 0, hashLength);
                return ComputeHash(bytes, hashLength, passes);
            }

            prng = new Prng((byte)data.Length);
            uint seed = 0;
            while (data.Position < data.Length)
            {
                byte b = (byte)data.ReadByte();
                seed ^= prng.NextByte((byte)(b * data.Position ^ seed));
            }
            prng = new Prng(seed);

            this.result = new byte[hashLength];

            for (int i = 0; i < passes * 2; i++)
            {
                data.Position = 0;
                while (data.Position < data.Length)
                {
                    byte b = (byte)data.ReadByte();
                    result[data.Position % hashLength] += (byte)(prng.NextByte(b) ^ (prng.NextByte((byte)data.Position) * b));
                }
            }

            return result;
        }
        public new byte[] ComputeHash(byte[] data, int hashLength = 32, int passes = 3)
        {
            prng = new Prng((byte)data.Length);
            uint seed = 0;
            for (int i = 0; i < data.Length; i++)
                seed += prng.NextByte((byte)(data[i] * i ^ seed));
            prng = new Prng(seed);

            data = pad(data, hashLength);

            HashCore(data, hashLength, passes);
            return result;
        }

        private byte[] pad(byte[] data, int hashLength)
        {
            if (data.Length >= hashLength)
                return data;

            byte[] result = new byte[hashLength];
            data.CopyTo(result, 0);
            for (int i = data.Length; i < result.Length; i++)
                result[i] = (byte)(prng.NextByte((byte)i) * i);
            return result;
        }

        private string getHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data)
                sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }
    }
}

