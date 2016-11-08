using System;
using System.Reflection;
using System.Text;

namespace Blender.Cryptography
{
    public class BlenderHashCodeGenerator
    {
        private static BlenderHash blenderHash;

        public static string GetHashCode(object o)
        {
            blenderHash = new BlenderHash();
            if (o is bool)
                blenderHash.ComputeHash(BitConverter.GetBytes((bool)o));
            else if (o is byte)
                blenderHash.ComputeHash(new byte[] { (byte)o });
            else if (o is char)
                blenderHash.ComputeHash(BitConverter.GetBytes((char)o));
            else if (o is double)
                blenderHash.ComputeHash(BitConverter.GetBytes((double)o));
            else if (o is Int16)
                blenderHash.ComputeHash(BitConverter.GetBytes((Int16)o));
            else if (o is Int32)
                blenderHash.ComputeHash(BitConverter.GetBytes((Int32)o));
            else if (o is Int64)
                blenderHash.ComputeHash(BitConverter.GetBytes((Int64)o));
            else if (o is Single)
                blenderHash.ComputeHash(BitConverter.GetBytes((Single)o));
            else if (o is string)
                blenderHash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(o.ToString()));
            else if (o is UInt16)
                blenderHash.ComputeHash(BitConverter.GetBytes((UInt16)o));
            else if (o is UInt32)
                blenderHash.ComputeHash(BitConverter.GetBytes((UInt32)o));
            else if (o is UInt64)
                blenderHash.ComputeHash(BitConverter.GetBytes((UInt64)o));
            else
            {
            
                PropertyInfo[] properties = o.GetType().GetProperties();
                MethodInfo[] methods = o.GetType().GetMethods();
                MemberInfo[] members = o.GetType().GetMembers();
                Type[] interfaces = o.GetType().GetInterfaces();

                blenderHash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(o.GetType().ToString()));
                hashString(blenderHash.Hash + properties.Length);

                foreach (PropertyInfo property in properties)
                {
                    hashString(blenderHash.Hash + property.Name);
                    hashString(blenderHash.Hash + property.PropertyType.ToString());
                    hashString(blenderHash.Hash + property.CanRead + property.CanWrite);
                    hashString(blenderHash.Hash + property.DeclaringType);
                    try
                    {
                        hashString(blenderHash.Hash + GetHashCode(property.GetValue(o, null)));
                    }
                    catch {}
                }
                foreach (MethodInfo method in methods)
                {
                    hashString(blenderHash.Hash + method.Name);
                    hashString(blenderHash.Hash + method.DeclaringType.ToString());
                    hashString(blenderHash.Hash + method.ReturnType.ToString());
                    hashString(blenderHash.Hash + method.IsPrivate + method.IsPublic);
                }
                foreach (MemberInfo member in members)
                {
                    hashString(blenderHash.Hash + member.DeclaringType.ToString());
                    hashString(blenderHash.Hash + member.Name);
                    hashString(blenderHash.Hash + member.ReflectedType.ToString());
                }
                foreach (Type type in interfaces)
                    hashString(blenderHash.Hash + type.ToString());
            }

            return blenderHash.Hash;
        }

        private static string hashString(string str)
        {
            blenderHash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(str));
            return blenderHash.Hash;
        }
    }
}

