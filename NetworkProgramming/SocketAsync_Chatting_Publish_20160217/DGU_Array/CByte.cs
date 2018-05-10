using System;

namespace DGU_Array
{
	/// <summary>
	/// http://stackoverflow.com/questions/1389821/array-copy-vs-buffer-blockcopy
	/// Buffer.BlockCopy vs Array.Copy
	/// Buffer.BlockCopy는 인덱스 기반()
	/// Array.Copy는 참조 기반이다.
	/// 성능은 크게 차이 나지 않는다.
	/// </summary>
	public static class CByte
    {
		/// <summary>
		/// byteA와 byteB를 합칩니다.
		/// </summary>
		/// <param name="byteA"></param>
		/// <param name="byteB"></param>
		/// <returns></returns>
		public static byte[] Combine(byte[] byteA, byte[] byteB)
		{
			byte[] byteReturn = new byte[byteA.Length + byteB.Length];

			Buffer.BlockCopy(byteA, 0, byteReturn, 0, byteA.Length);
			Buffer.BlockCopy(byteB, 0, byteReturn, byteA.Length, byteB.Length);

			return byteReturn;
        }

		/// <summary>
		/// byteB의 내용을 그대로 byteA로 복사합니다.
		/// 입력값의 크기가 다른건 예외처리 하지 않습니다.
		/// </summary>
		/// <param name="byteA"></param>
		/// <param name="byteB"></param>
		/// <returns></returns>
		public static void Copy_All(out byte[] byteA, byte[] byteB)
		{
			byteA = new byte[byteB.Length];
			Array.Copy(byteB, 0, byteA, 0, byteB.Length);

			//return byteA;
        }

		/// <summary>
		/// 데이터의 앞을 지정한 크기 만큼 지운다.
		/// </summary>
		/// <param name="byteA"></param>
		/// <param name="nLength"></param>
		/// <returns></returns>
		public static byte[] Remove_Left(byte[] byteA, int nLength)
		{
			byte[] byteReturn = new byte[byteA.Length - nLength];
			Array.Copy(byteA, nLength, byteReturn, 0, byteReturn.Length);

			return byteReturn;
        }

		/// <summary>
		/// 데이터의 뒤를 지정한 크기만큼 지운다.
		/// </summary>
		/// <param name="byteA"></param>
		/// <param name="nLength"></param>
		/// <returns></returns>
		public static byte[] Remove_Right(byte[] byteA, int nLength)
		{
			byte[] byteReturn = new byte[byteA.Length - nLength];
			Array.Copy(byteA, 0, byteReturn, 0, byteReturn.Length);

			return byteReturn;
		}

		/// <summary>
		/// 빈값이 찾고 찾은 자리에서 부터 오른쪽 내용을 지웁니다.
		/// </summary>
		/// <param name="byteA"></param>
		/// <returns></returns>
		public static byte[] Remove_Right_Null(byte[] byteA)
		{
			//뒤에서 부터 검색한다.
			int nCount = byteA.Length - 1;

			//빈값이 없을때까지 찾는다.
			while(0 == byteA[nCount])
			{
				--nCount;
			}

			byte[] byteReturn = new byte[nCount + 1];
			Array.Copy(byteA, byteReturn, nCount + 1);

			return byteReturn;
        }


    }
}
