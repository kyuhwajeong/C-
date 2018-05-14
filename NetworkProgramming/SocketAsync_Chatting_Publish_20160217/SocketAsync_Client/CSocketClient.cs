using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;


using DGU_Socket;
using DGU_Socket.SendData;
using SocketGlobal;
using SocketGlobal.SendData;


namespace SocketAsync_Client
{
	#region 외부에 연결할 델리게이트
	/// <summary>
	/// 접속됨
	/// </summary>
	public delegate void dgCSocketClient_Connect();
	/// <summary>
	/// 로그인까지 모든 과정이 완료됨
	/// </summary>
	public delegate void dgCSocketClient_ConnectComplete();
	/// <summary>
	/// 접속끊김을 알림
	/// </summary>
	public delegate void dgCSocketClient_Disconnect();

	/// <summary>
	/// 메인으로 보낼 데이터
	/// </summary>
	/// <param name="sdData"></param>
	public delegate void dgCSocketClient_ReceivePass(CSD_SA sdData);
	#endregion


	public class CSocketClient
	{
		#region 연결할 이벤트 ♥♥♥♥♥♥♥♥♥♥♥♥
		/// <summary>
		/// 접속됨
		/// </summary>
		public event dgCSocketClient_Connect OnConnect;
		private void OnConnect_Call()
		{
			if (null != this.OnConnect)
			{
				this.OnConnect();
			}
		}
		/// <summary>
		/// 모든 로그인과정이 끝났다.
		/// </summary>
		public event dgCSocketClient_ConnectComplete OnConnectComplete;
		private void OnConnectComplete_Call()
		{
			if (null != this.OnConnectComplete)
			{
				this.OnConnectComplete();
			}
		}
		/// <summary>
		/// 접속이 끊어짐.
		/// 이 이벤트에는 유저 끊기 관련 작업을 하면 안됩니다.
		/// </summary>
		public event dgCSocketClient_Disconnect OnDisconnect;
		private void OnDisconnect_Call()
		{
			if (null != this.OnDisconnect)
			{
				this.OnDisconnect();
			}
		}

		/// <summary>
		/// 메시지 받기 완료
		/// </summary>
		public event dgCSocketClient_ReceivePass OnReceivePass;
		private void OnReceivePass_Call(CSD_SA sdData)
		{
			if (null != this.OnReceivePass)
			{
				this.OnReceivePass(sdData);
			}
		}
		private void OnReceivePass_Call(string sData)
		{
			CSD_SA sdData = new CSD_SA();
			sdData.CommandType = CCommand.Command.Msg;
			sdData.Data_String = sData;

			this.OnReceivePass_Call(sdData);
        }
		#endregion


		/// <summary>
		/// 내 소켓
		/// </summary>
		private CSocketUser m_SocketCient;

		private string m_sID = "";

		public CSocketClient(string sID)
		{
			this.m_sID = sID;
		}

		/// <summary>
		/// 서버 연결 시작
		/// </summary>
		public void ServerConnect(string sIP, string sPort)
		{
			//소켓 생성
			Socket socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			//소켓 비동기 이벤트 연결 생성
			CSockertAssist sa = new CSockertAssist(sIP, Convert.ToInt32(sPort));
			sa.GetSAEA();
			//연결 완료 이벤트 연결
			sa.SAEA.Completed += new EventHandler<SocketAsyncEventArgs>(Connect_Completed);

			//서버 메시지 대기
			socketServer.ConnectAsync(sa.SAEA);
		}

		/// <summary>
		/// 연결 완료
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Connect_Completed(object sender, SocketAsyncEventArgs e)
		{
			this.m_SocketCient = new CSocketUser((Socket)sender);
			this.m_SocketCient.OnConnect += M_SocketCient_OnConnect;
			this.m_SocketCient.OnDisconnect += M_SocketCient_OnDisconnect;
			this.m_SocketCient.OnReceiveComplete += M_SocketCient_OnReceiveComplete;

			this.m_SocketCient.BeginReceive();
		}

		private void M_SocketCient_OnConnect()
		{
			//서버에 접속됨

			//로그인 시작
			this.ID_Check();

			//접속을 알린다.
			this.OnConnect_Call();
		}

		private void M_SocketCient_OnDisconnect()
		{
			this.OnDisconnect_Call();
		}

		private void M_SocketCient_OnReceiveComplete(CSendData_Original sdOri)
		{
			//데이터 변환
			CSD_SA sdData = new CSD_SA(sdOri);

			switch(sdData.CommandType)
			{
				case CCommand.Command.None: //없다
					break;
				case CCommand.Command.ID_Check_Ok:
					this.SendMeg_IDCheck_Ok();
					break;
				case CCommand.Command.ID_Check_Fail:
					this.SendMeg_IDCheck_Fail();
					break;
				case CCommand.Command.Login_Complete:
					this.SendMeg_Login_Complete();
					break;
				default:	//그외 명령어
					this.OnReceivePass_Call(sdData);
                    break;

			}
        }

		#region ID 체크 및 로그인
		/// <summary>
		/// 아이디 체크 요청
		/// </summary>
		private void ID_Check()
		{
			this.SendMsg(CCommand.Command.ID_Check, this.m_sID);
		}

		/// <summary>
		/// 아이디 성공
		/// </summary>
		private void SendMeg_IDCheck_Ok()
		{
			//아이디확인이 되었으면 서버에 로그인 요청을 하여 로그인을 끝낸다.
			this.SendMsg(CCommand.Command.Login, "");
		}

		/// <summary>
		/// 아이디체크 실패
		/// </summary>
		private void SendMeg_IDCheck_Fail()
		{
			//사유를 알려주고.
			this.OnReceivePass_Call("로그인 실패 : 다른 아이디를 이용해 주세요.");
			//연결 끊기
			this.m_SocketCient.Disconnect();
		}

		private void SendMeg_Login_Complete()
		{
			//연결 완료
			this.OnConnectComplete_Call();
		}

		#endregion

		

		

		//

		/// <summary>
		/// 연결 끊기.
		/// 'OnDisconnect'이벤트가 발생합니다.
		/// </summary>
		public void Disconnect()
		{
			this.m_SocketCient.Disconnect();
		}


		#region 메시지 보내기
		public void SendMsg(CCommand.Command typeCommand, byte[] byteData)
		{
			CSD_SA dsSendMsg = new CSD_SA();

			//데이터를 넣고
			dsSendMsg.CommandType = typeCommand;
			dsSendMsg.Data_Byte = byteData;

			this.SendMsg(dsSendMsg);
		}
		/// <summary>
		/// 서버로 메시지를 전달 합니다.
		/// </summary>
		/// <param name="sMsg"></param>
		public void SendMsg(CCommand.Command typeCommand, string sMsg)
		{
			CSD_SA dsSendMsg = new CSD_SA();

			//데이터를 넣고
			dsSendMsg.CommandType = typeCommand;
			dsSendMsg.Data_String = sMsg;

			this.SendMsg(dsSendMsg);
		}

		public void SendMsg(CSD_SA sdSA)
		{
			this.m_SocketCient.SendMsg((CSendData)sdSA);
		}
		#endregion

	}
}
