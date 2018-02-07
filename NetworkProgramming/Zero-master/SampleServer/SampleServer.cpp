#include "stdafx.h"
#include <iostream>
#include <conio.h>
#include <future>
#include <functional>

//#define USE_EXAMPLE_ASYNC_QUERY	// �񵿱� Query ���� : ��Ƽ�������϶����� remote������ ������ ������


// RMI Stub ���ٻ������ �ʰ� Ŭ���� �������̵� ������� ó���ϱ�
//#define USE_RMI_OVERRIDE_MODE


#include "../SampleCommon/Sample_proxy.h"
#include "../SampleCommon/Sample_proxy.cpp"
#include "../SampleCommon/Sample_stub.h"
#include "../SampleCommon/Sample_stub.cpp"


#ifdef USE_RMI_OVERRIDE_MODE
class C2SStub : public Rmi::Stub
{
public:
	Stub_Rmi_request_message_override;
};
C2SStub g_C2SStub;
Rmi::Proxy g_S2CProxy;


Stub_Rmi_request_message(C2SStub)
{
	printf_s("recved remote[%d] : %s\n", remote, CW2A(msg));
	g_S2CProxy.reponse_message(remote, Zero::CPackOption::Basic, testClass, dic_test, msg);

#ifdef USE_EXAMPLE_ASYNC_QUERY
	Svr.ExampleSQL_QueryAsyncRemote(remote);
#endif
	return true;
}
#endif


class CSampleServer : public Zero::IEventServer
{
public:
	#ifndef USE_RMI_OVERRIDE_MODE
	Rmi::Proxy proxy;
	Rmi::Stub stub;
	#endif
	Zero::CoreServerPtr	m_Core;

	#ifdef USE_EXAMPLE_ASYNC_QUERY
	Zero::AdoManager m_SQL;
	#endif
	
	bool Start(OUT Zero::CResultInfo &result)
	{
		m_Core = Zero::CoreServerPtr(Zero::ICoreServer::NewCore(this));
		#ifdef USE_RMI_OVERRIDE_MODE
		m_Core->Attach(&g_S2CProxy, &g_C2SStub);
		#else
		m_Core->Attach(&proxy, &stub);
		#endif
		

		Zero::CStartOption option;
		option.m_LogicThreadCount = 8;
		option.m_IoThreadCount = 8;

		return m_Core->Start(option, result);
	}

	void DisplayStatus();

	virtual void OnClientJoin(const Zero::RemoteID remote, const Zero::NetAddress& addr, Zero::ArrByte move_server, Zero::ArrByte move_param) override
	{
		printf_s("Join remote[%d]  addr[%s:%d]\n", remote, addr.m_ip.c_str(), addr.m_port);
	}
	virtual void OnClientLeave(const Zero::RemoteID remote, const bool bMoveServer) override
	{
		printf_s("Leave remote[%d]\n", remote);
	}

	virtual void OnInformation(const Zero::CResultInfo& info) override
	{
		printf_s("info : %s\n", CW2A(info.msg));
	}
	virtual void OnException(Zero::CException& e) override
	{
		printf_s("exception : %s\n", e.what());
	}


	// ���ẹ�� ���� �̺�Ʈ


	#ifdef USE_EXAMPLE_ASYNC_QUERY
	void ExampleSQL_Open();
	void ExampleSQL_QueryAsync();
	void ExampleSQL_QueryAsyncRemote(Zero::RemoteID remote);	// ��Ƽ �������϶� �������� ����(remote����)
	void Example_AsyncRequest();
	#endif

};


int _tmain(int argc, _TCHAR* argv[])
{
	CSampleServer Svr;

	Zero::CResultInfo result;
	if (Svr.Start(result))
	{
		printf_s("server start ok\n");
	}
	else
	{
		printf_s("server start error [%s]\n", CW2A(result.msg));
		_getch();
		return 0;
	}

	#ifndef USE_RMI_OVERRIDE_MODE
	Svr.stub.request_message = [&]Func_Rmi_request_message
	{
		printf_s("recved remote[%d] : %s\n", remote, CW2A(msg));
		Svr.proxy.reponse_message(remote, Zero::CPackOption::Basic, testClass, dic_test, msg);

		#ifdef USE_EXAMPLE_ASYNC_QUERY
		Svr.ExampleSQL_QueryAsyncRemote(remote);
		#endif
		return true;
	};
	#endif

	#ifdef USE_EXAMPLE_ASYNC_QUERY
	Svr.ExampleSQL_Open();
	Svr.ExampleSQL_QueryAsync();
	Svr.Example_AsyncRequest();
	#endif
	

	bool run_program = true;
	auto ReadLineAsync = [&run_program]()
	{
		Zero::StringA str;
		char aa[1024];
		std::cin >> aa;
		str = aa;
		return str;
	};

	auto fut = std::async(std::launch::async, ReadLineAsync);
	while (run_program)
	{
		if (fut._Is_ready())
		{
			auto cmd = fut.get();

			if (cmd=="/stat")
			{
				Svr.DisplayStatus();
			}
			else if (cmd=="/q")
			{
				Svr.m_Core->StopStart();
				run_program = false;
			}

			if (run_program)
				fut = std::async(std::launch::async, ReadLineAsync);
		}

		Sleep(10);
	}

	printf_s("Close...");

	Svr.m_Core->Stop();
	printf_s("Complete. press any key to exit\n");
	_getch();
	return 0;
}

