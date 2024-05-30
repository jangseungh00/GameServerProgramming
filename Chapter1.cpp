#include <stdio.h>


void fuc1()
{
    func2();
}

void func2()
{
    func3();
}

void func3()
{
    int a = 0;
}
int main()
{
    fuc1();
}