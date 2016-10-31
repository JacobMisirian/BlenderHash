using System;
using System.IO;
using System.Text;

namespace Blender.Cryptography
{
    public class BlenderHash
    {
        private Prng prng;
        private int hashLength;

        public string Hash(Stream data, int hashLength, int passes)
        {
            if (data.Length < hashLength)
            {
                byte[] bytes = new byte[hashLength];
                data.Read(bytes, 0, hashLength);
                return Hash(bytes, hashLength, passes);
            }

            uint seed = 0;
            while (data.Position < data.Length)
            {
                byte b = (byte)data.ReadByte();
                seed += new Prng(b).NextByte((byte)(b ^ seed));
            }
            prng = new Prng(seed);
            this.hashLength = hashLength;

            byte[] result = new byte[hashLength];

            for (int i = 0; i < passes * 2; i++)
            {
                data.Position = 0;
                while (data.Position < data.Length)
                    result[data.Position % hashLength] += (byte)(prng.NextByte((byte)data.ReadByte()) ^ prng.NextByte((byte)data.Position));
            }

            return getHexString(result);
        }
        public string Hash(byte[] data, int hashLength, int passes)
        {
            uint seed = 0;
            foreach (byte b in data)
                seed += new Prng(b).NextByte((byte)(b ^ seed));
            prng = new Prng(seed);
            this.hashLength = hashLength;

            data = pad(data);
            byte[] result = new byte[hashLength];

            for (int i = 0; i < passes * 2; i++)
                for (int j = 0; j < data.Length; j++)
                    result[j % hashLength] += (byte)(prng.NextByte(data[j]) ^ prng.NextByte((byte)j));

            return getHexString(result);
        }

        private byte[] pad(byte[] data)
        {
            if (data.Length >= hashLength)
                return data;

            byte[] result = new byte[hashLength];
            data.CopyTo(result, 0);
            for (int i = data.Length; i < result.Length; i++)
                result[i] = (byte)(prng.NextByte((byte)data.Length) ^ prng.NextByte((byte)i));
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

