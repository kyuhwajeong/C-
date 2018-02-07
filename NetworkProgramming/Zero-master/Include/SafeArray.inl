#pragma warning(disable:4127)
#pragma warning(disable:4189)

namespace Zero
{
	#pragma push_macro("new")
	#undef new
	template<class ClassType>
	static inline void CallConstructors(ClassType* pElements, intptr_t nElements)
	{
		for(intptr_t i=0; i<nElements; i++)
			::new( pElements+i )ClassType;
	}
	#pragma pop_macro("new")


	template<class ClassType, bool raw_type, typename size_type>
	inline CSafeArray<ClassType, raw_type, size_type>::CSafeArray()
	{
		Initialize();
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline CSafeArray<ClassType, raw_type, size_type>::CSafeArray(const CSafeArray &src)
	{
		Initialize();
		src.CopyTo(*this);
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline CSafeArray<ClassType, raw_type, size_type>::CSafeArray(size_type newSize, bool bReserve)
	{
		if( newSize == 0 )
		{
			Throw("init size is zero.");
			return;
		}
		Initialize();

		if( bReserve )
		{
			m_Capacity = newSize;
			m_Array = (ClassType*)CMemory::Alloc( m_Capacity * sizeof(ClassType) );
		}
		else
		{
			m_Capacity = m_Size = newSize;
			m_Array = (ClassType*)CMemory::Alloc( m_Capacity * sizeof(ClassType) );
			if( !raw_type )
				CallConstructors<ClassType>(m_Array, m_Size);
		}
	}

	template<class ClassType, bool raw_type, typename size_type>
	CSafeArray<ClassType, raw_type, size_type>::~CSafeArray()
	{
		ClearWithCapacity();
	}

	template<class ClassType, bool raw_type, typename size_type>
	void CSafeArray<ClassType, raw_type, size_type>::UseLowMemory()
	{
		m_UseMemory = -1;
	}

	template<class ClassType, bool raw_type, typename size_type>
	void CSafeArray<ClassType, raw_type, size_type>::UseHighMemory()
	{
		m_UseMemory = 1;
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline void CSafeArray<ClassType, raw_type, size_type>::Clear()
	{
		Resize(0, true);
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline void CSafeArray<ClassType, raw_type, size_type>::ClearWithCapacity()
	{
		if( m_Capacity == 0 )
			return;

		if( !raw_type )
		{
			if( m_Size > 0 )
			{
				for (size_type i = 0; i<m_Size; i++)
					m_Array[i].~ClassType();
			}
		}

		if( m_Array != NULL )
			CMemory::Free(m_Array);

		m_Array = NULL;
		m_Size = 0;
		m_Capacity = 0;
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline bool CSafeArray<ClassType, raw_type, size_type>::ExtendCapacity(size_type newCapacity)
	{
		// �䱸���� capacity�� ���������ΰ�� ���� ó����������!(�Լ��������ٸ���)
		if( newCapacity <= Count )
			return false;

		// ���� ������ ����
		size_type currentSize = Count; 

		// ũ�⸦ �÷�����
		Resize( newCapacity, false, true );

		// ���� ������� ����
		Resize( currentSize );

		return true;
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline void CSafeArray<ClassType, raw_type, size_type>::Resize(size_type newSize, bool bNoCopy, bool bForceCapacityMode)
	{
		if( newSize == m_Size )
			return;

		#ifdef _DEBUG
		if( raw_type )
		{
			int rrr = 0;
		}
		else
		{
			int qqq = 0;
		}
		#endif

		// ���������� ���̱�
		if( newSize < m_Size )
		{
			if( false == raw_type )
			{
				for (size_type i = 0; i<m_Size - newSize; i++)
					(m_Array+newSize)[i].~ClassType();
			}

			m_Size = newSize;
		}
		else // ������ Ȯ��
		{
			if( newSize <= m_Capacity )	// �̶��� ���� �޸𸮸� ���� �ʿ䰡 ����
			{
				// ������ �����Ͽ� �÷��ظ�ŭ ������ ȣ��
				if( !raw_type )
					CallConstructors<ClassType>(m_Array+m_Size, newSize-m_Size);
			}
			else
			{
				bool bCapacityZero = false;
				if( m_Capacity == 0 )
				{
					bCapacityZero = true;
				}

				if( bForceCapacityMode )
				{
					// Capacity��带 �����ϰ� ��Ȯ�� �ʿ��� ��ŭ�� Capacity �״�� �����Ѵ�
					m_Capacity = newSize;
				}
				else
				{
					m_Capacity = AutoCapacity(newSize);
				}

				if( bCapacityZero )
				{
					m_Array = (ClassType*)CMemory::Alloc( m_Capacity * sizeof(ClassType) );
					if(m_Array == NULL)
					{
						Throw("Capacity Zero Error");
					}
					if( !raw_type )
						CallConstructors<ClassType>(m_Array, newSize);
				}
				else
				{
					ClassType* oldData = m_Array;
					ClassType* newData;

					if( raw_type )
					{
						/**
						Realloc�� Alloc �ӵ� �׽�Ʈ�ʿ�, ���� Alloc�� �� ������� :
						- if( bNoCopy ) �϶� Realloc�������� Allocó���Ұ� (Realloc�� �޸� �Ҵ�+���������)
						*/
						newData = (ClassType*)CMemory::Realloc(oldData, m_Capacity * sizeof(ClassType));
						if(newData == NULL)
						{
							Throw("Realloc raw_type Error");
						}
						m_Array = newData;
					}
					else
					{
						newData = (ClassType*)CMemory::Alloc( m_Capacity * sizeof(ClassType) );
						if(newData == NULL)
						{
							Throw("memAlloc Capacity Error");
						}
						if( bNoCopy == false )
						{
							for( size_type index = 0; index < m_Size; index++ )
								newData[index] = m_Array[index];
						}

						// ���� alloc�߱� ������ �����Ѻκ� ���� ���� �κп� ���� ������ȣ���� �ʿ��ϴ�
						CallConstructors<ClassType>(newData+m_Size, newSize-m_Size);

						if( m_Size > 0 )
						{
							for (size_type i = 0; i<m_Size; i++)
								m_Array[i].~ClassType();
						}
						CMemory::Free(oldData);

						m_Array = newData;
					}
				}
			}
			m_Size = newSize;
		}
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline void CSafeArray<ClassType, raw_type, size_type>::Add(const ClassType& src)
	{
		Insert(m_Size, src);
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline bool CSafeArray<ClassType, raw_type, size_type>::AddIfUnique(const ClassType& src)
	{
		if( Contains(src) ) return false;
		Insert(m_Size, src);
		return true;
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline bool CSafeArray<ClassType, raw_type, size_type>::Contains(const ClassType& src)
	{
		for( size_type index = 0; index < m_Size; index++ )
		{
			if( m_Array[index] == src )
				return true;
		}
		return false;
	}

	template<class ClassType, bool raw_type, typename size_type>
	void CSafeArray<ClassType, raw_type, size_type>::Insert(size_type p_index, const ClassType& src)
	{
		if( m_Size < 0 || p_index < 0 || p_index > m_Size )
		{
			Throw("Insert Error size");
		}

		size_type moveAmount = m_Size - p_index;
		Resize( m_Size + 1 );
			
		if( raw_type )
		{
			memmove( m_Array+p_index+1, m_Array+p_index, sizeof(ClassType) * moveAmount );
		}
		else
		{
			for( size_type index = m_Size - 1; index > p_index; index-- )
				m_Array[index] = m_Array[index - 1];
		}

		m_Array[p_index] = src;
	}

	template<class ClassType, bool raw_type, typename size_type>
	void CSafeArray<ClassType, raw_type, size_type>::RemoveAt(size_type p_index)
	{
		if( BoundCheck(p_index) ) Throw("bound check fail");

		if( raw_type )
		{
			size_type moveAmount = m_Size - (p_index + 1);
			memmove( m_Array+p_index, m_Array+p_index+1, sizeof(ClassType) * moveAmount );
		}
		else
		{
			for (size_type index = p_index + 1; index < m_Size; index++)
				m_Array[index - 1] = m_Array[index];
		}
		Resize( m_Size - 1 );
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline bool CSafeArray<ClassType, raw_type, size_type>::Remove(const ClassType& value)
	{
		for( size_type i=0; i<m_Size; i++ )
		{
			if( m_Array[i] == value )
			{
				RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline ClassType& CSafeArray<ClassType, raw_type, size_type>::ElementAt(size_type p_index) const
	{
		if( BoundCheck(p_index) ) Throw("bound");
		return m_Array[p_index];
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline void CSafeArray<ClassType, raw_type, size_type>::Initialize()
	{
		m_Array = NULL;
		m_Size = 0;
		m_Capacity = 0;
		m_UseMemory = 0;
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline size_type CSafeArray<ClassType, raw_type, size_type>::AutoCapacity(size_type newSize)
	{
		size_type minCapacity = 0;
		size_type newCapacity = 0;
		switch(m_UseMemory)
		{
		case 0:
			{
				size_type nGrowSize = m_Size / 8;
				nGrowSize = __min(nGrowSize, 1024);
				nGrowSize = __max(nGrowSize, 4);
				newCapacity = __max(minCapacity, newSize + nGrowSize);
			}
			break;
		case 1:
			{
				size_type nGrowSize = m_Size / 8;
				nGrowSize = __max(nGrowSize,16);
				nGrowSize = __max(nGrowSize,(size_type)(64/sizeof(ClassType)));
				//minCapacity = 256;
				newCapacity = __max(minCapacity, newSize + nGrowSize);
			}
			break;
		case -1:
			newCapacity = newSize;
			break;
		}
		return newCapacity;
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline bool CSafeArray<ClassType, raw_type, size_type>::BoundCheck(const size_type& p_index) const
	{
		if( p_index < 0 || p_index >= m_Size )
		{
			Throw("Error");
		}
		return false;
	}

	template<class ClassType, bool raw_type, typename size_type>
	inline void CSafeArray<ClassType, raw_type, size_type>::CopyTo(CSafeArray &dest) const
	{
		dest.Resize(m_Size, true, true);

		if( raw_type )
		{
			memcpy( dest.GetData(), GetData(), sizeof(ClassType) * m_Size );
		}
		else
		{
			ClassType* pDest = dest.GetData();
			const ClassType* pSrc = GetData();
			for(size_type i=0; i<m_Size; i++ )
			{
				pDest[i] = pSrc[i];
			}
		}
	}
}

#pragma warning(default:4127)
#pragma warning(default:4189)