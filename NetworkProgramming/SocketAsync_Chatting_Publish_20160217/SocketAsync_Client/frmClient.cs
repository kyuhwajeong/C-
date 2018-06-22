using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketGlobal;
using SocketGlobal.SendData;
using DGU_Socket;
using DGU_Socket.SendData;

namespace SocketAsync_Client
{
	public partial class frmClient : Form
	{
		
		/// <summary>
		/// 나의 상태
		/// </summary>
		enum typeState
		{
			/// <summary>
			/// 없음
			/// </summary>
			None = 0,
			/// <summary>
			/// 연결중
			/// </summary>
			Connecting,
			/// <summary>
			/// 연결 완료
			/// </summary>
			Connect,
		}

		/// <summary>
		/// 클라이언트 처리
		/// </summary>
		private CSocketClient m_SocketCient;

		/// <summary>
		/// 나의 상태
		/// </summary>
		private typeState m_typeState = typeState.None;

		public frmClient()
		{
			InitializeComponent();
			this.Text = CGlobal.g_SiteTitle;

			//유아이 기본으로 세팅
			UI_Setting(typeState.None);
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			//로그인

			if ("" == txtID.Text)
			{
				//입력값이 없으면 리턴
				MessageBox.Show("아이디를 넣고 시도해 주세요");
				return;
			}
			else
			{
				//아이디가 있으면 로그인 시작

				//유아이를 세팅하고
				UI_Setting(typeState.Connecting);

				//클라이언트 객체를 생성하고
				this.m_SocketCient = new CSocketClient(this.txtID.Text);
				this.m_SocketCient.OnConnect += M_SocketCient_OnConnect;
				this.m_SocketCient.OnConnectComplete += M_SocketCient_OnConnectComplete;
				this.m_SocketCient.OnDisconnect += M_SocketCient_OnDisconnect;
				this.m_SocketCient.OnReceivePass += M_SocketCient_OnReceivePass;

				//서버 연결 시작
				this.m_SocketCient.ServerConnect(this.txtIP.Text, this.txtPort.Text);
			}

		}

		private void M_SocketCient_OnConnect()
		{
			DisplayMsg("*** 서버 연결됨 ***");
		}

		private void M_SocketCient_OnConnectComplete()
		{
			//접속이 완료 되었다.

			//UI갱신
			UI_Setting(typeState.Connect);

			//서버에 유저목록을 요청한다.
			this.m_SocketCient.SendMsg(CCommand.Command.User_List_Get, "");
		}

		private void M_SocketCient_OnDisconnect()
		{
			//유아이를 세팅하고
			UI_Setting(typeState.None);

			DisplayMsg("*** 서버 연결 끊김 ***");
		}

		private void M_SocketCient_OnReceivePass(CSD_SA sdData)
		{
#if Debug
			Console.WriteLine("client get - command : {0}", sdData.CommandType.ToString());
#endif

			switch (sdData.CommandType)
			{
				case CCommand.Command.None: //없다
					break;
				case CCommand.Command.Msg:    //메시지인 경우
					this.Command_Msg(sdData.Data_Byte_ToString());
					break;
				case CCommand.Command.User_Connect: //다른 유저가 접속 했다.
					this.SendMeg_User_Connect(sdData.Data_Byte_ToString());
					break;
				case CCommand.Command.User_Disonnect:   //다른 유저가 접속을 끊었다.
					this.SendMeg_User_Disconnect(sdData.Data_Byte_ToString());
					break;
				case CCommand.Command.User_List:    //유저 리스트 갱신
					this.SendMeg_User_List(sdData.Data_Byte_ToString());
					break;

				case CCommand.Command.Image:
					this.SendMeg_User_Image(sdData.Data_Byte);
					break;
			}
		}
		

		private void M_SocketCient_OnMessageComplete(CSendData_Original sdOri)
		{
			CSD_SA sdMessage = new CSD_SA(sdOri);
			//명령어를 분리한다.

		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			this.m_SocketCient.SendMsg(CCommand.Command.Msg, txtMsg.Text);
			txtMsg.Text = "";
		}

		/// <summary>
		/// UI 세팅
		/// </summary>
		/// <param name="typeSet"></param>
		private void UI_Setting(typeState typeSet)
		{
			//들어온 값을 세팅하고
			m_typeState = typeSet;

			switch (typeSet)
			{
				case typeState.None:	//기본
					if (false == InvokeRequired)
					{
						this.UI_None();
                    }
					else
					{
						this.Invoke(new Action(
						delegate()
						{
							this.UI_None();
						}));
					}
					break;
				case typeState.Connecting:  //연결중
					if (false == InvokeRequired)
					{
						this.UI_Connecting();
					}
					else
					{
						this.Invoke(new Action(
						delegate ()
						{
							this.UI_Connecting();
						}));
					}
					break;
				case typeState.Connect: //연결완료
					if (false == InvokeRequired)
					{
						this.UI_Connect();
					}
					else
					{
						this.Invoke(new Action(
						delegate ()
						{
							this.UI_Connect();
						}));
					}
					break;
			}
		}

