using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DGU_Socket
{
	public class CSocket_Fix_Data
	{
		/// <summary>
		/// 명령어 구분용 문자1
		/// </summary>
		public char Division1 { get; set; }
		/// <summary>
		/// 명령어 구분용 문자1
		/// </summary>
		public char Division2 { get; set; }
		/// <summary>
		/// 테이블을 표현할때 로우의 시작 구분용
		/// </summary>
		public char Division3 { get; set; }


		/// <summary>
		/// 구분자 - 테이블
		/// 데이터를 데이터셋 모양으로 사용할때 테이블을 구분하기위한 구분자.
		/// </summary>
		public char Division_Table { get; set; }
		/// <summary>
		/// 구분자 - 로우
		/// 데이터를 데이터셋 모양으로 사용할때 한줄 한줄을 구분하기위한 구분자.
		/// </summary>
		public char Division_Row { get; set; }
		/// <summary>
		/// 구분자 - 로우 아이템
		/// 데이터를 데이터셋 모양으로 사용할때 한줄에 있는 아이템들을 구분하기위한 구분자.
		/// </summary>
		public char Division_RowItem { get; set; }
	

		/// <summary>
		/// 기본 버퍼 사이즈.
		/// 수신된 버퍼는 최소 이 크기 이상의 양이여야 한다.
		/// </summary>
		public int Buffer_Basic { get; set; }
		/// <summary>
		/// 최대 버퍼 사이즈
		/// </summary>
		public int Buffer_Full { get; set; }

		/// <summary>
		/// 명령어의 크기
		/// </summary>
		public int CommandSize { get; set; }

		/// <summary>
		/// 헤더1의 크기
		/// </summary>
		public int DataHeader1Size { get; set; }
		/// <summary>
		/// 헤더2의 크기
		/// </summary>
		public int DataHeader2Size { get; set; }
	}
}
