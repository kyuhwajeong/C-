using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.Common;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Facility.Protocol;
using SocketGlobal;



namespace Server.ClientSession
{
	/// <summary>
	/// 세션으로부터 오는 리시브를 처리하기위한 클래스.
	/// 기존의 리시브필터가 유니코드 처리가 되지 않기 때문이 필요하다
	/// </summary>
	class claReceiveFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
	{
		public claReceiveFilter()
			: base(6)
		{
		}

		protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
		{
			return (int)header[offset + 4] * 256 + (int)header[offset + 5];
		}

		protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
		{
			return new BinaryRequestInfo(Encoding.UTF8.GetString(header.Array, header.Offset, 4), bodyBuffer.CloneRange(offset, length));
		}

		/// <summary>
		/// 메시지가 정확한지 판단.
		/// </summary>
		/// <param name="readBuffer"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <param name="toBeCopied"></param>
		/// <param name="rest"></param>
		/// <returns></returns>
		public override BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
		{
			rest = 0;

			//데이터가 있는지?
			if (null == readBuffer)
			{
				return NullRequestInfo;
			}

			//데이터가 있다.

			//리턴할 객체
			BinaryRequestInfo briReturn;
			//자른 데이터
			MessageData mdTemp = new MessageData();
			mdTemp.SetLength(length);

			//데이터를 오프셋을 기준으로 자른다.
			Buffer.BlockCopy(readBuffer, offset, mdTemp.Data, 0, length);

			//데이터를 넣는다.
			briReturn = new BinaryRequestInfo(mdTemp.GetData(), mdTemp.Data);

			return briReturn;
		}
	}
}
