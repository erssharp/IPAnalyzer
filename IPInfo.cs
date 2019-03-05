using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSIKS
{
    class IPInfo
    {
        public IPInfo(string ip, string host, string status)
        {
            IP = ip;
            Host = host;
            Status = status;
        }

        string IP { get; set; }
        string Host { get; set; }
        string Status { get; set; }
    }
}
