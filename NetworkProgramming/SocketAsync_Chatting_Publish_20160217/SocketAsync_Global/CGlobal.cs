using DGU_Socket;

namespace SocketGlobal
{

	public enum ChatType
	{
		Send,
		Receive,
		System
	}

	public class CGlobal
	{
		public static string g_SiteTitle = "Socket - SocketAsyncEventArgs";
		
		public static CCommand g_Command = new CCommand();


		static CGlobal()
		{
			//소켓에 사용하는 기본 정보를 입력합니다.

			DGU_CSocket.Socket_Fix_Data.Division1 = '▒';
			DGU_CSocket.Socket_Fix_Data.Division2 = ',';
			DGU_CSocket.Socket_Fix_Data.Division3 = '▦';
			DGU_CSocket.Socket_Fix_Data.Division_Table = '▧';
			DGU_CSocket.Socket_Fix_Data.Division_Row = ',';
			DGU_CSocket.Socket_Fix_Data.Division_RowItem = '▦';

			DGU_CSocket.Socket_Fix_Data.DataHeader1Size = 4;
			DGU_CSocket.Socket_Fix_Data.DataHeader2Size = 4;
			DGU_CSocket.Socket_Fix_Data.Buffer_Basic = 4;
			DGU_CSocket.Socket_Fix_Data.Buffer_Full = 8192;//소켓 기본 버퍼크기가 이만큼이다.

			DGU_CSocket.Socket_Fix_Data.CommandSize = 4;

			
		}
	}
}
