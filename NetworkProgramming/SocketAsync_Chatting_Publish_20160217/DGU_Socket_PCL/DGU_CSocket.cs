using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DGU_Socket
{
	public static class DGU_CSocket
	{

		/// <summary>
		/// 소켓에 사용할 데이터들
		/// </summary>
		public static CSocket_Fix_Data Socket_Fix_Data = new CSocket_Fix_Data();

		/// <summary>
		/// 소켓 처리에 필요한 버퍼 관련 기능
		/// </summary>
		public static CBuffer Util_Buffer = new CBuffer();
    }
}
