using System.Net;

namespace BlazorApp_Manage.Data // Đổi lại đúng namespace của Uy nếu cần
{
    public static class NetworkHelper
    {
        // Chú ý: Đã thêm 'string Range' vào kiểu trả về
        public static (string NetworkAddr, int CIDR, int MaxHosts, string Range) Calculate(string ip, string mask)
        {
            try
            {
                var ipAddr = IPAddress.Parse(ip.Trim());
                var maskAddr = IPAddress.Parse(mask.Trim());

                byte[] ipBytes = ipAddr.GetAddressBytes();
                byte[] maskBytes = maskAddr.GetAddressBytes();
                byte[] networkBytes = new byte[4];
                byte[] broadcastBytes = new byte[4];
                int cidr = 0;

                for (int i = 0; i < 4; i++)
                {
                    networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]); // Tính Network Address
                    broadcastBytes[i] = (byte)(networkBytes[i] | ~maskBytes[i]); // Tính Broadcast Address

                    // Đếm số bit 1 để tính CIDR
                    byte m = maskBytes[i];
                    while (m > 0) { cidr += m & 1; m >>= 1; }
                }

                string netAddr = new IPAddress(networkBytes).ToString();

                // Tính dải IP (từ IP mạng + 1 đến IP Broadcast - 1)
                string firstIp = $"{networkBytes[0]}.{networkBytes[1]}.{networkBytes[2]}.{networkBytes[3] + 1}";
                string lastIp = $"{broadcastBytes[0]}.{broadcastBytes[1]}.{broadcastBytes[2]}.{broadcastBytes[3] - 1}";

                int hostBits = 32 - cidr;
                int maxHosts = (int)Math.Pow(2, hostBits) - 2;

                return (netAddr, cidr, maxHosts, $"{firstIp} - {lastIp}");
            }
            catch
            {
                return ("Không hợp lệ", 0, 0, "N/A");
            }
        }
    }
}