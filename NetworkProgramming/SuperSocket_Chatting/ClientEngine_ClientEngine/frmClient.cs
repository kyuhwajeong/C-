using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketGlobal;
using SuperSocket.ClientEngine;

namespace ClientEngine_ClientEngine
{
	public partial class frmClient : Form
	{
		public string Title = "ClientEngine_ClientEngine";

		/// <summary>
		/// 명령어 클래스
		/// </summary>
		private claCommand m_insCommand = new claCommand();

		/// <summary>
		/// 내 세션
		/// </summary>
		private AsyncTcpSession m_insSession;

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
		/// 로그인 
		/// </summary>
		private bool m_bLogin = false;

		/// <summary>
		/// 나의 상태
		/// </summary>
		private typeState m_typeState = typeState.None;

		public frmClient()
		{
			InitializeComponent();

			//유아이 기본으로 세팅
			UI_Setting(typeState.None);
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			switch (m_typeState)
			{
				case typeState.None:	//기본
					if ("" == txtMsg.Text)
					{
						//입력값이 없으면 리턴
						MessageBox.Show("아이디를 넣고 시도해 주세요.");
						return;
					}
					else if (false == claGlobal.g_Number.IsNumeric(txtPort.Text))
					{
						//포트번호가 잘못 되었으면 리턴
						MessageBox.Show("포트번호를 정확하게 적어 주세요.");
						return;
					}
					else
					{
						//아이디가 있으면 로그인 시작

						//유아이를 세팅하고
						UI_Setting(typeState.Connecting);

						//소켓 생성
						IPEndPoint ipServer
							= new IPEndPoint(IPAddress.Parse("127.0.0.1")
												, Convert.ToInt32(txtPort.Text));
						EndPoint epTemp = ipServer as EndPoint;

						m_insSession = new AsyncTcpSession(epTemp);

						m_insSession.Connected += m_insSession_Connected;
						m_insSession.Closed += m_insSession_Closed;
						m_insSession.DataReceived += m_insSession_DataReceived;
						m_insSession.Error += new EventHandler<ErrorEventArgs>(m_insSession_Error);

						m_insSession.Connect();

						DisplayMsg("접속 시작");
					}
					break;

				case typeState.Connect:	//접속 상태
					//이상태에서는 메시지를 보낸다.
					StringBuilder sbData = new StringBuilder();
					sbData.Append(claCommand.Command.Msg.GetHashCode());
					sbData.Append(claGlobal.g_Division);
					sbData.Append(txtMsg.Text);
					SendMsg(sbData.ToString());
					txtMsg.Text = "";
					break;
			}
		}

		void m_insSession_Connected(object sender, EventArgs e)
		{
			if (true == m_insSession.IsConnected)
			{

				DisplayMsg("*** 서버 연결 성공 ***");
				//서버 연결이 성공하면 id체크를 시작한다.
				Login();
			}
			else
			{
				Disconnection();
			}
		}

		void m_insSession_Closed(object sender, EventArgs e)
		{
			Disconnection();
		}

		void m_insSession_DataReceived(object sender, DataEventArgs e)
		{
			if (true == m_insSession.IsConnected)
			{
				
				//정상데이터
				MessageData mdTemp = new MessageData();
				mdTemp.SetLength(e.Length);
				Buffer.BlockCopy(e.Data, e.Offset, mdTemp.Data, 0, e.Length);
				//mdTemp.SetData( e.Data);

				Console.WriteLine("확인 : " + mdTemp.GetData());

				MsgAnalysis(mdTemp.GetData());
			}
			else
			{
				Disconnection();
			}
		}
		void m_insSession_Error(object sender, ErrorEventArgs e)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 넘어온 데이터를 분석 한다.
		/// </summary>
		/// <param name="sMsg"></param>
		private void MsgAnalysis(string sMsg)
		{
			//구분자로 명령을 구분 한다.
			string[] sData = sMsg.Split(claGlobal.g_Division);

			//데이터 개수 확인
			if ((1 <= sData.Length))
			{
				//0이면 빈메시지이기 때문에 별도의 처리는 없다.

				//넘어온 명령
				claCommand.Command typeCommand
					= m_insCommand.StrIntToType(sData[0]);

				switch (typeCommand)
				{
					case claCommand.Command.None:	//없다
						break;
					case claCommand.Command.Msg:	//메시지인 경우
						Command_Msg(sData[1]);
						break;
					case claCommand.Command.ID_Check_Ok:	//아이디 성공
						Command_IDCheck_Ok();
						break;
					case claCommand.Command.ID_Check_Fail:	//아이디 실패
						SendMeg_IDCheck_Fail();
						break;
					case claCommand.Command.User_Connect:	//다른 유저가 접속 했다.
						Command_User_Connect(sData[1]);
						break;
					case claCommand.Command.User_Disonnect:	//다른 유저가 접속을 끊었다.
						Command_User_Disconnect(sData[1]);
						break;
					case claCommand.Command.User_List:	//유저 리스트 갱신
						Command_User_List(sData[1]);
						break;
				}
			}
		}