		private void UI_None()
		{
			txtIP.Enabled = true;
			txtPort.Enabled = true;
			txtID.Enabled = true;
			btnLogin.Enabled = true;

			btnAutoMsg.Enabled = false;
			btnSend.Enabled = false;
			btnImageSend.Enabled = false;
		}

		private void UI_Connecting()
		{
			txtIP.Enabled = false;
			txtPort.Enabled = false;
			txtID.Enabled = false;
			btnLogin.Enabled = false;

			btnAutoMsg.Enabled = false;
			btnSend.Enabled = false;
			btnImageSend.Enabled = false;

			this.DisplayMsg("*** 접속중 ***");
		}

		private void UI_Connect()
		{
			txtIP.Enabled = false;
			txtPort.Enabled = false;
			txtID.Enabled = false;
			btnLogin.Enabled = false;

			btnAutoMsg.Enabled = true;
			btnSend.Enabled = true;
			btnImageSend.Enabled = true;

			this.DisplayMsg("*** 접속 완료 ***");
		}

		/// <summary>
		/// 메시지 출력
		/// </summary>
		/// <param name="sMsg"></param>
		private void Command_Msg(string sMsg)
		{
			this.DisplayMsg(sMsg);
		}

		#region 유저 리스트 관련
		/// <summary>
		/// 접속한 유저가 있다.
		/// </summary>
		private void SendMeg_User_Connect(string sUserID)
		{
			this.Invoke(new Action(
						delegate()
						{
							listUser.Items.Add(sUserID);
						}));
		}

		/// <summary>
		/// 접속을 끊은 유저가 있다.
		/// </summary>
		/// <param name="sUserID"></param>
		private void SendMeg_User_Disconnect(string sUserID)
		{
			this.Invoke(new Action(
						delegate()
						{
							listUser.Items.RemoveAt(listUser.FindString(sUserID));
						}));
		}

		/// <summary>
		/// 유저리스트 
		/// </summary>
		/// <param name="sUserList"></param>
		private void SendMeg_User_List(string sUserList)
		{
			this.Invoke(new Action(
				delegate()
				{
					//리스트를 비우고
					listUser.Items.Clear();

					//리스트를 다시 채워준다.
					string[] sList = sUserList.Split(',');
					for (int i = 0; i < sList.Length; ++i)
					{
						listUser.Items.Add(sList[i]);
					}

				}));
		}

		#endregion

		private void SendMeg_User_Image(byte[] byteData)
		{
			MemoryStream mStream = new MemoryStream();
			Image image = Image.FromStream(new MemoryStream(byteData));
			/*
			byte[] pData = byteData;
			mStream.Write(pData, 0, pData.Length);
			Bitmap bm = new Bitmap(mStream, false);
			mStream.Dispose();*/

			this.Invoke(new Action(
						delegate ()
						{
							pbDownImage.Image = image;
						}));
		}

		/// <summary>
		/// 받아온 메시지를 출력 한다.
		/// </summary>
		/// <param name="nMessage"></param>
		/// <param name="nType"></param>
		private void DisplayMsg(string nMessage)
		{
			StringBuilder buffer = new StringBuilder();

			//출력할 메시지 완성
			buffer.Append(nMessage);

			//출력
			if (false == InvokeRequired)
			{
				listMsg.Items.Add(nMessage);
			}
			else
			{
				this.Invoke(new Action(
				delegate ()
				{
					listMsg.Items.Add(nMessage);
				}));
			}
		}

		private void Logout()
		{
			this.m_SocketCient.SendMsg(CCommand.Command.Logout, "");
		}

		/// <summary>
		/// 테스트용 자동 메시지 타이머
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.m_SocketCient.SendMsg(CCommand.Command.Msg, txtAutoMsg.Text);
		}

		private void btnAutoMsg_Click(object sender, EventArgs e)
		{
			if (true == timer1.Enabled)
			{
				timer1.Enabled = false;
			}
			else
			{
				timer1.Enabled = true;
			}
		}

		private void btnImageSend_Click(object sender, EventArgs e)
		{
			//이미지 전송

			//명령어 만들기
			byte[] byteTemp = File.ReadAllBytes(txtDir.Text);
			//파일 읽기
			/*byte[] buff = File.ReadAllBytes(txtDir.Text);
			FileStream fs = new FileStream(txtDir.Text, FileMode.Open);
			byteTemp = new byte[fs.Length];
			fs.Read(byteTemp, 0, byteTemp.Length);
			fs.Close();*/


			//전송 요청
			this.m_SocketCient.SendMsg(CCommand.Command.Image, byteTemp);
		}

        private void btnTRSelect_Click(object sender, EventArgs e)
        {
            this.m_SocketCient.SendMsg(CCommand.Command.TRSelect, "AAABBBBCCCC");
        }

		
	}
}
