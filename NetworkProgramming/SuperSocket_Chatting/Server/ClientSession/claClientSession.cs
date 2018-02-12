using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

using SocketGlobal;
using Server.CustomEventArgs;


namespace Server.ClientSession
{
	public class claClientSession : AppSession<claClientSession, BinaryRequestInfo>
	{
		#region 연결할 이벤트 ♥♥♥♥♥♥♥♥♥♥♥♥
		/// <summary>
		/// UI에 표시할 메시지
		/// </summary>
		public event dgMessage OnMessaged;
		#endregion

		/// <summary>
		/// 이 유저의 아이디
		/// </summary>
		public string UserID { get; set; }

		protected override void OnSessionStarted()
		{
			base.OnSessionStarted();
		}

		protected override void HandleException(Exception e)
		{
			this.Send("오류 : {0}", e.Message);
		}

		protected override void OnSessionClosed(CloseReason reason)
		{
			base.OnSessionClosed(reason);
		}

		/// <summary>
		/// 서버객체로 메시지를 넘길때 사용
		/// </summary>
		/// <param name="sMsg"></param>
		private void SendMsg_ServerLog(string sMsg)
		{
			LocalMessageEventArgs e 
				= new LocalMessageEventArgs(sMsg, Type.typeLocal.None);

			OnMessaged(this, e);
		}

        string str;//test
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sMsg"></param>
		public void SendMsg_User(string sMsg)
		{
			MessageData md = new MessageData();
			md.SetData(sMsg);


             str = System.Text.Encoding.Default.GetString(md.Data); //test
			//this.Send(md.GetData());
			this.Send(md.Data, 0, md.DataLength);
		}
	}
}
