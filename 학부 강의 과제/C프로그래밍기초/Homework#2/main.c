#include <stdio.h> 

int main(void) 
{ 
   int binary, copy, result = 0, temp = 1; 
   printf("2진수 입력 : "); 
   scanf("%d", &binary); 
   copy = binary; 
   while (copy!=0) 
   { 
      if (copy % 10) 
      result += temp; 
      copy /= 10; 
      temp *= 2; 
   } 
   printf("10진수 : %d\n", result); 
} 
