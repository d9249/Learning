#include <stdio.h>

int main() {
    int i, j, k;
    for (i = 1; i <= 10; i = i + 2) {
        for (k = 10 - i; k >= 1; k--) {
            printf(" ");
        }
        for (j = 1; j <= i; j++) {
            printf("*");
        }
        printf("\n");
    }
    return 0;
}
