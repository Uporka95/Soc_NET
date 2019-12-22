using System;

namespace Client_Debug
{
	class Program
	{
		static void Main(string[] args)
		{
			Server xerver = new Server("127.0.0.1", 8000);

			xerver.OnBadConnection += Xerver_OnBadConnection;
			xerver.OnSend += Xerver_OnSend;
			xerver.OnRecieve += Xerver_OnRecieve;
			xerver.OnDisconnect += Xerver_OnDisconnect;
			xerver.OnConnect += Xerver_OnConnect;

			xerver.Connect("Писька");
			xerver.StartRecieve();
			

			while (true)
			{
				xerver.Send(Console.ReadLine());
			}
		}

		private static void Xerver_OnConnect(object sender, ServerEventArgs args)
		{
			Console.WriteLine(args.EventMessage);
		}

		private static void Xerver_OnDisconnect(object sender, ServerEventArgs args)
		{
			Console.WriteLine(args.EventMessage);
		}

		private static void Xerver_OnRecieve(object sender, ServerEventArgs args)
		{
			Console.WriteLine(args.EventMessage);
		}

		private static void Xerver_OnSend(object sender, ServerEventArgs args)
		{
			Console.WriteLine(args.EventMessage);
		}

		private static void Xerver_OnBadConnection(object sender, ServerEventArgs args)
		{
			Console.WriteLine(args.EventMessage);
		}
		
		
	}
}
