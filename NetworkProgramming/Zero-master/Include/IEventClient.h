#pragma once
#include "IEventBase.h"


namespace Zero
{
	class ConnectionInfo
	{
	public:
		Zero::NetAddress addr;
		bool moved;
		ConnectionInfo() { moved=false; }
		ConnectionInfo(Zero::NetAddress ad, bool m) { addr = ad; moved = m; }
	};

	class IEventClient : public IEventBase
	{
	public:
		virtual ~IEventClient() {}


		// ���� ���� ���� : ���� ��������� Connect()ȣ��� ��� ���ϵǸ�, �������� �ణ�� �ð��Ŀ� ������ �����ȴ�
		virtual void OnJoin(const ConnectionInfo& info) = 0;


		// ���� ���� ����
		virtual void OnLeave(const ConnectionInfo& info) = 0;


		// ���ӽõ� ��� (���� : ������ �ƴ�)
		virtual void OnConnectResult(bool isConnectSuccess) {};


		// ���� �̵� �õ��� ������ ���
		virtual void OnMoveToServerFail() {};


		// UDP Ready : bTrust�� ���޺����� �ƴ� ���� ���¸� �ǹ��Ѵ� (�������� ��������, ������̳� �����ⰰ�� �Ҿ����� ��������)
		virtual void OnReadyUDP(const bool bTrust) {};


		// �Ͻ��� ��Ʈ��ũ ��ֽ� ���ẹ������ : ���ẹ���ɼ� ���ÿ��� �����
		virtual void OnRecoveryStart() {}


		// �Ͻ��� ��Ʈ��ũ ��ֽ� ���ẹ������ : �������н� OnLeave�̺�Ʈ ó����
		virtual void OnRecoveryEnd() {}


		// ������ �ִ� ���� ���� ���ѿ� �ɸ�, ���� �ڵ������� ���� ���� �ȴ�
		virtual void OnServerLimitConnection() {}
	};
}

