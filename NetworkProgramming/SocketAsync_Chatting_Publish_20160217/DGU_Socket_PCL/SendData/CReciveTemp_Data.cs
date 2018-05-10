using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DGU_Socket.SendData
{
	/// <summary>
	/// 리시브된 데이터를 잘라서 완성본을 임시저장하는 클래스.
	/// </summary>
	public class CReciveTemp_Data
	{
		/// <summary>
		/// 받아야할 전체 크기(헤더크기는 제외한 크기다.)
		/// </summary>
		public int Size { get; private set; }
		public byte[] Head { get; set; }
		public byte[] Bady { get; set; }

		/// <summary>
		/// 사용했는지 여부
		/// </summary>
		public bool Use { get; private set; }

		public CReciveTemp_Data()
		{
			this.Reset();
		}

		private void Reset()
		{
			this.Size = 0;
			this.Head = new byte[0];
			this.Bady = new byte[0];
		}

		public void Add_Head(byte[] byteData)
		{
			this.Head = DGU_Array.CByte.Combine(this.Head, byteData);
		}

		public void Add_Bady(byte[] byteData)
		{
			this.Bady = DGU_Array.CByte.Combine(this.Bady, byteData);
		}

		/// <summary>
		/// 입력되어 있는 헤더를 비트컨버트 하여 사이즈로 변환 한다.
		/// </summary>
		public void HeadToLength()
		{
			this.Size = BitConverter.ToInt32(this.Head, 0);
		}


		public byte[] GetBady()
		{
			this.Use = true;
			return this.Bady;
		}
	}
}
