using System;
using System.Collections.Generic;


namespace DGU_String
{
	public static class CNumber
	{
		/// <summary>
		/// 입력된 문자열이 숫자인지 안닌지 판단 합니다.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNumeric(string value)
		{
			if ((null == value)
				|| (true == string.IsNullOrEmpty(value))
				|| ("" == value))
			{
				//널이다
				return false;
			}

			int nIndex = 0;

			foreach (char cData in value)
			{
				if (false == Char.IsNumber(cData))
				{
					//인덱스가 0일때 '-'는 부호가 될수 있으므로 숫자로 판단한다.
					if ((0 == nIndex)
						&& ('-' != cData))
					{
						return false;
					}
				}

				++nIndex;
			}
			return true;
		}

		/// <summary>
		/// 문자열을 숫자로 바꿉니다.
		/// </summary>
		/// <param name="sData"></param>
		/// <returns></returns>
		public static int StringToInt(string sData)
		{
			int nReturn = 0;
			if (false == IsNumeric(sData))
			{
				nReturn = 0;
			}
			else
			{
				nReturn = Convert.ToInt32(sData);
			}

			return nReturn;
		}



	}
}
