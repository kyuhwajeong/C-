using DGU_Encoding;
using DGU_String;

namespace DGU_Socket
{
	public class CBuffer : CEncoding
	{
		#region 버퍼 관련
		/// <summary>
		/// 버퍼를 최대 크기로 만든다.
		/// </summary>
		/// <returns></returns>
		public byte[] Buffer_GetButter
		{
			get
			{
				return new byte[DGU_CSocket.Socket_Fix_Data.Buffer_Full];
			}
		}

		/// <summary>
		/// 버퍼의 최대 사이즈
		/// </summary>
		/// <returns></returns>
		public int Buffer_GetButterSize
		{
			get
			{
				return DGU_CSocket.Socket_Fix_Data.Buffer_Full;
			}
		}

		/// <summary>
		/// 기본 버퍼
		/// </summary>
		public byte[] Buffer_GetBasicButter
		{
			get
			{
				return new byte[DGU_CSocket.Socket_Fix_Data.Buffer_Basic];
			}
		}

		/// <summary>
		/// 버퍼의 기본 사이즈
		/// </summary>
		/// <returns></returns>
		public int Buffer_GetBasicSize
		{
			get
			{
				return DGU_CSocket.Socket_Fix_Data.Buffer_Basic;
			}
		}
			#endregion

		/// <summary>
		/// 지정한 숫자를 'g_DataHeader1Size'의 크기에 맞게 헤더를 생성합니다.
		/// </summary>
		/// <param name="intData"></param>
		/// <returns></returns>
		public byte[] IntToByte(int intData)
		{
			return base.StringToByte(string.Format("{0:D10}", intData.GetHashCode()));
		}

		/// <summary>
		/// 지정한 바이트배열을 숫자로 바꿔준다.
		/// </summary>
		/// <param name="byteData"></param>
		/// <returns></returns>
		public int ByteToInt(byte[] byteData)
		{
			return CNumber.StringToInt(base.ByteToString(byteData));
		}

		
	}
}
