using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Soc_NET
{
	class Program
	{
		static Mutex mutex = new Mutex();
		static void Main(string[] args)
		{

			Socket SocListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint IP = new IPEndPoint(IPAddress.Any, 8000);
			SocListener.Bind(IP);
			SocListener.Listen(10);


			Socket new_connection = SocListener.Accept();
			Console.WriteLine("Соединение установлено");

			Thread Out = new Thread(new ParameterizedThreadStart(MessageSending));
			Thread In = new Thread(new ParameterizedThreadStart(MessageReading));

			Out.Start(new_connection);
			In.Start(new_connection);
			while (true)
			{

			}

			
			new_connection.Close();
			
			Console.ReadLine();

			
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
			byte[] buffer;
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