using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using System.Net;
using System.Collections.ObjectModel;
using System.Data;
using System.Net.Sockets;

namespace AVSIKS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataTable dt = new DataTable();
        bool flag = false;

        public MainWindow()
        {
            InitializeComponent();
            DataColumn col1 = new DataColumn("IP", typeof(string));
            DataColumn col2 = new DataColumn("Host", typeof(string));
            DataColumn col3 = new DataColumn("Status", typeof(string));
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dg.ItemsSource = dt.DefaultView;
            mac.Text += GetMacAddress();
            string host = Dns.GetHostName();
            //try
            //{
                string ips = new WebClient().DownloadString("https://api.ipify.org");
                byte[] sb = IPCon.GetBytes(ips);
                IPAddress ip1 = new IPAddress(sb);
                exip.Text += ip1;
            //}
            //catch (WebException) { }
            IPAddress ip2 = Dns.GetHostEntry(host).AddressList[0];
            mip.Text = GetSubnetMask(ip2).ToString();            
            locip.Text += ip2;
            List<string> ipl = GetIPs();
            sip.Text = ipl[0];
            lip.Text = ipl.Last();

            IPAddress ip = new IPAddress(IPCon.GetBytes(ips));
            int bits = 24;

            uint mask = ~(uint.MaxValue >> bits);
            byte[] ipBytes = ip.GetAddressBytes();
            byte[] maskBytes = BitConverter.GetBytes(mask).Reverse().ToArray();
            byte[] startIPBytes = new byte[ipBytes.Length];
            byte[] endIPBytes = new byte[ipBytes.Length];
            for (int i = 0; i < ipBytes.Length; i++)
            {
                startIPBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
                endIPBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
            }
            IPAddress startIP = new IPAddress(startIPBytes);
            IPAddress endIP = new IPAddress(endIPBytes);

            sip.Text = startIP.ToString();
            lip.Text = endIP.ToString();
        }

        private List<string> GetIPs()
        {
            List<string> ips = new List<string>();
            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()).Where(ha => ha.AddressFamily == AddressFamily.InterNetwork))
            {
                ips.Add(ip.ToString());
            }
            return ips;
        }

        private string GetMacAddress()
        {
            string macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }

        public static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException($"Can't find subnetmask for IP address '{address}'");
        }

        private async Task<PingReply> Ping(string ip)
        {
            Ping ping = new Ping();
            PingReply reply = await ping.SendPingAsync(ip, 1000);
            return reply;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string start = sip.Text;
            string end = lip.Text;
            byte[] sb = IPCon.GetBytes(start);
            byte[] eb = IPCon.GetBytes(end);

            flag = false;

            //for (byte a = sb[0]; a <= eb[0] && a <= 255; a++)
            //    for (byte b = sb[1]; b <= eb[1] && b <= 255; b++)
            //        for (byte c = sb[2]; c <= eb[2] && c <= 255; c++)
            for (byte i = sb[3]; i <= eb[3] && i <= 255; i++)
            {
                if (flag)
                    break;

                PingReply reply = await Ping(IPCon.BytesToIP(sb[0], sb[1], sb[2], i));
                string ip = reply.Address.ToString();
                IPHostEntry host = Dns.GetHostEntry(IPCon.BytesToIP(sb[0], sb[1], sb[2], i));
                IPStatus status = reply.Status;
                DataRow dr = dt.NewRow();
                dr[0] = ip;
                dr[1] = host.HostName;
                dr[2] = status.ToString();
                dt.Rows.Add(dr);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataRowView drv = dg.SelectedItem as DataRowView;
            string ip = drv["IP"].ToString();
            na.Text = IPData.GetNetworkAddress(ip, mip.Text);
            wa.Text = IPData.GetBroadcastAddress(ip, mip.Text);
            ga.Text = IPData.GetGatewayAddress(ip, mip.Text);
            string dmn = drv["Host"].ToString();
            dn.Text = dmn;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            dt.Clear();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            flag = true;
        }
    }
}
