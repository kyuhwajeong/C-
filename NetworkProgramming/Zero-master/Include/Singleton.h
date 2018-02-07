#pragma once

/**
���� �̱��� Ŭ���� �����

- ���� �̱��� Ŭ���� �ڵ� ����

- Ŭ���� ���� static �ν��Ͻ� ���

- �ܺο��� Ŭ���� ����/���� ����, Ŭ���� ���� �ν��Ͻ��� ���� ����

- FUNCTION_SINGLETON�� �̿��� �������̽� ����(�̱��� Ŭ���� �ν��Ͻ� ���)

@code
	class CSample
	{
		DECLARE_SINGLETON(CSample);
	public:
		void CallFunction();
	};

	FUNCTION_SINGLETON( CSample, Sample );

@����
	Sample()->CallFunction();

*/
#define DECLARE_SINGLETON( className )\
public:\
	static className* Instance()\
	{\
		static className instance;\
		return &instance;\
	}\
private:\
	className();\
	className& operator=(const className&);\
	className(const className&);


#define FUNCTION_SINGLETON( className , funcName ) \
	static className* ##funcName()\
	{\
		return className::Instance();\
	};


