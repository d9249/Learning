#include <stdio.h>

int fib(int n);
int count = 0;

int main(void) {
    for (int i = 1; i <= 10; i++) {
        printf("fibo(%d) = %-6d\t count = %d\n", i, fib(i), count);
        count = 0;
    }
    return 0;
}

int fib(int n) {
    count++;
    if (n < 2) {
        return n;
    } else {
        return fib(n - 1) + fib(n - 2);
    }
}
