#pragma once
#include <time.h>

namespace Zero
{
	/**
	64��Ʈ GUID ������ - 8����Ʈ�� ������ DB bigint������ 1:1��Ī����
	*/
	class Guid64
	{
	public:
		Guid64();
		~Guid64();

		class CGuidInfo
		{
		public:
			int	m_Type1;
			int	m_Type2;
			int	m_Sequence;		// �ʱ��� �����ð��뿡 �����Ȱ�� ���а�(0���� 1,2,3...)
			tm	m_Time;			// ������ �ð�
		};

		Int64		Create(const byte& type1, const byte& type2);		// 1�ʳ��� 65536�� �̻� �õ��� ������(0����)
		Int64		CreateForce(const byte& type1, const byte& type2);	// �����Ұ�� ��õ��Ͽ� ���� ����ó��
		void		GetInfo(const Int64& guid, OUT CGuidInfo& outputData) const;

		struct tm	GetTime(const Int64& guid) const;			// Create�� �ð�
		UInt16		GetSequence(const Int64& guid) const;
		byte		GetType1(const Int64& guid) const;
		byte		GetType2(const Int64& guid) const;

	private:
		int m_nTime;
		int	m_nSequence;
	};
}





