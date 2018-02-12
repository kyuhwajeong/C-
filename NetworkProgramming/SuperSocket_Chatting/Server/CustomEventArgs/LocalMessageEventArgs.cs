using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Type;

namespace Server.CustomEventArgs
{
	/// <summary>
	/// 메시지 이벤트용 형식입니다.
	/// </summary>
	public class LocalMessageEventArgs : EventArgs
	{
		/// <summary>
		/// 메시지
		/// </summary>
		public readonly string Message = "";
		/// <summary>
		/// 메시지 타입
		/// </summary>
		public typeLocal Icon = typeLocal.None;

		/// <summary>
		/// 메시지 설정
		/// </summary>
		/// <param name="strMsg"></param>
		/// <param name="typeIcon"></param>
		public LocalMessageEventArgs(string strMsg, typeLocal typeIcon)
		{
			this.Message = strMsg;
			this.Icon = typeIcon;
		}

		public LocalMessageEventArgs()
		{
			this.Message = "";
			this.Icon = typeLocal.None;
		}
	}
}
