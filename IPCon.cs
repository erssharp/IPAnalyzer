using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSIKS
{
    public static class IPCon
    {
        public static long IPToLong(string ipstr)
        {
            return long.Parse(ipstr.Replace(".", null));
        }

        public static string LongToIP(long iplon)
        {
            return string.Format("{0}.{1}.{2}.{3}", iplon / 1000000000, iplon / 1000000 % 1000, iplon / 1000 % 1000, iplon % 1000);
        }

        public static byte FirstByte(string ip)
        {
            string b = null;
            char[] ipc = ip.ToCharArray();
            for (int i = 0; i < ipc.Length; i++)
            {
                if (ipc[i] != '.')
                    b += ipc[i];
                else
                    break;
            }
            return byte.Parse(b);
        }

        public static byte SecondByte(string ip)
        {
            string b = null;
            List<char> ipc = ip.ToCharArray().ToList<char>();
            for (int i = ipc.IndexOf('.') + 1; i < ipc.Count; i++)
            {
                if (ipc[i] != '.')
                    b += ipc[i];
                else
                    break;
            }
            return byte.Parse(b);
        }

        public static byte ThirdByte(string ip)
        {
            string b = null;
            List<char> ipc = ip.ToCharArray().ToList<char>();
            ipc.Remove('.');
            for (int i = ipc.IndexOf('.') + 1; i < ipc.Count; i++)
            {
                if (ipc[i] != '.')
                    b += ipc[i];
                else
                    break;
            }
            return byte.Parse(b);
        }

        public static byte FourthByte(string ip)
        {
            string b = null;
            List<char> ipc = ip.ToCharArray().ToList<char>();
            for (int i = ipc.LastIndexOf('.') + 1; i < ipc.Count; i++)
            {
                if (ipc[i] != '.')
                    b += ipc[i];
                else
                    break;
            }
            return byte.Parse(b);
        }

        public static string BytesToIP(byte b1, byte b2, byte b3, byte b4)
        {
            return string.Format("{0}.{1}.{2}.{3}", b1, b2, b3, b4);
        }

        public static byte[] GetBytes(string ip)
        {
            byte[] bytes = { IPCon.FirstByte(ip), IPCon.SecondByte(ip), IPCon.ThirdByte(ip), IPCon.FourthByte(ip) };
            return bytes;
        }
    }
}
