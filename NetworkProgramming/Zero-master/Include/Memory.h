#pragma once
#include <memory>

namespace Zero
{
	/**
	Ŭ���� �޸�Ǯ ��� ���

	- ���α׷� ���۽� �ڵ� �޸�Ǯ �Ҵ�, ����� ���� (�Ҵ� ������ ���� �����ؾ��� ��� CPoolMemŬ���� ���� ���)

	- Ŭ���� ���ο��� DECLARE_POOL_MEMORY

	- cpp���� IMPLEMENT_POOL_MEMORY ����

	  @param sizeBlock : �޸𸮺�� ����
	  @param bThreadSafe : ������Safe�ɼ�

	*/
	#define DECLARE_POOL_MEMORY(className)\
		typedef std::shared_ptr<Zero::CPoolMem<className>> CPoolMemPtr;\
		static CPoolMemPtr g_p##className##MemPool;\
	public:\
		inline void* operator new(size_t size)\
		{\
			return (void*)(g_p##className##MemPool->Alloc());\
		}\
		inline void operator delete(void* ptr, size_t size)\
		{\
			g_p##className##MemPool->Free((className*)ptr);\
		}

	#define IMPLEMENT_POOL_MEMORY(className,sizeBlock,bThreadSafe)\
		className::CPoolMemPtr className::g_p##className##MemPool(new Zero::CPoolMem<className>(sizeBlock,bThreadSafe));


	// �޸� �Ҵ� ������
	class CMemory
	{
	public:
		static void* Alloc(size_t size);
		static void* Realloc(void* p,size_t size);
		static void Free(void* p);
	};


	/**
	�޸� Ǯ Ŭ���� (���� �޸�Ǯ + �ǽð� �Ҵ� ���)
	-  a = new CPoolMem<ClassType>(nSize);
	*/
	template<class ClassType>
	class CPoolMem
	{
	private:
		struct tgaData
		{
			ClassType data;
			tgaData* pNext;
			bool bNeedFree;	// �Ｎ �Ҵ���� �޸�
		};

	public:
		CPoolMem(DWORD dwMax, bool bThreadSafe=true);
		~CPoolMem();

		DWORD GetCountInstant()		{ return m_dwAlloc; }				// �Ｎ�Ҵ� �� ����
		DWORD GetCountAlloc()		{ return m_dwCount; }				// �Ϲ��Ҵ� ��
		DWORD GetCount()			{ return m_dwCount + m_dwAlloc; }	// ��ü ��

		tgaData* GetBlock() {
			return m_pBlocks;
		}

		ClassType* Alloc();
		void Free(ClassType* pDel);
		void Lock();
		void UnLock();

	private:
		tgaData* m_pBlocks;
		tgaData* m_pInters;

		DWORD m_dwIndex;
		DWORD m_dwInters;
		DWORD m_dwMax;
		DWORD m_dwAlloc;
		DWORD m_dwCount;

		bool m_UseLock;
		CRITICAL_SECTION m_cs;
	};
}

#include "Memory.inl"
