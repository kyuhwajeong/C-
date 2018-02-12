using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

using Server.ClientSession;
using Server.CustomEventArgs;
using SocketGlobal;


namespace Server
{
	#region UI로 연결할 델리게이트
	
	/// <summary>
	/// 메시지 요청
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void dgMessage(claClientSession session, LocalMessageEventArgs e);
	#endregion

	public class claServer : AppServer<claClientSession, BinaryRequestInfo>
	{
		#region 연결할 이벤트 ♥♥♥♥♥♥♥♥♥♥♥♥
		/// <summary>
		/// UI에 표시할 메시지
		/// </summary>
		public event dgMessage OnMessaged;

		/// <summary>
		/// 유저의 로그인이 완료 되었다.
		/// </summary>
		public event dgMessage OnLoginUser;
		/// <summary>
		/// 유저가 로그아웃 하였다.
		/// </summary>
		public event dgMessage OnLogoutUser;

		#endregion

		/// <summary>
		/// 명령어 클래스
		/// </summary>
		private claCommand m_insCommand = new claCommand();

		public claServer()
			: base(new DefaultReceiveFilterFactory<claReceiveFilter, BinaryRequestInfo>())
		{

		}

		/// <summary>
		/// 새세션이 접속하면 발생
		/// </summary>
		/// <param name="session"></param>
		protected override void OnNewSessionConnected(claClientSession session)
		{
			//세션으로부터 받을 메시지용 이벤트
			session.OnMessaged += session_OnMessaged;

			base.OnNewSessionConnected(session);
		}

