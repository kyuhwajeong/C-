using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DGU_Socket
{
	#region 외부에 연결할 델리게이트
	/// <summary>
	/// 에러 리턴
	/// </summary>
	public delegate void dgCIPAddresses_Error(CIPAddresses.TypeError typeError);
	#endregion

	public class CIPAddresses
	{
		#region 연결할 이벤트 ♥♥♥♥♥♥♥♥♥♥♥♥
		/// <summary>
		/// 접속됨
		/// </summary>
		public event dgCIPAddresses_Error OnError;
		private void OnOnError_Call(TypeError typeError)
		{
			if (null != this.OnError)
			{
				this.OnError(typeError);
			}
		}
		#endregion

		/// <summary>
		/// 'CIPAddresses'의 처리 에러
		/// </summary>
		public enum TypeError
		{
			/// <summary>
			/// 지정된 호스트네임에서 IP를 찾을 수 없다.
			/// </summary>
			NotFindIP,
			/// <summary>
			/// 지정된 호스트네임에 2개 이상의 IP가 있다.
			/// </summary>
			MoreIP,
		}


		public IPEndPoint StringToEndPoint(string sHostName, int nPort)
		{
			IPAddress[] addresses = new IPAddress[1];

			if (true == IPAddress.TryParse(sHostName, out addresses[0]))
			{//유효한 아이피이다.
            }
			else
			{//유효하지 않은 아이피이다.
				//호스트 네임인지 확인한다.
				addresses = Dns.GetHostAddresses(sHostName);


				if (addresses.Length == 0)
				{//호스트네임에서 IP를 찾을 수 없다.
					this.OnOnError_Call(TypeError.NotFindIP);
					addresses[0] = null;
#if Debug
					throw new ArgumentException();
#endif
					
				}
				else if (addresses.Length > 1)
				{//지정된 호스트에 IP가 여러개다.
					this.OnOnError_Call(TypeError.MoreIP);
					addresses[0] = null;
#if Debug
					throw new ArgumentException();
#endif
				}
			}

			return new IPEndPoint(addresses[0], nPort);
		}
	}
}
