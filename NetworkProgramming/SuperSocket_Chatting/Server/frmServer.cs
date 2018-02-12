using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SocketGlobal;

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperSocket.SocketBase.Logging;
using Server.ClientSession;


namespace Server
{
	public partial class frmServer : Form
	{
		/// <summary>
		/// 서버 객체
		/// </summary>
		private claServer m_Server;

		public frmServer()
		{
			InitializeComponent();

			//서버 객체 생성
			m_Server = new claServer();
			m_Server.NewSessionConnected += m_Server_NewSessionConnected;
			m_Server.SessionClosed += m_Server_SessionClosed;

			m_Server.OnMessaged += m_Server_OnMessaged;
			m_Server.OnLoginUser += m_Server_OnLoginUser;
			m_Server.OnLogoutUser += m_Server_OnLogoutUser;
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if( false == claGlobal.g_Number.IsNumeric(txtPort.Text))
			{
				MessageBox.Show("정확한 포트 번호를 넣어 주세요");
				return;
			}

			//포트 넣기
			int nPort = Convert.ToInt32(txtPort.Text);

			ServerConfig serverConfig = new ServerConfig
			{
				Ip = "127.0.0.1",//테스트할때만 로컬 ip를 넣는다.
				Port = nPort,
			};

			//서버 설정 셋업
			m_Server.Setup(serverConfig);

			//서버 시작
			if (false == m_Server.Start())
			{
				DisplayLog("서버 시작 실패");
				return;
			}

			//버튼 표시
			BtnDisplay(false);
			DisplayLog("서버 시작");
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			m_Server.Stop();
			BtnDisplay(true);
			DisplayLog("서버 중지");
		}

		void m_Server_NewSessionConnected(claClientSession session)
		{
			DisplayLog("유저 접속 시작 ");
		}

		void m_Server_SessionClosed(claClientSession session, SuperSocket.SocketBase.CloseReason value)
		{
			DisplayLog("유저 끊김 : " + session.UserID);
		}

		void m_Server_OnMessaged(claClientSession session, CustomEventArgs.LocalMessageEventArgs e)
		{
			//서버객체에서 넘어온 메시지
			DisplayLog(e.Message);
		}

		void m_Server_OnLoginUser(claClientSession session, CustomEventArgs.LocalMessageEventArgs e)
		{
			//로그 유저 리스트에 추가
			this.Invoke(new Action(
				delegate()
				{
					listUser.Items.Add(session.UserID);
				}));
			
			//로그 남기기
			StringBuilder sbMsg = new StringBuilder(); ;
			sbMsg.Append("*** 접속자 : ");
			sbMsg.Append(session.UserID);
			sbMsg.Append(" ***");
			DisplayLog(sbMsg.ToString());
		}
		
		void m_Server_OnLogoutUser(claClientSession session, CustomEventArgs.LocalMessageEventArgs e)
		{
			StringBuilder sbMsg = new StringBuilder();
			sbMsg.Append(" *** ");
			sbMsg.Append(session.UserID);
			sbMsg.Append(" : 접속 끊김 *** ");

			//로그리스트에서 유저를 지움
			//출력
            if (String.IsNullOrEmpty(session.UserID) == false)
            {
                this.Invoke(new Action(
                            delegate()
                            {
                                listUser.Items.RemoveAt(listUser.FindString(session.UserID));
                            }));
            }
			//로그 기록
			DisplayLog(sbMsg.ToString());

		}

		/// <summary>
		/// 버튼을 화면에 표시하거나 가린다.
		/// </summary>
		/// <param name="bView"></param>
		private void BtnDisplay(bool bView)
		{
			if (true == bView)
			{
				btnStart.Enabled = true;
				btnStop.Enabled = false;
			}
			else
			{
				btnStart.Enabled = false;
				btnStop.Enabled = true;
			}
		}

		/// <summary>
		/// 받아온 메시지를 출력 한다.
		/// </summary>
		/// <param name="nMessage"></param>
		/// <param name="nType"></param>
		public void DisplayLog(String nMessage)
		{
			StringBuilder buffer = new StringBuilder();

			//출력할 메시지 완성
			buffer.Append(nMessage);

			//출력
			this.Invoke(new Action(
						delegate()
						{
							listLog.Items.Add(nMessage);
						}));
		}

	}
}
