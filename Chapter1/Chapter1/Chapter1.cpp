#include <vector> // std::vector 사용하기 위한 헤더파일
#include <iostream> // 표준 입출력 헤더파일
#include <chrono> // 시간 측정을 위한 chrono 라이브러리 헤더파일

using namespace std; // namespace std를 사용하여 std :: 생략
const int MaxCount = 150000; 

bool IsPrimeNumber(int number) // 소수 판별 함수
{
	if (number == 1) // 1이면 false
		return false;
	if (number == 2 || number == 3) // 2,3 이면 소수임으로 true
		return true;

	for (int i = 2; i < number - 1; i++) 
	{
		// 소수는 자기 자신으로만 나눠저야함으로 자기자신의 수 -1 일때 나눠지면 소수가 아님
		if ((number % i) == 0)
			return false;
	}
	return true;
}

void PrintNumbers(const vector<int>& primes)
// &는 참조자(reference)를 사용하면 실제 객체가 가르키고 있는 메모리 주소를 전달
// 즉 객체의 복사본을 만드는 것이 아닌 실제 객체에 접근 
// => 복사본을 만드는 메모리 사용량 줄임
// const : 상수 키워드를 사용하여 PrintNumbers 함수가 vector<int>& primes 값을 수정하지 못함ㄴ
{
	for (int v : primes) 
	{
		cout << v << endl;
	}
}

void main()
{
	vector<int> primes;

	auto t0 = chrono::system_clock::now(); // 현재 시간 측정

	for (int i = 0; i <= MaxCount; i++)
	{
		if (IsPrimeNumber(i))
		{
			primes.push_back(i);
		}

	}

	// chrono 헤더파일 사용하여 시간 측정 

	auto t1 = chrono::system_clock::now(); // 반복문이 끝난 시간 측정
	
	// duration 은 반복문이 실행된 시간초가 흐른값 - 시작하기 전 시간 값 = 계산하는데 걸린 시간
	auto duration = chrono::duration_cast<chrono::milliseconds>(t1 - t0).count();
	// 출력
	cout << "Took " << duration << " milliseconds." << endl;

	PrintNumbers(primes);
}