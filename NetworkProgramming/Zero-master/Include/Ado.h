#pragma once
#include <atlcomtime.h>
#include "String.h"

//#define DEF_MAKE_ADO
//#import "C:\Program Files\Common Files\System\ADO\msado15.dll" rename("EOF", "EndOfFile") no_namespace
#ifdef DEF_MAKE_ADO
	#import "C:\Program Files\Common Files\System\ADO\msado15.dll" no_namespace rename("EOF", "EndOfFile")rename("BOF","adoBOF")
#else
	#include "msado15.tlh"
#endif

#ifdef _UNICODE
#define DBFAIL_THROW(a)\
	if(!(a))\
	{\
		Zero::String str;\
		str.Format( _T("FAIL : %s, %s"), __FUNCTIONW__, (LPCWSTR)CA2W(#a) );\
		throw Zero::DBException( str );\
	}

#else
#define DBFAIL_THROW(a)\
	if(!(a))\
	{\
		Zero::String str;\
		str.Format(_T("FAIL : %s, %s"), __FUNCTIONW__, #a);\
		throw Zero::DBException(str);\
	}
#endif


namespace Zero
{
	class DBException : public CException
	{
	public:
		DBException(const TCHAR* msg);
		DBException(const TCHAR* msg,_com_error &e);
	};

	struct StartOptionAdo
	{
	public:
		StartOptionAdo();

		DECLARE_FullAccess(String,	InitCatalog,		m_strInitialCatalog);
		DECLARE_FullAccess(String,	UserID,				m_strUserID);
		DECLARE_FullAccess(String,	Password,			m_strPassword);
		DECLARE_FullAccess(int,		CommandTimeout,		m_nCommandTimeout);
		DECLARE_FullAccess(int,		ConnectionTimeout,	m_nConnectionTimeout);
		DECLARE_FullAccess(bool,	RetryConnection,	m_bRetryConnection);
		DECLARE_FullAccess(int,		MaxConnectionPool,	m_nMaxConnectionPool);


		/**
		timeout : Ÿ�Ӿƿ�(�ʴ���)
		retry : �翬�� ���

		poolsize : db Ŀ�ؼ� Ǯ ����(�ּ�1���̻�)
		provider : MSSQL = "SQLOLEDB.1", MySQL = "MSDASQL.1"

		*/
		void SetParam(String ip, String userID, String password, String catalog, int timeout=3, bool retry=true, int poolsize=20, String provider = _T("SQLOLEDB.1"));

		String GetConnectionString();
		String& GetProvider();
		String& GetDSN();

	private:
		String m_strConnectingString;
		String m_strInitialCatalog;
		String m_strDataSource;
		String m_strUserID;
		String m_strPassword;
		String m_strProvider;
		String m_strDSN;

		int m_nConnectionTimeout;
		int m_nCommandTimeout;
		bool m_bRetryConnection;
		int m_nMaxConnectionPool;

		void SetIP(String pString);
		void SetDSN(String pString);
		void SetProvider(String pString);
	};



	class CAdo
	{
	public:
		CAdo(StartOptionAdo&);
		~CAdo();

		void Init();
		BOOL Open(CursorLocationEnum CursorLocation=adUseClient);
		void Close();
		void Release();		// Ŀ�ؼ�Ǯ���� �����ϱ� ���� Ŀ�ǵ� ��ü �����
		void SetQuery(IN const TCHAR* tszQuery);
		void SetConnectionMode(ConnectModeEnum nMode);
		void ManualScopeRelease();

		DECLARE_FullAccess(bool, AutoCommit, m_bAutoCommit);
		DECLARE_FullAccess(BOOL, Success, m_IsSuccess);


		// ����� Ʈ����� ��� - CScopedAdo ����/�Ҹ�� �ʿ�(CScopedAdo����)
		void BeginTransaction();
		void CommitTransaction();
		void RollbackTransaction();

		BOOL IsSuccess();
		void SetCommit(BOOL bIsSuccess);

		void dump_com_error(_com_error&);
		void dump_user_error();

		BOOL GetFieldCount(int&);
		void MoveNext();
		BOOL GetEndOfFile();
		BOOL NextRecordSet();


		/**
		���ν��� �� SQL Text�� �����Ѵ�.
		- �ΰ���� adCmdStoreProc, adCmdTextó�� ����
		@param		CommandTypeEnum, ExecuteOptionEnum
		@return		����(TRUE) ����(FLASE)
		*/
		BOOL Execute(CommandTypeEnum CommandType = adCmdStoredProc, ExecuteOptionEnum OptionType = adOptionUnspecified);


		/**
		����/�Ǽ�/��¥�ð� �ʵ尪�� �д´�.
		- ���� ���� null�̸� ���и� �����Ѵ�.

		@return		����(TRUE) ����(FLASE)
		@param indexID �÷��̸�(��Ʈ��) �Ǵ� �ε�����ȣ(����)�� ������ ����

		���� : �ε�����ȣ�� ���ڷ� ������� �ݵ�� short������ �Է��Ͽ����Ѵ� 0���ε��� ---> (short)0

		*/
		template<typename T> BOOL GetFieldValue(IN const _variant_t& indexID, OUT T& Value);


		/**
		������ �ʵ尪�� �д´�.
		- ���� ���� null�̰ų� ���۰� �۴ٸ� ���и� �����Ѵ�.
		@param		���� ������ ���� ������ ũ��
		@return		����(TRUE) ����(FLASE)
		*/
		BOOL GetFieldValue(IN const _variant_t& indexID, OUT Zero::String& outString);


		/**
		binary �ʵ尪�� �д´�.
		- ���� ���� null�̰ų� ���۰� �۴ٸ� ���и� �����Ѵ�.
		@param		���� binary�� ���� ������ ũ��, ���� binary ũ��
		@return		����(TRUE) ����(FLASE)
		*/
		BOOL GetFieldValue(IN const _variant_t& indexID, OUT BYTE* pbyBuffer, IN int inSize, OUT int& outSize);


		/**
		Guidó��
		*/
		BOOL GetFieldValue(IN const _variant_t& indexID, OUT Zero::Guid& outGuid);
	

		/**
		����/�Ǽ�/��¥�ð� Ÿ���� �Ķ���� ����
		- null���� �Ķ���� ������ CreateNullParameter�� ���
		*/
		template<typename T> void CreateParam(IN TCHAR* tszName,IN enum DataTypeEnum Type, IN enum ParameterDirectionEnum Direction, IN T rValue);


		/**
		����/�Ǽ�/��¥�ð� Ÿ���� null�� �Ķ���� ����
		*/
		void CreateNullParameter(IN TCHAR*, IN enum DataTypeEnum, IN enum ParameterDirectionEnum);

		
		/**
		���ڿ� Ÿ�� �Ķ���� ����, ���� ������ �ּ� 0���� Ŀ�� �Ѵ�. null�� ������ TCHAR*�� NULL���� �ѱ��.
		*/
		void CreateParam(IN TCHAR* tszName,IN enum DataTypeEnum Type, IN enum ParameterDirectionEnum Direction, IN TCHAR* pValue, IN int nSize);


		/**
		String �Ķ���� ����

		@param nSize : String paramũ�� ����( 0�ΰ�� str�� ũ��� �ڵ� ���� )

		- adParamInput��Ȳ������ nSize���� �������� ��� : ����Ʈ�� 0�ΰ�� �ڵ����� ��Ʈ�� ������� ����

		- adParamOutput �Ǵ� adParamInputOutput�� Output���� ����ϴ°��

		  nSize���� �� �����ؼ� ����ؾ� ��Ʈ���� ©���� �ʰ� �о�ü� �ִ�(SP param���� ���ų� ũ��)

		*/
		void CreateParam(IN TCHAR* tszName,IN enum DataTypeEnum Type, IN enum ParameterDirectionEnum Direction, IN const String& str, IN int nSize = 0);

		/**
		binary Ÿ�� �Ķ���� ����, ���� ������ �ּ� 0���� Ŀ�� �Ѵ�. null�� ������ BYTE*�� NULL���� �ѱ��.
		*/
		void CreateParam(IN TCHAR* tszName,IN enum DataTypeEnum Type, IN enum ParameterDirectionEnum Direction, IN BYTE* pValue, IN int nSize);


		/**
		����/�Ǽ�/��¥�ð� Ÿ���� �Ķ���Ͱ� ����
		- null���� �Ķ���� ������ UpdateNullParameter�� ���
		*/
		template<typename T> void UpdateParameter(IN TCHAR* tszName, IN T rValue);

		// ����/�Ǽ�/��¥�ð� Ÿ���� �Ķ���� ���� null�� ����
		void UpdateNullParameter(IN TCHAR*);

		// ���ڿ� Ÿ�� �Ķ���� ����, ���� ������ �ּ� 0���� Ŀ�� �Ѵ�. null�� ���� TCHAR*�� NULL���� �ѱ��.
		void UpdateParameter(IN TCHAR*, IN TCHAR*, IN int);

		// binary Ÿ�� �Ķ���� ����, ���� ������ �ּ� 0���� Ŀ�� �Ѵ�. null�� ���� BYTE*�� NULL���� �ѱ��.
		void UpdateParameter(IN TCHAR*, IN BYTE*, IN int);

		// ����/�Ǽ�/��¥�ð� Ÿ���� �Ķ���� �� �б�
		template<typename T> BOOL GetParam(TCHAR* tszName, OUT T& Value);


		/**
		������ �Ķ���Ͱ��� �д´�.
		- ���� ���� null�̸� ���и� �����Ѵ�.
		@return		����(TRUE) ����(FLASE)
		*/
		BOOL GetParam(IN TCHAR*, OUT Zero::String&);

		/**
		���̳ʸ��� �Ķ���Ͱ��� �д´�.
		- ���� ���� null�̰ų� ���۰� �۴ٸ� ���и� �����Ѵ�.
		@param		���� ������ ���� ������ ũ��, ���� ������ ũ��
		@return		����(TRUE) ����(FLASE)
		*/
		BOOL GetParam(IN TCHAR*, OUT BYTE*, IN int, OUT int&);


	private:
		_ConnectionPtr m_pConnection;      
		_RecordsetPtr m_pRecordset;
		_CommandPtr m_pCommand;

		String m_strConnectingString;
		String m_strUserID;
		String m_strPassword;
		String m_strInitCatalog;
		String m_strProvider;
		String m_strDSN;
		int m_nConnectionTimeout;
		int m_nCommandTimeout;
		bool m_bRetryConnection;
		bool m_bAutoCommit;

		String m_strQuery;
		BOOL m_IsSuccess;
		String m_strCommand;
		String m_strColumnName;
		String m_strParameterName;

		// �翬�� �ɼ��� ���� ��� �翬�� �õ�
		BOOL RetryOpen();

		CAdo(const CAdo&);
		CAdo& operator= (const CAdo&);
	};

}

#include "Ado.inl"





