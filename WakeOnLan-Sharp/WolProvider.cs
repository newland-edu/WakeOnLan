using System;
using System.Net;
using System.Net.Sockets;

namespace WakeOnLan_Sharp
{
    class WolProvider
    {
        public static void WakeOnLan(string mac, string ip, int port)
        {
            IPEndPoint point;
            UdpClient client = new UdpClient();
            byte[] magicBytes = GetMagicPacket(mac);
            point = new IPEndPoint(IPAddress.Parse(ip), port);
            try
            {
                int result = client.Send(magicBytes, magicBytes.Length, point);
            }
            catch (SocketException ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// 拼装MAC魔术封包
        /// </summary>
        /// <param name="hexString">MAC地址字符串</param>
        /// <returns></returns>
        public static byte[] GetMagicPacket(string macString)
        {
            byte[] returnBytes = new byte[102];
            string commandString = "FFFFFFFFFFFF";
            for (int i = 0; i < 6; i++)
                returnBytes[i] = Convert.ToByte(commandString.Substring(i * 2, 2), 16);
            byte[] macBytes = StrToHexByte(macString);
            for (int i = 6; i < 102; i++)
            {
                returnBytes[i] = macBytes[i % 6];
            }
            return returnBytes;
        }
        /// <summary>
        /// MAC地址字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString">MAC地址字符串</param>
        /// <returns></returns>
        public static byte[] StrToHexByte(string hexString)
        {
            hexString = hexString.Replace("-", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
