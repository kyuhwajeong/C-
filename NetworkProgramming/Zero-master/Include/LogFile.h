#pragma once
#include "String.h"

namespace Zero
{
	enum eLogPriority
	{
		LOG_MSG = 0,
		LOG_EVT,
		LOG_LOW,
		LOG_MED,
		LOG_HIG,
		LOG_CRI,
		LOG_MAX
	};

	class LogFile
	{
	public:
		LogFile();
		virtual ~LogFile();

		void FileSave(String Filename,const TCHAR *pszParam, ...);		// Ư������ ������ ���
		void Save(eLogPriority Priority, const TCHAR *pszParam, ...);	// �α׳���(����)
		void Write(const TCHAR *pszParam, ...);							// �α׳���(�����)

		void StartWindow(int sx, int sy, int wid, int hei);
		void SetWindowOpen(bool bOpen);
		int  Win(TCHAR* pszParam, ...);
		void SetMsgBox(bool bTrue);


	protected:
		bool	m_bOpen;
		bool	m_bStartWindow;
		HWND	m_hwnd;
		HWND	m_hwndList;
		String	m_szFolder;
		bool	m_bMsgBox;

		void SystemLog();
		//void CreateLogWindow(int sx, int sy, int wid, int hei);

	private:
		String	m_FileName[LOG_MAX];
		String	m_Folder;
		String	m_SaveFileName;
		bool	m_bSavedFile;		// ���� ���� ����(���� 1ȸ�� ���� ����)

		bool WriteLog( String FileName, String LogText );
	};
}








