using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ClientInformation;

namespace Soc_NET
{
	class ConnectionsManager
	{
		Mutex cl_listMutex = new Mutex();
		List<Client> clients = new List<Client>();
		Socket Listener;

		public ConnectionsManager()
		{
			Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint IP = new IPEndPoint(IPAddress.Any, 8000);
			Listener.Bind(IP);
			Listener.Listen(10);

			Thread accepting = new Thread(AcceptingNew);
			accepting.Start();
		}

		void AcceptingNew()
		{
			Socket new_con = Listener.Accept();
			Thread accepting = new Thread(AcceptingNew);
			accepting.Start();
			ClientInfo clientInfo = RecieveInfo(new_con);
			if (clientInfo == null) return;
			Client new_client = new Client(new_con, clientInfo);
			ClientAddToChat(new_client);
			
		}

		void ClientAddToChat(Client client)
		{

			client.OnRecieve += Client_OnRecieve;
			client.OnDisconnect += Client_OnDisconnect;
			client.OnConnect += Client_OnConnect;
			client.ConnectToChat();

			cl_listMutex.WaitOne();
			clients.Add(client);
			cl_listMutex.ReleaseMutex();

		}
		private void Client_OnConnect(object sender, ClientEventArgs args)
		{
			foreach (Client client in clients)
			{
				client.Send(args.ToString());
			}
		}

		private void Client_OnDisconnect(object sender, ClientEventArgs args)
		{
			clients.Remove((Client)sender);
			foreach (Client client in clients)
			{
				client.Send(args.ToString());
			}
		}

		private void Client_OnRecieve(object sender, ClientEventArgs args)
		{
			foreach (Client client in clients)
			{
				if (client != (Client)sender) client.Send(args.ToString());
			}
			(sender as Client).StartRecieve();
		}

		public ClientInfo RecieveInfo(Socket soc)
		{
			ClientInfo clientInfo;
			byte[] buf = new byte[1000];
			try
			{
				soc.Receive(buf);
			}
			catch(Exception e)
			{
				if (e is SocketException) return null;
			}

			BinaryFormatter bf = new BinaryFormatter();
			var stream = new MemoryStream(buf, 0, 1000, true, true);
			clientInfo = (ClientInfo)bf.Deserialize(stream);
			return clientInfo;
			stream.Close();
			stream.Dispose();

		}

	}
}
