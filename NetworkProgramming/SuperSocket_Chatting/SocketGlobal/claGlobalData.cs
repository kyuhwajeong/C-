using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocketGlobal.CustomClass;

namespace SocketGlobal
{
	/// <summary>
	/// 데이터 구조체
	/// </summary>
	public struct MessageData
	{
		private int m_DataLength;
		private byte[] m_Data;

		public void SetLength(byte[] Data)
		{
			if (Data.Length < 4)
				return;
			m_DataLength = BitConverter.ToInt32(Data, 0);
		}
		public void SetLength(int nLength)
		{
			//데이터 길이를 세팅하고
			m_Data = new byte[nLength];
			//데이터 길이를 저장한다.
			m_DataLength = nLength;
		}
		public int DataLength
		{
			get { return m_DataLength; }
		}
		public void InitData()
		{
			m_Data = new byte[m_DataLength];
		}
		public byte[] Data
		{
			get 
			{ 
				return m_Data; 
			}
			set 
			{ 
				m_Data = value;
				//데이터의 길이를 저장한다.
				this.m_DataLength = m_Data.Length;
			}
		}
		public String GetData()
		{
			return Encoding.Unicode.GetString(m_Data);
		}
		public byte[] GetBuffer()
		{
			return new byte[4];
		}
		public void SetData(String Data)
		{
			m_Data = Encoding.Unicode.GetBytes(Data);
			m_DataLength = m_Data.Length;
		}
		public void SetData(byte[] Data)
		{
			m_Data = Data;
			m_DataLength = Data.Length;
		}
	}

	public enum ChatType
	{
		Send,
		Receive,
		System
	}

	public class claGlobal
	{
		public static string g_SiteTitle = "Socket - SocketAsyncEventArgs";

		/// <summary>
		/// 명령어 구분용 문자
		/// </summary>
		public static char g_Division = '▦';

		public static claNumber g_Number = new claNumber();
	}
}
