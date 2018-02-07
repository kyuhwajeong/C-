#include "stdafx.h"
#include <iostream>
#include <conio.h>
#include <future>
#include <functional>

#include "../SampleCommon/Sample_proxy.h"
#include "../SampleCommon/Sample_proxy.cpp"
#include "../SampleCommon/Sample_stub.h"
#include "../SampleCommon/Sample_stub.cpp"


// �׽�Ʈ �ڵ� : ���ӽõ��� ���� �������� �������� ������������ Ÿ�Ӿƿ� �ð� üũ�ϴ� ���� �ڵ�
//#define TEST_ASYNC_CONNECT_TIMEOUT_SAMPLE


class CSampleClient : public Zero::IEventClient
{
public:
	Rmi::Proxy proxy;
	Rmi::Stub stub;
	Zero::ICoreClientPtr m_Core;


	CSampleClient()
	{
		m_Core = Zero::ICoreClientPtr(Zero::ICoreClient::NewCore(this));
		m_Core->Attach(&proxy, &stub);
	}
	
	virtual void OnJoin(const Zero::ConnectionInfo& info) override
	{
		printf_s("Joined server.\n");
	}
	virtual void OnLeave(const Zero::ConnectionInfo& info) override
	{
		printf_s("Leave server\n");
	}

	// ���ӽõ� ����� ������ ����
	virtual void OnConnectResult(bool isConnectSuccess)
	{
		printf_s("Connect Result = %d\n", isConnectSuccess);
	}

	virtual void OnInformation(const Zero::CResultInfo& info) override
	{
		printf_s("info : %s\n", CW2A(info.msg.c_str()));
	}
	virtual void OnException(Zero::CException& e) override
	{
		printf_s("exception : %s\n", e.what());
	}
};

int _tmain(int argc, _TCHAR* argv[])
{
	CSampleClient sample_client;


#ifdef TEST_ASYNC_CONNECT_TIMEOUT_SAMPLE
	auto future_connect = std::async(std::launch::async,
		[sample_client]() {
		if (sample_client.m_Core->Connect(
			L"127.0.0.1",
			20000,
			0,
			0,
			false
		))
		{
			return true;
		}
		return false;
	});

	std::future_status s = future_connect.wait_until(std::chrono::system_clock::now() + std::chrono::milliseconds(500));

	if (s == std::future_status::ready)
	{
		auto result_connect = future_connect.get();
		if (result_connect)
		{
			printf_s("connect server ok.\n");
		}
		else
		{
			printf_s("connect server fail\n");
			_getch();
			return 0;
		}
	}
	else
	{
		// timeout���´ϱ� ���� ���� ���з� �����ϰ� �޼��� ���
		printf_s("connect server fail - time out\n");
		_getch();

		// �� ���� ���� ���ӽõ� ����� ������������ ��ٸ���
		auto result_connect = future_connect.get();

		// ���α׷� ����
		return 0;
	}
#else
	if (sample_client.m_Core->Connect(
		L"127.0.0.1",
		20000,
		0,
		0,
		false
		))
	{
		printf_s("connect server ok.\n");
	}
	else
	{
		printf_s("connect server fail\n");
		_getch();
		return 0;
	}
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

			if (cmd =="/q")
			{
				sample_client.m_Core->Leave();	// ����
			}
			else if (cmd =="/exit")
			{
				run_program = false;

				// ���� fut�� �������ΰ�� ���Ḧ ���� ó��(�ƹ��ų� �Է��� �޴´�)
				if (!fut._Is_ready())
				{
					printf_s("input any string to exit\n");
					fut.wait();
					return 0;
				}
			}
			else
			{
				RemoteClass::CUserClass _testClass;
				Zero::CDictionary<Int32,Int32> _testDic;
				_testDic.Add(12345,54321);

				// ��Ŷ������
				sample_client.proxy.request_message(Zero::Remote_Server, Zero::CPackOption::Basic, _testClass, _testDic, Zero::ZA2W(cmd));

				// ����ޱ�
				sample_client.stub.reponse_message = []Func_Rmi_reponse_message
				{
					printf_s("receved : %s\n", CW2A(msg));
					return true;
				};
			}

			if (run_program)
				fut = std::async(std::launch::async, ReadLineAsync);
		}

		sample_client.m_Core->NetLoop();
		Sleep(10);
	}

	printf_s("press any key to exit\n");
	_getch();
	return 0;
}

