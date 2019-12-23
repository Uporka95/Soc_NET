using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ClientInformation;

namespace Client_Debug
{
	enum EventDesc
	{
		Connection,
		Sent,
		Recieved,
		Disconnection,
		BadConnection
		
	}
	

	class ServerEventArgs : EventArgs
	{
		public string EventMessage { get; set; }
		public EventDesc Desc { get; private set; }

		public ServerEventArgs(string message, EventDesc desc)
		{
			EventMessage = message;
			Desc = desc;
		}

		public override string ToString()
		{
			return $"{EventMessage}";
		}
	}

	delegate void ServerEventHandler(object sender, ServerEventArgs args);
	class Server
	{
		IPEndPoint IP;
		Socket connection { get; set; }

		public event ServerEventHandler OnConnect;
		public event ServerEventHandler OnDisconnect;
		public event ServerEventHandler OnSend;
		public event ServerEventHandler OnRecieve;
		public event ServerEventHandler OnBadConnection;

		public Server(string ip_addres, int port)
		{
			connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			IP = new IPEndPoint(IPAddress.Parse(ip_addres), port);
		}


		public void Connect(string Nickname)
		{
			try
			{
				connection.Connect(IP);
			}
			catch(SocketException e)
			{
			
				this?.OnBadConnection(this, new ServerEventArgs(e.Message, EventDesc.BadConnection));
			}
			SendInfo(Nickname);
			this?.OnConnect(this, new ServerEventArgs("Успешное подключение", EventDesc.Connection));
			

		}

		public void Send(string Message)
		{
			try
			{
				connection.Send(Encoding.UTF8.GetBytes(Message));
			}
			catch (SocketException e)
			{
					this?.OnBadConnection(this, new ServerEventArgs(e.Message, EventDesc.BadConnection));
			}
			//this?.OnSend(this, new ServerEventArgs(Message, EventDesc.Sent));
		}

		public void StartRecieve()
		{
			Thread recieving = new Thread(Recieve);
			recieving.Start();
		}

		private void Recieve()
		{
			try
			{
				byte[] buf = new byte[1000];
				connection.Receive(buf);
				this?.OnRecieve(this, new ServerEventArgs(Encoding.UTF8.GetString(buf), EventDesc.Recieved));
			}
			catch (SocketException e)
			{
				OnBadConnection(this, new ServerEventArgs(e.Message, EventDesc.BadConnection));
				return;
			}
			StartRecieve();
		}

		public void SendInfo(string name)
		{
			ClientInfo clientInfo = new ClientInfo();
			clientInfo.Nickname = name;
			byte[] buf = new byte[1000];

			BinaryFormatter bf = new BinaryFormatter();
			var stream = new MemoryStream(buf,0,1000,true,true);
			bf.Serialize(stream, clientInfo);
			buf = stream.GetBuffer();

			stream.Close();
			stream.Dispose();
			
			try
			{
				connection.Send(buf);
			}
			catch (Exception e)
			{
				if (e is SocketException) this?.OnBadConnection(this, new ServerEventArgs("Сервер недоступен", EventDesc.BadConnection));
			}
		}
	}
}
