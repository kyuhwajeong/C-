using System;
using System.Collections.Generic;
using System.Text;

namespace DGU_Socket.SendData
{
	/// <summary>
	/// 메시지를 주고받기전에 바이트로 변환전 데이터를 가지고 있는 클래스.
	/// </summary>
	public class CSendData
	{
		/// <summary>
		/// 명령어
		/// </summary>
		public virtual int Command { get { return this.f_nCommand; } set { this.f_nCommand = value; } }
		/// <summary>
		/// 명령어 원본
		/// </summary>
		protected int f_nCommand { get; set; }

		/// <summary>
		/// 데이터(byte[]), 우선순위 1.
		/// </summary>
		public byte[] Data_Byte { get; set; }
		/// <summary>
		/// 바이트 데이터를 문자열로 바꿔 리턴합니다.
		/// </summary>
		/// <returns></returns>
		public string Data_Byte_ToString()
		{
			return DGU_CSocket.Util_Buffer.ByteToString(this.Data_Byte);
		}
		/// <summary>
		/// 바이트 데이터를 문자열로 바꿔 저장합니다.
		/// </summary>
		public void Data_Byte_ToString_Save()
		{
			this.Data_String = this.Data_Byte_ToString();
		}

		/// <summary>
		/// 데이터(string), 우선순위 3.
		/// 문자열리스트로된 리스트를 문자열로 변환한 데이터.
		/// </summary>
		public string Data_String { get; set; }
		/// <summary>
		/// 문자열 데이터를 문자열리스트로 만들어 리턴 합니다.
		/// </summary>
		/// <returns></returns>
		public List<string> Data_String_ToList()
		{
			string[] arrData = this.Data_String.Split(DGU_CSocket.Socket_Fix_Data.Division1);

			List<string> listReturn = new List<string>();
			foreach(string sTemp in arrData)
			{
				listReturn.Add(sTemp);
			}
            return listReturn;
        }
		/// <summary>
		/// 문자열 데이터를 문자열리스트로 저장합니다.
		/// </summary>
		public void Data_String_ToList_Save()
		{
			this.Data_List = this.Data_String_ToList();
		}


		/// <summary>
		/// 데이터(List), 우선순위 2.
		/// </summary>
		public List<string> Data_List { get; set; }
		/// <summary>
		/// 데이터 리스트를 초기화 합니다.
		/// </summary>
		public void Data_List_Reset()
		{
			this.Data_List = new List<string>();
		}
		/// <summary>
		/// 데이터를 데이터 리스트에 추가 합니다.
		/// </summary>
		/// <param name="sData"></param>
		public void Data_List_Add(string sData)
		{
			if (null == this.Data_List)
			{//리스트가 생성되지 않았으면 초기화 해준다.
				this.Data_List_Reset();
			}
			//데이터 추가
			this.Data_List.Add(sData);
		}
		/// <summary>
		/// 데이터배열을 아이템 구분용 구분자로 구분하여 리스트에 추가 합니다.
		/// </summary>
		/// <param name="sData"></param>
		/// <param name="sDivisionRow"></param>
		public void Data_List_Add(string[] sData, string sDivisionRow)
		{
			StringBuilder sbAdd = new StringBuilder();
			for(int i = 0; i < sData.Length; ++i)
			{
				sbAdd.Append(sData[i]);
				if( i < sData.Length - 1)
				{//맨 마지막엔 구분자를 넣지 않는다.
					sbAdd.Append(sDivisionRow);
				}
			}

			//데이터 추가
			this.Data_List_Add(sbAdd.ToString());
        }

		/// <summary>
		/// 데이터를 구분하기 위한 고유번호
		/// </summary>
		public int Index { get; set; }
		/// <summary>
		/// 받기나 보내기 작업이 완료되었는지 여부
		/// </summary>
		public bool Complete { get; set; }

		private void ResetClass()
		{
			Data_List = new List<string>();
			this.Complete = false;

			this.Command = -1;
            this.Data_String = "";
		}

		public CSendData()
		{
			ResetClass();
		}

		public CSendData(CSendData_Original dataOri)
		{
			ResetClass();

			this.DataOriginalToThis(dataOri);
		}

		public CSendData(int intCommand, string sData = "")
		{
			ResetClass();

			this.Command = intCommand;
			this.Data_String = sData;
			//입력된 데이터를 변환 한다.
			this.CreateDataSend();
		}

		public CSendData(int intCommand, params string[] sDatas)
		{
			ResetClass();

			this.Command = intCommand;

			foreach (string sTemp in sDatas)
			{
				this.Data_List.Add(sTemp);
			}

			//입력된 데이터를 변환 한다.
			this.CreateDataSend();

		}

