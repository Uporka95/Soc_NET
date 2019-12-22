using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;


namespace Soc_Client
{
	class Program
	{
		static Mutex mutex = new Mutex();
		static void Main(string[] args)
		{
			Socket Soc_Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint IPServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);

			Soc_Connection.Connect(IPServer);


			Thread Out = new Thread(new ParameterizedThreadStart(MessageSending));
			Thread In = new Thread(new ParameterizedThreadStart(MessageReading));

			Out.Start(Soc_Connection);
			In.Start(Soc_Connection);

			while (true)
			{

			}

			Soc_Connection.Close();

		}

		public static void MessageReading(object socket)
		{
			Socket soc = socket as Socket;
			byte[] buffer = new byte[2000];
			while (true)
			{
				soc.Receive(buffer);
				mutex.WaitOne();
				Console.WriteLine(Encoding.UTF8.GetString(buffer).Trim('\0'));

				mutex.ReleaseMutex();
			}
		}

		public static void MessageSending(object socket)
		{
			Socket soc = socket as Socket;
			byte[] buffer = new byte[2000];
			while (true)
			{
				string out_message = Console.ReadLine();
				buffer = Encoding.UTF8.GetBytes(out_message);
				soc.Send(buffer);
				mutex.WaitOne();
				mutex.ReleaseMutex();
			}
		}
	}
}
