using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public Server xerver;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Load_Loaded(object sender, RoutedEventArgs e)
		{
			
		}


		private void bConnect_Click(object sender, RoutedEventArgs e)
		{
			xerver = new Server(tbIP.Text, Convert.ToInt32(tbPort.Text));
			
			bConnect.IsEnabled = false;
			xerver.OnSend += Xerver_OnSend;
			xerver.OnBadConnection += Xerver_OnBadConnection;
			xerver.OnConnect += Xerver_OnConnect;
			xerver.OnDisconnect += Xerver_OnDisconnect;
			xerver.OnRecieve += Xerver_OnRecieve;
			xerver.Connect(tbNickname.Text);
		}

		private void Xerver_OnRecieve(object sender, ServerEventArgs args)
		{
			lvChatWindow.Items.Add(args.EventMessage);
		}

		private void Xerver_OnDisconnect(object sender, ServerEventArgs args)
		{
			throw new NotImplementedException();
		}

		private void Xerver_OnConnect(object sender, ServerEventArgs args)
		{
			lbUsers.Items.Add(xerver.clientInfo.Nickname);
		}

		private void Xerver_OnBadConnection(object sender, ServerEventArgs args)
		{
			throw new NotImplementedException();
		}

		private void Xerver_OnSend(object sender, ServerEventArgs args)
		{
			lvChatWindow.Items.Add(args.EventMessage);
		}

		private void tbMessageEntry_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			xerver.Send(tbMessageEntry.Text);
		}
	}
}