void CSampleServer::DisplayStatus()
{
	Zero::CServerState status;
	m_Core->GetCurrentState(status);

	Zero::StringA temp;

	temp.Format(
        "[NetInfo]  Connect/Join %d(%d)/%d  Connect(Server) %d/%d  Accpet/Max %d/%d]\n",

        // ���� ����� client
        status.m_CurrentClient,

        // ���ẹ�� ó�������� client
        status.m_RecoveryCount,

        // ������ ����Ϸ������ client
        status.m_JoinedClient,

        // ������ direct p2p ����� server
        status.m_ServerP2PCount,

        // ������ direct p2p ���� ����͸����� server(������ ���� �ڵ������� ���� ����͸�)
        status.m_ServerP2PConCount,

        // �� ������ �߰� ���� ������ ����
        status.m_nIoAccept,

        // �� ������ �ִ� ���� ������ ����
        status.m_MaxAccept
    );
	printf_s(temp);

	temp.Format(
        "[IO Info]  Close %d  Event %d  Recv %d  Send %d\n",

        // current io close
        status.m_nIoClose,

        // current io event
        status.m_nIoEvent,

        // current io recv socket
        status.m_nIoRecv,

        // current io send socket
        status.m_nIoSend
    );
    printf_s(temp);

	temp.Format(
        "[MemInfo]  Alloc/Instant[%d/%d], test[%s], EngineVersion[%d.%.4d]\n",

        // �̸� �Ҵ�� IO �޸�
        status.m_nAlloc,

        // �Ｎ �Ҵ�� IO �޸�
        status.m_nAllocInstant,

        // test data
        status.m_test_data.c_str(),

        // Core����
		m_Core->GetCoreVersion() / 10000,
		m_Core->GetCoreVersion() % 10000
    );
    printf_s(temp);


    // display thread information
	Zero::StringA strThr = "[ThreadInfo] (";
	intptr_t MaxDisplayThreadCount = status.m_arrThread.Count;;
    if (MaxDisplayThreadCount > 8)   // Limit max display thread information to 8
    {
		Zero::StringA tmp;
		tmp.Format("%d", MaxDisplayThreadCount);

        strThr += tmp;
        strThr += ") : ";
        MaxDisplayThreadCount = 8;
    }
    else
    {
		Zero::StringA tmp;
		tmp.Format("%d", MaxDisplayThreadCount);

        strThr += tmp;
        strThr += ") : ";
    }

    for (int i = 0; i < MaxDisplayThreadCount; i++)
    {
		Zero::StringA tmp;

        strThr += "[";
		tmp.Format("%d", status.m_arrThread[i].m_ThreadID);
        strThr += tmp;
        strThr += "/";
		tmp.Format("%d", status.m_arrThread[i].m_CountQueue);
        strThr += tmp;
        strThr += "/";
		tmp.Format("%d", status.m_arrThread[i].m_CountWorked);
        strThr += tmp;
        strThr += "] ";
    }
	strThr += "\n";
    printf_s(strThr);
}


#ifdef USE_EXAMPLE_ASYNC_QUERY
void CSampleServer::ExampleSQL_Open()
{
	Zero::StartOptionAdo start_option;
	Zero::String connectAddr;
	connectAddr.Format( L"%s,%d", L"127.0.0.1", 5000 );
	start_option.SetParam(
		connectAddr,	// SQL �ּ�
		L"hot",			// ����
		L"pass123",		// ��ȣ
		L"AdoTest",		// Catalog
		3,				// timeout�ð�(��)
		true,			// retry�ɼ�
		20,				// Ŀ�ؼ�Ǯ ������
		L"SQLOLEDB.1"	// Provider(MS_SQL)
		//L"MSDASQL.1"	// Provider(MY_SQL)
	);
	try
	{
		Zero::AdoManager p(new Zero::CAdoManager(start_option));
		m_SQL = p;
		if( m_SQL->IsOpenFail() )
		{
			printf_s("sql open fail\n");
		}
		else
		{
			printf_s("sql open success.\n");
		}
	}
	catch(Zero::DBException& e)
	{
		printf_s("sql exception %s\n", e.what());
	}
}

