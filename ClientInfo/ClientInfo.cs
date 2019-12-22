using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientInformation
{
	[Serializable]
	public class ClientInfo
	{
		public string Nickname { get; set; }
		public int ID { get; set; }
	}
}
