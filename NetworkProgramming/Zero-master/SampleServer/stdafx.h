// stdafx.h : ���� ��������� ���� ��������� �ʴ�
// ǥ�� �ý��� ���� ���� �� ������Ʈ ���� ���� ������
// ��� �ִ� ���� �����Դϴ�.
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>



// TODO: ���α׷��� �ʿ��� �߰� ����� ���⿡�� �����մϴ�.
#include "../include/NetLibServer.h"

#if defined(_WIN64)
#ifdef	_DEBUG
#pragma comment(lib, "../Lib/x64/debug/netlib.lib" )
#else	//	_DEBUG
#pragma comment(lib, "../Lib/x64/release/netlib.lib" )
#endif	//	_DEBUG
#else
#ifdef	_DEBUG
#pragma comment(lib, "../Lib/w32/debug/netlib.lib" )
#else	//	_DEBUG
#pragma comment(lib, "../Lib/w32/release/netlib.lib" )
#endif	//	_DEBUG
#endif

#include "../SampleCommon/Marshaler.h"