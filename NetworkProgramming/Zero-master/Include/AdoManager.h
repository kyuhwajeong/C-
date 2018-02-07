#pragma once
#include "Ado.h"

/**
����

- CAdoManager�� �̿��Ͽ� Ŀ�ؼ� Ǯ�� �̸� �����صΰ� �ʿ��Ҷ����� CScopedAdo�� �̿��� ����Ѵ�

@code sample

	// ---------------------------------------------------------
	// -------------    ���� �غ� �۾�    --------------------
	// ---------------------------------------------------------
	// ������ ���� ���Ǳ� ���� ����
	Zero::StartOptionAdo start_option;

	Zero::String strIP;
	int nPort = 1234;
	strIP.Format( L"%s,%d",  L"127.0.0.1", nPort );
	start_option.SetParam(strIP, "�������̵�", "������ȣ", "DB�̸�");

	try
	{
		Zero::AdoManager pAdoManager(new Zero::CAdoManager(start_option));
		if( pAdoManager->IsOpenFail() )
		{
			// �⺻ ���� ����
			return;
		}
	}
	catch(Zero::DBException& e)
	{
		// ����
	}
	// ---------------------------------------------------------


	// ---------------------------------------------------------
	// -------------    Query ����   --------------------
	// ---------------------------------------------------------
	Zero::CAdo* pAdo = NULL;
	try
	{
		Zero::CScopedAdo scopedado(pAdo, pAdoManager);
		if( !pAdo->IsSuccess() ) return 0;

		pAdo->SetQuery(_T("SELECT uuid FROM Userinfo"));
		pAdo->Execute(adCmdText);
		if( !pAdo->IsSuccess() ) return 0;

		Zero::Guid outGuid;	// �о�� ��

		while(!pAdo->GetEndOfFile())
		{
			short cnt = 0;
			DBFAIL_THROW(pAdo->GetFieldValue(cnt++, outGuid));

			pAdo->MoveNext();
		}
		if( !pAdo->IsSuccess() ) return 0;
	}
	catch(Zero::DBException& e)
	{
		Zero::ErrMsgA( e.what() );	// ����
		return 0;
	}
	return 1;
	// ---------------------------------------------------------


	// ---------------------------------------------------------
	// -------------    SP ����   --------------------
	// ---------------------------------------------------------
	Zero::String Proc = _T("���ν����̸�");
	Zero::CAdo* pAdo = NULL;

	try
	{
		Zero::CScopedAdo scopedado(pAdo, pAdoManager);

		// �Է½�ų �Ķ���� ����
		Zero::String strID = L"QWE";
		Zero::String strPW = L"asdzxc";

		pAdo->CreateParameter(_T("return"),adInteger, adParamReturnValue, 0);
		pAdo->CreateParameter(_T("@v_name"),adVarChar, adParamInput, strID );
		pAdo->CreateParameter(_T("@v_pass"),adVarChar, adParamInput, strPW );
		pAdo->CreateParameter(_T("@v_out"),adInteger, adParamOutput, 0);
		if( !pAdo->IsSuccess() ) return 0;

		// SPȣ��
		pAdo->SetQuery(Proc);
		pAdo->Execute();
		if( !pAdo->IsSuccess() ) return 0;

		// �ʵ� �о����
		Zero::Guid testGuid;
		Zero::String readID, readPW;
		while(!pAdo->GetEndOfFile())
		{
			short cnt = 0;
			DBFAIL_THROW(pAdo->GetFieldValue(cnt++, testGuid));
			DBFAIL_THROW(pAdo->GetFieldValue(cnt++, readID));
			DBFAIL_THROW(pAdo->GetFieldValue(cnt++, readPW));

			pAdo->MoveNext();
		}
		if( !pAdo->IsSuccess() ) return 0;

		// ���� ���ڵ�
		pAdo->NextRecordSet();
		if( !pAdo->IsSuccess() ) return 0;

		// �ʵ� �о����
		int readValue = 0;
		while(!pAdo->GetEndOfFile())
		{
			DBFAIL_THROW(pAdo->GetFieldValue(L"aaa", readValue));
			pAdo->MoveNext();
		}
		if( !pAdo->IsSuccess() ) return 0;


		// ���ϰ� �о����
		int nRtnParam = 0;
		pAdo->GetParameter(_T("return"), nRtnParam);


		// output�Ķ������ ��� �о����
		int nOutParam = 0;
		pAdo->GetParameter(_T("@v_out"), nOutParam);
	}
	catch(Zero::DBException& e)
	{
		// ����
	}
	// ---------------------------------------------------------
	// ---------------------------------------------------------


@sp code
	USE [AdoTest]
	GO
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER OFF
	GO

	ALTER  PROC [dbo].[InsertUser]
		@arg_name			NVARCHAR(50),
		@arg_pass			NVARCHAR(50),
		@arg_aaa            INT				OUTPUT
	AS
	BEGIN
		SET NOCOUNT ON
		SET XACT_ABORT ON

		DECLARE @v_UUID		uniqueidentifier
		Set @v_UUID = NEWID()

		INSERT INTO dbo.Userinfo values(@v_UUID, @arg_name, @arg_pass)

		SELECT * FROM Userinfo where uuid = @v_UUID

		SELECT top 1 aaa FROM AdoTest
		
		SET @arg_aaa = 123678
		
		return 321
	END

*/
namespace Zero
{
	class CAdoManager;
	typedef std::shared_ptr<CAdoManager> AdoManager;

