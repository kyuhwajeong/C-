using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAsync
{
	public enum EClientState
	{
		Available = 0,
		Connecting,
		Connected,
		Disconnecting,
		Disconnected,
		Sending,
		SendDone,
		Receiving,
		ReceiveDone,
		Failed,
		CleanUp
	}
}
