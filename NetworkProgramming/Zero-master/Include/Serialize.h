#pragma once
#include "Guid.h"

namespace Zero
{
	/**
	���ۼ���

	- ���� ���� ��� (���ι��� �ڵ� ����)

	- �ܺι��� ���� ������ (��Ʈ��ũ ��Ŷ ������ ���� �������� �ʰ� �״�� ��밡����)

	  1. ReSize �Ұ�

	  2. PushDataó���� ����ũ�Ⱑ �Ѿ�� �����͸� ä������Ҷ� ������ (���ι��� ����Ұ�쿣 ����ũ�� �ڵ����� �þ)


	- Serialize �ڵ�ȭ ���� - AUTO_SERIALIZE1 ~ AUTO_SERIALIZE30 ���� ��밡��

	@code
		class CData : public CSerializable
		{
		public:
			int m_Number1;
			int m_Number2;

			AUTO_SERIALIZE2(m_Number1, m_Numbe2);	// 2���� ������ ó����
		};
	@end

	- enum �ڵ� ����ȭ ��Ű�� ����
	@code
		AUTO_SERIALIZE_ENUM(enumValue);				// �Ϲݿ뵵(32��Ʈ)

		AUTO_SERIALIZE_ENUM16(enumValue);			// 16��Ʈ enum ���� -INT16_MAX 32767~32767���� ����

		AUTO_SERIALIZE_ENUM8(enumValue);			// 8��Ʈ enum ���� -INT8_MAX
	@end

	*/

	class CSerializer;
	class CSerializable
	{
	public:
		virtual void Serialize(bool bStore, CSerializer& clsSerial) = 0;
	};

	// ��Ʈ�� ����ȭ�� �ִ� ���� ���� ���� ���� : MaxLengthString(1024)
	class CSerializer
	{
	public:
		CSerializer();
		~CSerializer();
		CSerializer(const CSerializer &src);

		void CursorReset(intptr_t pos=0);
		void SetExternalBuffer(byte* pBuf, const intptr_t nSize);				// �ܺι��� ����ϱ� ����
		void ExtendCapacity(intptr_t isize);									// ����Capacity���� : �ø��⸸ ����

		inline bool		IsExternalBuffer() const { return m_bExternalBuffer; }	// ���� �ܺι��� ��������� �˾Ƴ���
		inline byte*	GetData()	const { return m_pBuffer; }					// ������ ����
		inline intptr_t	GetCursor() const { return m_nCursor; }					// ���� Ŀ�� ��ġ
		intptr_t		GetBuffSize() const;									// ���� ������

		inline void SetForceCursor(int n) { m_nCursor = n;  }	// ������(managed�뵵)

		CSafeArray<byte, true,Int32>* GetArr() { return &m_Data; }

		// ����ũ�� ���� - Write����/��Ŷ�о���̱� �뵵, ���� NoCopy�ɼ� ����� - �ܺι��� ���� ������ ���� �Ұ���!
		void ReSizeBuffer(intptr_t isize, bool ignoreCapacityMode=false/*��Ŷ�о���̴� �뵵�� ���� Capacity�ø�������*/);

		// �⺻ ������ Ÿ�� ���
	#define MAKE_SERIALIZE_DATA(data)\
		inline CSerializer& operator<<(const data& b)	{ PushData(	(char*)&b,	sizeof(data) );	return *this; }\
		inline CSerializer& operator>>(data &b)			{ PopData(	(char*)&b,	sizeof(data) );	return *this; }
		MAKE_SERIALIZE_DATA(bool);
		MAKE_SERIALIZE_DATA(sbyte);
		MAKE_SERIALIZE_DATA(byte);
		MAKE_SERIALIZE_DATA(Int16);
		MAKE_SERIALIZE_DATA(UInt16);
		MAKE_SERIALIZE_DATA(Int32);
		MAKE_SERIALIZE_DATA(UInt32);
		MAKE_SERIALIZE_DATA(Int64);
		MAKE_SERIALIZE_DATA(UInt64);
		MAKE_SERIALIZE_DATA(float);
		MAKE_SERIALIZE_DATA(double);
		MAKE_SERIALIZE_DATA(Guid);

		CSerializer& operator<<(CSerializable& object);
		CSerializer& operator<<(CSerializable* object);
		CSerializer& operator>>(CSerializable& object);
		CSerializer& operator>>(CSerializable* object);
		CSerializer& operator<<(const StringW& data);
		CSerializer& operator>>(StringW& data);
		CSerializer& operator<<(const StringA& data);
		CSerializer& operator>>(StringA& data);
		CSerializer& operator<<(const std::wstring& data);
		CSerializer& operator>>(std::wstring& data);
		CSerializer& operator<<(const std::string& data);
		CSerializer& operator>>(std::string& data);

		template<typename T> CSerializer& operator<<(const T& data);
		template<typename T> CSerializer& operator<<(const T* data);
		template<typename T> CSerializer& operator>>(T& data);
		template<typename T> CSerializer& operator>>(T* data);

		template<typename elem, bool raw_type, typename size_type> CSerializer& operator<<(const CSafeArray<elem, raw_type, size_type>& Ar);
		template<typename elem, bool raw_type, typename size_type> CSerializer& operator>>(CSafeArray<elem, raw_type, size_type>& Ar);

		template<typename elem> CSerializer& operator<<(const std::vector<elem>& Ar);
		template<typename elem> CSerializer& operator>>(std::vector<elem>& Ar);

		template<class K, class T> CSerializer& operator<<(const TMap<K,T>& Ar);
		template<class K, class T> CSerializer& operator>>(TMap<K,T>& Ar);
		template<typename K, typename V> CSerializer& operator<<(const CDictionary<K,V>& Ar);
		template<typename K, typename V> CSerializer& operator>>(CDictionary<K,V>& Ar);

		CSerializer& operator=(const CSerializer& src)
		{
			src.CopyTo(*this);
			return *this;
		}

	public:
		void PushData(const char* pData, intptr_t nSize);
		void PopData(char* pData, intptr_t nSize);
	
	private:
		enum { MaxLengthString = 1024, MaxSizeMap = 1048576 };

		byte* m_pBuffer;			// ���� ������� ����
		bool m_bExternalBuffer;		// �ܺ� ���۸� ���������
		intptr_t m_nExternalSize;	// �ܺ� ���� ������

		CSafeArray<byte, true,Int32> m_Data;
		intptr_t m_nCursor;

		void Initialize();
		void CopyTo(CSerializer &dest) const;
	};
}

#include "Serialize.inl"

