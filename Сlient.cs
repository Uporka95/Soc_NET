using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using ClientInformation;

namespace Soc_NET
{

	class ClientEventArgs : EventArgs
	{
		public ClientInfo Info { get; set; }
		public string EventMessage { get; set; }
		public ClientEventArgs(string message, ClientInfo info)
		{
			EventMessage = message;
			Info = info;
		}

		public override string ToString()
		{
			return $"{Info.Nickname}: {EventMessage}";
		}
	}

	delegate void ClientEventhandler(object sender, ClientEventArgs args);
	class Client
	{
		
		Socket Connection { get; set; }
		public ClientInfo Info { get; set; }


		public event ClientEventhandler OnRecieve; 
		public event ClientEventhandler OnDisconnect;
		public event ClientEventhandler OnConnect;

		public Client(Socket connection, ClientInfo info)
		{
			Connection = connection;
			info.ID = CurrentID++;
		}

		public void ConnectToChat()
		{
			StartRecieve();
			this?.OnConnect(this, new ClientEventArgs("вошел в чат", Info));
		}
		public void Send(string message)
		{
			try
			{
				Connection.Send(Encoding.UTF8.GetBytes(message));
			}
			catch (Exception e)
			{
				if (e is SocketException)
				this.OnDisconnect(this, new ClientEventArgs("отключился", Info));
			}
		}

		public void StartRecieve()
		{
			Thread recieving = new Thread(Recieve);
			recieving.Start();
		}

		public void Recieve()
		{
			try
			{
				byte[] buf = new byte[1000];
				if (Connection.Receive(buf) > 0)
				{

				}
				this?.OnRecieve(this, new ClientEventArgs(Encoding.UTF8.GetString(buf), Info));
			}
			catch (Exception e)
			{
				if (e is SocketException) OnDisconnect(this, new ClientEventArgs("отключился", Info));
			}
		}

		


		static int CurrentID = 0;
	}
}
