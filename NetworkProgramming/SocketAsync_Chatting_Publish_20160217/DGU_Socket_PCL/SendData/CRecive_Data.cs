using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DGU_Socket.SendData
{
	public class CRecive_Data
	{
		/// <summary>
		/// 가지고 있는 버퍼
		/// </summary>
		public byte[] Buffer
		{
			get
			{
				return this.m_byetBuffer;
			}
			set
			{
				this.m_byetBuffer = value;
				//속도를 위해 메모리를 희생한다.
				this.Length = this.m_byetBuffer.Length;
            }
		}
		/// <summary>
		/// 가지고 있는 버퍼(원본)
		/// </summary>
		private byte[] m_byetBuffer;

		/// <summary>
		/// 가지고 있는 버퍼의 크기
		/// </summary>
		public int Length
		{
			get
			{
				return this.m_nLength;
            }
			private set
			{
				this.m_nLength = value;
			}

		}
		/// <summary>
		/// 가지고 있는 버퍼의 크기(원본)
		/// </summary>
		private int m_nLength = 0;

		public CRecive_Data(byte[] byteBuffer)
		{
			byte[] byteBufferTemp = new byte[byteBuffer.Length];
			DGU_Array.CByte.Copy_All(out byteBufferTemp, byteBuffer);
			this.Buffer = byteBufferTemp;
        }

		/// <summary>
		/// 가지고 있는 데이터의 앞쪽에서 지정한 만큼 잘라서 준다.
		/// 잘리고 남은값만 저장된다.(잘라낸 앞쪽값은 리턴된 후 제거됨.)
		/// 가지고 있는 데이터보다 지정한 값이 크면 모든 값을 준다.
		/// </summary>
		/// <param name="nCutSize"></param>
		/// <returns></returns>
		public byte[] Cut(int nCutSize)
		{
			int nCutSize_Re = nCutSize;
			if(nCutSize_Re > this.Buffer.Length )
			{//가지고 있는 데이터가 지정된 값보다 작다.
				//가지고 있는 데이터의 크기로 지정한다.
				nCutSize_Re = this.Buffer.Length;
            }

			//지정한 만큼 버퍼를 지정하고.
			byte[] byteReturn = new byte[nCutSize_Re];

			//지정된 만큼 데이터를 자른다.
			System.Buffer.BlockCopy(this.Buffer
									, 0
									, byteReturn
									, 0
									, nCutSize_Re);

			//남은 데이터 당기기
			this.Buffer = DGU_Array.CByte.Remove_Left(this.Buffer, nCutSize_Re);

            return byteReturn;
		}
		
    }
}