void CSampleServer::Example_AsyncRequest()
{
	// �񵿱� ��û ó�� ����
	m_Core->Async_Request(
		Zero::Remote_None,	// Ư�� remote������ ������ ���������
		[=](void* &p)
	{
		int* a = new int;
		*a = 12345;
		p = (void*)a;
		printf_s("�񵿱� Request : input  addr(%d) value(%d)\n", a, *a);
	},
		[=](void* p)
	{
		int* r = (int*)p;
		printf_s("�񵿱� Request : result addr(%d) value(%d)\n", r, *r);

		SAFE_DELETE(r);
	});
}

void CSampleServer::ExampleSQL_QueryAsync()
{
	const TCHAR* request_query = _T("SELECT uuid FROM Userinfo");

	// �񵿱� ��û ����
	bool bRequestOk = m_Core->Async_Query(
		Zero::RemoteID_Define::Remote_None,
		m_SQL,
		request_query, [=](bool bComplete, Zero::CAdo* pAdo, intptr_t counter)
	{
		// �񵿱� ��� ó��
		if (bComplete)
		{
			try
			{
				while (!pAdo->GetEndOfFile())
				{
					short cnt = 0;

					Zero::Guid outGuid;	// �о�� ��
					DBFAIL_THROW(pAdo->GetFieldValue(cnt++, outGuid));
					printf_s("�񵿱� query ���� : guid[%s]\n", CW2A(outGuid.ToStringW()));

					pAdo->MoveNext();
				}

				if (!pAdo->IsSuccess())
				{
					// ����
				}
				return;
			}
			catch(Zero::DBException& e)
			{
				printf_s("�񵿱� query exception %s\n", e.what());
			}
		}
	});
	if (bRequestOk == false)
	{
		printf_s("�񵿱� query ��û ����!  [%s]\n", CW2A(request_query));
	}
}

void CSampleServer::ExampleSQL_QueryAsyncRemote(Zero::RemoteID remote)
{
	// �񵿱� ��û(SPȣ��)
	m_Core->Async_SP(
		remote,					// �̷��� Ư�� remote client�� ID ������ ����޴� ������ ��� remote client�� �������̸� ������ ������ ����ȴ�
		m_SQL,
		_T("InsertUser"),
		[=](Zero::CAdo* pAdo)
	{
		// SP �Ķ���� �غ� : �� �������� ���������� ������ ���������� �ȵ����� �Ķ���� ������ �ٸ� �����Ϳ� �������� ����
		pAdo->CreateParam(_T("return"), adInteger, adParamReturnValue, 0);
		pAdo->CreateParam(_T("@v_name"), adVarChar, adParamInput, Zero::String(L"ID�̸�"));
		pAdo->CreateParam(_T("@v_pass"), adVarChar, adParamInput, Zero::String(L"�̰�����ȣabc"));
	},
		[=](bool bComplete, Zero::CAdo* pAdo, intptr_t counter)
	{
		/**
		��û�� ��� �޾Ƽ� ó���ϴ� ����
		- �̶� remote��ȣ�� client�� ���� ���������� �ѹ� üũ���� �ʿ䰡 ����
		  �񵿱� ȣ���̴ϱ� ��û ������ �������̶� ��������� ������¾ƴҼ� �����ϱ�
		*/

		if (bComplete)
		{
			try
			{
				Zero::Guid outGuid;

				while (!pAdo->GetEndOfFile())
				{
					short cnt = 0;
					DBFAIL_THROW(pAdo->GetFieldValue(cnt++, outGuid));
					pAdo->MoveNext();
				}
				if (!pAdo->IsSuccess()) return;

				// ���ϰ� �о����
				int nRtnParam = 0;
				pAdo->GetParam(_T("return"), nRtnParam);

				//// OUT�Ķ���� test
				//pAdo->GetParam(_T("@v_test"), test_int);

				// ���� ��� �����ֱ�
				/*if (���� �ش� client�� �������ΰ��)
				{
					�ش� Remote client���� ��� ����
				}*/
				printf_s("�񵿱� spó�� ���� remoteID[%d] outGuid[%s]\n", remote, outGuid.ToStringA().c_str());
				return;
			}
			catch(Zero::DBException& e)
			{
				printf_s("�񵿱� sp exception : %s\n", e.what());
			}
		}

		printf_s("�񵿱� spó�� ���� remoteID[%d]\n", remote);
	});
}
#endif