using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using DGU_Socket.SendData;


namespace DGU_Socket
{
	#region 외부에 연결할 델리게이트
	/// <summary>
	/// 접속됨
	/// </summary>
	public delegate void dgSocketUser_Connect();
	/// <summary>
	/// 접속끊김을 알림
	/// </summary>
	public delegate void dgSocketUser_Disconnect();

	/// <summary>
	/// 메시지 받기 완료
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void dgSocketUser_ReceiveComplete(CSendData_Original sdOri);
	/// <summary>
	/// 메시지 보내기 완료
	/// </summary>
	public delegate void dgSocketUser_SendComplete();
	#endregion

	public class CSocketUser
	{
		#region 연결할 이벤트 ♥♥♥♥♥♥♥♥♥♥♥♥
		/// <summary>
		/// 접속됨
		/// </summary>
		public event dgSocketUser_Connect OnConnect;
		private void OnConnect_Call()
		{
			if (null != this.OnConnect)
			{
				this.OnConnect();
			}
		}
		/// <summary>
		/// 접속이 끊어짐.
		/// 이 이벤트에는 유저 끊기 관련 작업을 하면 안됩니다.
		/// </summary>
		public event dgSocketUser_Disconnect OnDisconnect;
		private void OnDisconnect_Call()
		{
			if (null != this.OnDisconnect)
			{
				this.OnDisconnect();
			}

			//어떤식으로든 접속이 끊어지면 소켓을 초기화한다.
			this.m_socketMe = null;
		}

		/// <summary>
		/// 메시지 받기 완료
		/// </summary>
		public event dgSocketUser_ReceiveComplete OnReceiveComplete;
		private void OnReceiveComplete_Call(CSendData_Original sdOri)
		{
			if (null != this.OnReceiveComplete)
			{
				this.OnReceiveComplete(sdOri);
			}
		}
		/// <summary>
		/// 메시지 보내기가 완료 되었다.
		/// </summary>
		private event dgSocketUser_SendComplete OnSendComplete;
		private void OnSendComplete_Call()
		{
			if (null != this.OnSendComplete)
			{
				this.OnSendComplete();
			}
		}
		#endregion

		/// <summary>
		/// 이 유저의 소켓정보
		/// </summary>
		private Socket m_socketMe { get; set; }

		public IPEndPoint IPInfo
		{
			get
			{
				if (null == this.m_socketMe)
				{
					return null;
				}
				else
				{
					return this.m_socketMe.RemoteEndPoint as IPEndPoint;
                }
			}
                    
		}

		/// <summary>
		/// 리시브 데이터 처리 메니저
		/// </summary>
		private CReciveData_Manager m_RDM = new CReciveData_Manager();

		public CSocketUser(Socket socketUser)
		{
			this.m_socketMe = socketUser;
		}

		public void BeginReceive()
		{
			if (true == this.m_socketMe.Connected)
			{
				//받기용 'SocketAsyncEventArgs'생성
				SocketAsyncEventArgs saeaReceiveArgs = new SocketAsyncEventArgs();
				
				//받음 완료 이벤트 연결
				saeaReceiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SaeaReceiveArgs_Completed);
				//기본버퍼 세팅
				saeaReceiveArgs.SetBuffer(DGU_CSocket.Util_Buffer.Buffer_GetButter
											, 0
											, DGU_CSocket.Util_Buffer.Buffer_GetButterSize);
				//받음 보냄
				m_socketMe.ReceiveAsync(saeaReceiveArgs);

				//접속됨
				this.OnConnect_Call();
			}
			else
			{//접속이 끊김
				this.OnDisconnect_Call();
			}
		}

		private void SaeaReceiveArgs_Completed(object sender, SocketAsyncEventArgs e)
		{
			//넘어온 정보 소켓 정보
			Socket socketClient = (Socket)sender;

			if (false == socketClient.Connected)
			{   //유저의 접속이 끊겼다.
				//접속 끊김을 알린다.
				this.OnDisconnect_Call();
				return;
			}

			//리시브된 데이터를 잠깐 기다린다.
			//Thread.Sleep(50);
			
			
			//리시브된 데이터가 있으면 일단 버퍼에 쌓아둔다.
			this.m_RDM.Buffer_Add(e.Buffer, e.BytesTransferred);
			//Console.WriteLine("{0}", this.m_RDM.Buffer.Count);
#if Debug
			Console.WriteLine("{0}", e.BytesTransferred);
#endif

			//쌓아둔 버퍼가 완성되었는지 확인
			this.m_RDM.ReceiveData_Check();

			
			//다음 데이터를 기다린다.
			//socketClient.ReceiveAsync(e);

			//완성된 리시브데이터가 있는지 확인 한다.
			for (int i = 0; i < this.m_RDM.ReciveData_Complet.Count; ++i)
			{
				CReciveTemp_Data insReciveTemp = this.m_RDM.ReciveData_Complet[i];
				if (false == insReciveTemp.Use)
				{
					//오리지널 데이터로 변환한다.
					CSendData_Original mdRecieveMsg = new CSendData_Original();
					mdRecieveMsg.Bady = insReciveTemp.GetBady();
					this.OnReceiveComplete_Call(mdRecieveMsg);
				}
			}

			//사용한 항목을 삭제 합니다.
			this.m_RDM.CompletList_Organize();

			//다음 데이터를 기다린다.
			socketClient.ReceiveAsync(e);
		}

		public void SendMsg(CSendData dsSendMsg)
		{
			//보낼 데이터 변환
			CSendData_Original doSendMsg = dsSendMsg.CreateDataOriginal();
			byte[] byteAllBady = doSendMsg.AllBady_Get();

			//서버에 보낼 객체를 만든다.
			SocketAsyncEventArgs saeaServer = new SocketAsyncEventArgs();
			//데이터 길이 세팅
			saeaServer.SetBuffer(byteAllBady
								, 0
								, byteAllBady.Length);
			//보내기 완료 이벤트 연결
			saeaServer.Completed += new EventHandler<SocketAsyncEventArgs>(Send_Completed);
			//보낼 데이터 설정
			//saeaServer.UserToken = doSendMsg.Data;

#if Debug
			Console.WriteLine("Send - Length : {0}, comm : {1}"
								, doSendMsg.Length
								, dsSendMsg.Command);
#endif

			//보내기가 완료되었는지 여부
			m_socketMe.SendAsync(saeaServer);
			//비동기로 보낼꺼니까 리턴은 항상 true다.
		}

		/// <summary>
		/// 메시지 보내기 완료.
		/// 'OnSendComplete_Call'은 샌드메시지에서 처리하므로 여기서는 하지 않는다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Send_Completed(object sender, SocketAsyncEventArgs e)
		{
			/*Socket socketSend = (Socket)sender;
			CSendData_Original dsMsg = new CSendData_Original();
			dsMsg.Data = e.Buffer;
			socketSend.Send(dsMsg.Data);*/
		}

		/// <summary>
		/// 이 유저가 가지고 있는 소켓을 끊습니다.
		/// 이 메소드를 실행하면 'OnDisconnect'이벤트가 발생합니다.
		/// </summary>
		public void Disconnect()
		{
			this.m_socketMe.Disconnect(false);
		}

	}
}
