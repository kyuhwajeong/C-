using System;
using DGU_String;

namespace SocketGlobal
{
	/// <summary>
	/// 소켓에 사용될 명령어
	/// </summary>
	public class CCommand
	{
		public enum Command
		{
			/// <summary>
			/// 기본 상태
			/// </summary>
			None = 0,
			/// <summary>
			/// 아이디 체크(C→S)
			/// </summary>
			ID_Check,
			/// <summary>
			/// 아이디 체크 성공(C←S)
			/// </summary>
			ID_Check_Ok,
			/// <summary>
			/// 아이디 체크 실패(C←S)
			/// </summary>
			ID_Check_Fail,

			/// <summary>
			/// 접속한 유저가 있다.(C←S)
			/// </summary>
			User_Connect,
			/// <summary>
			/// 접속을 끊은 유저가 있다.(C←S)
			/// </summary>
			User_Disonnect,
			/// <summary>
			/// 유저 리스트를 보냅니다.(C←S)
			/// </summary>
			User_List,
			/// <summary>
			/// 유저 리스트를 갱신을 요청 합니다.(C→S)
			/// </summary>
			User_List_Get,

			/// <summary>
			/// 아이디 무결성이 확인 된 후 호출함(C→S)
			/// </summary>
			Login,
			/// <summary>
			/// 서버에서 모든 로그인 과정이 완료됨.(C←S)
			/// </summary>
			Login_Complete,
			/// <summary>
			/// 로그아웃(C→S)
			/// </summary>
			Logout,

			/// <summary>
			/// 이미지(C↔S)
			/// </summary>
			Image,

			/// <summary>
			/// 메시지 전송(C↔S)
			/// </summary>
			Msg,
		}

		/// <summary>
		/// 문자열로된 숫자를 명령어 타입으로 바꿔줍니다.
		/// 입력된 문자열이 올바르지 않다면 기본상태를 줍니다.
		/// </summary>
		/// <param name="sData"></param>
		/// <returns></returns>
		public Command StrIntToType(string sData)
		{
			//넘어온 명령
			CCommand.Command typeCommand = CCommand.Command.None;

			if (true == CNumber.IsNumeric(sData))
			{
				//입력된 명령이 숫자라면 명령 타입으로 변환한다.
				//입력된 명령이 숫자가 아니면 명령 없음 처리(기본값)를 한다.
				typeCommand = (CCommand.Command)Convert.ToInt32(sData);
			}

			return typeCommand;
		}

	}
}
