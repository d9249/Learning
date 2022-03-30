#include<stdio.h>

int main(void) {
    int used;
    int charge;
    int tax;

    printf("전기 사용량 (kWh) : ");
    scanf("%d", &used);
    if (used <= 200) {
        charge = 910 + used*93.0;
    } else if (used <= 400) {
		printf("기본료 : 1600\n");
        charge = 1600 + 200*93.0 + (used-200)*187.0;
    } else {
		printf("기본료 : 7300\n");
        charge = 7300 + 200*93.0 + 200*187.0 +(used-400)*280.0;
    }
	printf("전력량요금 : %.d\n", charge);
    tax = charge*0.1;
	printf("부가세 : %.d\n", tax);
    charge += tax;
    printf("합계 : %.d\n", charge);
	return 0;
}