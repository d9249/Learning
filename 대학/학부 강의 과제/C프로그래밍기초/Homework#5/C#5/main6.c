#include <stdio.h>

int main() {
    int i, j, x;
    for (i = 5; i >= 1; i--) {
        for (x = 1; x <= 5-i; x++) {
            printf(" ");
        }
        for (j = 1; j <= i*2-1; j++) {
            printf("*");
        }
        printf("\n");
    }
    return 0;
}
