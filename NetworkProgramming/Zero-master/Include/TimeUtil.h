#pragma once
#pragma comment(lib,"winmm.lib")

namespace Zero
{
	class CTIME
	{
	public:
		CTIME();
		~CTIME();

		bool GetPrecision(DWORD dwTimeMS);		// ���нð����(��������������)
		bool Get(DWORD dwTimeMS);				// �ð����
		void Reset();
		DWORD GetTimeBack(DWORD dwTimeMS);		// �����ִ½ð� ���ϱ�

	private:
		DWORD m_dwBacksettime;
		DWORD m_dwNowsettime;
	};

	class CFPS
	{
	public:
		CFPS();
		~CFPS();

		float Get();

	private:
		bool  m_bUpdate;
		DWORD m_dwStartTime, m_dwFrame;
		float m_fFPS;
	};


	/**
	{
		Zero::AutoTimer t;		// Ÿ�̸� ����
		Zero::String dummy;
		DBRECV->Exec_Login( pParam->idName, pParam->password, dummy );
		DWORD tt = t.Get();		// ���ۺ��� ��������� �����ð�
	}
	*/
	class AutoTimer
	{
	private:
		DWORD m_Time;
	public:
		AutoTimer();
		void Reset();
		DWORD Get();
	};


	class TUtil
	{
	public:

		// timeGetTime��
		static DWORD GetTime();


		// time_t -> Int64������ ��ȯ
		static void MakeTime2Int64( const time_t& t_value, Int64& outputValue );


		// MakeTime2Int64�Լ��� ������� Int64 ���� ������ time_t������ �����Ѵ�
		static void RestoreInt64_2Time( const Int64& makedValue, time_t& outputValue );


		// �ð� �˾Ƴ��� @param nAfterTimeSec �̷��� �ð�(60�̸� 60�� ���� �ð��� �ǹ�)
		static void GetTime_Value(time_t& outputTime, const int& nAfterTimeSec=0);


		// �ð� �˾Ƴ��� (��ð���/��е�...)
		static void GetTime_AfterHour(time_t& outputTime, const int& nAfterTimeHour);
		static void GetTime_AfterMin(time_t& outputTime, const int& nAfterTimeMin);


		// Ư���ð����� Ư���ð��� �Ǳ���� ���ʰ� ���Ҵ��� �˾Ƴ��� : 0���� �������� ���ϵǸ� �����ð��� �ǹ���
		static void GetRemainTime(const time_t& Time1, const time_t& Time2, int& outputTime);
	};
}


