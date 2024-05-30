#include <vector>
#include <iostream>
#include <chrono>
#include <memory>
#include <thread>

using namespace std;

const int MaxCount = 150000;
const int ThreadCount = 4;

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

int main()
{
	int num = 1;
	vector<int> primes;

	auto t0 = chrono::system_clock::now();

	// 작동할 워커 쓰레드
	vector<shared_ptr<thread>> threads;

	for (int i = 0; i < ThreadCount; i++)
	{
		// new thread[&]() => Lamda식
		// [=] () {} () // 모든 외부 변수 result1, result2, result3, result4를 복사해서 람다 함수 내부에서 사용
		// [&] () {} () // 모든 외부 변수 result1, result2, result3, result4를 참조해서 람다 함수 내부에서 사용
		// => 람다식 내부에서 num, primes 같은 변수 직접 접근 가능
		shared_ptr<thread> thread(new thread([&]() {
			while (true)
			{
				int n;
				n = num;
				num++;

				if (n >= MaxCount)
					break;

				if (IsPrimeNumber(n))
				{
					primes.push_back(n);
				}
			}
			}));
		// 생성된 thread를 std::vector에 저장
		threads.push_back(thread);
	}

	for (auto thread : threads)
	{
		thread->join();

	}

	auto t1 = chrono::system_clock::now();

	auto duration = chrono::duration_cast<chrono::microseconds>(t1 - t0).count();

	cout << "Took " << duration << " milliseconds " << endl;

	return 0;

}