		/// <summary>
		/// 메시지 출력
		/// </summary>
		/// <param name="sMsg"></param>
		private void Command_Msg(string sMsg)
		{
			DisplayMsg(sMsg);
		}

		/// <summary>
		/// 아이디 성공
		/// </summary>
		private void Command_IDCheck_Ok()
		{
			this.Invoke(new Action(
						delegate()
						{
							this.Text = txtMsg.Text + " - " + Title;
							labID.Text = txtMsg.Text;
							txtMsg.Text = "";
						}));

			//UI갱신
			UI_Setting(typeState.Connect);

			//아이디확인이 되었으면 서버에 로그인 요청을 하여 로그인을 끝낸다.
			StringBuilder sbData = new StringBuilder();
			sbData.Append(claCommand.Command.Login.GetHashCode());
			sbData.Append(claGlobal.g_Division);

			//테스트동안 지우기
			SendMsg(sbData.ToString());
		}

		/// <summary>
		/// 아이디체크 실패
		/// </summary>
		private void SendMeg_IDCheck_Fail()
		{
			DisplayMsg("로그인 실패 : 다른 아이디를 이용해 주세요.");
			//연결 끊기
			Disconnection();
		}

		/// <summary>
		/// 접속한 유저가 있다.
		/// </summary>
		private void Command_User_Connect(string sUserID)
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
		private void Command_User_Disconnect(string sUserID)
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
		private void Command_User_List(string sUserList)
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
					try
					{
						btnSend.Text = "로그인";
					}
					catch
					{
						this.Invoke(new Action(
							delegate()
							{
								btnSend.Text = "로그인";
							}));
					}
					break;
				case typeState.Connecting:	//연결중
					txtMsg.Enabled = false;
					btnSend.Text = "연결중";
					btnSend.Enabled = false;
					break;
				case typeState.Connect:	//연결완료
					this.Invoke(new Action(
						delegate()
						{
							txtMsg.Enabled = true;
							btnSend.Text = "보내기";
							btnSend.Enabled = true;
						}));
					break;
			}
		}

		/// <summary>
		/// 받아온 메시지를 출력 한다.
		/// </summary>
		/// <param name="nMessage"></param>
		/// <param name="nType"></param>
		private void DisplayMsg(String nMessage)
		{
			StringBuilder buffer = new StringBuilder();

			//출력할 메시지 완성
			buffer.Append(nMessage);

			//출력
			this.Invoke(new Action(
						delegate()
						{
							listMsg.Items.Add(nMessage);
						}));

		}


		/// <summary>
		/// 접속이 끊겼다.
		/// </summary>
		private void Disconnection()
		{
			//아직 끈기지 않았다면 끊김 처리
			m_insSession.Close();

			//유아이를 세팅하고
			UI_Setting(typeState.None);
			this.Text = Title;
			labID.Text = "";

			DisplayMsg("*** 서버 연결 끊김 ***");
		}

		/// <summary>
		/// 아이디 체크 요청
		/// </summary>
		private void Login()
		{
			StringBuilder sbData = new StringBuilder();
			sbData.Append(claCommand.Command.ID_Check.GetHashCode());
			sbData.Append(claGlobal.g_Division);
			sbData.Append(txtMsg.Text);

			SendMsg(sbData.ToString());
		}

		private void Logout()
		{
			StringBuilder sbData = new StringBuilder();
			sbData.Append(claCommand.Command.Logout.GetHashCode());
			sbData.Append(claGlobal.g_Division);

			SendMsg(sbData.ToString());
		}

		/// <summary>
		/// 서버로 메시지를 전달 합니다.
		/// </summary>
		/// <param name="sMsg"></param>
		private void SendMsg(string sMsg)
		{
			MessageData md = new MessageData();
			md.SetData(sMsg);

			m_insSession.Send(md.Data, 0, md.DataLength);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			StringBuilder sbData = new StringBuilder();
			sbData.Append(claCommand.Command.Msg.GetHashCode());
			sbData.Append(claGlobal.g_Division);
			sbData.Append(txtAutoMsg.Text);
			SendMsg(sbData.ToString());
		}

		private void button1_Click(object sender, EventArgs e)
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

		
	}
}