		protected override void OnSessionClosed(claClientSession session, CloseReason reason)
		{
			//로그아웃 처리를 하여 유저가 끊김을 알린다.
			OnLogoutUser(session, null);
			base.OnSessionClosed(session, reason);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void session_OnMessaged(claClientSession session,  LocalMessageEventArgs e)
		{
			//OnMessaged(e);	
		}

		/// <summary>
		/// 데이터를 받았다!
		/// </summary>
		/// <param name="session"></param>
		/// <param name="requestInfo"></param>
		protected override void ExecuteCommand(claClientSession session, BinaryRequestInfo requestInfo)
		{
			//UI에 메시지를 표시한다.
			LocalMessageEventArgs e
				= new LocalMessageEventArgs(requestInfo.Key, Type.typeLocal.None);
			OnMessaged(session, e);

			//사용자 클래스에서 넘어온 데이터 처리
			MsgAnalysis(session, requestInfo.Key);
		}

		/// <summary>
		/// 데이터를 분석한다.
		/// </summary>
		/// <param name="sMsg"></param>
		private void MsgAnalysis(claClientSession session, string sMsg)
		{
			//구분자로 명령을 구분 한다.
			string[] sData = sMsg.Split(claGlobal.g_Division);

			//데이터 개수 확인
			if ((0 >= sData.Length))
			{
				//0이면 빈메시지이기 때문에 별도의 처리는 없다.
				return;
			}

			//메시지 처리
			StringBuilder sbMsg = new StringBuilder();

			//넘어온 명령
			claCommand.Command typeCommand
				= m_insCommand.StrIntToType(sData[0]);

			switch (typeCommand)
			{
				case claCommand.Command.Msg:	//메시지
					sbMsg.Clear();
					sbMsg.Append(session.UserID);
					sbMsg.Append(" : ");
					sbMsg.Append(sData[1]);

					Command_SendMsg(sbMsg.ToString());
					break;
				case claCommand.Command.User_List_Get:	//유저 리스트 갱신 요청
					Command_User_List_Get(session);
					break;

				case claCommand.Command.ID_Check:	//아이디 체크
					Command_IDCheck(session, sData[1]);
					break;
				case claCommand.Command.Login:	//로그인
					Command_Login(session);
					break;
				case claCommand.Command.Logout:	//로그아웃
					Command_Logout(session);
					break;
			}
		}

		/// <summary>
		/// 명령 처리 - 메시지 보내기
		/// </summary>
		/// <param name="sMsg"></param>
		private void Command_SendMsg(string sMsg)
		{
			StringBuilder sbMsg = new StringBuilder();
			//명령어 부착
			sbMsg.Append(claCommand.Command.Msg.GetHashCode().ToString());
			//구분자 부착
			sbMsg.Append(claGlobal.g_Division);
			//메시지 완성
			sbMsg.Append(sMsg);

			//전체 유저에게 메시지 전송
			SendMsg_All(sbMsg.ToString());

		}

		/// <summary>
		/// 아이디를 체크 합니다.
		/// </summary>
		/// <param name="session"></param>
		/// <param name="sID"></param>
		private void Command_IDCheck(claClientSession session, string sID)
		{
			//사용 가능 여부
			bool bReturn = true;

			//모든 유저의 아이디 체크
			foreach (claClientSession insUserTemp in this.GetAllSessions())
			{
				if (insUserTemp.UserID == sID)
				{
					//같은 유저가 있다!
					//같은 유저가 있으면 그만 검사한다.
					bReturn = false;
					break;
				}
			}

			if (true == bReturn)
			{
				//사용 가능

				//아이디를 지정하고
				session.UserID = sID;

				//유저에게 로그인이 성공했음을 알림
				StringBuilder sbMsg = new StringBuilder();
				//접속자에게 먼저 로그인이 성공했음을 알린다.
				sbMsg.Append(claCommand.Command.ID_Check_Ok.GetHashCode());
				sbMsg.Append(claGlobal.g_Division);
				session.SendMsg_User(sbMsg.ToString());

				//유저가 접속 했음을 직접 알리지 말고 'ID_Check_Ok'를 받은
				//클라이언트가 직접 요청한다.
				SendMsg_All("접속 성공");
			}
			else
			{
				//검사 실패를 알린다.

				StringBuilder sbMsg = new StringBuilder();

				sbMsg.Append(claCommand.Command.ID_Check_Fail.GetHashCode().ToString());
				sbMsg.Append(claGlobal.g_Division);

				session.SendMsg_User(sbMsg.ToString());
			}

		}

		/// <summary>
		/// 유저가 아이디체크를 끝내고 접속한다!
		/// </summary>
		/// <param name="session"></param>
		private void Command_Login(claClientSession session)
		{
			StringBuilder sbMsg = new StringBuilder();
			
			//로그인이 완료된 유저에게 유저 리스트를 보낸다.
			Command_User_List_Get(session);

			//전체 유저에게 접속자를 알린다.
			sbMsg.Clear();
			sbMsg.Append(claCommand.Command.User_Connect.GetHashCode());
			sbMsg.Append(claGlobal.g_Division);
			sbMsg.Append(session.UserID);

			//전체 유저에게 메시지 전송
			SendMsg_All(sbMsg.ToString());

			//서버에 로그인 로그를 남긴다.
			OnLoginUser(session, null);

		}

		private void Command_Logout(claClientSession session)
		{
			StringBuilder sbMsg = new StringBuilder();

			//전체 유저에게 끊긴자를 알린다.
			sbMsg.Append(claCommand.Command.User_Disonnect.GetHashCode());
			sbMsg.Append(claGlobal.g_Division);
			sbMsg.Append(session.UserID);

			//전체 유저에게 메시지 전송
			SendMsg_All(sbMsg.ToString());

			//서버에 로그아웃 로그를 남긴다.
			OnLogoutUser(session, null);
		}


		/// <summary>
		/// 명령 처리 - 유저 리스트 갱신 요청
		/// </summary>
		/// <param name="insUser"></param>
		private void Command_User_List_Get(claClientSession insUser)
		{
			StringBuilder sbList = new StringBuilder();

			//명령 만들기
			sbList.Append(claCommand.Command.User_List.GetHashCode());
			sbList.Append(claGlobal.g_Division);

			//this.GetAllSessions()의 버그인지 타이밍 문제인지는 모르겠지만
			//this.GetAllSessions()의 리스트에서 방금접속한 유저가 들어있지 않는 경우가 있다.
			//일단 리스트를 만들때 방금접속한 유저는 제외하고 만든 후 다시 추가 해주는 방법을 사용하자.
			//리스트 만들기
			foreach (claClientSession insUser_Temp in this.GetAllSessions())
			{
				//유저아이디가 다를때만 리스트에 추가
				if (insUser_Temp.UserID != insUser.UserID)
				{
					sbList.Append(insUser_Temp.UserID);
					sbList.Append(",");
				}
			}

			//자기 아이디는 따로 추가
			sbList.Append(insUser.UserID);

			//요청에 응답해준다.
			insUser.SendMsg_User(sbList.ToString());
		}

		private void SendMsg_All(string sMsg)
		{
			foreach (claClientSession insUserTemp in this.GetAllSessions())
			{
				insUserTemp.SendMsg_User(sMsg);
			}
		}

	}
}
