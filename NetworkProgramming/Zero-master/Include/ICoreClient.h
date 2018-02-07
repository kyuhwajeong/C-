#pragma once
#include "IEventClient.h"

namespace Zero
{
	class ICoreClient : public IPacket
	{
	public:
		virtual ~ICoreClient() {};

		static ICoreClient* NewCore(IEventClient* pEventHandler, bool bTestMode = false);

		/**

		������ ����, �����ߴٰ� �ﰢ ������ �������°� �ƴϸ� ������ ������ �Ϸ�Ǹ� OnJoin �̺�Ʈ�� ���Եȴ�

		@bMobile : true ������ ������ɵ��� �� ����Ͽ� ����ȭ�� ������� ó��

		@bUseRecovery

		- �ڵ����ẹ���ɼ�, �Ͻ��� ������ֳ� WIFI��ȯ ���� ��Ȳ���� �ڵ����� �������ش�

		- ���������� �˾Ƽ� �������ֱ� ������ �ƹ��ϵ� �Ͼ�� ������ó�� ���������� ó���� �� �ִ�

		- �������� ���� �߻� �̺�Ʈ : OnRecoveryStart

		- �����Ϸ� ���� �߻� �̺�Ʈ : OnRecoveryEnd

		- �������� ���� �߻� �̺�Ʈ : OnLeave

		*/
		virtual bool Connect(String ipaddr, UInt16 portnum, UInt32 protocol_version=0, int portudp=0/*udp������=0*/, bool bMobile=false, bool bUseRecovery=false) = 0;


		// �翬��
		virtual bool ReConnect() = 0;
		virtual bool ReConnect(String ipaddr, UInt16 portnum) = 0;


		// ���� �̵� : ���� ���� �ּ� �Է� : �̵� �� ������ ���ٵ��� ��Ÿ ������ �õ��� ������ ��� OnMoveToServerFail �̺�Ʈ �߻�
		virtual void MoveToServer(NetAddress addr) = 0;


		// ���� �̵� : ���� Ÿ�� �Է� -> �ش� Ÿ�� ������ �ڵ� �ε座����
		virtual void MoveToServer(int ServerType) = 0;


		// ���� �����ϱ�
		virtual bool Leave() = 0;


		// ���� �������
		virtual bool IsJoined() = 0;


		// �������� ��Ʈ��ũ ���� ����
		virtual bool IsNetworkConnect() = 0;


		// �������� ��Ʈ��ũ ���ῡ ���� ��������
		virtual bool IsNetworkAuthed() = 0;


		// ��Ʈ��ũ ��Ŷ �� �̺�Ʈ ó��(�������� ȣ���ʿ�)
		virtual void NetLoop() = 0;

		// NetLoop�� ������ �Լ�
		virtual void FrameMove() = 0;


		// ���� ���� ����(�Ϲ����� ����) : ����� ��찡 �ƴ϶�� Leave()�� ����Ͽ� �������� ����ó���� �ϴ°��� ����
		virtual void ForceLeave() = 0;


		// ��������� ���� ���� ����(�Ϲ����� ����) : ����� ��찡 �ƴ϶�� Leave()�� ����Ͽ� �������� ����ó���� �ϴ°��� ����
		virtual void Destroy() = 0;


		// �ﰢ ���ẹ�� : ���ẹ�� �ɼ��϶� �ǽð� WIFI��ȯ������ ���, IOS��� Reachability�� ��������
		virtual void FastRecovery() = 0;


		// �׷� ID
		virtual RemoteID GetGroupRemoteID() = 0;


		virtual UInt32 GetCoreVersion() = 0;
		virtual UInt32 GetProtocolVersion() = 0;
	};
	typedef std::shared_ptr<Zero::ICoreClient> ICoreClientPtr;
}

