using System.Text;

namespace DGU_Encoding
{
	public class CEncoding
	{
		public string ByteToString(byte[] byteData, int index, int count)
		{
			return Encoding.UTF8.GetString(byteData, index, count);
		}
		public string ByteToString(byte[] byteData)
		{
			return this.ByteToString(byteData, 0, byteData.Length);
		}
		public byte[] StringToByte(string sData)
		{
			return Encoding.UTF8.GetBytes(sData);
		}
	}
}
