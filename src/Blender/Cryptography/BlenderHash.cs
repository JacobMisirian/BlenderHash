using System;
using System.Text;

namespace Blender.Cryptography
{
    public class BlenderHash
    {
        private Prng prng;
        private int hashLength;

        public string Hash(byte[] data, int hashLength, int passes)
        {
            uint seed = 0;
            foreach (byte b in data)
                seed += new Prng(b).NextByte((byte)(b ^ seed));
            prng = new Prng(seed);
            this.hashLength = hashLength;

            data = pad(data);
            byte[] result = new byte[hashLength];

            for (int i = 0; i < passes; i++)
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
                result[i] = prng.NextByte((byte)data.Length);
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

