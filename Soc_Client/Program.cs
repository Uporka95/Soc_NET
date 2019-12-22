using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace Soc_Client
{
	class Program
	{
		static void Main(string[] args)
		{
			Socket Soc_Connetion = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint IPServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);

			Soc_Connetion.Connect(IPServer);

			byte[] buf = new byte[255];
			Soc_Connetion.Send(Encoding.UTF8.GetBytes("Привет сервер"));
			Soc_Connetion.Receive(buf);
			Console.WriteLine(Encoding.UTF8.GetString(buf));

			Soc_Connetion.Close();
			Console.ReadLine();
		}
	}
}
