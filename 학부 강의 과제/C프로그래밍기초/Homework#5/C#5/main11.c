#include <stdio.h>

int main() {
    int i, j;
    for (i = 1; i <= 10; i = i + 2) {
        for (j = 1; j <= i; j++) {
            printf("*");
        }
        printf("\n");
    }
    return 0;
}
