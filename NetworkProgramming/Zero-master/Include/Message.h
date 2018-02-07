#pragma once

namespace Zero
{
	/**
	�޼��� Stream Ŭ����

	- �⺻�� : ���� ���� ��� (���� ������ �ڵ� ����)

	- �ܺι��� ��� ���� : ���۸� ����/�б�� zero copy,  �� �� ������� �翬�� �ܺο��� �˾Ƽ� �����ؾ���

	*/
	class CMessage
	{
	public:
		CMessage();
		~CMessage();

		CSerializer m_Stream;

	public:
		inline CMessage& operator=(const CMessage& src)
		{
			src.CopyTo(*this);
			return *this;
		}

		inline CMessage(const CMessage &src)
		{
			src.CopyTo(*this);
		}

		// ����
		void WriteStart(Zero::PacketType PktType, Zero::CPackOption& pkDefault, const Int16 PkInternalValue=0, const bool isIDL=false);
		void Write(const CSerializable* pData);

		// PkOption�����
		void WriteOption(const byte* pSrc, const Zero::CPackOption* pOption);

		// �б�
		Zero::PacketType ReadStart(OUT Zero::CPackOption& pkDefault);
		void Read(const CSerializable* pData);

		// ������ ����
		inline byte* GetData()			{ return m_Stream.GetData(); }	// ������
		inline byte* GetData() const	{ return m_Stream.GetData(); }	// ������
		inline intptr_t GetCursor() const { return m_Stream.GetCursor(); }		// Ŀ�� ��ġ
		inline intptr_t GetBufSize() const	{ return m_Stream.GetBuffSize(); }	// ���� ũ��

		// ������ ���� (���ι��� ��� - �޸� ����) --> �޸� ����� �� �ʿ��� ��ŭ�� ����ϰԲ� ó����
		void RestoreInternal(const byte *ibuf, const intptr_t isize);

		// ������ ���� (�ܺι��� �״�� ����ϱ�)
		void RestoreExternal(byte *ibuf, const intptr_t isize);

	private:
		inline void CopyTo(CMessage &dest) const
		{
			dest.m_Stream = m_Stream;
		}
	};


	class CRecvedMessage
	{
	public:
		CMessage msg;
		RemoteID remote;

		PacketType pkID;
		CPackOption pkop;

		CRecvedMessage() : remote(Remote_None), pkID(PacketType_MaxCount)
		{
		}
		void From(RemoteID remoteID, PacketType id, CPackOption op)
		{
			remote = remoteID;
			pkID = id;
			pkop = op;
		}
	};
}








