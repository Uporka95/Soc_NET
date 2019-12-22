using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Soc_NET
{
	class Program
	{
		static void Main(string[] args)
		{

			Socket SocListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint IP = new IPEndPoint(IPAddress.Any, 8000);
			SocListener.Bind(IP);
			SocListener.Listen(10);


			Socket new_connection = SocListener.Accept();
			Console.WriteLine("Соединение установлено");

			while (true)
			{
				new_connection
				while (true)
				{

				}
			}
			string d = "Привет клиент";
			byte[] buf = Encoding.UTF8.GetBytes(d);

			new_connection.Send(buf);
			Console.WriteLine("Сообщение передано");
			new_connection.Close();
			Task.Run(async () => await new_connection.BeginReceive( });
			Console.ReadLine();

			
		}
		public static async Task WaitForMessageAsync(Socket socket)
		{
			Asyn
			await Task.Run( () =>
			while (socket.BeginReceive())
			{

			})
		}
	}
}