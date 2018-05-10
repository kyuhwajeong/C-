using DGU_Array;
using System;

namespace DGU_Socket.SendData
{
	public struct CSendData_Original
	{
		/// <summary>
		/// 세팅된 데이터 크기
		/// </summary>
		public int Length
		{
			get
			{
				return this.m_nLength;
			}
			set
			{
				//데이터 길이를 저장하고
				this.m_nLength = value;
				//데이터 길이를 세팅한다
				this.m_byteData = new byte[this.m_nLength];
			}
		}
		/// <summary>
		/// 세팅된 데이터 크기(원본)
		/// </summary>
		private int m_nLength;


		public byte[] Head
		{
			get
			{
				return m_byteHead;
            }
			private set
			{
				this.m_byteHead = value;
            }
		}
        private byte[] m_byteHead;

		/// <summary>
		/// 데이터
		/// </summary>
		public byte[] Bady
		{
			get
			{
				return this.m_byteData;
			}
			set
			{
				this.m_byteData = value;
				if (null == this.m_byteData)
				{   //데이터가 없다
					this.m_nLength = 0;
                }
				else
				{	//데이터가 있다.
					//데이터 크기 저장
					this.m_nLength = m_byteData.Length;
				}
			}
		}
		/// <summary>
		/// 데이터(원본)
		/// </summary>
		private byte[] m_byteData;


		


		/// <summary>
		/// 비트로 바꾼 값을 리턴한다.
		/// </summary>
		/// <returns></returns>
		public byte[] getBitConverter()
		{
			byte[] byteReturn = BitConverter.GetBytes(this.Length);
			this.ButterCount = byteReturn.Length;
            return byteReturn;
        }

		/// <summary>
		/// 비트로 바꾼 버퍼의 카운터
		/// </summary>
		public int ButterCount { get; private set; }

		public void SetHead()
		{
			if( 0 < this.Length)
			{//세팅된 데이터가 있다.
				//데이터의 크기를 비트 컨버팅해서 해더로 저장한다.
				this.Head = BitConverter.GetBytes(this.Length);
			}
		}

		/// <summary>
		/// 헤드와 바디를 합친 데이터
		/// </summary>
		/// <returns></returns>
		public byte[] AllBady_Get()
		{
			return CByte.Combine(this.Head, this.Bady);

		}
    }
}
