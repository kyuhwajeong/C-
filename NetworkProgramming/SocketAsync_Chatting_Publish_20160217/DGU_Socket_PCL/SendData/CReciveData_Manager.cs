using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DGU_Socket.SendData
{
	#region 연결할 델리게이트
	#endregion

	public class CReciveData_Manager
	{
		/*
		 * SocketAsyncEventArgs를 이용하여 
		 * (1) 버퍼보다 큰 데이터를 보내게
		 * (2) 샌드를 여러번 하게 되면
		 * 리시브가 여러번 온다.
		 * 리시브가 오면 이 클래스에서 저장해두었다가 해더 크기가 많큼 잘라서 처리해준다.
		 */

		#region 연결할 이벤트 ♥♥♥♥♥♥♥♥♥♥♥♥
		#endregion

		/// <summary>
		/// 리시브온 데이터는 1차로 이곳에 쌓아둡니다.
		/// </summary>
		//private List<CRecive_Data> Buffer { get; set; }
		public List<CRecive_Data> Buffer { get; set; }

		/// <summary>
		/// 원본에서 잘라낸 데이터 임시저장
		/// </summary>
		private CReciveTemp_Data m_Temp { get; set; }

		/// <summary>
		/// 완성된 리시브 데이터 리스트
		/// </summary>
		public List<CReciveTemp_Data> ReciveData_Complet { get; private set; }


		public CReciveData_Manager()
		{
			this.Buffer = new List<CRecive_Data>();
			this.m_Temp = new CReciveTemp_Data();
			this.ReciveData_Complet = new List<CReciveTemp_Data>();
		}

		/// <summary>
		/// 리시브된 데이터를 추가합니다.
		/// </summary>
		/// <param name="byteRecive"></param>
		public void Buffer_Add(byte[] byteRecive, int nLength)
		{
			this.Buffer.Add(new CRecive_Data(
				DGU_Array.CByte.Remove_Right(byteRecive
											, byteRecive.Length - nLength)));
		}

		public void ReceiveData_Check()
		{
			bool bLoop = true;

			while(true == bLoop)
			{
				bLoop = this.ReceiveData_Check_One();
			}


		}

		/// <summary>
		/// 가지고 있는 리시브데이터를 가지고 완성된 데이터가 있는지 확인한다.
		/// </summary>
		/// <returns></returns>
		private bool ReceiveData_Check_One()
		{
			bool bHead = false;
			bool bBady = false;

			//1. 이미 완성된 헤더가 있는지 확인한다.
			if ( 0 < this.m_Temp.Size)
			{//이미 완성된 헤더가 있다.
				bHead = true;
            }
			else
			{//완성된 헤더가 없다.
			 //헤더를 만든다.
				bHead = this.Cut_Head();
            }

			//2. 헤더를 기준으로 바디를 만든다.
			if(true == bHead)
			{//헤더가 있을때만 처리
			 //바디를 만든다.
				bBady = this.Cut_Bady();
            }

			//3. 남은 데이터를 한번더 확인해 본다.
			//오버플로 나서 for문으로 바꿈
			/*if(true == bBady)
			{//완성된 바디가 있다.
			 //완성된 바디가 있다면 한번더 받아온 데이터를 확인한다.
				this.ReceiveData_Check();

				//재귀함수로 구현하면 더이상 데이터를 만들수 없으면 완료된다.
            }*/

			return bBady;
        }

		#region 임시데이터 만들기
		/// <summary>
		/// 데이터에서 맨앞 'DataHeader1Size'만큼 잘라서 리시브의 크기로 변환하고
		/// 'CReciveTemp_Data'를 생성해서 리스트에 추가한다.
		/// </summary>
		/// <returns>충분한 데이터가 있는지 여부</returns>
		private bool Cut_Head()
		{
			//충분한 데이터가 있는지 여부
			bool bReturn = false;

			int nBuffer = 0;
			int nCount = 0;
			for (int i = 0; i < this.Buffer.Count; ++i)
			{
				//가지고 있는 사이즈를 더해준다.
				nBuffer += this.Buffer[i].Length;
				if (DGU_CSocket.Socket_Fix_Data.DataHeader1Size <= nBuffer)
				{//가지고 있는 데이터가 헤더만큼 있다.
				 //몇개를 합친건지 기록하고(인덱스라 0부터 시작이니 1을 더해준다.)
					nCount = i + 1;
					//충분한 데이터가 있다.
					bReturn = true;
					break;
				}
			}

			if (true == bReturn)
			{//충분한 데이터가 있으면 임시 리시브데이터를 만든다.
				this.m_Temp = new CReciveTemp_Data();
				byte[] byteTemp;
				int nCountTemp = DGU_CSocket.Socket_Fix_Data.DataHeader1Size;

				//데이터를 임시 리시브데이터에 입력한다.
				for (int i = 0; i < nCount; ++i)
				{
					//버퍼만큼 데이터를 자르고
					byteTemp = this.Buffer[i].Cut(nCountTemp);
					//계산용 임시 카운터에 자른만큼 카운터를 빼주고
					nCountTemp -= byteTemp.Length;
					//임시리시브데이터에 데이터를 추가한다.
					this.m_Temp.Add_Head(byteTemp);

					//'nCountTemp'가 0이 되면 for문도 조건이 완료될테니
					//따로 처리하지 않는다.
				}

				//헤더를 데이터 사이즈로 변환 한다.
				this.m_Temp.HeadToLength();
			}

			//4. 비어있는 버퍼를 지운다.
			this.Buffer_Organize();

			return bReturn;
		}

		private bool Cut_Bady()
		{
			//충분한 데이터가 있는지 여부
			bool bReturn = false;

			int nBuffer = 0;
			int nCount = 0;
			for (int i = 0; i < this.Buffer.Count; ++i)
			{
				//가지고 있는 사이즈를 더해준다.
				nBuffer += this.Buffer[i].Length;
				if ( this.m_Temp.Size <= nBuffer)
				{//가지고 있는 데이터가 헤더만큼 있다.
				 //몇개를 합친건지 기록하고
					nCount = i + 1;
					//충분한 데이터가 있다.
					bReturn = true;
					break;
				}
			}

			if (true == bReturn)
			{//충분한 데이터가 있다.
				
				//임시 버퍼를 만들고
				byte[] byteTemp;
				int nCountTemp = this.m_Temp.Size;

				//데이터를 임시 리시브데이터에 입력한다.
				for (int i = 0; i < nCount; ++i)
				{
					//버퍼만큼 데이터를 자르고
					byteTemp = this.Buffer[i].Cut(nCountTemp);
					//계산용 임시 카운터에 자른만큼 카운터를 빼주고
					nCountTemp -= byteTemp.Length;
					//임시리시브데이터에 데이터를 추가한다.
					this.m_Temp.Add_Bady(byteTemp);

					//'nCountTemp'가 0이 되면 for문도 조건이 완료될테니
					//따로 처리하지 않는다.
				}

				//데이터를 모두 옮겼으면 임시저장소에 있는 데이터를 리스트로 옮긴다.
				this.ReciveData_Complet.Add(this.m_Temp);
				//임시 저장소는 초기화 한다.
				this.m_Temp = new CReciveTemp_Data();
			}

			//4. 비어있는 버퍼를 지운다.
			this.Buffer_Organize();

			return bReturn;
		}

		private void Buffer_Organize()
		{
			//내용이 비어있는 버퍼를 찾아 지운다.
			this.Buffer.RemoveAll(x => x.Length == 0);
		}

		#endregion

		/// <summary>
		/// 'ReciveData_Complet'에서 사용한 항목을 정리합니다.
		/// </summary>
		public void CompletList_Organize()
		{
			this.ReciveData_Complet.RemoveAll(x => x.Use == true);
        }

	}
}
