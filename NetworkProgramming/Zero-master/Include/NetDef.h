#pragma once

// �۾� ������
//#define DEF_RSA_MODE	// RSAȰ��ȭ : C++����, ������ ���ӽ����� �������� �����Ͱ� ���ŵ�(��ŷ�����)

#ifndef ZASSERT
#define	ZASSERT(condition, state)	if( (condition) == false ) { _ASSERT(condition); state; }
#endif

#ifdef _UNICODE
	#define	ASSERT_THROW(condition)\
		if( (condition) == false ) {\
			_ASSERT(condition);\
			Zero::String txt;\
			txt.Format( _T("%s (%s)"), __TFUNCTION__, (LPCWSTR)CA2W(#condition) );\
			throw Zero::CException(txt);\
		}
	#define FAIL_THROW(condition)\
		if( (condition) == false ) {\
			Zero::String txt;\
			txt.Format( _T("%s (%s)"), __TFUNCTION__, (LPCWSTR)CA2W(#condition) );\
			throw Zero::CException(txt);\
		}
#else
	#define	ASSERT_THROW(condition)\
		if( (condition) == false ) {\
			_ASSERT(condition);\
			Zero::String txt;\
			txt.Format( _T("%s (%s)"), __TFUNCTION__, #condition );\
			throw Zero::CException(txt);\
		}
	#define FAIL_THROW(condition)\
		if( (condition) == false ) {\
			Zero::String txt;\
			txt.Format( _T("%s (%s)"), __TFUNCTION__, #condition );\
			throw Zero::CException(txt);\
		}
#endif





