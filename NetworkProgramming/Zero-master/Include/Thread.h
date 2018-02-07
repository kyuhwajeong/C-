#pragma once

namespace Zero
{
	class CThread
	{
	public:
		CThread();
		virtual ~CThread();

		bool Start();
		void Stop();
		
		virtual void OnProcess(CThread* p) = 0;		// Stop��� ���������� �ڵ����� ��� ����Ǵ� �Լ�

	private:
		HANDLE hThread;
		UINT ThreadID;
		bool bClose;
		bool bStart;

		static unsigned int WINAPI Loop(LPVOID p);
	};


	class CControlThread
	{
	public:
		CControlThread();
		virtual ~CControlThread();

		bool	CreateThread( DWORD dwTimeMS );		// ������ ����, ó���ѹ��� ���� -dwTimeMS ������ ����� �ð� MS����
		void	DestroyThread();					// ������ ����
		void	Run();								// ������ ������ �����尡 ���۽��� ȣ��
		void	Stop();								// �۵� ���� (���۵� -> Run()�Լ� ���)

		inline bool	IsRun() { return m_bIsRun; }	// ���� �����尡 ����������

		// ������ ���� ī��Ʈ (OnProcess���� ȸ��)
		inline DWORD GetTickCount() { return m_dwTickCount; }

	private:
		HANDLE	m_hThread;
		HANDLE	m_hQuitEvent;
		bool	m_bIsRun;
		DWORD	m_dwWaitTickMS;
		DWORD	m_dwTickCount;

		static unsigned int WINAPI CallTickThread(LPVOID p);
		void	TickThread();

		virtual void	OnProcess() = 0;
	};
}

