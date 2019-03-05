using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSIKS
{
    static class IPData
    {
        public static string GetNetworkAddress(string ip, string mask)
        {
            byte fb = (byte)(IPCon.FirstByte(ip) & IPCon.FirstByte(mask));
            byte sb = (byte)(IPCon.SecondByte(ip) & IPCon.SecondByte(mask));
            byte tb = (byte)(IPCon.ThirdByte(ip) & IPCon.ThirdByte(mask));
            byte fob = (byte)(IPCon.FourthByte(ip) & IPCon.FourthByte(mask));
            return IPCon.BytesToIP(fb, sb, tb, fob);
        }

        public static string GetBroadcastAddress(string ip, string mask)
        {
            string nwa = GetNetworkAddress(ip, mask);
            byte fb = (byte)(~(IPCon.FirstByte(mask)) | (IPCon.FirstByte(nwa)));
            byte sb = (byte)(~(IPCon.SecondByte(mask)) | (IPCon.SecondByte(nwa)));
            byte tb = (byte)(~(IPCon.ThirdByte(mask)) | (IPCon.ThirdByte(nwa)));
            byte fob = (byte)(~(IPCon.FourthByte(mask)) | (IPCon.FourthByte(nwa)));
            return IPCon.BytesToIP(fb, sb, tb, fob);
        }

        public static string GetGatewayAddress(string ip, string mask)
        {
            byte fb = (byte)(IPCon.FirstByte(ip) & IPCon.FirstByte(mask));
            byte sb = (byte)(IPCon.SecondByte(ip) & IPCon.SecondByte(mask));
            byte tb = (byte)(IPCon.ThirdByte(ip) & IPCon.ThirdByte(mask));
            byte fob = (byte)((IPCon.FourthByte(ip) & IPCon.FourthByte(mask)) + 1);
            return IPCon.BytesToIP(fb, sb, tb, fob);
        }
    }
}