	class CAdoManager
	{
		enum{ MAX_ARRAY_SIZE=100 };

		class CriticalSection
		{
		public:
			CriticalSection()	{ InitializeCriticalSection(&cs); }
			~CriticalSection()	{ DeleteCriticalSection(&cs); }
			void Lock()			{ EnterCriticalSection(&cs); }
			void Unlock()		{ LeaveCriticalSection(&cs); }
		private:
			CRITICAL_SECTION cs;
		};

		class ScopedLock
		{
		public:
			ScopedLock(CriticalSection &cs):m_cs(cs){ m_cs.Lock(); }
			~ScopedLock(){ m_cs.Unlock(); }
		private:
			CriticalSection &m_cs;
		};
		
	public:
		explicit CAdoManager(StartOptionAdo& start_option);
		~CAdoManager();

		void PutAdo(CAdo* pAdo);
		CAdo* GetAdo();
		StartOptionAdo GetConfigAdo() { return m_start_option; }

		int GetPoolCount() { return m_nTopPos+1; }	// �����ִ� ADOǮ ���� (+1�ϴ� ������ 0���ʹϱ� : 0=1��)

		bool IsOpenFail() { return m_bOpenFail; }

	private:
		int m_count_new;
		bool m_bOpenFail;
		int m_nTopPos;
		int m_nMaxAdo;
		CAdo* m_pAdoStack[MAX_ARRAY_SIZE];
		StartOptionAdo m_start_option;
		CriticalSection m_Lock;
	};

	/**
	��ü ������ Ŀ�ؼ�Ǯ�κ��� ADO��ü�� ���� �� �Ҹ�� ADO��ü�� Ŀ�ؼ�Ǯ�� �����ش�.

	- Ǯ���� �����µ� �����ϴ� ��� �Ｎ���� ���� ��ü�� �Ҵ��Ͽ� ����Ѵ�

	@param bAutoCommit	�ΰ���� ����� Ʈ�����

	*/
	class CScopedAdo
	{
	public:
		explicit CScopedAdo(CAdo* &pAdo, Zero::AdoManager pAdoManager, bool bAutoCommit = false);
		~CScopedAdo();

		bool IsError() { return m_bErrorOpen; }

		int SetManualMode();	// ���� ���� Ado Pool ������ �����Ѵ�
		void Release();

	private:
		Zero::AdoManager m_pAdoManager;
		CAdo* m_pAdo;
		bool m_bErrorOpen;
		bool m_bManualRelease;
		bool m_bReleased;
	};
}






