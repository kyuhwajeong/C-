using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DGU_Socket
{
	public class CSockertAssist
	{
		/// <summary>
		/// 완성된 엔드포인트
		/// </summary>
		public IPEndPoint IP { get; private set; }
		/// <summary>
		/// 완성된 소켓 비동기 이벤트 인수
		/// </summary>
		public SocketAsyncEventArgs SAEA { get; set; }

		public CSockertAssist()
		{
			this.IP = null;
		}

		public CSockertAssist(string sIP, int nPort)
		{
			CIPAddresses ip = new CIPAddresses();
			ip.OnError += Ip_OnError;
			this.IP = ip.StringToEndPoint(sIP, nPort);
		}

		private void Ip_OnError(CIPAddresses.TypeError typeError)
		{
#if Debug
			throw new NotImplementedException();
#endif
		}

		public CSockertAssist(IPAddress ip, int nPort)
		{
			this.IP = new IPEndPoint(ip, nPort);
		}

		public CSockertAssist(IPEndPoint epIP)
		{
			this.IP = epIP;
        }

		/// <summary>
		/// 생성자로 생성한 정보를 가지고 'SocketAsyncEventArgs'를 생성합니다.
		/// </summary>
		public SocketAsyncEventArgs GetSAEA()
		{
			return this.GetSAEA(this.IP);
		}


		/// <summary>
		/// 지정한 정보를 가지고 'SocketAsyncEventArgs'를 생성합니다.
		/// 생성된 정보는 'SAEA'로도 접근 가능합니다.
		/// </summary>
		/// <param name="epIP"></param>
		/// <param name="nPort"></param>
		public SocketAsyncEventArgs GetSAEA(IPEndPoint epIP)
		{
			if (null == epIP)
			{//엔드포인트 정보가 없다.
				return null;
			}

			//소켓 비동기 이벤트 인수
			this.SAEA = new SocketAsyncEventArgs();
			this.SAEA.RemoteEndPoint = epIP;
			
			return this.SAEA;
        }
	}
}
