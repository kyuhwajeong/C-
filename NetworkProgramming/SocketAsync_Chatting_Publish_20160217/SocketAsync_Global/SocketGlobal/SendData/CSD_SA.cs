using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DGU_Socket.SendData;

namespace SocketGlobal.SendData
{
	/// <summary>
	/// 이 프로젝트용 샌드 데이터
	/// </summary>
	public class CSD_SA : CSendData
	{
		/// <summary>
		/// 명령어(enum)
		/// </summary>
		public CCommand.Command CommandType
		{
			get
			{
				return (CCommand.Command)this.f_nCommand;
			}
			set
			{
				this.f_nCommand = value.GetHashCode();
			}
		}

		public CSD_SA()
			: base()
		{
		}

		public CSD_SA(CSendData_Original dataOri)
			: base(dataOri)
		{
		}

		public CSD_SA(CCommand.Command typeCommand, string sData = "")
			: base(typeCommand.GetHashCode(), sData)
		{
		}

		public CSD_SA(CCommand.Command typeCommand, params string[] sDatas)
			: base(typeCommand.GetHashCode(), sDatas)
		{
		}

	}
}
