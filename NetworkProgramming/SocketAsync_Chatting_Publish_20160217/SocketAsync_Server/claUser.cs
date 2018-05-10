using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

using SocketGlobal;
using System.Threading;
using SocketGlobal.SendData;
using DGU_Socket.SendData;
using DGU_Socket;

namespace SocketAsync_Server
{
	/// <summary>
	/// 유저 클래스
	/// </summary>
	public class claUser
	{
		#region 연결할 이벤트 ♥♥♥♥♥♥♥♥♥♥♥♥
		/// <summary>
		/// 접속
		/// </summary>
		public event dgConnect OnConnected;
		/// <summary>
		/// 끊김
		/// </summary>
		public event dgDisconnect OnDisconnected;
		/// <summary>
		/// 메시지
		/// </summary>
		public event dgMessage OnMessaged;
		#endregion

		/// <summary>
		/// 명령어 클래스
		/// </summary>
		private CCommand m_insCommand = new CCommand();

		/// <summary>
		/// 이 유저의 소켓정보
		/// </summary>
		//public Socket m_socketMe;
		public CSocketUser Socket_User;

		/// <summary>
		/// 이 유저의 아이디
		/// </summary>
		public string UserID { get; set; }

		/// <summary>
		/// 유저 객체를 생성합니다.
		/// </summary>
		/// <param name="socketClient">접속된 유저의 소켓</param>
		public claUser(Socket socketClient)
		{
			//소켓 저장
			this.Socket_User = new CSocketUser(socketClient);
			this.Socket_User.OnDisconnect += Disconnected;
			this.Socket_User.OnReceiveComplete += Socket_User_OnMessageComplete;

			this.Socket_User.BeginReceive();
		}

		private void Socket_User_OnMessageComplete(CSendData_Original sdOri)
		{
			CSD_SA sdMessage = new CSD_SA(sdOri);

#if Debug
			Console.WriteLine("server get - command : {0}", sdMessage.CommandType.ToString());
#endif

			switch (sdMessage.CommandType)
			{
				case CCommand.Command.None: //없다
					break;

				case CCommand.Command.Login:    //로그인 완료
					this.Connected();
					break;

				default:
					SendMeg_Main(sdMessage);
					break;
			}
		}


		/// <summary>
		/// 나 연결 됨.
		/// </summary>
		public void Connected()
		{
			//접속함을 알리고
			OnConnected(this);

			//유저에게 모든 과정이 끝났음을 알린다.
			this.SendMsg_User(CCommand.Command.Login_Complete, "");
		}

		/// <summary>
		/// 나 끊김
		/// </summary>
		public void Disconnected()
		{
			if (null != OnDisconnected)
			{
				OnDisconnected(this);
			}
		}


		/// <summary>
		/// 서버로 메시지를 보냅니다.
		/// </summary>
		/// <param name="sMag"></param>
		private void SendMeg_Main(CSD_SA dsRecieveMsg)
		{
			OnMessaged(this, dsRecieveMsg);
		}
		

		public void SendMsg_User(CCommand.Command typeCommand, string sMsg)
		{
			CSD_SA dsMsg = new CSD_SA();
			dsMsg.CommandType = typeCommand;
			dsMsg.Data_String = sMsg;
			this.SendMsg_User(dsMsg);
        }

		public void SendMsg_User(CCommand.Command typeCommand, byte[] byteData)
		{
			CSD_SA sdSendMsg = new CSD_SA();

			//데이터를 넣고
			sdSendMsg.CommandType = typeCommand;
			sdSendMsg.Data_Byte = byteData;

			this.SendMsg_User(sdSendMsg);
		}

		/// <summary>
		/// 이 유저에게 메시지를 보낸다.
		/// </summary>
		/// <param name="sMsg"></param>
		public void SendMsg_User(CSendData sdMsg)
		{
			this.Socket_User.SendMsg(sdMsg);
		}

		

	}
}
