#include <stdio.h>
int main()
{
    int num1, num2, count =0;
	int x, y, r, count2=0;
    
    printf("두 정수 입력: ");
    scanf("%d %d",&num1,&num2);
    
	if (num1 >= num2) {
		x = num1; 
		y = num2;
	} 
	else { 
		x = num2; 
		y = num1;
	}
	
    while(num1!= num2)
    {
        if(num1 > num2) {
			num1 -= num2;
			count+=1;	
		}
        else {
			num2 -= num1;
			count+=1;	
		}
    }
	printf("빼기 방법 최대 공약수: %d, 반복 : %d\n", num1, count);
	
	while (y != 0) {
		r = x % y ; 
		x = y ; 
		y = r ;	
		count2 += 1;
	}
	printf("나머지 방법 최대 공약수 : %d, 반복 : %d\n", x, count2);
    
    return 0;
}