		/// <summary>
		/// 지정한 리스트의 내용을 구분자를 이용하여 한줄로 만든다.
		/// </summary>
		/// <param name="listString"></param>
		/// <param name="sDivision"></param>
		/// <returns></returns>
		private string ListToString(List<string> listString, string sDivision)
		{
			if( 0 >= listString.Count)
			{	//리스트에 내용이 없으면 빈값을 준다.
				return null;
			}

			//리스트에 내용이 있으면 구분자를 이용하여 한줄로 만든다.
			StringBuilder sbReturn = new StringBuilder();
			//리스트의 내용을 한줄로 만든다.
			for(int i = 0; i < listString.Count; ++i)
			{ 
				sbReturn.Append(listString[i]);
				if (i < listString.Count - 1)
				{
					sbReturn.Append(sDivision);
				}
			}

			return sbReturn.ToString();
		}

		

		/// <summary>
		/// 명령어를 바이트 형태로 바꾼다.
		/// </summary>
		/// <returns></returns>
		private byte[] ByteToCommand()
		{
			//여기의 크기는 'CSocketGlobal.CommandSize'로 결정함
			return DGU_CSocket.Util_Buffer.StringToByte(
					string.Format("{0:D4}", this.Command));
		}

		/// <summary>
		/// 데이터오리지널을 이 클래스로 만들어 줍니다.
		/// </summary>
		/// <param name="dataOri"></param>
		public void DataOriginalToThis(CSendData_Original dataOri)
		{
			this.DataOriginalToThis(dataOri.Bady);
		}

		/// <summary>
		/// 데이터오리지널(바이트형태)을 이클래스로 만들어 줍니다.
		/// </summary>
		/// <param name="byteOri"></param>
		private void DataOriginalToThis(byte[] byteOri)
		{
			byte[] byteTemp;

			//명령어를 잘라 붙여 넣는다.
			byteTemp = new byte[DGU_CSocket.Socket_Fix_Data.CommandSize];
			//명령어 복사
			Buffer.BlockCopy(byteOri
								, 0
								, byteTemp
								, 0
								, DGU_CSocket.Socket_Fix_Data.CommandSize);
			//명령어로 변환
			this.f_nCommand = DGU_CSocket.Util_Buffer.ByteToInt(byteTemp);

			//데이터 복사
			this.Data_Byte = new byte[byteOri.Length - DGU_CSocket.Socket_Fix_Data.CommandSize];
			Buffer.BlockCopy(byteOri
								, DGU_CSocket.Socket_Fix_Data.CommandSize
								, this.Data_Byte
								, 0
								, byteOri.Length - DGU_CSocket.Socket_Fix_Data.CommandSize);
		}

		/// <summary>
		/// 이 클래스를 오리지널 클래스로 바꿔줍니다.
		/// </summary>
		/// <returns></returns>
		public CSendData_Original CreateDataOriginal()
		{
			if ((null == Data_Byte)
				|| (0 >= Data_Byte.Length))
			{	//문자열 데이터다.
				return CreateDataOriginal_String();
			}
			else
			{	//바이트 데이터다.
				return CreateDataOriginal_Byte();
			}
		}

		/// <summary>
		/// 문자열을 오리지널 클래스로 바꿉니다.
		/// </summary>
		/// <returns></returns>
		private CSendData_Original CreateDataOriginal_String()
		{
			//입력된 데이터를 변환 한다.
			this.CreateDataSend();

			//바이트 처리를 한다.
			return CreateDataOriginal_Byte();
		}

		/// <summary>
		/// 바이트를 오리지널 클래스로 바꿉니다.
		/// </summary>
		/// <returns></returns>
		private CSendData_Original CreateDataOriginal_Byte()
		{
			//리턴할 보내기용 원본 데이터
			CSendData_Original dataReturn = new CSendData_Original();
			//데이터 공간 확보
			dataReturn.Bady = new byte[DGU_CSocket.Socket_Fix_Data.CommandSize + this.Data_Byte.Length];

			//명령어 복사
			Buffer.BlockCopy(ByteToCommand()
								, 0
								, dataReturn.Bady
								, 0
								, DGU_CSocket.Socket_Fix_Data.CommandSize);
			//데이터 복사
			Buffer.BlockCopy(this.Data_Byte
								, 0
								, dataReturn.Bady
								, DGU_CSocket.Socket_Fix_Data.CommandSize
								, this.Data_Byte.Length);

			//헤드를 만듭니다.
			dataReturn.SetHead();

            return dataReturn;
		}

		/// <summary>
		/// 입력된 데이터를 한줄짜리 데이터로 변환 합니다.
		/// </summary>
		private void CreateDataSend()
		{
			//문자열리스트를 한줄짜리 문자열로 변환한다.
			string sResult
				= this.ListToString(this.Data_List
									, DGU_CSocket.Socket_Fix_Data.Division1.ToString());

			if (null != sResult)
			{	//리스트에 내용이 있을 때만 리스트결과를 쓴다.
				this.Data_String = sResult;
			}

			//문자열 데이터를 바이트로 변환 해준다.
			this.Data_Byte = DGU_CSocket.Util_Buffer.StringToByte(this.Data_String);
		}


	}
}
